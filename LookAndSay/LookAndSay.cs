using System;
using System.Collections.Generic;
using System.Text;

namespace SelfDescSuite
{
    public static class LookAndSay
    {
        /// <summary>
        /// Describes the input string by outputing 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Describe(string input)
        {
            if (input.Length == 0) return string.Empty;
            const char stop = (char)1 ;

            var sb = new StringBuilder();

            var times = 1;
            var repeat = input[0];

            foreach (var chr in input.Skip(1) + stop)
            {
                if (chr != repeat)
                {
                    sb.Append(times.ToString() + repeat);
                    repeat = chr;
                    times = 1;
                }
                else ++times;
            }

            return sb.ToString();
        }
    }
}
