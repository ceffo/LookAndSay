using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using LookAndSay.Models;
using Optional;
using Optional.Linq;

namespace LookAndSay
{
    internal static class Helpers
    {
        /// <summary>
        /// Call the given function iteratively, starting with the seed
        /// </summary>
        /// <typeparam name="T">Type of the sequence</typeparam>
        /// <param name="seed">Seed of the sequence</param>
        /// <param name="next">Function that yields the next sequence element</param>
        /// <returns>The sequence starting with the seed</returns>
        public static IEnumerable<T> Iterate<T>(this T seed, Func<T, T> next)
        {
            T current = seed;
            while (true)
            {
                yield return current;
                current = next(current);
            }
        }

        /// <summary>
        /// Profile an enumerable, returning the TimeSpan elapsed after each iteration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<Profiled<T>> Profile<T>(this IEnumerable<T> source)
        {
            var sw = new Stopwatch();
            sw.Start();

            foreach (var v in source)
            {
                yield return new Profiled<T>(v, sw.Elapsed);
                sw.Restart();
            }
        }

        /// <summary>
        /// Skip first num characters of a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Skip(this string input, int num) => input.Substring(num, input.Length - num);

        /// <summary>
        /// Pretty prints a timespan
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string PrettyPrint(this TimeSpan span)
        {
            var time = new[]
                {
                    new { Unit = "d", Value = span.Days },
                    new { Unit = "h", Value = span.Hours},
                    new { Unit = "m", Value = span.Minutes},
                    new { Unit = "s", Value = span.Seconds},
                    new { Unit = "ms", Value = span.Milliseconds},
                };

            var filtred = time.SkipWhile(t => t.Value == 0)
                              .Take(2)
                              .Where(t => t.Value != 0)
                              .Select(t => $"{t.Value}{t.Unit}")
                              .ConcatIfEmpty(() => $"{span.TotalMilliseconds*1000:F0}ns");
            
            return string.Join(' ', filtred);
        }

        public static IEnumerable<T> ConcatIfEmpty<T>(this IEnumerable<T> source, Func<T> gen) => source.Any() ? source : EnumerableEx.Return(gen());

        public static IEnumerable<T> Optionaly<T>(this IEnumerable<T> source,
            bool condition,
            Func<IEnumerable<T>, IEnumerable<T>> transf) => 
                condition ? transf(source) : source;

        /// <summary>
        /// Optionaly transforms a source based on an optional value
        /// </summary>
        /// <param name="source">The source sequence</param>
        /// <param name="optional">Optional value</param>
        /// <param name="transf">Transformation that takes the optional value and the sequence</param>
        /// <returns>Transformed sequence or original sequence</returns>
        public static IEnumerable<TValue> Optionaly<TValue,TOption>(this IEnumerable<TValue> source, 
            Option<TOption> optional, 
            Func<IEnumerable<TValue>, TOption, IEnumerable<TValue>> transf)
        {
            return optional.Match(
                some: u => transf(source, u),
                none: () => source
            );
        }

        /// <summary>
        /// Like TakeWhile but yields the first item that breaks the predicate
        /// </summary>
        /// <param name="source">Source sequence</param>
        /// <param name="predicate">Predicate to stop taking elements</param>
        /// <returns></returns>
        public static IEnumerable<T> TakeWhileWithLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            bool prevPredResult = true;

            foreach (var x in source)
            {
                if (!prevPredResult)
                    yield break;
                yield return x;
                prevPredResult = predicate(x);
            }
        }

        public static string DescribeProperties<T>(this T obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();
            var publicProperties = type.GetProperties(BindingFlags.Public|BindingFlags.Instance);
            var optional = typeof(Option<>).GetGenericTypeDefinition();
            foreach(var propInfo in publicProperties)
            {
                var propType = propInfo.PropertyType;
                var value = propInfo.GetValue(obj);
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == optional)
                {
                    var hasValueProp = propType.GetProperty("HasValue");
                    var hasValue = (bool) hasValueProp.GetValue(value);
                    if (!hasValue) continue;
                    var valueor = propType.GetMethod("ValueOr", propType.GenericTypeArguments) ;

                    value = valueor?.Invoke(value, new object[] { null }) ?? value;
                }

                sb.AppendLine($"{propInfo.Name}: {value}");
            }
            return sb.ToString();
        }
    }
}
