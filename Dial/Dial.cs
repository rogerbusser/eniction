using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Dial
{
    public class Dial : Control
    {
        Grid _knob;
        RotateTransform _value;
        bool _hasCapture = false;

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty KnobProperty =
        DependencyProperty.Register("Knob", typeof(UIElement),
        typeof(Dial), null);

        public static readonly DependencyProperty FaceProperty =
        DependencyProperty.Register("Face", typeof(UIElement),
        typeof(Dial), null);

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            //set { SetValue(ValueProperty, value); }
            set { SetPosition( value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public UIElement Knob
        {
            get { return (UIElement)GetValue(KnobProperty); }
            set { SetValue(KnobProperty, value); }
        }

        public UIElement Face
        {
            get { return (UIElement)GetValue(FaceProperty); }
            set { SetValue(FaceProperty, value); }
        }

        double AngleQuadrant(double width, double height, Point point)
        {
            double radius = width / 2;
            Point centre = new Point(radius, height / 2);
            Point start = new Point(0, height / 2);
            double triangleTop = Math.Sqrt(Math.Pow((point.X - centre.X), 2) + Math.Pow((centre.Y - point.Y), 2));
            double triangleHeight = (point.Y > centre.Y) ? point.Y - centre.Y : centre.Y - point.Y;

            return ((triangleHeight * Math.Sin(90)) / triangleTop) * 100;
        }

        double GetAngle(Point point)
        {
            double diameter = _knob.ActualWidth;
            double height = _knob.ActualHeight;
            double radius = diameter / 2;
            double rotation = AngleQuadrant(diameter, height, point);

            if ((point.X > radius) && (point.Y <= radius))
            {
                rotation = 90.0 + (90.0 - rotation);
            }
            else if ((point.X > radius) && (point.Y > radius))
            {
                rotation = 180.0 + rotation;
            }
            else if ((point.X < radius) && (point.Y > radius))
            {
                rotation = 270.0 + (90.0 - rotation);
            }

            return rotation;
        }

        private void SetPosition(double rotation)
        {
            if (Minimum > 0 && Maximum > 0 && Minimum < 360 && Maximum <= 360)
            {
                if (rotation < Minimum) { rotation = Minimum; }
                if (rotation > Maximum) { rotation = Maximum; }
            }

            _value.Angle = rotation;
            //Value = rotation;
            SetValue(ValueProperty, rotation);
        }

        public Dial()
        {
            this.DefaultStyleKey = typeof(Dial);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _knob = ((Grid)GetTemplateChild("Knob"));
            _value = ((RotateTransform)GetTemplateChild("DialValue"));

            if (Minimum > 0 && Minimum < 360) { SetPosition(Minimum); }
            _knob.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
           // _knob.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
            {
                _hasCapture = false;
            };

            _knob.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
          //  _knob.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                _hasCapture = true;
                SetPosition(GetAngle(e.GetCurrentPoint(_knob).Position));
                // SetPosition(GetAngle(e.GetPosition(_knob)));
            };

            _knob.PointerMoved += (object sender, PointerRoutedEventArgs e) =>
           // _knob.MouseMove += (object sender, MouseEventArgs e) =>
            {
                if (_hasCapture)
                {
                    SetPosition(GetAngle(e.GetCurrentPoint(_knob).Position));
                    //SetPosition(GetAngle(e.GetPosition(_knob)));
                }
            };

            _knob.PointerExited += (object sender, PointerRoutedEventArgs e) =>
            //_knob.MouseLeave += (object sender, MouseEventArgs e) =>
            {
                _hasCapture = false;
            };
        }
    }
}