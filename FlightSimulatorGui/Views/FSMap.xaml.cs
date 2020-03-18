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
        public FSMap()
        {
            InitializeComponent();

            myMap.Mode = new AerialMode();

            myPolyline.Locations = new LocationCollection() {
                new Location(47.6424,-122.3219),
                new Location(47.8424,-102.1747),
                new Location(47.67856,-112.130994)};
                }
    }
}
