using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRV.Models
{
    public class Robot
    {
        public string Name { get; set; }

        private int _position = 0;
        public int Position 
        {
            get { return _getPosition(); }
        }

        private int _rotation = 0;
        public int Rotation
        {
            get { return _getRotation(); }
        }



        private int _getPosition()
        {
            if (_position > 10) 
            {
                return 10;
            }
            if (_position < 0)
            {
                return 0;
            }
            return _position;
        }

        private int _getRotation()
        {
            if (_rotation > 360)
            {
                _rotation -= 360;
                _getRotation();
            }
            if (_rotation < 0)
            {
                _rotation += 360;
                _getRotation();
                return 0;
            }
            return _rotation;
        }
    }
}
