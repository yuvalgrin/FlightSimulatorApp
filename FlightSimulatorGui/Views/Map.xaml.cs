using FlightSimulatorGui.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlightSimulatorGui.Views
{
    public partial class Map : UserControl
    {
        public int Zoom = 8;
        public Map()
        {
            InitializeComponent();
            this.DataContext = (Application.Current as App).MapViewModel;
            initTimer();
        }
        
        /* This timer will centerize the into the airplane pushpin once every 2 seconds
         */
        public void initTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            myMap.SetView(new Location(pin.Location.Latitude, pin.Location.Longitude), myMap.ZoomLevel) ;
        }

    }
}
