using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace LookAndSay
{
    public enum ShowType { Raw, Digits, Log};

    public class Options
    {
        [Value(0, MetaValue="seed",
            HelpText="Seed number to start the iteration.")]
        public int? Seed { get; set; }
        
        [Option('m', "max_iterations",
            HelpText="Maximum number of iterations")]
        public int? MaxIterations { get; set; }

        [Option('t', "max_time",
            HelpText="Maximum time in seconds per iterations")]
        public double? MaxSeconds { get; set; }

        [Option('r', "result",
            Default=ShowType.Raw,
            HelpText="Result type (Raw, Digits, Log)")]
        public ShowType ShowResult { get; set; }
    }
}