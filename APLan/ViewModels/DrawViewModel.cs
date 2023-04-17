using APLan.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using APLan.HelperClasses;
using APLan.Model.CADlogic;
using APLan.Model.CustomObjects;
using APLan.Converters;

namespace APLan.ViewModels
{
    public class DrawViewModel : BaseViewModel
    {
        #region enum
        public enum SelectedTool
        {
            None,
            MultiSelect,
            Drag,
            Move,
            Rotate,
            Scale
        }
        public enum SelectedToolForCAD
        {
            None,
            TwoPointsLine,
            HorizontalLine,
            AngularLine,
            VerticalLine,
            ParallelLine,
            Circle,
            Ellipse,
            Polyline,
            TwoPointArc,
            ThreePointArc
        }
        #endregion
         
        #region attributes
        //canvas data
        private DrawLogic drawLogic;
        private double canvasRotation;
        private double xdiff;
        private double ydiff;
        private Point OldPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
        private SolidColorBrush gridColor;
        private double _rotateItems = 0;
        private static double canvasScale = 1;
        private double gridThicnkess = 0.5;
        private double lineThicnkess = 2;
        private double canvasSize = 1000000;
        private double signalSize;
        public static double sharedCanvasSize = 1000000; //this should be always equal canvasSize.
        public static double drawingScale = 1;
        public static double signalSizeForConverter;
        public static Point GlobalDrawingPoint = new Point(0, 0);
        public static Line indicationLine;
        private Ellipse indicationEllipse;
        private Path indicationArcPath;
        private PathGeometry indicationPathGeometry;
        private PathFigure indicationPathFigure;
        private ArcSegment indicationArcSegment;
        //for mouse location.
        private System.Windows.Point pointer;
        //mouse location info.
        private string Instruction = String.Empty;
        //multiselection rectangle info.
        public static Rectangle selectionRectangle = null;
        private static System.Windows.Point firsPoint;
        private static Point InitialMovePoint;
        private static Point InitialDragPoint;
        private static Point MoveOffest;
        private static Point DraOffset;

        public static Point InitialLinePoint;
        bool captureLine;
        bool captureEllipseCenterPoint;
         
        public ArrayList Multiselected;
        public ArrayList tempSelected;

        public static SelectedTool tool;
        public static SelectedToolForCAD toolCAD;

        private double distanceForParallelLine;
        private double angleLine;
        private Point centerPoint;
        private double height;
        private double width;
        private int twoPointEllipse;
        private CustomPolyLine polyineDrawing;
        private int previousSignificantPoint;
        private Point StartPointForArc;
        private CustomBezierCurve curve;

        private  Color selectedColorForACAD;
        private RectangleGeometry recGeometry;

        #endregion
        
