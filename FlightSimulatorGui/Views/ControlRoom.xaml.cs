﻿using System;
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
    /// Interaction logic for FSDataView.xaml
    /// </summary>
    public partial class ControlRoom : UserControl
    {
        public ControlRoom()
        {
            InitializeComponent();
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            string data = tbInput.Text;
        }
    }
}