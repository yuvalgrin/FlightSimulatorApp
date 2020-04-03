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

        public static readonly DependencyProperty AileronProperty =
        DependencyProperty.Register("Aileron", typeof(double), typeof(Joystick), null);
        private Double Aileron
        {
            get { return Convert.ToDouble(GetValue(AileronProperty)); }
            set
            {
                if (value > 1)
                    value = 1;
                else if (value < -1)
                    value = -1;
                SetValue(AileronProperty, value);
            }
        }

        public static readonly DependencyProperty ElevatorProperty =
        DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick), null);
        private Double Elevator
        {
            get { return Convert.ToDouble(GetValue(ElevatorProperty)); }
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
            Aileron = 0;
            Elevator = 0;
        }

        public void notifyKnobMove(object sender, VirtualJoystickEventArgs e)
        {
            (Application.Current as App).JoystickViewModel.joyAileronUpdate(e.Aileron);
            (Application.Current as App).JoystickViewModel.joyElevatorUpdate(e.Elevator);
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
            Point deltaPoint;

            // If the mouse does not hold the knob currently
            if (!Knob.IsMouseCaptured)
                return;

            double deltaX = e.GetPosition(Base).X - clickPoint.X;
            double deltaY = e.GetPosition(Base).Y - clickPoint.Y;
            deltaPoint = new Point(deltaX, deltaY);

            Double distance = calcDist(deltaPoint);
            if (Double.IsNaN(distance) || distance == 0)
                return;

            Aileron = Math.Round(2.1 * deltaPoint.X / canWidth, 2);
            Elevator = Math.Round(-2.1 * deltaPoint.Y / canHeight, 2);
            knobPosition.X = deltaPoint.X;
            knobPosition.Y = deltaPoint.Y;

            if (Moved != null)
                Moved(this, new VirtualJoystickEventArgs { Aileron = Aileron, Elevator = Elevator });
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
        public double Aileron { get; set; }
        public double Elevator { get; set; }
    }
}