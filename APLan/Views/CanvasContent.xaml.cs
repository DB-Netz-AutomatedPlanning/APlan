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
using APLan.HelperClasses;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for CanvasContent.xaml
    /// </summary>
    public partial class CanvasContent : UserControl
    {
        private SolidColorBrush themeColor;
        public CanvasContent()
        {
            InitializeComponent();
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }
        private void GotFocus(object sender, RoutedEventArgs e)
        {

            Point newPoint;
            if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomPolyLine))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomPolyLine;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.Points[0], null, null, null);
            }else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomNode))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomNode;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.NodePoint, null, null, null);
            }
           
            scrollToTarget(newPoint);

            //((TextBox)sender).BorderBrush = Brushes.Red;
            //ListViewItem listViewItem =HelperClasses.VisualTreeHelpers.FindAncestor<ListViewItem>(((TextBox)sender)) as ListViewItem;
            //listViewItem.Focus();
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {


            if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomPolyLine))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomPolyLine;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomNode))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomNode;
                dataContext.Color = Brushes.Red;
            }
        }
        private void scrollToTarget(Point newPoint)
        {
            var element = Views.Draw.drawing;
            var scroll = VisualTreeHelpers.FindAncestor<ScrollViewer>(element);

            TransformGroup transformGroup = (TransformGroup)element.LayoutTransform;
            var Scaletransform = (ScaleTransform)transformGroup.Children[0];

            scroll.ScrollToHorizontalOffset(newPoint.X * Scaletransform.ScaleX - scroll.ActualWidth / 2);
            scroll.ScrollToVerticalOffset(newPoint.Y * Scaletransform.ScaleY - scroll.ActualHeight / 2);
        }
    }
}
