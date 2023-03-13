using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AplanCADViewerSideBar.xaml
    /// </summary>
    public partial class AplanCADViewerSideBar : UserControl
    {
        public AplanCADViewerSideBar()
        {
            InitializeComponent();
        }

        

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if(Tg_Btn.IsChecked == false)
            {
                popup_uc.PlacementTarget = Line;
                popup_uc.Placement = PlacementMode.Right;
                popup_uc.IsOpen = true;
                Header.PopupText.Text = "Line";
            }
            
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_uc.Visibility = Visibility.Collapsed;
            popup_uc.IsOpen = false;
        }

        private void Circle_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_uc.Visibility = Visibility.Collapsed;
            popup_uc.IsOpen = false;
           
        }

        private void Circle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                popup_uc.PlacementTarget = Circle;
                popup_uc.Placement = PlacementMode.Right;
                popup_uc.IsOpen = true;
                Header.PopupText.Text = "Circle";
            }
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                popup_uc.PlacementTarget = Ellipse;
                popup_uc.Placement = PlacementMode.Right;
                popup_uc.IsOpen = true;
                Header.PopupText.Text = "Ellipse";
            }
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_uc.Visibility = Visibility.Collapsed;
            popup_uc.IsOpen = false;
        }

        private void Polyline_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_uc.Visibility = Visibility.Collapsed;
            popup_uc.IsOpen = false;           
        }

        private void Polyline_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                popup_uc.PlacementTarget = Polyline;
                popup_uc.Placement = PlacementMode.Right;
                popup_uc.IsOpen = true;
                Header.PopupText.Text = "Polyline";
            }
        }

        private void Curve_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                popup_uc.PlacementTarget = Curve;
                popup_uc.Placement = PlacementMode.Right;
                popup_uc.IsOpen = true;
                Header.PopupText.Text = "Curve";
            }
        }

        private void Curve_MouseLeave(object sender, MouseEventArgs e)
        {
            popup_uc.Visibility = Visibility.Collapsed;
            popup_uc.IsOpen = false;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
