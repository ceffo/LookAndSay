
using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using Optional;
using Optional.Linq;

namespace LookAndSay
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(HandleErrors)
                ;
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
        }

        private static void RunWithOptions(Options options)
        {
            var seed = options.Seed.HasValue ? options.Seed.Value : 1;
            var maxTimeOpt = options.MaxSeconds.ToOption().Select(TimeSpan.FromSeconds);
            var maxit = options.MaxIterations.ToOption();
            var maxIterationsOpt = maxTimeOpt.Match(some: _ => maxit, none: ()=>maxit.Else(10.Some()));
            var showType = options.ShowResult;

            Console.WriteLine($"Seed={seed} MaxTime={maxTimeOpt.Select(Helpers.PrettyPrint)} MaxIterations={maxIterationsOpt} Show={showType}");      
            Console.WriteLine($"----------------");

            seed.ToString()
                .Iterate(StringExtensions.Describe)
                .Optionaly(maxIterationsOpt, (source, maxIt) => source.Take(maxIt+1)) // adding one since 0 is seed
                .Profile()
                .Optionaly(maxTimeOpt, (source, maxTime) => source.TakeWhileWithLast(profiled => profiled.Tag < maxTime))
                .ForEach((profiled,i) =>
                    Console.WriteLine($"{i,3} | {ShowItem(profiled, showType)} | {profiled.Tag.PrettyPrint()}"))
                ;
        }

        private static string ShowItem(string str, ShowType type)
        {
            switch (type)
            {
                case ShowType.Raw:
                    return str;
                case ShowType.Digits:
                    return str.Length.ToString();
                case ShowType.Log:
                    return $"{Math.Log10(str.Length):F2}";
            }
            return string.Empty;
        }
    }
}
