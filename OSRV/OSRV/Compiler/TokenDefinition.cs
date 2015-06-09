using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class TokenDefinition
    {
        public TokenDefinition(string type, Regex regex)
            : this(type, regex, false)
        {
        }

        public TokenDefinition(string type, Regex regex, bool isIgnored)
        {
            Type = type;
            Regex = regex;
            IsIgnored = isIgnored;
        }

        public bool IsIgnored { get; private set; }
        public Regex Regex { get; private set; }
        public string Type { get; private set; }
    }
}
