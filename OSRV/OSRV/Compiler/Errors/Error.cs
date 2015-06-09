using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Compiler.Errors
{
    public class Error
    {
        public string Message { get; set; }

        public string Reason { get; set; }

        public int LineNumber { get; set; }

        public int ColumnNumber { get; set; }

    }
}
