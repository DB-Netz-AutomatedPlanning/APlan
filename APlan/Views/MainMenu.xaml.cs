using APLan.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            this.Loaded += MenuItem_OnLoaded;
        }
        private void MenuItem_OnLoaded(object sender, RoutedEventArgs e)
        {
            
            Window window = Window.GetWindow(this);
            foreach (InputBinding ib in this.InputBindings)
            {
                window.InputBindings.Add(ib);
            }
        }
        private void MenuItem_VisualizedData(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                ((UserControl)MainWindow.visualized_Data).Visibility = Visibility.Collapsed;
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                ((UserControl)MainWindow.visualized_Data).Visibility = Visibility.Visible;

                ((UserControl)MainWindow.aplanCADViewer).Visibility = Visibility.Collapsed;
                ((UserControl)MainWindow.Canvas_Content).Visibility = Visibility.Collapsed;
                canvasContentItem.IsChecked = false;
                aplanCadViewerItem.IsChecked = false;
            }

        }
        private void MenuItem_CanvasContent(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                ((UserControl)MainWindow.Canvas_Content).Visibility = Visibility.Collapsed;
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                ((UserControl)MainWindow.Canvas_Content).Visibility = Visibility.Visible;
                ((UserControl)MainWindow.visualized_Data).Visibility = Visibility.Collapsed;
                ((UserControl)MainWindow.aplanCADViewer).Visibility = Visibility.Collapsed;
                visualizedDataItem.IsChecked = false;
                aplanCadViewerItem.IsChecked = false;
            }

        }
        private void MenuItem_AplanCADViewer(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                ((UserControl)MainWindow.aplanCADViewer).Visibility = Visibility.Collapsed;
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                ((UserControl)MainWindow.aplanCADViewer).Visibility = Visibility.Visible;
                ((UserControl)MainWindow.visualized_Data).Visibility = Visibility.Collapsed;
                ((UserControl)MainWindow.Canvas_Content).Visibility = Visibility.Collapsed;
                visualizedDataItem.IsChecked = false;
                canvasContentItem.IsChecked = false;
            }

        }
        private void MenuItem_Symbols(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                MainWindow.c0.Width = new GridLength(1, GridUnitType.Star);
                MainWindow.c1.Width = new GridLength(6, GridUnitType.Star);
                MainWindow.c2.Width = new GridLength(0, GridUnitType.Star);
                MainWindow.c3.Width = new GridLength(0.01, GridUnitType.Star);
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                MainWindow.c0.Width = new GridLength(1, GridUnitType.Star);
                MainWindow.c1.Width = new GridLength(5, GridUnitType.Star);
                MainWindow.c2.Width = new GridLength(0.01, GridUnitType.Star);
                MainWindow.c3.Width = new GridLength(1, GridUnitType.Star);
            }
        }
        private void MenuItem_Planningtab(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                MainWindow.r1.Height = new GridLength(0);
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                MainWindow.r1.Height = new GridLength(130);
            }
        }
        private void MenuItem_Signal(object sender, RoutedEventArgs e)
        {
            MenuItem item = ((MenuItem)sender);
            if (item.IsChecked)
            {
                ((MenuItem)sender).IsChecked = false;
                MainWindow.r2.Height = new GridLength(20, GridUnitType.Star);
                MainWindow.r4.Height = new GridLength(0.5, GridUnitType.Star);
            }
            else
            {
                ((MenuItem)sender).IsChecked = true;
                MainWindow.r2.Height = new GridLength(13, GridUnitType.Star);
                MainWindow.r4.Height = new GridLength(7.5, GridUnitType.Star);
            }
        }
        private void MenuItem_Grid_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            if (menuItem.IsChecked)
            {
                System.Windows.Application.Current.Resources["backGround"] = new SolidColorBrush(Colors.White);
                menuItem.IsChecked = false;
            }
            else
            {
                System.Windows.Application.Current.Resources["backGround"] = new SolidColorBrush(Colors.Black);
                menuItem.IsChecked = true;
            }
        }
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProject chooseProject = new();
            chooseProject.ShowDialog();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void Validator_Click(object sender, RoutedEventArgs e)
        {
            Validator validator = new();
            validator.ShowDialog();
        }
    }
}
