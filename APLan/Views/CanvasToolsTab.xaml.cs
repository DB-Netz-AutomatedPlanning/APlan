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

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for CanvasTools.xaml
    /// </summary>
    public partial class CanvasToolsTab : UserControl
    {
        public CanvasToolsTab()
        {
            InitializeComponent();
        }
        private void canvasRotateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //RotateTransform transform = (RotateTransform)MainWindow.drawing.RenderTransform;
            //transform.Angle = e.NewValue;
            //MainWindow.drawing.RenderTransform = transform;
        }
    }
}
