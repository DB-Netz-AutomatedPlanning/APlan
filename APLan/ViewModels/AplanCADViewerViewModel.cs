using APLan.Commands;
using System.Windows;
using System.Windows.Input;
using System;

namespace APLan.ViewModels
{
    public class AplanCADViewerViewModel : BaseViewModel 
    {
        #region attributes  
        private DrawViewModel drawViewModel;
        private Visibility aplanCadToolViewVisibility;
        private Visibility parallelLineContentVisibility;
        private Visibility angularLineContentVisibility;
        #endregion

        #region properties
        public Visibility AplanCadToolViewVisibility
        {
            get => aplanCadToolViewVisibility;
            set
            {
                aplanCadToolViewVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility ParallelLineContentVisibility
        {
            get => parallelLineContentVisibility;
            set
            {
                parallelLineContentVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility AngularLineContentVisibility
        {
            get => angularLineContentVisibility;
            set
            {
                angularLineContentVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand LineDrawing2Points { get; set; }
        public ICommand ParallelLineDrawing { get; set; }
        public ICommand HorizontalLineDrawing { get; set; }
        public ICommand AngularDrawing { get; set; }
        public ICommand VerticalLineDrawing { get; set; }
        public ICommand CircleDrawing { get; set; }
        public ICommand EllipseDrawing { get; set; }
        public ICommand PolylineDrawing { get; set; }
        public ICommand ArcDrawingTwoPointCenter { get; set; } 
        public ICommand threePointCurve { get; set; }
        #endregion

        #region constructor
        public AplanCADViewerViewModel()
        {
            
            AplanCadToolViewVisibility = Visibility.Collapsed;
           
            LineDrawing2Points = new RelayCommand(ExecuteLineDrawing2Points);
            HorizontalLineDrawing = new RelayCommand(ExecuteHorizontalLineDrawing);
            VerticalLineDrawing = new RelayCommand(ExecuteVerticalLineDrawing);
            ParallelLineDrawing = new RelayCommand(ExecuteParallelLineDrawing);
            CircleDrawing = new RelayCommand(ExecuteCircleDrawing);
            EllipseDrawing = new RelayCommand(ExecuteEllipseDrawing);
            PolylineDrawing = new RelayCommand(ExecutePolylineDrawing);
            ArcDrawingTwoPointCenter = new RelayCommand(ExecuteArcDrawing2Points);
            threePointCurve = new RelayCommand(ExecutethreePointCurve);
            AngularDrawing = new RelayCommand(ExecuteAngularDrawing);
            drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;


        }
        #endregion

        #region logic
        private void ExecuteAngularDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.AngularLine)
            {
                drawViewModel.Instructions = "Select a point in line and angle";
                ParallelLineContentVisibility = Visibility.Collapsed;
                AngularLineContentVisibility = Visibility.Visible;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.AngularLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                AplanCadToolViewVisibility = Visibility.Visible;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.AngularLine)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                AplanCadToolViewVisibility = Visibility.Collapsed;
            }
        }
        private void ExecutethreePointCurve(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.ThreePointArc)
            {
                drawViewModel.Instructions = "Select a start point for curve";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.ThreePointArc;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.ThreePointArc)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteVerticalLineDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.VerticalLine)
            {
                drawViewModel.Instructions = "Select a Start Point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.VerticalLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.VerticalLine)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteArcDrawing2Points(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.TwoPointArc)
            {
                drawViewModel.Instructions = "Select a center Point for arc";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.TwoPointArc;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.TwoPointArc)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }     
        private void ExecutePolylineDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Polyline)
            {
                drawViewModel.Instructions = "Drag the points";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Polyline;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Polyline)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteEllipseDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Ellipse)
            {
                drawViewModel.Instructions = "Select a center point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Ellipse;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Ellipse)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteCircleDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Circle)
            {
                drawViewModel.Instructions = "Select a center point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Circle;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Circle)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }


        }
        private void ExecuteLineDrawing2Points(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.TwoPointsLine)
            {
                 
                drawViewModel.Instructions = "Select StartPoint";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.TwoPointsLine;
               
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.TwoPointsLine)
            {
                 
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;
                
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
            
           
        }
        private void ExecuteParallelLineDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.ParallelLine)
            {                
                drawViewModel.Instructions = "Select a line and distance";
                ParallelLineContentVisibility = Visibility.Visible;
                AngularLineContentVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.ParallelLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                AplanCadToolViewVisibility = Visibility.Visible;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.ParallelLine)
            {
                
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                AplanCadToolViewVisibility = Visibility.Collapsed;
            }

        }
        private void ExecuteHorizontalLineDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.HorizontalLine)
            {
                drawViewModel.Instructions = "Select StartPoint";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.HorizontalLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.HorizontalLine)
            {
                drawViewModel.Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        #endregion

    }
}
