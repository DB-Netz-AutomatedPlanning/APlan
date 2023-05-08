using APLan.Commands;
using APLan.Converters;
using APLan.HelperClasses;
using APLan.Model.CADlogic;
using APLan.ViewModels;
using Models.TopoModels.EULYNX.rsmCommon;
using NPOI.OpenXmlFormats.Spreadsheet;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Controls;
using Canvas = System.Windows.Controls.Canvas;

namespace APLan.Model.CustomObjects
{
    public class CustomPoint : CustomItem
    {
        public string PointType { get; set; }
        private Point _point;
        private double _radius;
        private SolidColorBrush _color;
        public Point Point
        {
            get => _point;
            set
            {
                _point = value;
                OnPropertyChanged();
            }

        }
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged();
            }

        }
        public SolidColorBrush Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }

        }
        private ICommand _PointMouseEnter;
        public ICommand PointMouseEnter
        {
            get
            {
                return _PointMouseEnter ??= new RelayCommand(
                   x =>
                   {
                       ExecutePointMouseEnter((MouseEventArgs)x);
                   });
            }
        }
        private ICommand _PointMouseLeave;
        public ICommand PointMouseLeave
        {
            get
            {
                return _PointMouseLeave ??= new RelayCommand(
                   x =>
                   {
                       ExecutePointMouseLeave((MouseEventArgs)x);
                   });
            }
        }

        private ICommand _PointMouseDown;
        public ICommand PointMouseDown
        {
            get
            {
                return _PointMouseDown ??= new RelayCommand(
                   x =>
                   {
                       ExecutePointMouseDown((MouseEventArgs)x);
                   });
            }
        }
        public CustomPoint()
        {
            Visibility = Visibility.Collapsed;
            Radius = 1.5;             
            Color = Brushes.Red;
            VisualizedDataViewModel.StaticPropertyPointVisibilityChanged += PropertiesChange;
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        private void PropertiesChange(object sender, PropertyChangedEventArgs e)
        {
            if (PointType != null && e.PropertyName.Equals("GleisKantenPointsVisibility") && PointType.Equals(nameof(dataModelsTypes.GleisKantenPoints)))
            {
                Visibility = VisualizedDataViewModel.GleisKantenPointsVisibility;
            }
            if (PointType != null && e.PropertyName.Equals("Entwurfselement_LA_PointsVisibility") && PointType.Equals(nameof(dataModelsTypes.Entwurfselement_LA_Points)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_LA_PointsVisibility;
            }
            if (PointType != null && e.PropertyName.Equals("Entwurfselement_KM_PointsVisibility") && PointType.Equals(nameof(dataModelsTypes.Entwurfselement_KM_Points)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_KM_PointsVisibility;
            }
            if (PointType != null && e.PropertyName.Equals("Entwurfselement_HO_PointsVisibility") && PointType.Equals(nameof(dataModelsTypes.Entwurfselement_HO_Points)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_HO_PointsVisibility;
            }
            if (PointType != null && e.PropertyName.Equals("Entwurfselement_UH_PointsVisibility") && PointType.Equals(nameof(dataModelsTypes.Entwurfselement_UH_Points)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_UH_PointsVisibility;
            }
        }
        private void ExecutePointMouseEnter(MouseEventArgs e)
        {
            Radius = 5;
            Color = Brushes.Blue;
            ContentPresenter contentPresenter = VisualTreeHelpers.FindAncestor<ContentPresenter>((Path)e.Source);
            Canvas canvasRoot = VisualTreeHelpers.FindAncestor<Canvas>(contentPresenter);
            ScrollViewer myScrollViewer = VisualTreeHelpers.FindAncestor<ScrollViewer>(canvasRoot);

            // get the current zoom level of the Canvas
            double currentZoom = DrawViewModel.CanvasScale;

            var x = Canvas.GetLeft(contentPresenter) * currentZoom;
            var y = Canvas.GetTop(contentPresenter) * currentZoom;


            // get the position of the Canvas relative to the ScrollViewer
            GeneralTransform transform = Transform.Identity;
            Point positionInScrollViewer = transform.Transform(new(x, y));

            // subtract the ScrollViewer's scroll position to get the position of the point within the ScrollViewer's viewport
            Point positionInViewPort = new Point(positionInScrollViewer.X - myScrollViewer.HorizontalOffset, positionInScrollViewer.Y - myScrollViewer.VerticalOffset);

            // now positionInViewPort contains the translated point
            Point screenPoint = myScrollViewer.PointToScreen(positionInViewPort);

            //scroll to the point on screen.
            SetCursorPos((int)screenPoint.X, (int)screenPoint.Y);
        }
        private void ExecutePointMouseLeave(MouseEventArgs e)
        {
            Radius = 1.5;
            Color = Brushes.Red;
        }
        private void ExecutePointMouseDown(MouseEventArgs e)
        {
            //DrawLogic.clickedPoints[DrawLogic.clickedPoints.Count - 1] = this.Point;
            //var point = ((CustomPoint)((Path)e.Source).DataContext).Point;
            //if (DrawViewModel.toolCAD.Equals(DrawViewModel.SelectedToolForCAD.TwoPointsLine))
            //{
            //    if (DrawViewModel.InitialLinePoint.X!=-1)
            //    {

            //    }
            //    var converter = new CoodrinatesSinglePointConverter();
            //    DrawViewModel.InitialLinePoint = (Point)converter.Convert(point,null,null,null);
            //}

            //Radius = 1.5;
            //Color = Brushes.Green;
        }
    }
}
