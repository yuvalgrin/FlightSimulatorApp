using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace FlightSimulatorGui.Views
{
    /// <summary>
    /// Interaction logic for FSMap.xaml
    /// </summary>
    public partial class FSMap : UserControl
    {
        Image airplane = new Image();
        MapLayer mapLayer = new MapLayer();
        Location airplaneLocation = new Location();
        public FSMap()
        {
            InitializeComponent();
            initAirplane();
        }

        private void initAirplane()
        {
            airplane.Source = new BitmapImage(new Uri("/FlightSimulatorGui;component/Resources/airplane.png", UriKind.Relative));
            airplane.Width = 128;
            airplane.Height = 128;

            mapLayer.AddChild(airplane, airplaneLocation, PositionOrigin.Center);
            myMap.Children.Add(mapLayer);

            setAirplaneLocation(31.643854, 34.920341);
        }

        public void setAirplaneLocation(double lat, double lon)
        {
            airplaneLocation.Latitude = lat;
            airplaneLocation.Longitude = lon;

        }
    }
}
