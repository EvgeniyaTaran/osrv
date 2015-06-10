using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OSRV.Compiler.Errors;

namespace OSRV.Loggers
{
    public sealed class CompilationLogger
    {
        private static CompilationLogger _compilationLoggerInstance;
        private static readonly string fileName = "D:\\CompilationLog.txt";

        public static CompilationLogger Instance
        {
            get
            {
                if (_compilationLoggerInstance == null)
                {
                    _compilationLoggerInstance = new CompilationLogger();
                }
                return _compilationLoggerInstance;
            }
        }

        private CompilationLogger() { }

        public void Log(IEnumerable<string> messagesCollection)
        {
            File.AppendAllLines(fileName, new string[] { DateTime.Now.ToString(new CultureInfo("ru-RU")) });
            File.AppendAllLines(fileName, messagesCollection);
            File.AppendAllLines(fileName, new string[] { Environment.NewLine });
        }

        public void Log(IEnumerable<Error> errorsCollection)
        {
            IEnumerable<string> errorMessages = errorsCollection.Select(
                e => string.Format("{3}: [Строка {0}, позиция {1}] {2}",
                e.LineNumber,
                e.ColumnNumber,
                e.Message,
                (e is SyntaxError ? "Синтаксическая ошибка" : "Семантическая ошибка")));
            Log(errorMessages);
        }
    }
}