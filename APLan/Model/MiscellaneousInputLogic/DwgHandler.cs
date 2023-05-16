using ACadSharp;

using ACadSharp.Types;
using ACadSharp.Types.Units;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.Model.HelperClasses;
using Models.TopoModels.EULYNX.rsmCommon;

using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;



namespace APLan.Model.MiscellaneousInputLogic
{
    public class DwgHandler
    {
        public Point firspoint;
        public Point GlobalDrawingPoint;
        public ObservableCollection<CustomPolyLine> Polyline_List;
        public ObservableCollection<CustomCircle> Ellipse_List;
        public ObservableCollection<CustomTextBlock> Text_List;
        public ObservableCollection<CustomArc> Arc_List;
      
        public DwgHandler(Point firspoint, ObservableCollection<CustomPolyLine> Polyline_List, ObservableCollection<CustomCircle> Ellipse_List, ObservableCollection<CustomTextBlock> Text_List, ObservableCollection<CustomArc> Arc_List)
        {
            this.firspoint = firspoint;
            this.Polyline_List = Polyline_List;
            this.Ellipse_List = Ellipse_List;
            this.Arc_List = Arc_List;
            this.Text_List = Text_List;
        }

        #region DWG logic

                
        public void createInsertEntitesRecursion(ACadSharp.Entities.Insert newInsert)
        {
            
            ACadSharp.Tables.BlockRecord block = newInsert.Block;
            
            if (block.IsExplodable == false)
            {
                return;
            }
            foreach (ACadSharp.Entities.Entity entity in block.Entities)
            {
                if (entity is ACadSharp.Entities.Line lines)
                {
                    createLineObjectDwg(lines,newInsert);
                }
                else if (entity is ACadSharp.Entities.Arc arc)
                {
                    createArcObjectDwg(arc);
                }
                else if (entity is ACadSharp.Entities.Circle circle)
                {
                    createCircleObjDwg(circle);

                }
                else if (entity is ACadSharp.Entities.TextEntity txt)
                {
                    createTextObjDwg(txt);
                }
                else if (entity is ACadSharp.Entities.LwPolyline lwpline1)
                {
                    IEnumerable<ACadSharp.Entities.Entity> listOfEntity;
                    if (lwpline1.IsClosed == true)
                    {
                        listOfEntity = lwpline1.Explode();

                        foreach (ACadSharp.Entities.Entity lwpoylineEntity in listOfEntity)
                        {
                            if (lwpoylineEntity is ACadSharp.Entities.Line newline)
                            {
                                createLineObjectDwg(newline, null);
                            }
                            else if (lwpoylineEntity is ACadSharp.Entities.Arc arcLwPolyline)
                            {
                                createArcObjectDwg(arcLwPolyline);
                            }
                        }
                    }
                    else
                    {
                        createLwPolylineObjectDwg(lwpline1);
                    }


                }
                else if (entity is ACadSharp.Entities.Ellipse l)
                {
                    createEllipseObjectDwg(l);
                }
                else if (entity is ACadSharp.Entities.Insert ler)
                {
                    createInsertEntitesRecursion(ler);
                }
                else if (entity is ACadSharp.Entities.MText mtext)
                {
                    createMtextObjDwg(mtext);
                }
            }
        }     
       
                   
            public void createDwgProject(string DWG)
        {
            ACadSharp.CadDocument doc = ACadSharp.IO.DwgReader.Read(DWG);
            ACadSharp.CadDocument transfer = new ACadSharp.CadDocument();
            transfer.Header.Version = doc.Header.Version;
            
            List<ACadSharp.Entities.Entity> entities = new List<ACadSharp.Entities.Entity>(doc.Entities);
            foreach (var item in entities)
            {
                ACadSharp.Entities.Entity e = doc.Entities.Remove(item);
                transfer.Entities.Add(e);
            }
            
            List<ACadSharp.Entities.Insert> listofInserts = new List<ACadSharp.Entities.Insert>();
            
            foreach (var e in transfer.Entities)
            {
                if (e is ACadSharp.Entities.Insert insert)
                {                    
                    
                  createInsertEntitesRecursion(insert);                       

                }
                else if (e is ACadSharp.Entities.Circle circle)
                {
                    if (circle.ObjectType == ObjectType.CIRCLE)
                    {
                        createCircleObjDwg(circle);
                    }

                    else if (circle.ObjectType == ObjectType.ARC && e is ACadSharp.Entities.Arc arc1)
                    {
                        createArcObjectDwg(arc1);
                    }

                }
                else if (e is ACadSharp.Entities.Line lines)
                {
                    createLineObjectDwg(lines,null);
                }
                else if (e is ACadSharp.Entities.LwPolyline lwpline1)
                {
                    IEnumerable<ACadSharp.Entities.Entity> listOfEntity;
                    if (lwpline1.IsClosed == true)
                    {
                        listOfEntity = lwpline1.Explode();

                        foreach (ACadSharp.Entities.Entity lwpoylineEntity in listOfEntity)
                        {
                            if (lwpoylineEntity is ACadSharp.Entities.Line newline)
                            {
                                createLineObjectDwg(newline,null);
                            }
                            else if (lwpoylineEntity is ACadSharp.Entities.Arc arcLwPolyline)
                            {
                                createArcObjectDwg(arcLwPolyline);

                            }
                        }
                    }
                    else
                    {
                        createLwPolylineObjectDwg(lwpline1);
                    }

                }
                else if (e is ACadSharp.Entities.Ellipse ellipse)
                {
                    createEllipseObjectDwg(ellipse);
                }
                else if (e is ACadSharp.Entities.TextEntity txt)
                {
                    createTextObjDwg(txt);
                }
                else if (e is ACadSharp.Entities.Arc arc)
                {
                    createArcObjectDwg(arc);
                }
                else if (e is ACadSharp.Entities.MText mtext)
                {
                    createMtextObjDwg(mtext);
                }
            }
        }
        private static double Distance(Point pointA, Point pointB)
        {
            return Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
        }

         
        private void createLineObjectDwg(ACadSharp.Entities.Line lines,ACadSharp.Entities.Insert insert)
        {
                double X1, Y1, X2, Y2;       
             
                X1 = lines.StartPoint.X;
                Y1 = lines.StartPoint.Y;
                X2 = lines.EndPoint.X;
                Y2 = lines.EndPoint.Y;
             
           CustomPolyLine newpolylinewpf = new CustomPolyLine();
           
            System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
            System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
            List<CustomPoint> pc = new();
            pc.Add(new() { Point = startpoint });
            pc.Add(new() { Point = endpoint });
            newpolylinewpf.CustomPoints = pc;
            if (firspoint.X == 0)
            {
                firspoint.X = startpoint.X;
                firspoint.Y = startpoint.Y;
                GlobalDrawingPoint = firspoint;
            }

            newpolylinewpf.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
            newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Startpoint",
                Value = "" + startpoint + ""

            });
            newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "EndPoint",
                Value = "" + endpoint + ""

            });
            newpolylinewpf.Visibility = Visibility.Visible;
            Polyline_List.Add(newpolylinewpf);
        }
        private void createArcObjectDwg(ACadSharp.Entities.Arc arcLwPolyline)
        {
            System.Windows.Point endPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.EndAngle) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.EndAngle) * arcLwPolyline.Radius));
            System.Windows.Point startPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.StartAngle) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.StartAngle) * arcLwPolyline.Radius));
            double sweep = 0.0;
            if (arcLwPolyline.EndAngle < arcLwPolyline.StartAngle)
                sweep = (360 + arcLwPolyline.EndAngle) - arcLwPolyline.StartAngle;
            else sweep = Math.Abs(arcLwPolyline.EndAngle - arcLwPolyline.StartAngle);
            bool IsLargeArc = sweep >= 180;

            Size size = new System.Windows.Size(arcLwPolyline.Radius, arcLwPolyline.Radius);
            SweepDirection sweepDirection = arcLwPolyline.Normal.Z > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

            CustomArc newArc = new CustomArc();
            newArc.StartPoint = new() { Point = startPoint };
            newArc.EndPoint = new() { Point = endPoint };
            newArc.Radius = arcLwPolyline.Radius;
            newArc.Size = new Size(newArc.Radius, newArc.Radius);

            newArc.SweepDirection = sweepDirection;
            newArc.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
            if (firspoint.X == 0)
            {
                firspoint.X = startPoint.X;
                firspoint.Y = startPoint.Y;
                GlobalDrawingPoint = firspoint;
            }
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "StartPoint",
                Value = "" + startPoint + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "EndPoint",
                Value = "" + endPoint + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Radius",
                Value = "" + newArc.Radius + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Center",
                Value = "" + newArc.Center + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "IsLargeArc",
                Value = "" + newArc.IsLargeArc + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Thickness",
                Value = "" + newArc.Thickness + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "SweepDirection",
                Value = "" + sweepDirection + ""

            });
            newArc.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Normal",
                Value = "" + newArc.Normal + ""

            });
            Arc_List.Add(newArc);
        }
        private void createLwPolylineObjectDwg(ACadSharp.Entities.LwPolyline lwpline1)
        {
            if (lwpline1.ObjectType == ObjectType.LWPOLYLINE)
            {
                List<ACadSharp.Entities.LwPolyline.Vertex> vertexCollection = lwpline1.Vertices;
                List<CustomPoint> pointCollection_HO = new();

                CustomPolyLine newPolyline_lw = new CustomPolyLine();
                foreach (ACadSharp.Entities.LwPolyline.Vertex singleVertex in vertexCollection)
                {
                    System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Location.X, singleVertex.Location.Y);

                    pointCollection_HO.Add(new() { Point = vertexPoint_HO });

                    newPolyline_lw.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Points",
                        Value = "" + vertexPoint_HO + ""

                    });
                    if (firspoint.X == 0)
                    {
                        firspoint.X = vertexPoint_HO.X;
                        firspoint.Y = vertexPoint_HO.Y;
                        GlobalDrawingPoint = firspoint;
                    }

                }

                newPolyline_lw.CustomPoints = pointCollection_HO;
                newPolyline_lw.Color = new SolidColorBrush() { Color = Colors.DarkBlue };
                Polyline_List.Add(newPolyline_lw);
            }

        }
        private void createEllipseObjectDwg(ACadSharp.Entities.Ellipse l)
        {
            if (l.ObjectType == ObjectType.ELLIPSE)
            {
                CustomCircle newCustomEllipse = new();
                System.Windows.Point centerVertex = new System.Windows.Point(l.Center.X, l.Center.Y);
                Point endPointForMajorAxis = new Point(l.EndPoint.X, l.EndPoint.Y);

                double MajorAxis = 2 * Distance(centerVertex, endPointForMajorAxis);
                newCustomEllipse.RadiusX = (MajorAxis / 2);
                newCustomEllipse.RadiusY = ((MajorAxis * l.RadiusRatio) / 2); // minoraxis/majoraxis =  radius ratio
                newCustomEllipse.Thickness = l.Thickness;
                newCustomEllipse.Center = new() { Point = centerVertex };
                if (firspoint.X == 0)
                {
                    firspoint.X = centerVertex.X;
                    firspoint.Y = centerVertex.Y;
                    GlobalDrawingPoint = firspoint;
                }
                newCustomEllipse.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "RadiusX",
                    Value = "" + newCustomEllipse.RadiusX + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "RadiusY",
                    Value = "" + newCustomEllipse.RadiusY + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Center",
                    Value = "" + centerVertex + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Thickness",
                    Value = "" + l.Thickness + ""

                });
                Ellipse_List.Add(newCustomEllipse);
            }

        }
        private void createCircleObjDwg(ACadSharp.Entities.Circle circle)
        {
            if (circle.IsInvisible == false && circle.ObjectType == ObjectType.CIRCLE)
            {
                CustomCircle newCustomEllipse = new();
                System.Windows.Point centerVertex = new System.Windows.Point(circle.Center.X, circle.Center.Y);

                newCustomEllipse.RadiusX = circle.Radius;
                newCustomEllipse.RadiusY = circle.Radius;
                newCustomEllipse.Thickness = circle.Thickness;
                newCustomEllipse.Center = new() { Point = centerVertex };
                if (firspoint.X == 0)
                {
                    firspoint.X = centerVertex.X;
                    firspoint.Y = centerVertex.Y;
                    GlobalDrawingPoint = firspoint;
                }
                newCustomEllipse.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "RadiusX",
                    Value = "" + newCustomEllipse.RadiusX + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "RadiusY",
                    Value = "" + newCustomEllipse.RadiusY + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Center",
                    Value = "" + centerVertex + ""

                });
                newCustomEllipse.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Thickness",
                    Value = "" + circle.Thickness + ""

                });
                Ellipse_List.Add(newCustomEllipse);
            }
        }
        private void createTextObjDwg(ACadSharp.Entities.TextEntity txt)
        {
            if (txt.IsInvisible == false && txt.Value.Length > 0 && txt.ObjectType == ObjectType.TEXT)
            {
                CustomTextBlock textBlock = new CustomTextBlock();
                Point newPoint = new Point((((double)txt.InsertPoint.X)), (((double)txt.InsertPoint.Y)));
                textBlock.NodePoint = new() { Point = newPoint };
                textBlock.Name = txt.Value;
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Name",
                    Value = "" + txt.Value + ""

                });
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "NodePoint",
                    Value = "" + newPoint + ""

                });
                textBlock.Height = txt.Height;
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Height",
                    Value = "" + txt.Height + ""

                });

                textBlock.ObliqueAngle = (txt.ObliqueAngle * 180) / (Math.PI);
                if(textBlock.ObliqueAngle < 0)
                {
                    textBlock.ObliqueAngle = ((txt.ObliqueAngle+2*Math.PI) * 180) / (Math.PI);
                }

                textBlock.RotationAngle =360- ((txt.Rotation * 180) / (Math.PI));



                if (txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Left)
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Left;
                }
                else if (txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Right)
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Right;
                }
                else if (txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Center ||
                    txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Middle)
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Center;
                }
                else if (txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Aligned ||
                    txt.HorizontalAlignment == ACadSharp.Entities.TextHorizontalAlignment.Fit
                     )
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Stretch;
                }
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "TextAlignment",
                    Value = "" + textBlock.TxtHoriAlignment + ""

                });
                if (txt.VerticalAlignment == ACadSharp.Entities.TextVerticalAlignmentType.Top)
                {
                    textBlock.TxtVoriAlignment = System.Windows.VerticalAlignment.Top;
                }
                else if (txt.VerticalAlignment == ACadSharp.Entities.TextVerticalAlignmentType.Bottom)
                {
                    textBlock.TxtVoriAlignment = System.Windows.VerticalAlignment.Bottom;
                }
                else if (txt.VerticalAlignment == ACadSharp.Entities.TextVerticalAlignmentType.Middle)
                {
                    textBlock.TxtVoriAlignment = System.Windows.VerticalAlignment.Center;
                }

                Text_List.Add(textBlock);
            }
        }

        private void createMtextObjDwg(ACadSharp.Entities.MText txt)
        {
            if (txt.IsInvisible == false && txt.Value.Length > 0 && txt.ObjectType == ObjectType.MTEXT)
            {
                CustomTextBlock textBlock = new CustomTextBlock();
                Point newPoint = new Point((((double)txt.InsertPoint.X)), (((double)txt.InsertPoint.Y)));
                textBlock.NodePoint = new() { Point = newPoint };
                textBlock.Name = txt.Value;
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Name",
                    Value = "" + txt.Value + ""

                });
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "NodePoint",
                    Value = "" + newPoint + ""

                });
                textBlock.Height = txt.Height;
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "Height",
                    Value = "" + txt.Height + ""

                });

                textBlock.RotationAngle = (txt.Rotation * 180) / (Math.PI);
                if (textBlock.RotationAngle < 0)
                {
                    textBlock.RotationAngle = ((txt.Rotation + 2 * Math.PI) * 180) / (Math.PI);
                }
                


                

                Text_List.Add(textBlock);
            }
        }


        #endregion
    }
}
