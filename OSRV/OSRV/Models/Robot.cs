using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OSRV.Models
{
    public class Robot
    {
        public Robot(Rectangle rect)
        {
            rectangle = rect;
        }
        public Rectangle rectangle;
        public string Name { get; set; }

        private int _position = 0;
        public int Position 
        {
            get { return _getPosition(); }
            set { if (value + _position < 500)
            { _position = value; }
            else { MessageBox.Show("Sorry, you have entered incorrrect position for robot {0}, position won't be changed", this.Name); };
            }
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
        public void Move (string name, int meters)
        {
            this.Name = name;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = Position;
            animation.To = meters*20;
            Position = Position + meters * 20;
            animation.Duration = new Duration(TimeSpan.Parse("0:0:5"));
            rectangle.BeginAnimation(Canvas.LeftProperty, animation);
        }
    }
}
