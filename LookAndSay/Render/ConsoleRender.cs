using System;
using System.Collections.Generic;
using System.Linq;
using LookAndSay.Models;
using LookAndSay.Options;

namespace LookAndSay.Render
{
    public class ConsoleRender : IRenderResults
    {
        public ConsoleRender()
        {
        }

        public void Render(LookAndSayParameters parameters, IEnumerable<Profiled<string>> results)
        {
            Console.WriteLine();
            Console.WriteLine("Look And Say");
            Console.WriteLine("------------");
            Console.Write(parameters.DescribeProperties());
            Console.WriteLine("------------");
            results.ForEach( (result,i) => 
            { 
                Console.WriteLine($"{i,3} | {ShowItem(result, parameters.ShowResultType)} | {result.Tag.PrettyPrint()}");
            });
            Console.WriteLine("Done.");
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