using System.Collections.Generic;
using LookAndSay.Models;
using LookAndSay.Options;

namespace LookAndSay.Render
{
    public interface IRenderResults
    {
         void Render(LookAndSayParameters parameters, IEnumerable<Profiled<string>> results);
    }
}