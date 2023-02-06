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
            //else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomRectangle))
            //{
            //    var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomRectangle;
            //    dataContext.Color = themeColor;
            //    Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
            //    newPoint = (Point)converter.Convert(dataContext.Points[0], null, null, null);
            //}
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomCircle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomCircle;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.EllipseVertexCenter, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomEllipse))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomEllipse;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.EllipseVertexCenter, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomArc))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomArc;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.StartPoint, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomTextBlock))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomTextBlock;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.NodePoint, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomImage))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomImage;
                
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                Point recPoint = new Point(dataContext.SetLeft, dataContext.SetTop);
                newPoint = (Point)converter.Convert(recPoint, null, null, null);
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
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomRectangle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomRectangle;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomCircle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomCircle;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomArc))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomArc;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomTextBlock))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomTextBlock;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(HelperClasses.CustomImage))
            {
                var dataContext = ((ListViewItem)sender).DataContext as HelperClasses.CustomImage;
                 
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
