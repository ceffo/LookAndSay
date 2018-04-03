using CommandLineParser.Core;
using System;
using System.Linq;

namespace SelfDescSuite
{
    public enum ShowType { Result, Digits, Log};

    public class ApplicationArguments
    {
        public int Seed { get; set; }
        public int MaxIterations { get; set; }
        public int MaxSeconds { get; set; }
        public ShowType ShowResult { get; set; }
    }

    class Program
    {

        static int Main(string[] args)
        {
            var p = new FluentCommandLineParser<ApplicationArguments>();

            p.Setup(arg => arg.Seed)
             .As('s', "seed")
             .WithDescription("Seed number to start the iteration")
             .SetDefault(1);

            p.Setup(arg => arg.MaxIterations)
             .As('i', "maxIterations")
             .WithDescription("Maximum iterations")
             .SetDefault(10);

            p.Setup(a => a.MaxSeconds)
             .As('m', "maxSeconds")
             .WithDescription("Maximum time in seconds for an iteration");

            p.Setup(a => a.ShowResult)
             .As('r', "showResult")
             .WithDescription("Result type. Result: result number; Digits: number of digits in the result; Log: Log10 of number of digits")
             .SetDefault(ShowType.Result);

            p.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text));

            var result = p.Parse(args);

            if (result.HasErrors)
            {
                Console.WriteLine(result.ErrorText);
                p.HelpOption.ShowHelp(p.Options);
                return 1;
            }

            if (result.HelpCalled)
            {
                return 0;
            }

            var seed = p.Object.Seed.ToString();
            var maxTime = p.Object.MaxSeconds > 0 ? TimeSpan.FromSeconds(p.Object.MaxSeconds) : TimeSpan.MaxValue;
            var maxIterations = p.Object.MaxIterations;
            var showType = p.Object.ShowResult;

            Console.WriteLine($"Seed: {seed}");
            Console.WriteLine($"----");

            seed.Iterate(LookAndSay.Describe)
                .Take(maxIterations+1)
                .Profile()
                .TakeWhile(t => t.Item2 < maxTime)
                .ForEach((t,i) => Console.WriteLine($"{i,3} | {ShowItem(t.Item1, showType)} | {t.Item2.PrettyPrint()}"));

            return 0;
        }

        private static string ShowItem(string str, ShowType type)
        {
            switch (type)
            {
                case ShowType.Result:
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
