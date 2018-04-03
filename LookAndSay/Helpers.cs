using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SelfDescSuite
{
    internal static class Helpers
    {
        /// <summary>
        /// Call the given function iteratively, starting with the seed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> Iterate<T>(this T seed, Func<T, T> func)
        {
            T current = seed;
            while (true)
            {
                yield return current;
                current = func(current);
            }
        }

        /// <summary>
        /// Profile an enumerable, returning the TimeSpan elapsed after each iteration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T, TimeSpan>> Profile<T>(this IEnumerable<T> source)
        {
            var sw = new Stopwatch();
            sw.Start();

            foreach (var v in source)
            {
                yield return Tuple.Create(v, sw.Elapsed);
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
    }
}
