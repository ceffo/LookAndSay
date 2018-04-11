
using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using LookAndSay.Options;
using LookAndSay.Render;
using Optional;
using Optional.Linq;

namespace LookAndSay
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(HandleErrors)
                ;
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
        }

        private static void RunWithOptions(CommandLineOptions options)
        {
            var parameters = options.ToParameters();

            var results = parameters.Seed
                .Iterate(StringExtensions.Describe)
                .Optionaly(parameters.MaxIterations, (source, maxIt) => source.Take(maxIt+1)) // adding one since 0 is seed
                .Profile()
                .Optionaly(parameters.MaxTimePerIteration, (source, maxTime) => source.TakeWhileWithLast(profiled => profiled.Tag < maxTime))
                ;

            RenderFactory
                .Create(options.RenderType)
                ?.Render(parameters, results);
        }
    }
}
