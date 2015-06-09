using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class TokenPosition
    {
        public TokenPosition(int index, int line, int column)
        {
            Index = index;
            Line = line;
            Column = column;
        }

        public int Column { get; private set; }
        public int Index { get; private set; }
        public int Line { get; private set; }
    }
}
