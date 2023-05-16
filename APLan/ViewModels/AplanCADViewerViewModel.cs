using APLan.Commands;
using System.Windows;
using System.Windows.Input;
using System;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Xaml.Behaviors;

namespace APLan.ViewModels
{
    public class AplanCADViewerViewModel : BaseViewModel 
    {
        #region attributes  
        private DrawViewModel drawViewModel;
        private Visibility aplanCadToolViewVisibility;
        private Visibility parallelLineContentVisibility;
        private Visibility angularLineContentVisibility;
        private double distanceForParallelLine;
        private double angleLine;
        private Color selectedColorForACAD;
        private string Instruction = String.Empty;
        private static Visibility _visible;

        #endregion

        #region properties
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyPointVisibilityChanged;
        public static void RaiseStaticPropertyPointVisibilityChanged(string PropertyName)
        {
            StaticPropertyPointVisibilityChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));
        }

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
        public Color SelectedColorForACAD
        {
            get => selectedColorForACAD;
            set
            {
                selectedColorForACAD = value;
                OnPropertyChanged("SelectedColorForACAD");
            }
        }
        public double DistanceForParallelLine
        {
            get => distanceForParallelLine;
            set
            {
                distanceForParallelLine = value;
                OnPropertyChanged("DistanceForParallelLine");
            }
        }
        public double AngleForAngularLine
        {
            get => angleLine;
            set
            {
                angleLine = value;
                OnPropertyChanged("AngleForAngularLine");
            }
        }
        public string Instructions
        {
            get => Instruction;
            set
            {
                if (Instruction != value)
                {
                    Instruction = value;
                    OnPropertyChanged();
                }
            }
        }

        public static Visibility DrawingPointVisibility
        {
            get => _visible;
            set
            {                
                    _visible = value;
                    RaiseStaticPropertyPointVisibilityChanged("DrawingPointVisibility");               
            }
        }


        #endregion

        #region commands
        public ICommand LineDrawing2Points
        {
            get;
            set;             
            
        }
        public ICommand ParallelLineDrawing { get; set; }
        public ICommand HorizontalLineDrawing { get; set; }
        public ICommand AngularDrawing { get; set; }
        public ICommand VerticalLineDrawing { get; set; }
        public ICommand CircleDrawing { get; set; }
        public ICommand EllipseDrawing { get; set; }
        public ICommand PolylineDrawing { get; set; }
        public ICommand ArcDrawingTwoPointCenter { get; set; } 
        public ICommand threePointCurve { get; set; }
        public ICommand horizontalScaler { get; set; }
        public ICommand verticalScaler { get; set; }
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
            horizontalScaler = new RelayCommand(ExecuteHorizontalScaler);
            verticalScaler = new RelayCommand(ExecuteVerticalScaler);            

            DistanceForParallelLine = 20d;
            AngleForAngularLine = 45d;
            SelectedColorForACAD = Colors.Black;

            DrawingPointVisibility = Visibility.Visible;

        }
        #endregion

        #region logic
        private void ExecuteVerticalScaler(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.VerticalDistance)
            {
                Instructions = "Select two point to measure";
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.VerticalDistance;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.VerticalDistance)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteHorizontalScaler(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.HorizontalDistance)
            {
                Instructions = "Select two point to measure";                 
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.HorizontalDistance;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.HorizontalDistance)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteAngularDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.AngularLine)
            {
                Instructions = "Select a point in line and angle";
                ParallelLineContentVisibility = Visibility.Collapsed;
                AngularLineContentVisibility = Visibility.Visible;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.AngularLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                AplanCadToolViewVisibility = Visibility.Visible;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.AngularLine)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                AplanCadToolViewVisibility = Visibility.Collapsed;
            }
        }
        private void ExecutethreePointCurve(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.ThreePointArc)
            {
                Instructions = "Select a start point for curve";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.ThreePointArc;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.ThreePointArc)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }
        private void ExecuteVerticalLineDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.VerticalLine)
            {
                Instructions = "Select a Start Point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.VerticalLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.VerticalLine)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }
        private void ExecuteArcDrawing2Points(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.TwoPointArc)
            {
                Instructions = "Select a center Point for arc";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.TwoPointArc;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.TwoPointArc)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }     
        private void ExecutePolylineDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Polyline)
            {
                Instructions = "Left Mouse click to draw lines";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Polyline;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Polyline)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }
        private void ExecuteEllipseDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Ellipse)
            {
                Instructions = "Select a center point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Ellipse;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Ellipse)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }
        private void ExecuteCircleDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.Circle)
            {
                Instructions = "Select a center point";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.Circle;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.Circle)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }


        }
        private void ExecuteLineDrawing2Points(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.TwoPointsLine)
            {
                 
                Instructions = "Select StartPoint";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.TwoPointsLine;
               
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;                

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.TwoPointsLine)
            {
                 
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;
                
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
            
           
        }
        private void ExecuteParallelLineDrawing(object parameter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.ParallelLine)
            {                
                Instructions = "Select a line and distance";
                ParallelLineContentVisibility = Visibility.Visible;
                AngularLineContentVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.ParallelLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                AplanCadToolViewVisibility = Visibility.Visible;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.ParallelLine)
            {
                
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawingPointVisibility = Visibility.Collapsed;
            }

        }
        private void ExecuteHorizontalLineDrawing(object paramter)
        {
            if (DrawViewModel.toolCAD != DrawViewModel.SelectedToolForCAD.HorizontalLine)
            {
                Instructions = "Select StartPoint";
                AplanCadToolViewVisibility = Visibility.Collapsed;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.HorizontalLine;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;

            }
            else if (DrawViewModel.toolCAD == DrawViewModel.SelectedToolForCAD.HorizontalLine)
            {
                Instructions = String.Empty;
                DrawViewModel.toolCAD = DrawViewModel.SelectedToolForCAD.None;

                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                DrawingPointVisibility = Visibility.Collapsed;
            }
        }
        #endregion

    }
}
