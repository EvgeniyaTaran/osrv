using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class SentenceDefinition
    {
        public SentenceDefinition(Regex regex)
            : this(regex, false)
        {
        }

        public SentenceDefinition(Regex regex, bool isIgnored)
        {
            Regex = regex;
            IsIgnored = isIgnored;
        }

        public bool IsIgnored { get; private set; }
        public Regex Regex { get; private set; }
    }
}
