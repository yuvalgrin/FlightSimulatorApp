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
using FlightSimulatorGui.server;

namespace FlightSimulatorGui.Views
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private Storyboard centerKnob;
        public Joystick()
        {
            InitializeComponent();
            //centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            //Aileron = 0;
            //Elevator = 0;
            //lastAileron = 0;
            //lastElevator = 0;
            //Released?.Invoke(this);
        }
    }
}