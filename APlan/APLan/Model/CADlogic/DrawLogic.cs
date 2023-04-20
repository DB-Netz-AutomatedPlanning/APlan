using APLan.Converters;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.ViewModels;
using APLan.Views;
using GeoJSON.Net.Geometry;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static ACadSharp.Objects.MLStyle;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace APLan.Model.CADlogic
{
    public class DrawLogic
    {
        #region attributes
        public static List<Point> clickedPoints;
        public static List<Point> clickedPointOriginalCoord;
        private AplanCADViewerViewModel aplanviewModel;
        private static Canvas newCanvas;
        //indication line attribute
        private static Line indicatorLine;
        private static Ellipse indicatorEllipse;
        private static Path indicationArcPath;
        private static ArcSegment indicationArcSegment;
        #endregion


        #region constructor
        public DrawLogic()
        {
            clickedPoints = new();
            clickedPointOriginalCoord = new();
            newCanvas = new();
            //indication line 
            indicatorLine = new();
            indicatorEllipse = new();
            indicationArcPath = new();
            indicationArcSegment = new();            
            
        }
        #endregion

        #region drawlogic     
        public static T DeepCopyReflection<T>(T input)
        {
            var type = input.GetType();
            var properties = type.GetProperties();
            T clonedObj = (T)Activator.CreateInstance(type);
           
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    object value = property.GetValue(input);
                    if (value != null && value.GetType().IsClass && !value.GetType().FullName.StartsWith("System."))
                    {
                        property.SetValue(clonedObj, DeepCopyReflection(value));
                    }
                    else
                    {
                        property.SetValue(clonedObj, value);
                    }
                }
            }
            return clonedObj;
        }
        public void pasteSelected(ObservableCollection<object> clipboard, ObservableCollection<CustomPolyLine> lines,
            ObservableCollection<CustomCircle> circle, ObservableCollection<CustomArc> arc, ObservableCollection<CustomTextBlock> textblocks,
            Stack<object> Undostack)
        {
            if(clipboard.Count == 0)
            {
                MessageBox.Show("No object copied to paste");
                return;
            }
            foreach(object customShape in clipboard )
            {
                if(customShape.GetType() == typeof(CustomPolyLine))
                {
                    CustomPolyLine customPolyLine = (CustomPolyLine)customShape;
                    
                    lines.Add(customPolyLine);
                    Undostack.Push(customPolyLine);
                    
                }
                else if(customShape.GetType() == typeof(CustomCircle))
                {
                    CustomCircle customCircle = (CustomCircle)customShape;
                    circle.Add(customCircle);
                    Undostack.Push(customCircle);

                }
                else if (customShape.GetType() == typeof(CustomArc))
                {
                    CustomArc customArc = (CustomArc)customShape;
                    arc.Add(customArc);
                    Undostack.Push(customArc);

                }
                else if (customShape.GetType() == typeof(CustomTextBlock))
                {
                    CustomTextBlock customTextBlock = (CustomTextBlock)customShape;
                    textblocks.Add(customTextBlock);
                    Undostack.Push(customTextBlock);

                }

            }
        }
        public void copySelected(ObservableCollection<UIElement> selected, ObservableCollection<object> clipboard, ObservableCollection<CustomPolyLine> lines
            ,ObservableCollection<CustomCircle> circle, ObservableCollection<CustomArc> arc, ObservableCollection<CustomTextBlock> textblocks)
        {
            if(clipboard.Count > 0)
            {
                clipboard.Clear();
            }
            foreach (UIElement selectedToCopy in selected)
            {
                object dataContext = (selectedToCopy as FrameworkElement).DataContext;

                if (dataContext != null && dataContext.GetType() == typeof(CustomPolyLine))
                {
                    CustomPolyLine polyline = (CustomPolyLine)dataContext;
                    object newobject = DeepCopyReflection(polyline);

                    //CustomPolyLine customPolyLine = new CustomPolyLine();

                    //Point startpoint = polyline.CustomPoints[0].Point;
                    //Point endpoint = polyline.CustomPoints[1].Point;
                    //customPolyLine.CustomPoints.Add(new CustomPoint { Point = startpoint });
                    //customPolyLine.CustomPoints.Add(new CustomPoint { Point = endpoint });
                    //customPolyLine.ShapeAttributeInfo.Add(new KeyValue()
                    //{
                    //    Key = "StartPoint",
                    //    Value = "" + customPolyLine.CustomPoints[0].Point + ""
                    //});
                    //customPolyLine.ShapeAttributeInfo.Add(new KeyValue()
                    //{
                    //    Key = "Endpoint",
                    //    Value = "" + customPolyLine.CustomPoints[1].Point + ""

                    //});
                    //customPolyLine.Color = polyline.Color;
                    clipboard.Add(polyline);                    
                   
                }
                else if (dataContext != null && dataContext.GetType() == typeof(CustomCircle))
                {
                    CustomCircle newCircle = (CustomCircle)dataContext;
                    object newobject = DeepCopyReflection(newCircle);
                    clipboard.Add(newobject);

                }
                else if (dataContext != null && dataContext.GetType() == typeof(CustomArc))
                {
                    CustomArc newArc = (CustomArc)dataContext;
                    object newobject = DeepCopyReflection(newArc);
                    clipboard.Add(newobject);

                }
                else if (dataContext != null && dataContext.GetType() == typeof(CustomTextBlock))
                {
                    CustomTextBlock newTextBlock = (CustomTextBlock)dataContext;
                    object newobject = DeepCopyReflection(newTextBlock);
                    clipboard.Add(newobject);

                }
            }

        }
        public void drawIndicatorLine(MouseEventArgs e, Point GLobalPoint, double canvasSize,DrawViewModel.SelectedToolForCAD drawtype)
        {
            if(clickedPoints.Count >= 1)
            {    
                if(drawtype == DrawViewModel.SelectedToolForCAD.TwoPointsLine)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    indicatorLine.Visibility = Visibility.Visible;
                    indicatorLine.X2 = currentPoint.X;
                    indicatorLine.Y2 = currentPoint.Y;
                }
               else if(drawtype == DrawViewModel.SelectedToolForCAD.HorizontalLine)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    indicatorLine.Visibility = Visibility.Visible;
                    indicatorLine.X2 = currentPoint.X;
                    indicatorLine.Y2 = indicatorLine.Y1;
                }
                else if(drawtype == DrawViewModel.SelectedToolForCAD.VerticalLine)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    indicatorLine.Visibility = Visibility.Visible;
                    indicatorLine.X2 = indicatorLine.X1;
                    indicatorLine.Y2 = currentPoint.Y;
                }
                else if(drawtype == DrawViewModel.SelectedToolForCAD.Polyline)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    indicatorLine.Visibility = Visibility.Visible;
                    indicatorLine.X2 = currentPoint.X;
                    indicatorLine.Y2 = currentPoint.Y;
                }
                else if(drawtype == DrawViewModel.SelectedToolForCAD.Ellipse)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    if(clickedPoints.Count == 1)
                    {
                        double radiusX = Math.Abs(Point.Subtract(currentPoint, clickedPointOriginalCoord[0]).Length);
                        indicatorEllipse.Width = 2 * radiusX;
                        indicatorEllipse.Height = 2 * (radiusX / 3);
                        indicatorEllipse.StrokeThickness = 2;
                        IEnumerable<double> dashing = new double[] { 10, 10 };
                        indicatorEllipse.Opacity = 0.5;
                        indicatorEllipse.StrokeDashArray = new DoubleCollection(dashing);
                        indicatorEllipse.Stroke = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                        Canvas.SetLeft(indicatorEllipse, clickedPointOriginalCoord[0].X - (indicatorEllipse.Width) / 2);
                        Canvas.SetTop(indicatorEllipse, clickedPointOriginalCoord[0].Y - (indicatorEllipse.Height) / 2);

                        if (!newCanvas.Children.Contains(indicatorEllipse))
                        {
                            newCanvas.Children.Add(indicatorEllipse);
                        }
                    }
                    else if(clickedPoints.Count == 2)
                    {
                        double radiusY = Math.Abs(Point.Subtract(currentPoint, clickedPointOriginalCoord[0]).Length);
                        indicatorEllipse.Height = 2 * radiusY;
                        Canvas.SetTop(indicatorEllipse, clickedPointOriginalCoord[0].Y - (indicatorEllipse.Height) / 2);                         
                    }                   
                    
                }
                else if(drawtype == DrawViewModel.SelectedToolForCAD.Circle)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    double radius = Math.Abs(Point.Subtract(currentPoint, clickedPointOriginalCoord[0]).Length);
                    indicatorEllipse.Width = 2 * radius;
                    indicatorEllipse.Height = 2 * radius;
                    indicatorEllipse.StrokeThickness = 2;
                    IEnumerable<double> dashing = new double[] { 10, 10 };
                    indicatorEllipse.Opacity = 0.5;
                    indicatorEllipse.StrokeDashArray = new DoubleCollection(dashing);
                    indicatorEllipse.Stroke = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    Canvas.SetLeft(indicatorEllipse, clickedPointOriginalCoord[0].X - (indicatorEllipse.Width) / 2);
                    Canvas.SetTop(indicatorEllipse, clickedPointOriginalCoord[0].Y - (indicatorEllipse.Height) / 2);

                    if (!newCanvas.Children.Contains(indicatorEllipse))
                    {
                        newCanvas.Children.Add(indicatorEllipse);
                    }
                }
                else if(drawtype == DrawViewModel.SelectedToolForCAD.TwoPointArc)
                {
                    Point currentPoint = e.GetPosition(newCanvas);
                    if(clickedPoints.Count == 1)
                    {                        
                        indicatorLine.Visibility = Visibility.Visible;
                        indicatorLine.X2 = currentPoint.X;
                        indicatorLine.Y2 = currentPoint.Y;
                    }
                    else if(clickedPoints.Count == 2)
                    {
                        indicationArcPath.Visibility = Visibility.Visible;
                        indicationArcSegment.Point = currentPoint;                         
                    }
                }
            }
            
        }
         
        private static double Distance(Point pointA, Point pointB)
        {
            return Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
        }
        public void drawVerticalDistance(MouseEventArgs e, double canvasSize,Point GlobalPoint,ObservableCollection<CustomArrowLine> arrows, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select End Point of Line";
                }
                else if (clickedPoints.Count == 2)
                {
                    CustomArrowLine newarrowline = new CustomArrowLine();

                    Point StartPoint = new Point(Math.Round(clickedPoints[0].X, 2), Math.Round(clickedPoints[0].Y, 2));
                    Point EndPoint = new Point(Math.Round(clickedPoints[1].X, 2), Math.Round(clickedPoints[1].Y, 2));
                    double distanceBetweenPoints = Math.Round(Distance( StartPoint, EndPoint));
                    newarrowline.Width = 10;
                    newarrowline.Height = distanceBetweenPoints;
                    newarrowline.SetCanvasPoint = EndPoint;
                    newarrowline.DistanceValue = distanceBetweenPoints;
                    newarrowline.GeometryData = Geometry.Parse("M 5 3 7 0 L 9 3 M 7 0 L 7 10 M 5 7 L 7 10 L 9 7");
                    newarrowline.ThicknessAttrib = new Thickness(10,distanceBetweenPoints / 2,0,0);
                    arrows.Add(newarrowline);
                    UndoStack.Push(newarrowline);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();                     
                }
            }
        }
        public void drawHorizontalDistance(MouseEventArgs e, double canvasSize, Point GLobalPoint,ObservableCollection<CustomArrowLine> arrows, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GLobalPoint, canvasSize);
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select End Point of Line";                   
                }
                else if (clickedPoints.Count == 2)
                {               
                    CustomArrowLine newarrowline = new CustomArrowLine();
                    Point StartPoint = new Point(Math.Round(clickedPoints[0].X, 2), Math.Round(clickedPoints[0].Y, 2));
                    Point EndPoint = new Point(Math.Round(clickedPoints[1].X, 2), Math.Round(clickedPoints[1].Y, 2));
                    double distanceBetweenPoints = Math.Round(Distance(StartPoint, EndPoint));
                    newarrowline.Width = distanceBetweenPoints;
                    newarrowline.Height = 10;
                    newarrowline.DistanceValue = distanceBetweenPoints;
                    newarrowline.SetCanvasPoint = StartPoint;
                    newarrowline.ThicknessAttrib = new Thickness(distanceBetweenPoints / 2, 10, 0, 0);
                    newarrowline.GeometryData = Geometry.Parse("M 4 2 L 0 4 L 4 6 M 0 4 L 20 4 M 16 2 L 20 4 L 16 6");
                    arrows.Add(newarrowline);
                    UndoStack.Push(newarrowline);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                    
                }
            }
           
        }
        
        public void drawAngularLine(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_List, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                Canvas c = VisualTreeHelpers.FindAncestor<Canvas>((DependencyObject)e.OriginalSource);
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
                        PolyLineSegment newLineSegment = (PolyLineSegment)pathSegmentCollection[0];


                        PointCollection endPoints = newLineSegment.Points;
                        Point endPoint = endPoints[endPoints.Count-1];
                        double length = Distance(endPoint, startPoint);

                        Point StartPoint = e.GetPosition(element);
                        Point newStartPoint = coordianteConverter(startPoint, GlobalPoint, canvasSize);
                        Point newEndPoint = new Point(newStartPoint.X + Math.Cos(aplanviewModel.AngleForAngularLine * (Math.PI / 180)) * length, newStartPoint.Y + Math.Sin(aplanviewModel.AngleForAngularLine * (Math.PI / 180)) * length);
                        
                         
                        CustomPolyLine newLine = new CustomPolyLine();

                        newLine.CustomPoints.Add(new() { Point = newStartPoint });


                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "StartPoint",
                            Value = "" + newLine.CustomPoints[0].Point + ""

                        });

                        newLine.CustomPoints.Add(new() { Point = newEndPoint });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "EndPoint",
                            Value = "" + newLine.CustomPoints[1].Point + ""

                        });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "Angle",
                            Value = "" + aplanviewModel.AngleForAngularLine + ""

                        });


                        newLine.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                        newLine.Color.Freeze();
                        polyline_List.Add(newLine);
                        clickedPoints.Clear();
                        clickedPointOriginalCoord.Clear();
                        UndoStack.Push(newLine);
                         

                    }
                }

            }
        }
        public void drawParallelLine(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_List, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                Canvas c = VisualTreeHelpers.FindAncestor<Canvas>((DependencyObject)e.OriginalSource);
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
                        PolyLineSegment newLineSegment = (PolyLineSegment)pathSegmentCollection[0];


                        PointCollection endPoints = newLineSegment.Points;
                        var v = endPoints[endPoints.Count-1] - startPoint;
                        var n = new Vector(v.Y, -v.X);
                        n.Normalize();
                        var distance = aplanviewModel.DistanceForParallelLine;
                        var p3 = startPoint + n * distance;
                        var p4 = p3 + v;
                        Point newP3 = coordianteConverter(p3, GlobalPoint, canvasSize);
                        CustomPolyLine newLine = new CustomPolyLine();

                        newLine.CustomPoints.Add(new() { Point = newP3 });


                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "StartPoint",
                            Value = "" + newLine.CustomPoints[0].Point + ""

                        });
                        Point newP4 = coordianteConverter(p4, GlobalPoint, canvasSize);
                        newLine.CustomPoints.Add(new() { Point =  newP4 });
                        newLine.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "EndPoint",
                            Value = "" + newLine.CustomPoints[1].Point + ""

                        });


                        newLine.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                        newLine.Color.Freeze();
                        polyline_List.Add(newLine);                      
                        UndoStack.Push(newLine);
                        clickedPoints.Clear();
                        clickedPointOriginalCoord.Clear();


                    }
                }

            }
        }
        public void drawArc(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomArc> arc_List, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select Start Point for Arc";
                    addIndicationLineToCanvas(e);

                }
                else if (clickedPoints.Count == 2)
                {
                    newCanvas.Children.Remove(indicatorLine);
                    resetIndicatorLine();
                    aplanviewModel.Instructions = "Select End Point for Arc";
                    indicationArcPath = new Path();
                    indicationArcPath.Stroke = new SolidColorBrush { Color = aplanviewModel.SelectedColorForACAD };
                    indicationArcPath.StrokeThickness = 2;
                    indicationArcPath.Opacity = 0.5;

                    PathGeometry indicationPathGeometry = new PathGeometry();
                    PathFigure indicationPathFigure = new PathFigure();
                    indicationArcSegment = new ArcSegment();

                    indicationPathFigure.StartPoint = clickedPointOriginalCoord[1];
                    double radius = Distance(clickedPointOriginalCoord[0], clickedPointOriginalCoord[1]);
                    indicationArcSegment.Size = new System.Windows.Size(radius, radius);
                    indicationArcSegment.SweepDirection = SweepDirection.Clockwise;
                    IEnumerable<double> dashing = new double[] { 10, 10 };
                    indicationArcPath.StrokeDashArray = new DoubleCollection(dashing);

                    indicationPathFigure.Segments.Add(indicationArcSegment);
                    indicationPathGeometry.Figures.Add(indicationPathFigure);
                    indicationArcPath.Data = indicationPathGeometry;
                    indicationArcPath.Visibility = Visibility.Collapsed;
                    newCanvas.Children.Add(indicationArcPath);
                }
                else if(clickedPoints.Count == 3)
                {
                    CustomArc newArc = new CustomArc();
                    newArc.Center = new() { Point = clickedPoints[0] };
                    newArc.StartPoint = new() { Point = clickedPoints[1] };
                    newArc.EndPoint = new() { Point = clickedPoints[2] };
                    newArc.Radius = Distance(clickedPoints[0], clickedPoints[1]);

                    SweepDirection sweepDirection = SweepDirection.Clockwise;

                    newArc.Thickness = 2;

                    newArc.Size = new System.Windows.Size(newArc.Radius, newArc.Radius);
                    newArc.SweepDirection = sweepDirection;


                    newArc.IsLargeArc = false;
                    newArc.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Center",
                        Value = "" + newArc.Center.Point + ""
                    });
                    newArc.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "StartPoint",
                        Value = "" + newArc.StartPoint.Point + ""
                    });
                    newArc.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "EndPoint",
                        Value = "" + newArc.EndPoint.Point + ""
                    });
                    newArc.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Radius",
                        Value = "" + newArc.Radius + ""
                    });
                    newArc.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    UndoStack.Push(newArc);
                    arc_List.Add(newArc);
                     
                    clickedPoints.Clear();      
                   
                    newCanvas.Children.Remove(indicationArcPath);
                    resetIndicationArcPath();
                    clickedPointOriginalCoord.Clear();
                }
               

            }
        }
        public void drawPolyline(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_List, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                aplanviewModel.Instructions = "Right Mouse Click to Stop line";
                if(clickedPoints.Count == 1)
                {
                    addIndicationLineToCanvas(e);
                }
                else
                {
                    indicatorLine.X1 = e.GetPosition(newCanvas).X;
                    indicatorLine.Y1 = e.GetPosition(newCanvas).Y;
                }
               
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);
                CustomPolyLine line = new() { CustomPoints = new(), Color = Brushes.Black };
                clickedPoints.ForEach(x => line.CustomPoints.Add(new() { Point = x }));
                clickedPoints.ForEach(x => line.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Points",
                    Value = "" + x + ""

                }));
                line.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                polyline_List.Add(line);
                UndoStack.Push(line);
                newCanvas.Children.Remove(indicatorLine);
                clickedPoints.Clear();
                clickedPointOriginalCoord.Clear();
                resetIndicatorLine();
            }
        }
        public void drawHorizontalLine(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_list, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e,GlobalPoint, canvasSize);
                if(clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select End Point of Line";
                    addIndicationLineToCanvas(e);
                }
                else if (clickedPoints.Count == 2)
                {                   
                    CustomPolyLine line = new() { CustomPoints = new(), Color = Brushes.Black };
                    clickedPoints.ForEach(x => line.CustomPoints.Add(new() { Point = x }));
                    line.CustomPoints[1].Point = new Point(clickedPoints[1].X, clickedPoints[0].Y);
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "StartPoint",
                        Value = "" + line.CustomPoints[0].Point + ""

                    });
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "EndPoint",
                        Value = "" + line.CustomPoints[1].Point + ""
                    });

                    line.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    polyline_list.Add(line);
                    UndoStack.Push(line);
                    newCanvas.Children.Remove(indicatorLine);                    
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                    resetIndicatorLine();
                }
            }
        }
        public void drawVerticalLine(MouseEventArgs e, double canvasSize,Point GLobalPoint, ObservableCollection<CustomPolyLine> polyline_List, Stack<object> UndoStack)
        {            

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GLobalPoint, canvasSize);
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select End Point of Line";
                    addIndicationLineToCanvas(e);
                }
                else if (clickedPoints.Count == 2)
                {
                    CustomPolyLine line = new() { CustomPoints = new(), Color = Brushes.Black };                               
                    clickedPoints.ForEach(x => line.CustomPoints.Add(new() { Point = x }));
                    line.CustomPoints[1].Point = new Point(clickedPoints[0].X, clickedPoints[1].Y);
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "StartPoint",
                        Value = "" + line.CustomPoints[0].Point + ""

                    });                     
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "EndPoint",
                        Value = "" + line.CustomPoints[1].Point + ""

                    });

                    line.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    polyline_List.Add(line);
                    UndoStack.Push(line);
                    newCanvas.Children.Remove(indicatorLine);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                    resetIndicatorLine();
                }
            }


        }
        public void drawThreePointArc(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomBezierCurve> Bezier_Curve_List, Stack<object> UndoStack)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select End Point for Arc";
                }
                else if (clickedPoints.Count == 2)
                {
                    aplanviewModel.Instructions = "Select Third Point for Arc";
                }
                else if (clickedPoints.Count == 3)
                {
                    CustomBezierCurve curve = new() { CustomPoints = new(), Color = Brushes.Black };
                    clickedPoints.ForEach(x => curve.CustomPoints.Add(new() { Point = x }));
                    curve.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    curve.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "StartPoint",
                        Value = "" + curve.CustomPoints[0].Point + ""

                    });
                    curve.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "EndPoint",
                        Value = "" + curve.CustomPoints[1].Point + ""

                    });
                    curve.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "AmyPoint",
                        Value = "" + curve.CustomPoints[2].Point + ""

                    });
                    Bezier_Curve_List.Add(curve);
                    UndoStack.Push(curve);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                }
            }
        }

        public void drawEllipse(MouseEventArgs e,double canvasSize, Point GlobalPoint, ObservableCollection<CustomCircle> Ellipse_List, Stack<object> UndoStack)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select Width of Ellipse";
                    
                }
                else if (clickedPoints.Count == 2)
                {
                    aplanviewModel.Instructions = "Select Height of Ellipse";
                }
                else if (clickedPoints.Count == 3)
                {
                    CustomCircle circle = new();
                    circle.Center = new() { Point = clickedPoints[0] };                      
                    circle.RadiusX = Math.Abs(Point.Subtract(clickedPoints[1], clickedPoints[0]).Length);
                    circle.RadiusY = Math.Abs(Point.Subtract(clickedPoints[2], clickedPoints[0]).Length);
                    circle.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "RadiusX",
                        Value = "" + circle.RadiusX + ""

                    });
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "RadiusY",
                        Value = "" + circle.RadiusY + ""

                    });
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "CircleCenter",
                        Value = "" + circle.Center.Point + ""

                    });
                    Ellipse_List.Add(circle);
                    UndoStack.Push(circle);
                    newCanvas.Children.Remove(indicatorEllipse);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                    resetIndicationEllipse();


                }

            }
        }
        public void drawCircle(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomCircle> Circle_List, Stack<object> UndoStack)
        {           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );
                if (clickedPoints.Count == 1)
                {
                    aplanviewModel.Instructions = "Select radius of circle";
                }
                else if (clickedPoints.Count == 2)
                {                     
                    CustomCircle circle = new();
                    circle.Center = new() { Point = clickedPoints[0] };
                    double radius = Math.Abs(Point.Subtract(clickedPoints[1], clickedPoints[0]).Length);
                    circle.RadiusX = radius;
                    circle.RadiusY = radius;
                    circle.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "RadiusX",
                        Value = "" + circle.RadiusX + ""

                    });
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "RadiusY",
                        Value = "" + circle.RadiusY + ""

                    });
                    circle.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "CircleCenter",
                        Value = "" + circle.Center.Point + ""

                    });
                    Circle_List.Add(circle);
                    UndoStack.Push(circle);
                    clickedPoints.Clear();

                    clickedPointOriginalCoord.Clear();
                    newCanvas.Children.Remove(indicatorEllipse);
                    resetIndicationEllipse();
                }

            }  
            
        }
        public void drawTwoPointLine(MouseEventArgs e, double canvasSize, Point GlobalPoint,ObservableCollection<CustomPolyLine> PolyLine, Stack<object> UndoStack)
        {          
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e,GlobalPoint,canvasSize);
                if(clickedPoints.Count == 1)
                {
                    //instruction to show
                    aplanviewModel.Instructions = "Select End Point of Line";
                    //indicator line code
                    addIndicationLineToCanvas(e);
                }               
                if (clickedPoints.Count == 2)
                {                 
                    CustomPolyLine line = new() { CustomPoints = new(),Color=Brushes.Black};
                    clickedPoints.ForEach(x => line.CustomPoints.Add(new() { Point = x }));

                    line.Color = new SolidColorBrush() { Color = aplanviewModel.SelectedColorForACAD };
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "StartPoint",
                        Value = "" + line.CustomPoints[0].Point + ""

                    });
                    line.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Endpoint",
                        Value = "" + line.CustomPoints[1].Point + ""

                    });
                    PolyLine.Add(line);
                    UndoStack.Push(line);
                    clickedPoints.Clear();
                    clickedPointOriginalCoord.Clear();
                    newCanvas.Children.Remove(indicatorLine);
                    resetIndicatorLine();

                }
            }
           
        }
        private void addPointAccordingToEventSource(MouseEventArgs e,Point GlobalPoint,double canvasSize)
        {
            aplanviewModel = System.Windows.Application.Current.FindResource("aplanCADViewerViewModel") as AplanCADViewerViewModel;
            if (e.OriginalSource is Path && ((Path)e.OriginalSource).DataContext is CustomPoint)
            {
                clickedPoints.Add(((CustomPoint)((Path)e.OriginalSource).DataContext).Point);
            }
            else if (e.OriginalSource is Canvas)
            {
                newCanvas = e.Source as Canvas;
                clickedPoints.Add(coordianteConverter(Mouse.GetPosition((Canvas)e.OriginalSource), GlobalPoint, canvasSize));
                clickedPointOriginalCoord.Add(e.GetPosition(newCanvas));
            }
            else
            {
                newCanvas = VisualTreeHelpers.FindAncestor<Canvas>((DependencyObject)e.OriginalSource);
                clickedPoints.Add(coordianteConverter(Mouse.GetPosition(newCanvas), GlobalPoint, canvasSize ));
                clickedPointOriginalCoord.Add(e.GetPosition(newCanvas));
            }
        }
        private Point coordianteConverter(Point point, Point globalPoint, double canvasSize )
        {
            double xCoordinatesAdjust = globalPoint.X - canvasSize / 2;
            double yCoordinatesAdjust = canvasSize / 2 + globalPoint.Y;

            point.X = point.X + xCoordinatesAdjust;
            
            point.Y = -point.Y + yCoordinatesAdjust;             

            return point;
    }
        private void resetIndicatorLine()
        {
            indicatorLine.X1 = 0;
            indicatorLine.X2 = 0;
            indicatorLine.Y1 = 0;
            indicatorLine.Y2 = 0;
        }
        private void resetIndicationEllipse()
        {
            indicatorEllipse.Height = 0;
            indicatorEllipse.Width = 0;
        }
        private void resetIndicationArcPath()
        {
            indicationArcSegment.Point = new Point(0, 0);
            indicationArcSegment.Size = new System.Windows.Size(0, 0);
            indicationArcSegment.IsLargeArc = false;
            indicationArcSegment.SweepDirection = SweepDirection.Clockwise;
            
        }
        private void addIndicationLineToCanvas(MouseEventArgs e)
        {
            indicatorLine.X1 = e.GetPosition(newCanvas).X;
            indicatorLine.Y1 = e.GetPosition(newCanvas).Y;
            indicatorLine.StrokeThickness = 2;
            indicatorLine.Opacity = 0.5;
            IEnumerable<double> dashing = new double[] { 10, 10 };
            indicatorLine.StrokeDashArray = new DoubleCollection(dashing);
            indicatorLine.Stroke = new SolidColorBrush { Color = aplanviewModel.SelectedColorForACAD };
            indicatorLine.Visibility = Visibility.Collapsed;
            newCanvas.Children.Add(indicatorLine);
        }
        #endregion
    }
}
