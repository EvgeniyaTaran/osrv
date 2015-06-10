using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Loggers
{
    public class CompileResult
    {
        private static CompileResult _actionLoggerInstance;
        private static readonly string fileName = "D:\\Code.txt";

        public static CompileResult Instance
        {
            get
            {
                if (_actionLoggerInstance == null)
                {
                    _actionLoggerInstance = new CompileResult();
                }
                return _actionLoggerInstance;
            }
        }

        private CompileResult() { }

        public void Log(IEnumerable<string> messagesCollection)
        {
            File.AppendAllLines(fileName, messagesCollection);
            File.AppendAllLines(fileName, new string[] { Environment.NewLine });
        }
    }
}
