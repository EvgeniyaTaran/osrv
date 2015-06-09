using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class MyCommand
    {
        public ActionType ActionType { get; set; }
        public string RobotName { get; set; }
        public int Value { get; set; }
    }

    public enum ActionType 
    {
        Move,
        Rotate
    }

    public enum RobotsActivity
    {
        None,
        R1,
        R2,
        Both
    }
}
