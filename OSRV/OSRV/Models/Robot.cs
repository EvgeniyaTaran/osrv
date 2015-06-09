using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OSRV.Models
{
    public class Robot
    {
        public Robot(Rectangle rect, MainWindow window)
        {
            _rectangle = rect;
            _window = window;
        }
        private Rectangle _rectangle;

        private MainWindow _window;
        public string Name { get; set; }

        private TimeSpan _time = TimeSpan.FromSeconds(0);
        public TimeSpan Time { get { return _time; } }
        //private int _position = 0;
        public int Position 
        {
            get { return (int)Canvas.GetLeft(_rectangle); }
            set { if (value  < 500 && value >= 0)
            { Canvas.SetLeft(_rectangle, (int)value); }
            else { MessageBox.Show("Sorry, you have entered incorrrect position for robot {0}, position won't be changed", this.Name); };
            }
        }

        private int _rotation = 0;
        public int Rotation
        {
            get { return _getRotation(); }
            set { _rotation = value;}
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
        public void Rotate (int degrees)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = Rotation;
            animation.To = Rotation + degrees;
            animation.Duration = new Duration(TimeSpan.FromSeconds(5));
            animation.BeginTime = this.Time;
            RotateTransform transform = new RotateTransform();
            _rectangle.RenderTransform = transform;
            _rectangle.RenderTransformOrigin = new Point(0.5, 0.5);
            transform.BeginAnimation(RotateTransform.AngleProperty, animation);
            this.Rotation = Rotation + degrees;
            this._time = this._time + TimeSpan.FromSeconds(5); 
        }

        public void Move (int meters)
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(5));
            // Create DoubleAnimation, set DA timeline duration, preserve end state of rectangle after completed animation run
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.Duration = duration;
            myDoubleAnimation.FillBehavior = FillBehavior.HoldEnd;
            //Create storyboard, set SB timeline duration, add animation to storyboard, set rectangle as target 
            //and specify the direction in which rectangle moves 
            Storyboard sb = new Storyboard();
            sb.Duration = duration;
            sb.Children.Add(myDoubleAnimation);
            Storyboard.SetTarget(myDoubleAnimation, _rectangle);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Canvas.LeftProperty));
            //Move robot by x meters
            myDoubleAnimation.By = meters * 20;
            _window.AppWindow.TabRoot.Resources.Add(Guid.NewGuid(), sb);
            // Begin the animation.
            sb.Begin();
            //Set rectangle left position on canva to the new position
            Position = (int)Canvas.GetLeft(_rectangle) + meters * 20;
            //Set begin time for the next run of double animation
            this._time = this._time + TimeSpan.FromSeconds(5);
        }
    }
}
