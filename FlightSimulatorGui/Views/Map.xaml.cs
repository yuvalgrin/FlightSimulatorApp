﻿using FlightSimulatorGui.ViewModel;
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

namespace FlightSimulatorGui.Views
{
    public partial class Map : UserControl
    {
        MapViewModel mapViewModel;

        public Map()
        {
            InitializeComponent();
            mapViewModel = new MapViewModel();
            this.DataContext = mapViewModel;
        }

    }
}
