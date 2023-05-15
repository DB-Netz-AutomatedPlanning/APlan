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
using APLan.Model.CustomObjects;
using APLan.Converters;
using System.ComponentModel;

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
            else if(((ListViewItem)sender).DataContext.GetType() == typeof(ERDM.Tier_1.TrackNode))
            {
                var dataContext = ((ListViewItem)sender).DataContext as ERDM.Tier_1.TrackNode;
                APLan.Model.Converters.GetGeoCoordinatesConverter converter = new APLan.Model.Converters.GetGeoCoordinatesConverter();

               
               newPoint = (Point)converter.Convert(dataContext.isLocatedAtGeoCoordinates, null, null, null);
            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(ERDM.Tier_1.TrackEdge))
            {
                var dataContext = ((ListViewItem)sender).DataContext as ERDM.Tier_1.TrackEdge;
                APLan.Model.Converters.GetTrackNodeFromTrackEdgeAttributeConverter converter = new APLan.Model.Converters.GetTrackNodeFromTrackEdgeAttributeConverter();


                newPoint = (Point)converter.Convert(dataContext.hasStartTrackNode, null, null, null);
            }


            scrollToTarget(newPoint);

            //((TextBox)sender).BorderBrush = Brushes.Red;
            //ListViewItem listViewItem =VisualTreeHelpers.FindAncestor<ListViewItem>(((TextBox)sender)) as ListViewItem;
            //listViewItem.Focus();
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
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(ERDM.Tier_1.TrackNode))
            {
                var dataContext = ((ListViewItem)sender).DataContext as ERDM.Tier_1.TrackNode;

            }
            else if (((ListViewItem)sender).DataContext.GetType() == typeof(ERDM.Tier_1.TrackEdge))
            {
                var dataContext = ((ListViewItem)sender).DataContext as ERDM.Tier_1.TrackEdge;

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
