using System;

namespace LookAndSay.Models
{
    /// <summary>
    /// Associate a Timespan to a value
    /// </summary>
    public class Profiled<T> : Tagged<T, TimeSpan>
    {
        public Profiled(T value, TimeSpan tag) : base(value, tag)
        {
        }
    }
}