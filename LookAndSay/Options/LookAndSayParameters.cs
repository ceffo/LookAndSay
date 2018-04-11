using System;
using LookAndSay.Render;
using Optional;
using Optional.Linq;

namespace LookAndSay.Options
{
    public class LookAndSayParameters
    {
        private const int DefaultNumIterations = 10;
        private CommandLineOptions _CmdLineOptions;
        
        public LookAndSayParameters(CommandLineOptions cmdLine)
        {
            this._CmdLineOptions = cmdLine;

            _Seed = cmdLine.Seed.HasValue ? cmdLine.Seed.Value : 1;
            MaxTimePerIteration = cmdLine.MaxSeconds.ToOption().Select(TimeSpan.FromSeconds);
            var maxit = cmdLine.MaxIterations.ToOption();
            MaxIterations = MaxTimePerIteration.Match(some: _ => maxit, none: ()=>maxit.Else(DefaultNumIterations.Some()));
        }

        private long _Seed;
        public string Seed => _Seed.ToString();
        public Option<TimeSpan> MaxTimePerIteration { get; }
        public Option<int> MaxIterations { get; }
        public ShowType ShowResultType => _CmdLineOptions.ShowResult;
    }

    public static class Extensions
    {
        public static LookAndSayParameters ToParameters(this CommandLineOptions options)
        => new LookAndSayParameters(options);

    }
}