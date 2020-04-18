using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using FlightSimulatorApp.ViewModel;
using System.Globalization;
using System.Windows.Threading;

namespace FlightSimulatorApp.Views
{
    public partial class Joystick : UserControl
    {
        public delegate void OnScreenJoystickEventHandler(Joystick sender, VirtualJoystickEventArgs args);
        public event OnScreenJoystickEventHandler Moved;

        private DispatcherTimer _timer = new DispatcherTimer();
        private Storyboard _centKnob;
        private Point _clickPoint;
        private Double _canWidth;
        private Double _canHeight;

        public static readonly DependencyProperty RudderProperty =
        DependencyProperty.Register("Rudder", typeof(double), typeof(Joystick), null);
        private Double Rudder
        {
            get { return Convert.ToDouble(GetValue(RudderProperty), CultureInfo.InvariantCulture); }
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < -1)
                    value = -1;
                SetValue(RudderProperty, value);
            }
        }

        public static readonly DependencyProperty ElevatorProperty =
        DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick), null);
        private Double Elevator
        {
            get { return Convert.ToDouble(GetValue(ElevatorProperty), CultureInfo.InvariantCulture); }
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < -1)
                    value = -1;
                SetValue(ElevatorProperty, value);
            }
        }


        // Costructor to init all the cs
        public Joystick()
        {
            InitializeComponent();
            Moved += notifyKnobMove;
            this.DataContext = (Application.Current as App).JoystickViewModel;
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick +=
                delegate (Object sender, EventArgs e) {
                    _timer.Stop();
                };
            _centKnob = Knob.Resources["CenterKnob"] as Storyboard;
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Rudder = 0;
            Elevator = 0;
        }

        public void notifyKnobMove(object sender, VirtualJoystickEventArgs e)
        {
            if (e != null && !_timer.IsEnabled)
            {
                (Application.Current as App).JoystickViewModel.JoyRudderUpdate(e.Rudder);
                (Application.Current as App).JoystickViewModel.JoyElevatorUpdate(e.Elevator);
            }

            if (!_timer.IsEnabled)
                _timer.Start();
        }

        private void Knob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clickPoint = e.GetPosition(Base);
            Knob.CaptureMouse();
            _centKnob.Stop();
        }

        private void Knob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _centKnob.Begin();
            _canWidth = Base.ActualWidth - KnobBase.ActualWidth;
            _canHeight = Base.ActualHeight - KnobBase.ActualHeight;
            Knob.ReleaseMouseCapture();
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            // If the mouse does not hold the knob currently
            if (!Knob.IsMouseCaptured)
                return;

            Point deltaPoint = calcDeltaPoint(e.GetPosition(Base));

            Double distance = CalcDist(deltaPoint);
            if (Double.IsNaN(distance) || distance == 0)
                return;

            Rudder = Math.Round(2.1 * deltaPoint.X / _canWidth, 2);
            Elevator = Math.Round(-2.1 * deltaPoint.Y / _canHeight, 2);

            SetKnobPosition(deltaPoint);

            if (Moved != null)
                Moved(this, new VirtualJoystickEventArgs { Rudder = Rudder, Elevator = Elevator });
        }

        private Point calcDeltaPoint(Point curP)
        {
            double deltaX = curP.X - _clickPoint.X;
            double deltaY = curP.Y - _clickPoint.Y;
            return new Point(deltaX, deltaY);
        }

        private void SetKnobPosition(Point p)
        {
            knobPosition.X = p.X;
            knobPosition.Y = p.Y;
        }

        private Double CalcDist(Point deltaPoint)
        {
            double delta = deltaPoint.Y * deltaPoint.Y + deltaPoint.X * deltaPoint.X;
            double distance = Math.Round(Math.Sqrt(delta));
            if (distance >= _canHeight / 2)
                return Double.NaN;
            if (distance >= _canWidth / 2)
                return Double.NaN;
            return distance;
        }

    }

    public class VirtualJoystickEventArgs
    {
        public double Rudder { get; set; }
        public double Elevator { get; set; }
    }
}