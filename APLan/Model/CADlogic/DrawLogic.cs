using APLan.Converters;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.ViewModels;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
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
        public static List<Point> clickedPoints;
        public CustomPolyLine newPolyline;
         
        public DrawLogic()
        {
            clickedPoints = new();
             
             
        }

        public void drawPolyline(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_List)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize);  
                if(clickedPoints.Count == 1)
                {
                    CustomPolyLine newPolyline = new CustomPolyLine();
                    newPolyline.CustomPoints.Add(new() { Point = clickedPoints[clickedPoints.Count-1] });
                    newPolyline.Color = new SolidColorBrush() { Color = Colors.Black };
                    polyline_List.Add(newPolyline);
                }
                else
                {
                    newPolyline.CustomPoints.Add(new() { Point = clickedPoints[clickedPoints.Count-1] });
                }
                newPolyline.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Points",
                    Value = "" + clickedPoints[clickedPoints.Count - 1] + ""
                });
              
                
            }
            if(e.RightButton == MouseButtonState.Pressed)
            {
                clickedPoints.Clear();
            }
        }
        public void drawHorizontalLine(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomPolyLine> polyline_list)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e,GlobalPoint, canvasSize);

                if (clickedPoints.Count == 2)
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

                    line.Color = new SolidColorBrush() { Color = Colors.Black };
                    polyline_list.Add(line);
                    clickedPoints.Clear();
                }
            }
        }
        public void drawVerticalLine(MouseEventArgs e, double canvasSize,Point GLobalPoint, ObservableCollection<CustomPolyLine> polyline_List)
        {            

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GLobalPoint, canvasSize);

                if (clickedPoints.Count == 2)
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

                    line.Color = new SolidColorBrush() { Color = Colors.Black };
                    polyline_List.Add(line);
                    clickedPoints.Clear();
                }
            }


        }
        public void drawThreePointArc(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomBezierCurve> Bezier_Curve_List)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );

                if (clickedPoints.Count == 3)
                {
                    CustomBezierCurve curve = new() { CustomPoints = new(), Color = Brushes.Black };
                    clickedPoints.ForEach(x => curve.CustomPoints.Add(new() { Point = x }));
                    Bezier_Curve_List.Add(curve);
                    clickedPoints.Clear();
                }
            }
        }

        public void drawEllipse(MouseEventArgs e,double canvasSize, Point GlobalPoint, ObservableCollection<CustomCircle> Ellipse_List)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );

                if (clickedPoints.Count == 3)
                {
                    CustomCircle circle = new();
                    circle.Center = new() { Point = clickedPoints[0] };                      
                    circle.RadiusX = Math.Abs(Point.Subtract(clickedPoints[1], clickedPoints[0]).Length);
                    circle.RadiusY = Math.Abs(Point.Subtract(clickedPoints[2], clickedPoints[0]).Length);
                    circle.Color = new SolidColorBrush() { Color = Colors.Black };
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
                        Value = "" + circle.Center + ""

                    });
                    Ellipse_List.Add(circle);
                    clickedPoints.Clear();

                    //UndoStack.Push(newEllipse);
                    // Instructions = "Select center Point";
                }

            }
        }
        public void drawCircle(MouseEventArgs e, double canvasSize, Point GlobalPoint, ObservableCollection<CustomCircle> Circle_List)
        {           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e, GlobalPoint, canvasSize );
                
                if (clickedPoints.Count == 2)
                {                     
                    CustomCircle circle = new();
                    circle.Center = new() { Point = clickedPoints[0] };
                    double radius = Math.Abs(Point.Subtract(clickedPoints[1], clickedPoints[0]).Length);
                    circle.RadiusX = radius;
                    circle.RadiusY = radius;
                    circle.Color = new SolidColorBrush() { Color = Colors.Black };
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
                        Value = "" + circle.Center + ""

                    });
                    Circle_List.Add(circle);
                    clickedPoints.Clear();
                    
                    //UndoStack.Push(newEllipse);
                    // Instructions = "Select center Point";
                }

            }  
            
        }
        public void drawTwoPointLine(MouseEventArgs e, double canvasSize, Point GlobalPoint,ObservableCollection<CustomPolyLine> PolyLine)
        {
          
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                addPointAccordingToEventSource(e,GlobalPoint,canvasSize );

                if (clickedPoints.Count == 2)
                {
                    CustomPolyLine line = new() { CustomPoints = new(),Color=Brushes.Black};
                    clickedPoints.ForEach(x => line.CustomPoints.Add(new() { Point = x }));
                    PolyLine.Add(line);
                    clickedPoints.Clear();
                }
            }
           
        }
        private void addPointAccordingToEventSource(MouseEventArgs e,Point GlobalPoint,double canvasSize )
        {

            if (e.OriginalSource is Path && ((Path)e.OriginalSource).DataContext is CustomPoint)
            {
                clickedPoints.Add(((CustomPoint)((Path)e.OriginalSource).DataContext).Point);
            }
            else if (e.OriginalSource is Canvas)
            {
                clickedPoints.Add(coordianteConverter(Mouse.GetPosition((Canvas)e.OriginalSource), GlobalPoint, canvasSize));
            }
            else
            {
                Canvas canvas = VisualTreeHelpers.FindAncestor<Canvas>((DependencyObject)e.OriginalSource);
                clickedPoints.Add(coordianteConverter(Mouse.GetPosition(canvas), GlobalPoint, canvasSize ));
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
    }
}
