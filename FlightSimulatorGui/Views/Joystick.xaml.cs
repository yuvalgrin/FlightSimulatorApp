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
using FlightSimulatorGui.ViewModel;
using System.Globalization;

namespace FlightSimulatorGui.Views
{
    public partial class Joystick : UserControl
    {
        public delegate void OnScreenJoystickEventHandler(Joystick sender, VirtualJoystickEventArgs args);
        public event OnScreenJoystickEventHandler Moved;

        private Storyboard centerKnob;
        private Point clickPoint;
        private Double canWidth;
        private Double canHeight;

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

            centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Rudder = 0;
            Elevator = 0;
        }

        public static void notifyKnobMove(object sender, VirtualJoystickEventArgs e)
        {
            (Application.Current as App).JoystickViewModel.JoyRudderUpdate(e.Rudder);
            (Application.Current as App).JoystickViewModel.JoyElevatorUpdate(e.Elevator);
        }

        private void Knob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickPoint = e.GetPosition(Base);
            Knob.CaptureMouse();
            centerKnob.Stop();
        }

        private void Knob_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            centerKnob.Begin();
            canWidth = Base.ActualWidth - KnobBase.ActualWidth;
            canHeight = Base.ActualHeight - KnobBase.ActualHeight;
            Knob.ReleaseMouseCapture();
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            // If the mouse does not hold the knob currently
            if (!Knob.IsMouseCaptured)
                return;

            Point deltaPoint = calcDeltaPoint(e.GetPosition(Base));

            Double distance = calcDist(deltaPoint);
            if (Double.IsNaN(distance) || distance == 0)
                return;

            Rudder = Math.Round(2.1 * deltaPoint.X / canWidth, 2);
            Elevator = Math.Round(-2.1 * deltaPoint.Y / canHeight, 2);

            setKnobPosition(deltaPoint);

            if (Moved != null)
                Moved(this, new VirtualJoystickEventArgs { Rudder = Rudder, Elevator = Elevator });
        }

        private Point calcDeltaPoint(Point curP)
        {
            double deltaX = curP.X - clickPoint.X;
            double deltaY = curP.Y - clickPoint.Y;
            return new Point(deltaX, deltaY);
        }

        private void setKnobPosition(Point p)
        {
            knobPosition.X = p.X;
            knobPosition.Y = p.Y;
        }

        private Double calcDist(Point deltaPoint)
        {
            double delta = deltaPoint.Y * deltaPoint.Y + deltaPoint.X * deltaPoint.X;
            double distance = Math.Round(Math.Sqrt(delta));
            if (distance >= canHeight / 2)
                return Double.NaN;
            if (distance >= canWidth / 2)
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