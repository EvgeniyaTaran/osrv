using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class Sentence
    {
        public Sentence(string value, int line) 
        {
            Value = value;
            LineNumber = line;
        }
        public string Value { get; set; }

        public int LineNumber { get; set; }
    }
}