        #region properties
        public double CanvasRotation
        {
            get => canvasRotation;
            set {
                canvasRotation = value;
                OnPropertyChanged();
            }
            
        }
        public double CanvasSize
        {
            get => canvasSize;
        }
        public static double CanvasScale
        {
            get => canvasScale;
            set => canvasScale = canvasScale * value;
        }
        public double GridThicnkess
        {
            get
            {
                if (gridThicnkess <= 0.5)
                {
                    return gridThicnkess;
                }
                return 0.5;
                // return gridThicnkess;
            }
            set
            {
                gridThicnkess = (1 / value) * 0.5;
                OnPropertyChanged("GridThicnkess");
            }
        }
        public double LineThicnkess
        {
            get
            {
                if (lineThicnkess <= 2)
                {
                    return lineThicnkess;
                }
                return 2;
            }

            set
            {

                lineThicnkess = (1 / value) * 2;
                OnPropertyChanged("LineThicnkess");
            }
        }
        public double RotateTextBox
        {
            get => _rotateItems;
            set
            {
                _rotateItems = value;
                OnPropertyChanged("RotateTextBox");
            }
        }
        public double ScaleTextBox
        {
            get => _rotateItems;
            set
            {
                _rotateItems = value;
                OnPropertyChanged("RotateTextBox");
            }
        }
        public double SignalSize
        {
            get => signalSize; set
            {
                signalSize = value;
                signalSizeForConverter = value;
            }
        }
        public string Instructions
        {
            get => Instruction;
            set
            {
                if(Instruction != value)
                {
                    Instruction = value;
                    OnPropertyChanged();
                }
            }
        }
        public SolidColorBrush GridColor
        {
            get => gridColor;
            set
            {
                gridColor = value;
                OnPropertyChanged();
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
        public  Color SelectedColorForACAD
        {
            get => selectedColorForACAD;
            set
            {
                selectedColorForACAD = value;
                OnPropertyChanged("SelectedColorForACAD");
            }
        }
        public RectangleGeometry RecGeometry
        {
            get => recGeometry;
            set
            {
                recGeometry = value;
                OnPropertyChanged("RecGeometry");
            }
        }

        #endregion

        #region constructor
        public DrawViewModel()
        {
            drawLogic = new();
            //store and load.
            toBeStored = new List<UIElement>();
            

            selected = new ObservableCollection<UIElement>();
            Multiselected = new ArrayList();
            tempSelected = new ArrayList();

            tool = SelectedTool.None;
            toolCAD = SelectedToolForCAD.None;
            pointer = new Point(double.PositiveInfinity, double.PositiveInfinity);
            firsPoint = new Point(0, 0);
           
            InitialMovePoint = new Point(-1, -1);
            InitialDragPoint = new Point(-1, -1);
            MoveOffest = new Point(-1, -1);
            DraOffset = new Point(-1, -1);

            InitialLinePoint = new Point(-1, -1);
            captureLine = false;
            captureEllipseCenterPoint = false;
            centerPoint = new Point(-1, -1);
            



            SignalSize = 10;

            RotateSelectionButton = new RelayCommand(ExecuteRotateSelectionButton);
            
            GridColorActivation = new RelayCommand(ExecuteGridColorActivation);
            GridColor = Brushes.Gray;

            distanceForParallelLine = 20d;
            AngleForAngularLine = 45d;
            height = 0d;
            width = 0d;
            twoPointEllipse = 0;
            previousSignificantPoint = 0;

            StartPointForArc = new Point(-1, -1);

            UndoStack = new Stack<object>();
            RedoStack = new Stack<object>();
            SelectedColorForACAD = Colors.Black;

            RecGeometry = new RectangleGeometry();
        }
        #endregion

        #region commands

        private ICommand _MouseleftButtonDownCommand;
        private ICommand _MouserightButtonDownCommand;
        private ICommand _MouseMiddleDownCommand;
        private ICommand _MouseWheelCommand;
        private ICommand _DrawingMouseMoveCommand;
        private ICommand _BasCanvasMouseMoveCommand;
  

        private ICommand _ObjectLodaded;

        public ICommand GridColorActivation { get; set; }
        private ICommand _RotateCanvasSlider { get; set; }
        private ICommand _RotateItemSlider { get; set; }
        private ICommand _ScaleItemSlider { get; set; }
        public ICommand RotateSelectionButton { get; set; }
        


        public ICommand LeftMouseButtonDown
        {
            get
            {
                return _MouseleftButtonDownCommand ??= new RelayCommand(
                   x =>
                   {
                       ExecuteMouseLeftButtonDownDrawingCanvas((MouseEventArgs)x);
                   });
            }
        }
        public ICommand MouseMiddleDownCommand
        {
            get
            {
                return _MouseMiddleDownCommand ??= new RelayCommand(
                   x =>
                   {
                       MessageBox.Show("MiddelPressed");
                   });
            }
        }
        public ICommand RightMouseButtonDown
        {
            get
            {
                return _MouserightButtonDownCommand ??= new RelayCommand(
                   x =>
                   {
                       MessageBox.Show("RightPressed");
                   });
            }
        }
        public ICommand MouseWheelCommand
        {
            get
            {
                return _MouseWheelCommand ??= new RelayCommand(
                   x =>
                   {
                       ExecuteMouseWheelDrawingCanvas((MouseWheelEventArgs)x);
                   });
            }
        }
        public ICommand DrawingMouseMoveCommand
        {
            get
            {
                return _DrawingMouseMoveCommand ??= new RelayCommand(
                   x =>
                   {
                       ExecuteMouseMoveDrawingCanvas((MouseEventArgs)x);
                   });
            }
        }
        public ICommand BasCanvasMouseMoveCommand
        {
            get
            {
                return _BasCanvasMouseMoveCommand ??= new RelayCommand(
                   x =>
                   {
                       ExecuteMouseMoveBaseCanvas((MouseEventArgs) x);
                   });
            }
        }
        public ICommand ObjectLoaded
        {
            get
            {
                return _ObjectLodaded ??= new RelayCommand(
                   x =>
                   {
                       ExecuteLoadObjects((UIElement) x);
                   });
            }
        }
        public ICommand RotateCanvasSlider
        {
            get
            {
                return _RotateCanvasSlider ??= new RelayCommand(
                   x =>
                   {
                       ExecuteRotateCanvasSliderChange((RoutedPropertyChangedEventArgs<double>)x);
                   });
            }
        }
        public ICommand RotateItemSlider
        {
            get
            {
                return _RotateItemSlider ??= new RelayCommand(
                   x =>
                   {
                       ExecuteRotateItemSliderChange((RoutedPropertyChangedEventArgs<double>)x);
                   });
            }
        }
        public ICommand ScaleItemSlider
        {
            get
            {
                return _ScaleItemSlider ??= new RelayCommand(
                   x =>
                   {
                       ExecuteScaleItemSliderChange((RoutedPropertyChangedEventArgs<double>)x);
                   });
            }
        }


        #endregion

        #region mouse events logic
        /// <summary>
        /// logic whenever the mouse is moving. applied on the base canvas for hitTesting.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteMouseMoveBaseCanvas(MouseEventArgs e)
        {
            if (tool==SelectedTool.MultiSelect)
            {
                multiselectAlgo(e); // apply multiselection while the mouse if moving if selection is allowed.
            } 
        }

        /// <summary>
        /// logic applied to the drawing canvas.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteMouseMoveDrawingCanvas(MouseEventArgs e)
        {
            //get necessary elements for interaction
            Canvas element;
            if (e.Source is not Canvas)
            {
               element = VisualTreeHelpers.FindAncestor<Canvas>((UIElement)e.Source);
            }
            else
            {
                element = e.Source as Canvas;
            }
            
            var scroll  = VisualTreeHelpers.FindAncestor<ScrollViewer>(element);

            //update the mouse coordinates.

            DrawingLowerTabViewModel.Xlocation = (((e.GetPosition(element).X  - canvasSize / 2)+ GlobalDrawingPoint.X) * (1 / CoordinatesConverter.scaleValue)).ToString();
            DrawingLowerTabViewModel.Ylocation = (((-e.GetPosition(element).Y + canvasSize / 2) + GlobalDrawingPoint.Y) * (1 / CoordinatesConverter.scaleValue)).ToString();
     
            //DrawingLowerTabViewModel.Xlocation = (e.GetPosition(element).X).ToString();
            //DrawingLowerTabViewModel.Ylocation = (e.GetPosition(element).Y).ToString();

            if (element.Children.Contains(indicationLine))
            {
                indicationLine.X2= Mouse.GetPosition(element).X;
                indicationLine.Y2 = Mouse.GetPosition(element).Y;
            }

                                     
            if(element.Children.Contains(indicationLine) && toolCAD == SelectedToolForCAD.TwoPointArc)
            {                
                indicationLine.X2 = Mouse.GetPosition(element).X;
                indicationLine.Y2 = Mouse.GetPosition(element).Y;
            }
            else if(element.Children.Contains(indicationArcPath) && toolCAD == SelectedToolForCAD.TwoPointArc)
            {
                Point endPoint = e.GetPosition(element);       
                 
                indicationArcSegment.Point = endPoint;                               
                
            }
                        
            double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
            double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;
            if(toolCAD == SelectedToolForCAD.ThreePointArc && twoPointEllipse == 2)
            {
                curve.AnyPoint = new Point(e.GetPosition(element).X + xCoordinatesAdjust, -e.GetPosition(element).Y + yCoordinatesAdjust);
            }

            if (toolCAD == SelectedToolForCAD.Polyline)
            {
                if (polyineDrawing == null) return;
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    polyineDrawing.Color = new SolidColorBrush() { Color = SelectedColorForACAD };
                    polyineDrawing.Points.Freeze();
                    polyineDrawing.Color.Freeze();
                    Lines.Add(polyineDrawing);
                    UndoStack.Push(polyineDrawing);
                    polyineDrawing = null;
                    return;
                }

                // Get previous significant point to determine distance
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point previousPoint = polyineDrawing.CustomPoints[previousSignificantPoint].Point;
                    Point currentPoint = e.GetPosition(element);
                    currentPoint.X = currentPoint.X + xCoordinatesAdjust;
                    currentPoint.Y = -currentPoint.Y + yCoordinatesAdjust;

                    // If we have a new significant point (distance > 10) remove all intermediate points
                    if (Distance(currentPoint, previousPoint) > 10)
                    {
                        for (int i = polyineDrawing.CustomPoints.Count - 1; i > previousSignificantPoint; i--)
                            polyineDrawing.CustomPoints.RemoveAt(i);

                        // and set the new point as the latest significant point
                        previousSignificantPoint = polyineDrawing.CustomPoints.Count;
                    }
                    polyineDrawing.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Points",
                        Value = "" + currentPoint + ""

                    });

                    polyineDrawing.CustomPoints.Add(new() { Point = currentPoint });
                }
                
            }
            //dragging
            if (tool == SelectedTool.Drag && e.LeftButton == MouseButtonState.Pressed)
            {
                dragSelection(e);
            }

