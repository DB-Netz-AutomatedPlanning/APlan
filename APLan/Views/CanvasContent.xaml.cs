using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;

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
        private new void GotFocus(object sender, RoutedEventArgs e)
        {

            Point newPoint;
            if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomPolyLine))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomPolyLine;
                dataContext.Color = themeColor;
                
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.CustomPoints[0].Point, null, null, null);
            }else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomCircle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomCircle;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.Center.Point, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomCircle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomCircle;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.Center, null, null, null);
            } 
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomArc))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomArc;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.StartPoint, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomTextBlock))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomTextBlock;
                dataContext.Color = themeColor;
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                newPoint = (Point)converter.Convert(dataContext.NodePoint, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomImage))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomImage;
                
                Converters.CoodrinatesSinglePointConverter converter = new Converters.CoodrinatesSinglePointConverter();
                Point recPoint = new Point(dataContext.SetLeft, dataContext.SetTop);
                newPoint = (Point)converter.Convert(recPoint, null, null, null);
            }

            scrollToTarget(newPoint);
        }
        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomPolyLine))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomPolyLine;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomNode))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomNode;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomRectangle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomRectangle;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomCircle))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomCircle;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomArc))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomArc;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomTextBlock))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomTextBlock;
                dataContext.Color = Brushes.Red;
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(CustomImage))
            {
                var dataContext = ((ListViewItem)sender).DataContext as CustomImage;
                 
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
