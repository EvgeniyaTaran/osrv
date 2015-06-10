using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OSRV.Loggers
{
    public sealed class ActionLogger
    {
        private static ActionLogger _actionLoggerInstance;
        private static readonly string fileName = "D:\\Log.txt";

        public static ActionLogger Instance
        {
            get
            {
                if (_actionLoggerInstance == null)
                {
                    _actionLoggerInstance = new ActionLogger();
                }
                return _actionLoggerInstance;
            }
        }

        private ActionLogger() { }

        public void Log(IEnumerable<string> messagesCollection)
        {
            File.AppendAllLines(fileName, new string[] { DateTime.Now.ToString(new CultureInfo("ru-RU")) });
            File.AppendAllLines(fileName, messagesCollection);
            File.AppendAllLines(fileName, new string[] { Environment.NewLine });
        }
    }
}