            if (tool == SelectedTool.MultiSelect)
            {
                multiselectAlgo(e); // apply multiselection while the mouse if moving if selection is allowed.
            }

            // canvas drag
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                var mainWindow = VisualTreeHelpers.FindAncestor<Window>(element);
                var drawingTab = VisualTreeHelpers.FindAncestor<UserControl>(element);
                RotateTransform transform=(RotateTransform)drawingTab.RenderTransform;
                var rotatedAngle = transform.Angle;
                xdiff = 0;
                ydiff = 0;

                double x = e.GetPosition(drawingTab).X;
                double y = e.GetPosition(drawingTab).Y;

                Point currentwindowPoint = new Point(x, y);

                if (OldPoint.X == double.NegativeInfinity)
                {
                    OldPoint = currentwindowPoint;
                }
                if (!currentwindowPoint.Equals(OldPoint))
                {
                    xdiff = x - OldPoint.X;
                    ydiff = y - OldPoint.Y;

                    scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - xdiff);
                    scroll.ScrollToVerticalOffset(scroll.VerticalOffset - ydiff);
                    OldPoint = currentwindowPoint;
                }

            }
            if (e.MiddleButton == MouseButtonState.Released && e.Source.GetType() == typeof(Canvas))
            {
                pointer = new Point(double.PositiveInfinity, double.PositiveInfinity);
                OldPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            }
            if (e.LeftButton == MouseButtonState.Released)
            {
                InitialDragPoint = new Point(-1, -1);
                polyineDrawing = null;
            }

            
        }

 

        /// <summary>
        /// apply the mouseWheel action on the drawing canvas.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteMouseWheelDrawingCanvas(MouseWheelEventArgs e)
        {
            Canvas element;
            if (e.Source is not Canvas)
            {
                element = VisualTreeHelpers.FindAncestor<Canvas>((UIElement)e.Source);
            }
            else
            {
                element = e.Source as Canvas;
            }
            var scroll = VisualTreeHelpers.FindAncestor<ScrollViewer>(element);

            var position = e.GetPosition(element);

            TransformGroup transformGroup = (TransformGroup)element.LayoutTransform;
            var Scaletransform=(ScaleTransform)transformGroup.Children[0];
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);
            CanvasScale = scale;
            GridThicnkess = CanvasScale;
            LineThicnkess = CanvasScale;
            if (Scaletransform.ScaleX > 0.1 || scale > 1)
            {
                Scaletransform.ScaleX *= scale;
                Scaletransform.ScaleY *= scale;
                
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                scroll.ScrollToHorizontalOffset(position.X * Scaletransform.ScaleX - e.GetPosition(scroll).X);
                scroll.ScrollToVerticalOffset(position.Y * Scaletransform.ScaleY - e.GetPosition(scroll).Y);
            }
            if (Scaletransform.ScaleX <= 0.1)
            {
                //to compensate the scrolling down.
                scroll.LineUp();
                scroll.LineUp();
                scroll.LineUp();
            }
        }
        /// <summary>
        /// apply leftMouseDown on the drawing canvas.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteMouseLeftButtonDownDrawingCanvas(MouseEventArgs e)
        {
            switch (tool)
            {
                case SelectedTool.Move:
                    moveSelection(e); //moving the selected items
                    break;
                
                default:
                    singleSelection(e); // select an item.
                    break;
            }
           
            switch (toolCAD)
            {
                case SelectedToolForCAD.TwoPointsLine:
                    drawTwoPointLine(e);
                    break;
                case SelectedToolForCAD.ParallelLine:
                    drawParallelLine(e, DistanceForParallelLine);
                    break;
                case SelectedToolForCAD.Circle:
                    drawCircle(e);
                    break;
                case SelectedToolForCAD.Ellipse:
                    drawEllipse(e);
                    break;
                case SelectedToolForCAD.Polyline:
                    drawPolyline(e);
                    break;
                case SelectedToolForCAD.TwoPointArc:
                    drawTwoPointArc(e);
                    break;
                case SelectedToolForCAD.HorizontalLine:
                    drawHorizontalLine(e);
                    break;
                case SelectedToolForCAD.VerticalLine:
                    drawVerticalLine(e);
                    break;
                case SelectedToolForCAD.ThreePointArc:
                    drawThreePointArc(e);
                    break;
                case SelectedToolForCAD.AngularLine:
                    drawAngularLine(e,AngleForAngularLine);
                    break;
                default:
                    break;
            }
        }
        #endregion
    
        #region button logic
        /// <summary>
        /// rotate the selected items from the canvas by a specific value.
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteRotateSelectionButton(object parameter)
        {
            foreach (UIElement element in selected)
            {
                if (element.GetType() != typeof(System.Windows.Shapes.Path))
                {
                    //update this part when we have different objects than image.
                    Matrix newMatrix = new Matrix();
                    newMatrix.RotatePrepend(RotateTextBox - CustomProperites.GetRotation(element));

                    Matrix oldMatrix = element.RenderTransform.Value;

                    MatrixTransform m = new MatrixTransform();

                    m.Matrix = newMatrix * oldMatrix;
                    element.RenderTransformOrigin = new Point(0.5, 0.5);
                    element.RenderTransform = m;


                    //Old rotation
                    CustomProperites.SetRotation(element, RotateTextBox);
                }
            }
        }
        /// <summary>
        /// rotate the selected items by a slider.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteRotateCanvasSliderChange(RoutedPropertyChangedEventArgs<double> e)
        {
            CanvasRotation = e.NewValue;
        }
        private void ExecuteRotateItemSliderChange(RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (UIElement element in selected)
            {
                if (element.GetType() != typeof(System.Windows.Shapes.Path) && element.GetType() != typeof(CustomSignal))
                {
                    //update this part when we have different objects than image.
                    Matrix newMatrix = new Matrix();
                    newMatrix.RotatePrepend(e.NewValue - CustomProperites.GetRotation(element));

                    Matrix oldMatrix = element.RenderTransform.Value;

                    MatrixTransform m = new MatrixTransform();

                    m.Matrix = newMatrix * oldMatrix;
                    if (element is Image)
                    {
                        element.RenderTransformOrigin = new Point(0.5, 0.5);
                    }else if (element is TextBox)
                    {
                        element.RenderTransformOrigin = new Point(0, 0);
                    }
                     

                    element.RenderTransform = m;


                    //Old rotation
                    CustomProperites.SetRotation(element, e.NewValue);
                }
                 
               
                
            }
        }
        /// <summary>
        /// scale selected items by a slider.
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteScaleItemSliderChange(RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (UIElement element in selected)
            {
                if (element.GetType() != typeof(System.Windows.Shapes.Path) && element.GetType() != typeof(CustomSignal))
                {
                    //update this part when we have different objects than image.
                    Matrix newMatrix = new Matrix();
                    newMatrix.Scale(1/CustomProperites.GetScale(element), 1/CustomProperites.GetScale(element));
                    newMatrix.Scale(e.NewValue, e.NewValue);

                    Matrix oldMatrix = element.RenderTransform.Value;

                    MatrixTransform m = new MatrixTransform();

                    m.Matrix = newMatrix * oldMatrix;
                    element.RenderTransformOrigin = new Point(0.5, 0.5);
                    element.RenderTransform = m;


                    //Old rotation
                    CustomProperites.SetScale(element, e.NewValue);
                }
            }
        }

        /// <summary>
        /// add the loaded objects to the Canvas for future manipulation.
        /// </summary>
        /// <param name="e"></param>
        public void ExecuteLoadObjects(UIElement e)
        {
            toBeStored.Add(e);
            Canvas.SetLeft(e, 0);
            Canvas.SetTop(e, 0);
            //canvas.Children.Add(e);
        }
        public void ExecuteGridColorActivation(object parameter)
        {
            var item = (MenuItem)parameter;
        }
        #endregion

        #region CAD drawing logic
        private void drawTwoPointLine(MouseEventArgs e)
        {
            drawLogic.drawTwoPointLine(e, sharedCanvasSize, GlobalDrawingPoint, Lines);
        }
        private void drawThreePointArc(MouseEventArgs e)
        {
            drawLogic.drawThreePointArc(e, sharedCanvasSize, GlobalDrawingPoint, BezierCurves);    
        }
        private void drawTwoPointArc(MouseEventArgs e)
        {
            Canvas drawingCanvas = null;
            if (e.Source.GetType() != typeof(Canvas))
            {
                drawingCanvas = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
            }
            else
            {
                drawingCanvas = (Canvas)e.Source;
            }
            if (captureEllipseCenterPoint == false && centerPoint.X == -1)
            {
                centerPoint = e.GetPosition(drawingCanvas);
                //double Radius = Math.Abs(Point.Subtract(circumPoint, centerPoint).Length);
                indicationLine = new Line();
                indicationLine.X1 = centerPoint.X;
                indicationLine.Y1 = centerPoint.Y;
                indicationLine.StrokeThickness = 2;
                indicationLine.Opacity = 0.5;
                IEnumerable<double> dashing = new double[] { 10, 10 };
                indicationLine.StrokeDashArray = new DoubleCollection(dashing);
                indicationLine.Stroke = new SolidColorBrush { Color = SelectedColorForACAD };
                captureEllipseCenterPoint = true;
                drawingCanvas.Children.Add(indicationLine);
                Instructions = "Select Start Point";
            }


            if (twoPointEllipse == 2 && (centerPoint.X != Mouse.GetPosition(drawingCanvas).X || centerPoint.Y != Mouse.GetPosition(drawingCanvas).Y) && captureEllipseCenterPoint == true && e.LeftButton == MouseButtonState.Pressed)
            {
                drawingCanvas.Children.Remove(indicationArcPath);
                Point endPoint = e.GetPosition(drawingCanvas);
                double distance = Distance(StartPointForArc, endPoint);


                //Path arc_path = new Path();
                //arc_path.Stroke = Brushes.Gray;
                //arc_path.StrokeThickness = 2;
                //PathGeometry pathGeometry = new PathGeometry();
                //PathFigure pathFigure = new PathFigure();
                //ArcSegment arcSegment = new ArcSegment();

                //pathFigure.StartPoint = StartPointForArc;
                double radius = Distance(centerPoint, StartPointForArc);
                //arcSegment.Size = new Size(radius, radius);
                //arcSegment.Point = endPoint;
                //arcSegment.SweepDirection = SweepDirection.Clockwise;

                //if (distance >= 2* radius)
                //{
                //    arcSegment.IsLargeArc = true;
                //}


                //pathFigure.Segments.Add(arcSegment);
                //pathGeometry.Figures.Add(pathFigure);
                //arc_path.Data = pathGeometry;

                //drawingCanvas.Children.Add(arc_path);
                if (NewProjectViewModel.firspoint.X == 0)
                {
                    NewProjectViewModel.firspoint.X = StartPointForArc.X;
                    NewProjectViewModel.firspoint.Y = StartPointForArc.Y;
                    DrawViewModel.GlobalDrawingPoint = NewProjectViewModel.firspoint;
                }

                double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
                double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;

                CustomArc newArc = new CustomArc();

                newArc.StartPoint = new() { Point = new Point(StartPointForArc.X + xCoordinatesAdjust, -StartPointForArc.Y + yCoordinatesAdjust) };
                newArc.EndPoint = new() { Point = new Point(endPoint.X + xCoordinatesAdjust, -endPoint.Y + yCoordinatesAdjust) };
                newArc.Radius = radius;
                newArc.Center = new() { Point = new Point(centerPoint.X + xCoordinatesAdjust, -centerPoint.Y + yCoordinatesAdjust) };
                double EndAngle = (180 / Math.PI) * Math.Acos((newArc.EndPoint.Point.X - newArc.Center.Point.X) / newArc.Radius);
                double StartAngle = (180 / Math.PI) * Math.Acos((newArc.StartPoint.Point.X - newArc.Center.Point.X) / newArc.Radius);

                //double sweep = 0.0;
                //if (EndAngle < StartAngle)
                //    sweep = (360 + EndAngle) - StartAngle;
                //else sweep = Math.Abs(EndAngle - StartAngle);
                //bool IsLargeArc = sweep >= 180;
                newArc.Normal = new netDxf.Vector3(0, 0, -1);
                SweepDirection sweepDirection = SweepDirection.Clockwise;


                newArc.Thickness = 2;
                newArc.Color = new SolidColorBrush() { Color = SelectedColorForACAD };
                newArc.Size = new Size(newArc.Radius, newArc.Radius);
                newArc.SweepDirection = sweepDirection;


                newArc.IsLargeArc = false;


                Arcs.Add(newArc);
                UndoStack.Push(newArc);


                twoPointEllipse += 1;
            }


            if (twoPointEllipse == 0)
            {
                twoPointEllipse = 1;
            }
            else if (twoPointEllipse == 1)
            {
                drawingCanvas.Children.Remove(indicationLine);
                indicationArcPath = new Path();

                indicationArcPath.Stroke = new SolidColorBrush { Color = SelectedColorForACAD };
                indicationArcPath.StrokeThickness = 2;
                indicationArcPath.Opacity = 0.5;

                indicationPathGeometry = new PathGeometry();
                indicationPathFigure = new PathFigure();
                indicationArcSegment = new ArcSegment();
                StartPointForArc = e.GetPosition(drawingCanvas);
                indicationPathFigure.StartPoint = StartPointForArc;
                double radius = Distance(centerPoint, StartPointForArc);
                indicationArcSegment.Size = new Size(radius, radius);
                indicationArcSegment.SweepDirection = SweepDirection.Clockwise;
                IEnumerable<double> dashing = new double[] { 10, 10 };
                indicationArcPath.StrokeDashArray = new DoubleCollection(dashing);


                indicationPathFigure.Segments.Add(indicationArcSegment);
                indicationPathGeometry.Figures.Add(indicationPathFigure);
                indicationArcPath.Data = indicationPathGeometry;
                //StartPointForArc = e.GetPosition(drawingCanvas);
                drawingCanvas.Children.Add(indicationArcPath);

                Instructions = "Select endPoint for arc";

                //Canvas.SetLeft(indicationArcPath, centerPoint.X - (radius));
                //Canvas.SetTop(indicationArcPath, centerPoint.Y - (radius));


                twoPointEllipse += 1;
            }
            else if (twoPointEllipse == 3)
            {
                captureEllipseCenterPoint = false;
                centerPoint = new Point(-1, -1);
                StartPointForArc = new Point(-1, -1);
                twoPointEllipse = 0;
                Instructions = "Select Center Point for Arc";

            }

        }
        private void drawPolyline(MouseEventArgs e)
        {
            drawLogic.drawPolyline(e, sharedCanvasSize, GlobalDrawingPoint, Lines);
        }
        private void drawEllipse(MouseEventArgs e)
        {
            drawLogic.drawEllipse(e, sharedCanvasSize, GlobalDrawingPoint, Ellipses);
        }
        private void drawCircle(MouseEventArgs e)
        {
             
            drawLogic.drawCircle(e, sharedCanvasSize, GlobalDrawingPoint, Ellipses);
        }
        private void drawVerticalLine(MouseEventArgs e)
        {
            drawLogic.drawVerticalLine(e,sharedCanvasSize,GlobalDrawingPoint,Lines);            
        }
        private void drawHorizontalLine(MouseEventArgs e)
        {
            drawLogic.drawHorizontalLine(e, sharedCanvasSize, GlobalDrawingPoint, Lines);
        }
        private void drawAngularLine(MouseEventArgs e, double angularDistance)
        {
            if (e.Source.GetType() != typeof(Canvas))
            {
                Canvas c = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
                Point pt = e.GetPosition(c);


                HitTestResult result = VisualTreeHelper.HitTest(c, pt);
                if (result != null && ((UIElement)result.VisualHit).IsVisible == true)
                {
                    UIElement element = (UIElement)result.VisualHit;
                    if (element.GetType() == typeof(System.Windows.Shapes.Path))
                    {
                        Path selectedPath = (Path)element;
                        PathGeometry parentGeometry = selectedPath.Data.GetFlattenedPathGeometry();
                        PathFigureCollection parentFigure = parentGeometry.Figures;



                        Point startPoint = parentFigure[0].StartPoint;
                        PathSegmentCollection pathSegmentCollection = parentFigure[0].Segments;
                        LineSegment newLineSegment = (LineSegment)pathSegmentCollection[0];


                        Point endPoint = newLineSegment.Point;
                        double length = Distance(endPoint, startPoint);


                        Point newStartPoint = e.GetPosition(element);
                        Point newEndPoint = new Point(newStartPoint.X + Math.Cos(angularDistance * (Math.PI / 180)) * length, newStartPoint.Y + Math.Sin(angularDistance * (Math.PI / 180)) * length);
                        if (NewProjectViewModel.firspoint.X == 0)
                        {
                            NewProjectViewModel.firspoint.X = newStartPoint.X;
                            NewProjectViewModel.firspoint.Y = newStartPoint.Y;
                            DrawViewModel.GlobalDrawingPoint = NewProjectViewModel.firspoint;
                        }
                        double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
                        double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;
                        CustomPolyLine newLine = new CustomPolyLine();

                        newLine.CustomPoints.Add(new() { Point = new Point(newStartPoint.X + xCoordinatesAdjust, -newStartPoint.Y + yCoordinatesAdjust) });



                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "StartPoint",
                            Value = "" + newLine.CustomPoints[0].Point + ""

                        });


                        newLine.CustomPoints.Add(new() { Point = new Point(newEndPoint.X + xCoordinatesAdjust, -newEndPoint.Y + yCoordinatesAdjust) });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "EndPoint",
                            Value = "" + newLine.CustomPoints[1].Point + ""

                        });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "Angle",
                            Value = "" + angularDistance + ""

                        });


                        newLine.Color = new SolidColorBrush() { Color = SelectedColorForACAD };
                        newLine.Color.Freeze();
                        Lines.Add(newLine);
                        UndoStack.Push(newLine);

                    }
                }
            }
        }
        private void drawParallelLine(MouseEventArgs e, double distanceForParallelLine)
        {
            if (e.Source.GetType() != typeof(Canvas))
            {
                Canvas c = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
                Point pt = e.GetPosition(c);


                HitTestResult result = VisualTreeHelper.HitTest(c, pt);
                if (result != null && ((UIElement)result.VisualHit).IsVisible == true)
                {
                    UIElement element = (UIElement)result.VisualHit;
                    if (element.GetType() == typeof(System.Windows.Shapes.Path))
                    {
                        Path selectedPath = (Path)element;
                        PathGeometry parentGeometry = selectedPath.Data.GetFlattenedPathGeometry();
                        PathFigureCollection parentFigure = parentGeometry.Figures;



                        Point startPoint = parentFigure[0].StartPoint;
                        PathSegmentCollection pathSegmentCollection = parentFigure[0].Segments;
                        LineSegment newLineSegment = (LineSegment)pathSegmentCollection[0];


                        Point endPoint = newLineSegment.Point;
                        var v = endPoint - startPoint;
                        var n = new Vector(v.Y, -v.X);
                        n.Normalize();
                        var distance = distanceForParallelLine;
                        var p3 = startPoint + n * distance;
                        var p4 = p3 + v;
                        //Line newParallelLine = new Line();
                        //newParallelLine.X1 = p3.X;
                        //newParallelLine.Y1 = p3.Y;
                        //newParallelLine.X2 = p4.X;
                        //newParallelLine.Y2 = p4.Y;
                        //newParallelLine.Stroke = SystemColors.WindowFrameBrush;
                        //toBeStored.Add(newParallelLine);
                        //c.Children.Add(newParallelLine);
                        if (NewProjectViewModel.firspoint.X == 0)
                        {
                            NewProjectViewModel.firspoint.X = p3.X;
                            NewProjectViewModel.firspoint.Y = p3.Y;
                            DrawViewModel.GlobalDrawingPoint = NewProjectViewModel.firspoint;
                        }
                        double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
                        double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;
                        CustomPolyLine newLine = new CustomPolyLine();

                        newLine.CustomPoints.Add(new() { Point = new Point(p3.X + xCoordinatesAdjust, -p3.Y + yCoordinatesAdjust) });



                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "StartPoint",
                            Value = "" + newLine.CustomPoints[0].Point + ""

                        });


                        newLine.CustomPoints.Add(new() { Point = new Point(p4.X + xCoordinatesAdjust, -p4.Y + yCoordinatesAdjust) });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "EndPoint",
                            Value = "" + newLine.CustomPoints[1].Point + ""

                        });


                        newLine.Color = new SolidColorBrush() { Color = SelectedColorForACAD };
                        newLine.Color.Freeze();
                        Lines.Add(newLine);
                        UndoStack.Push(newLine);

                    }
                }
            }

        }

        #endregion

        #region additional logic
        private static double Distance(Point pointA, Point pointB)
        {
            return Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
        }
        /// <summary>
        /// algorithm to select multiple items at once.
        /// </summary>
        /// <param name="e"></param>
        private void multiselectAlgo( MouseEventArgs e)
        {
            var element = VisualTreeHelpers.FindAncestor<Canvas>((UIElement)e.Source,"baseCanvas");
           
            
            element.Children.Remove(selectionRectangle);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                selectionRectangle = new Rectangle() { Fill = Brushes.LightSkyBlue, Opacity=0.5, IsHitTestVisible=false};
                Panel.SetZIndex(selectionRectangle,2);
                element.Children.Add(selectionRectangle);

                var mouseX = Mouse.GetPosition(element).X;
                var mouseY = Mouse.GetPosition(element).Y;

                if (firsPoint.X == 0)
                {
                    firsPoint = Mouse.GetPosition(element);
                   
                }
                else
                {
                    //var userControl = VisualTreeHelpers.FindAncestor<UserControl>(element);
                    //var userControlAngle = ((RotateTransform)userControl.RenderTransform).Angle;

                    selectionRectangle.LayoutTransform = new RotateTransform() { Angle = 0, CenterX = 0, CenterY = 0 };

                    selectionRectangle.Width = Math.Abs(mouseX - firsPoint.X);
                    selectionRectangle.Height = Math.Abs(mouseY - firsPoint.Y);

                 

                    Canvas.SetLeft(selectionRectangle, firsPoint.X);
                    Canvas.SetTop(selectionRectangle, firsPoint.Y);
                    

                    ScaleTransform s = new ScaleTransform();
                    RectangleGeometry expandedHitTestArea = new RectangleGeometry(new Rect(Canvas.GetLeft(selectionRectangle), Canvas.GetTop(selectionRectangle), selectionRectangle.Width, selectionRectangle.Height));
                    if (mouseX - firsPoint.X >= 0 && mouseY - firsPoint.Y < 0)
                    {
                        s.ScaleX = 1;
                        s.ScaleY = -1;
                        selectionRectangle.RenderTransform = s;
                        expandedHitTestArea = new RectangleGeometry(new Rect(Canvas.GetLeft(selectionRectangle), Canvas.GetTop(selectionRectangle) - selectionRectangle.Height, selectionRectangle.Width, selectionRectangle.Height));

                    }
                    else if (mouseX - firsPoint.X <= 0 && mouseY - firsPoint.Y < 0)
                    {
                        s.ScaleX = -1;
                        s.ScaleY = -1;
                        selectionRectangle.RenderTransform = s;
                        expandedHitTestArea = new RectangleGeometry(new Rect(Canvas.GetLeft(selectionRectangle) - selectionRectangle.Width, Canvas.GetTop(selectionRectangle) - selectionRectangle.Height, selectionRectangle.Width, selectionRectangle.Height));

                    }
                    else if (mouseX - firsPoint.X <= 0 && mouseY - firsPoint.Y > 0)
                    {
                        s.ScaleX = -1;
                        s.ScaleY = 1;
                        selectionRectangle.RenderTransform = s;
                        expandedHitTestArea = new RectangleGeometry(new Rect(Canvas.GetLeft(selectionRectangle) - selectionRectangle.Width, Canvas.GetTop(selectionRectangle), selectionRectangle.Width, selectionRectangle.Height));
                    }
                    RecGeometry = expandedHitTestArea;
                    multiSelectHitTest(expandedHitTestArea, element);
                }
            }
            if (e.LeftButton == MouseButtonState.Released && firsPoint.X!=0)
            {
                firsPoint = new Point(0, 0);
                foreach (UIElement ee in Multiselected)
                {
                    if (selected.Contains(ee) == false)
                    {
                        selected.Add(ee);
                        ee.Opacity = 0.5;
                    }
                }
                Multiselected.Clear();
            }
        }
        /// <summary>
        /// hit testing by a rectanlge for multiselection.
        /// </summary>
        /// <param name="expandedHitTestArea"></param>
        private void multiSelectHitTest(RectangleGeometry expandedHitTestArea, Canvas canvas)
        {
            //here the selection is UIElement we need to adabt for other objects
            foreach (UIElement e in tempSelected)
            {
                Multiselected.Remove(e);
                if (selected.Contains(e) == false)
                {
                    e.Opacity = 1;
                }
            }
           
            tempSelected.Clear();
            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(canvas,
                new HitTestFilterCallback(MyHitTestFilter),
                new HitTestResultCallback(MultiSelectionHitTestResult),
                new GeometryHitTestParameters(expandedHitTestArea));

            // Perform actions on the hit test results list.
            if (tempSelected.Count > 0)
            {
                
                ProcessMultiSelectHitTestResultsList();
            }
        }
        /// <summary>
        /// select a single item.
        /// </summary>
        /// <param name="e"></param>
        private void singleSelection(MouseEventArgs e)
        {

            if (e.Source.GetType() != typeof(Canvas))
            {
                Canvas c = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
                Point pt = e.GetPosition(c);
                // Perform the hit test against a given portion of the visual object tree.
                HitTestResult result = VisualTreeHelper.HitTest(c, pt);

                if (result==null)
                {
                    ClearSelection(selected);
                }

                if (result != null && ((UIElement)result.VisualHit).IsVisible == true)
                {
                    UIElement element = (UIElement)result.VisualHit;
                    if (tool==SelectedTool.None)
                    {
                        ClearSelection(selected);
                        addItemToSelection(selected, element);
                    }
                    else if (tool == SelectedTool.MultiSelect && !selected.Contains(element))
                    {
                        addItemToSelection(selected,element);
                    }
                    else if(tool == SelectedTool.MultiSelect && selected.Contains(element))
                    {
                        removeItemFromSelection(selected, element);
                    }
                }
            }

        }
        /// <summary>
        /// move all selected items by two clicks.
        /// </summary>
        /// <param name="e"></param>
        private void moveSelection(MouseEventArgs e)
        {
            Canvas drawingCanvas = null;
            if (e.Source.GetType() != typeof(Canvas))
            {
                drawingCanvas = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
            }
            else
            {
                drawingCanvas = (Canvas)e.Source;
            }


            if (InitialMovePoint.X == -1)
            {
                var position = Mouse.GetPosition(drawingCanvas);
                InitialMovePoint = position;
                indicationLine = new Line();
                indicationLine.X1 = position.X;
                indicationLine.Y1 = position.Y;
                indicationLine.StrokeThickness = 2;
                IEnumerable<double> dashing = new double[] { 10, 10 };
                indicationLine.StrokeDashArray = new DoubleCollection(dashing);
                indicationLine.Stroke = Brushes.Gray;
                
                drawingCanvas.Children.Add(indicationLine);
            }
            else if (InitialMovePoint.X != -1)
            {
                MoveOffest.X = Mouse.GetPosition(drawingCanvas).X - InitialMovePoint.X;
                MoveOffest.Y = Mouse.GetPosition(drawingCanvas).Y - InitialMovePoint.Y;
                InitialMovePoint = new Point(-1, -1);

                drawingCanvas.Children.Remove(indicationLine);
                updateSelectionMoveOperation(MoveOffest.X, MoveOffest.Y);
            }
        }
        /// <summary>
        /// drag all selected items.
        /// </summary>
        /// <param name="e"></param>
        private void dragSelection(MouseEventArgs e)
        {
            Canvas drawingCanvas = null;
            if (e.Source.GetType() != typeof(Canvas))
            {
                drawingCanvas = (Canvas)VisualTreeHelper.GetParent((DependencyObject)e.Source);
            }
            else
            {
                drawingCanvas = (Canvas)e.Source;
            }

            if (InitialDragPoint.X == -1)
            {
                InitialDragPoint = Mouse.GetPosition(drawingCanvas);
            }
            DraOffset.X = Mouse.GetPosition(drawingCanvas).X - InitialDragPoint.X;
            DraOffset.Y = Mouse.GetPosition(drawingCanvas).Y - InitialDragPoint.Y;
            updateSelectionMoveOperation(DraOffset.X, DraOffset.Y);
            InitialDragPoint = Mouse.GetPosition(drawingCanvas);

        }
        /// <summary>
        /// update information of objects whenever they are moved.
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        private void updateSelectionMoveOperation(double offsetX, double offsetY)
        {
            //for now it is for UIElements only wee need to update whenever other types are introduced.
            foreach (UIElement element in selected)
            {
                if (element.GetType() != typeof(System.Windows.Shapes.Path) && element.GetType() != typeof(CustomSignal))
                {
                    Matrix newMatrix = new Matrix();
                    newMatrix.Translate(offsetX, offsetY);
                    Matrix oldMatrix = element.RenderTransform.Value;
                    MatrixTransform m = new MatrixTransform();
                    m.Matrix = oldMatrix * newMatrix;
                    element.RenderTransform = m;
                }
                else if(element.GetType() == typeof(System.Windows.Shapes.Path))
                {
                    Matrix newMatrix = new Matrix();
                    newMatrix.Translate(offsetX, offsetY);
                    Matrix oldMatrix = element.RenderTransform.Value;
                    MatrixTransform m = new MatrixTransform();
                    m.Matrix = oldMatrix * newMatrix;
                    element.RenderTransform = m;
                }

            }
        }

        /// <summary>
        /// filtering the selection for a specific objects only.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public HitTestFilterBehavior MyHitTestFilter(DependencyObject o)
        {
            
            // Test for the object value you want to filter.
            if (o.GetType() == typeof(CustomCanvasText))
            {
                tempSelected.Add(o);
                // Visual object and descendants are NOT part of hit test results enumeration.
                return HitTestFilterBehavior.Continue;
            }
            else
            {
                // Visual object is part of hit test results enumeration.
                return HitTestFilterBehavior.Continue;
            }
        }
        /// <summary>
        /// logic applied on the selected items after hitTest.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public HitTestResultBehavior MultiSelectionHitTestResult(HitTestResult result)
        {
            //MessageBox.Show(result.VisualHit.GetType().ToString());
            // Test for the object value you want to filter.
            

            if (result.VisualHit.GetType() == typeof(CustomCanvasSignal) || result.VisualHit.GetType() == typeof(System.Windows.Shapes.Path))
            {
                //MessageBox.Show(result.VisualHit.GetType().ToString());
                tempSelected.Add(result.VisualHit);
                // Visual object and descendants are NOT part of hit test results enumeration.
                return HitTestResultBehavior.Continue;
            }
            else
            {
                // Visual object is part of hit test results enumeration.
                return HitTestResultBehavior.Continue;
            }
            // Set the behavior to return visuals at all z-order levels.
            //return HitTestResultBehavior.Continue;
        }
        /// <summary>
        /// sub-logic for multiselection.
        /// </summary>
        void ProcessMultiSelectHitTestResultsList()
        {
            foreach (UIElement e in tempSelected)
            {
                
                if (e.GetType() != typeof(Canvas)&& e.IsVisible==true && selected.Contains(e) == false)
                {
                    e.Opacity = 0.5;
                    Multiselected.Add(e); 
                }
            }
        }
        public void colorSelection(ObservableCollection<UIElement> selected)
        {
            foreach (UIElement selec in selected)
            {
                selec.Opacity = 0.5;
            }
        }
        public void addItemToSelection(ObservableCollection<UIElement> selected,UIElement element)
        {
            selected.Add(element);
            element.Opacity = 0.5;
        }
        public void removeItemFromSelection(ObservableCollection<UIElement> selected, UIElement element)
        {
            selected.Remove(element);
            element.Opacity = 1;
        }

        /// <summary>
        /// resent all the points that is used for drawing logic.
        /// </summary>
        public static void resetPointers()
        {
            firsPoint = new Point(0, 0);
            InitialMovePoint = new Point(-1, -1);
            InitialDragPoint = new Point(-1, -1);
            MoveOffest = new Point(-1, -1);
            DraOffset = new Point(-1, -1);
        }
        #endregion
    }
}
