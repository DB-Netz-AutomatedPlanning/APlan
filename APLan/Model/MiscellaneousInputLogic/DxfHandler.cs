using ACadSharp;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.ViewModels;
using ERDM.Tier_3;
using netDxf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Point = System.Windows.Point;
namespace APLan.Model.MiscellaneousInputLogic
{
    public class DxfHandler
    {
        public Point firspoint;
        public Point GlobalDrawingPoint;         
        public ObservableCollection<CustomPolyLine> Polyline_List;
        public ObservableCollection<CustomCircle> Ellipse_List;
        public ObservableCollection<CustomTextBlock> Text_List;
        public ObservableCollection<CustomArc> Arc_List;
        public DxfHandler(Point firspoint, ObservableCollection<CustomPolyLine> Polyline_List, ObservableCollection<CustomCircle> Ellipse_List, ObservableCollection<CustomTextBlock> Text_List, ObservableCollection<CustomArc> Arc_List)
        {
            this.firspoint = firspoint;
             
            this.Polyline_List = Polyline_List;
            this.Ellipse_List = Ellipse_List;
            this.Arc_List = Arc_List;
            this.Text_List = Text_List;
        }
        private void createLineObjectDxf(netDxf.Entities.Line l)
        {
           
            double X1 = l.StartPoint.X;
            double Y1 = l.StartPoint.Y;
            double X2 = l.EndPoint.X;
            double Y2 = l.EndPoint.Y;
            CustomPolyLine newpolylinewpf = new CustomPolyLine();

            System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
            System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
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
            Polyline_List.Add(newpolylinewpf);
        }
        private void createCircleObjDxf(netDxf.Entities.Circle circle)
        {
            CustomCircle newEllipse = new CustomCircle();
            double radius = circle.Radius;
            double thickness = circle.Thickness;
            System.Windows.Point centerVertex = new System.Windows.Point(circle.Center.X, circle.Center.Y);
            if (firspoint.X == 0)
            {
                firspoint.X = centerVertex.X;
                firspoint.Y = centerVertex.Y;
                GlobalDrawingPoint = firspoint;
            }

            newEllipse.RadiusX = radius;
            newEllipse.RadiusY = radius;
            newEllipse.Thickness = thickness;
            newEllipse.Center = new() { Point = centerVertex };
            newEllipse.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
            newEllipse.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Radius",
                Value = "" + radius + ""

            });
            newEllipse.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Center",
                Value = "" + centerVertex + ""

            });
            newEllipse.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Thickness",
                Value = "" + thickness + ""

            });
            Ellipse_List.Add(newEllipse);

        }

        private void createEllipseObjectDxf(netDxf.Entities.Ellipse l)
        {
            CustomCircle newCustomEllipse = new();
            System.Windows.Point centerVertex = new System.Windows.Point(l.Center.X, l.Center.Y);

            newCustomEllipse.RadiusX = (l.MajorAxis / 2);
            newCustomEllipse.RadiusY = (l.MinorAxis / 2);
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

        private void createArcObjectDxf(netDxf.Entities.Arc newArc)
        {
            System.Windows.Point endPoint = new System.Windows.Point((newArc.Center.X + Math.Cos(newArc.EndAngle * Math.PI / 180) * newArc.Radius), (newArc.Center.Y + (Math.Sin(newArc.EndAngle * (Math.PI / 180)) * newArc.Radius)));
            System.Windows.Point startPoint = new System.Windows.Point((newArc.Center.X + Math.Cos(newArc.StartAngle * Math.PI / 180) * newArc.Radius), (newArc.Center.Y + Math.Sin(newArc.StartAngle * Math.PI / 180) * newArc.Radius));
            double sweep = 0.0;
            if (newArc.EndAngle < newArc.StartAngle)
                sweep = (360 + newArc.EndAngle) - newArc.StartAngle;
            else sweep = Math.Abs(newArc.EndAngle - newArc.StartAngle);
            bool IsLargeArc = sweep >= 180;

            Size size = new System.Windows.Size(newArc.Radius, newArc.Radius);
            SweepDirection sweepDirection = newArc.Normal.Z > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

            CustomArc newArc2 = new CustomArc();
            newArc2.StartPoint = new() { Point = startPoint };
            newArc2.EndPoint = new() { Point = endPoint };
            newArc2.Radius = newArc.Radius;
            newArc2.SweepDirection = sweepDirection;
            newArc2.Thickness = newArc.Thickness;
            newArc2.Normal = newArc.Normal;
            newArc2.IsLargeArc = false;
            newArc2.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
            newArc2.Size = new Size(newArc.Radius, newArc.Radius);
            if (firspoint.X == 0)
            {
                firspoint.X = startPoint.X;
                firspoint.Y = startPoint.Y;
                GlobalDrawingPoint = firspoint;
            }
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "StartPoint",
                Value = "" + startPoint + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "EndPoint",
                Value = "" + endPoint + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Radius",
                Value = "" + newArc.Radius + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Center",
                Value = "" + newArc.Center + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "IsLargeArc",
                Value = "" + newArc2.IsLargeArc + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Thickness",
                Value = "" + newArc2.Thickness + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "SweepDirection",
                Value = "" + sweepDirection + ""

            });
            newArc2.ShapeAttributeInfo.Add(new KeyValue()
            {
                Key = "Normal",
                Value = "" + newArc2.Normal + ""

            });

            Arc_List.Add(newArc2);
        }

        private void createLwPolylineObjectDxf(netDxf.Entities.LwPolyline lwpline1)
        {

            List<netDxf.Entities.LwPolylineVertex> vertexCollection = lwpline1.Vertexes;
            List<CustomPoint> pointCollection_HO = new();

            CustomPolyLine newPolyline_lw = new CustomPolyLine();
            foreach (netDxf.Entities.LwPolylineVertex singleVertex in vertexCollection)
            {
                System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

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

        private void createTextObjxf(netDxf.Entities.Text txt)
        {
            if ( txt.IsVisible == true && txt.Value.Length > 0)
            {
                CustomTextBlock textBlock = new CustomTextBlock();
                Point newPoint = new Point((((double)txt.Position.X)), (((double)txt.Position.Y)));
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

                textBlock.RotationAngle = Math.Abs(txt.Rotation);


                if (txt.Alignment == netDxf.Entities.TextAlignment.TopLeft ||
                                            txt.Alignment == netDxf.Entities.TextAlignment.MiddleLeft ||
                                            txt.Alignment == netDxf.Entities.TextAlignment.BottomLeft ||
                                                txt.Alignment == netDxf.Entities.TextAlignment.BaselineLeft)
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Left;
                }
                else if (txt.Alignment == netDxf.Entities.TextAlignment.TopRight ||
                    txt.Alignment == netDxf.Entities.TextAlignment.MiddleRight ||
                    txt.Alignment == netDxf.Entities.TextAlignment.BottomRight ||
                        txt.Alignment == netDxf.Entities.TextAlignment.BaselineRight)
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Right;
                }
                else if (txt.Alignment == netDxf.Entities.TextAlignment.TopCenter ||
                     txt.Alignment == netDxf.Entities.TextAlignment.MiddleCenter ||
                     txt.Alignment == netDxf.Entities.TextAlignment.BottomCenter ||
                         txt.Alignment == netDxf.Entities.TextAlignment.BaselineCenter ||
                         txt.Alignment == netDxf.Entities.TextAlignment.Middle
                         )
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Center;
                }
                else if (txt.Alignment == netDxf.Entities.TextAlignment.Aligned ||
                     txt.Alignment == netDxf.Entities.TextAlignment.Fit
                     )
                {
                    textBlock.TxtHoriAlignment = System.Windows.HorizontalAlignment.Stretch;
                }
                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                {
                    Key = "TextAlignment",
                    Value = "" + textBlock.TxtHoriAlignment + ""

                });

                Text_List.Add(textBlock);
            }
        }

       
        private void createInsertObjDxf(netDxf.Entities.Insert insert)
        {
            List<netDxf.Entities.EntityObject> boundary2 = new List<netDxf.Entities.EntityObject>();
            boundary2 = insert.Explode();
            foreach (netDxf.Entities.EntityObject e in boundary2)
            {
                // if the entities is circle type
                if (e is netDxf.Entities.Circle circle)
                {
                    createCircleObjDxf(circle);

                }
                //if the entiites is text type
                else if (e is netDxf.Entities.Text txt)
                {
                    if (txt.Layer.Name != "0")
                    {
                        createTextObjxf(txt);
                    }

                }
                // if the entities is of image type
                else if (e is netDxf.Entities.Image Im)
                {

                }
                // if the entities is of arc type
                else if (e is netDxf.Entities.Arc arc)
                {
                    createArcObjectDxf(arc);
                }

                //if the entities is of lwpolyline
                else if (e is netDxf.Entities.LwPolyline lwpline1)
                {
                    List<netDxf.Entities.EntityObject> listOfEntity = new List<netDxf.Entities.EntityObject>();
                    // checking if the lwpolyline is enclosed or not.
                    if (lwpline1.IsClosed == true)
                    {
                        //splitting the lwpolyline in to small entities
                        listOfEntity = lwpline1.Explode();

                        foreach (netDxf.Entities.EntityObject lwpoylineEntity in listOfEntity)
                        {
                            if (lwpoylineEntity is netDxf.Entities.Line newline)
                            {
                                createLineObjectDxf(newline);
                            }
                            else if (lwpoylineEntity is netDxf.Entities.Arc arcLwPolyline)
                            {
                                createArcObjectDxf(arcLwPolyline);
                            }
                        }
                    }
                    else
                    {
                        createLwPolylineObjectDxf(lwpline1);
                    }

                }
                //if entities is of type line
                else if (e is netDxf.Entities.Line lines)
                {

                    createLineObjectDxf(lines);
                }

                else if (e is netDxf.Entities.Ellipse l)
                {
                    createEllipseObjectDxf(l);

                }
                else if(e is netDxf.Entities.Insert insertRec)
                {
                   createInsertObjDxf(insertRec);
                }

            }
        }
        public void createDxfProject(string DXF)
        {

            netDxf.Header.DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(DXF);
            // netDxf is only compatible with AutoCad2000 and higher DXF versions
            if (dxfVersion < netDxf.Header.DxfVersion.AutoCad2000)
            {
                return;
            }
            DxfDocument dxfReader = DxfDocument.Load(DXF);
           
            #region POINTSDXF
            if (dxfReader.Points.Count() > 0)
            {
                foreach (netDxf.Entities.Point pnts in dxfReader.Points)
                {
                    if (pnts.Layer.Name == "GlobalDrawingPoint")
                    {
                        System.Windows.Point globalPoint = new System.Windows.Point((pnts.Position.X), (pnts.Position.Y));

                        GlobalDrawingPoint = globalPoint;
                        firspoint.X = globalPoint.X;
                        firspoint.Y = globalPoint.Y;
                    }
                }
            }

            #endregion

            #region INSERTSDXF
            if (dxfReader.Inserts.Count() > 0)
            {
                foreach (netDxf.Entities.Insert lwpline in dxfReader.Inserts)
                {
                    //splitting the Inserts entities in to netDXF recognizable entities
                   
                    createInsertObjDxf(lwpline);


                }
            }
            #endregion

            #region LWPOLYLINEDXF

            foreach (netDxf.Entities.LwPolyline lwpline1 in dxfReader.LwPolylines)
            {
                List<netDxf.Entities.EntityObject> listOfEntity = new List<netDxf.Entities.EntityObject>();
                if (lwpline1.IsClosed == true)
                {
                    listOfEntity = lwpline1.Explode();

                    foreach (netDxf.Entities.EntityObject lwpoylineEntity in listOfEntity)
                    {
                        if (lwpoylineEntity is netDxf.Entities.Line newline)
                        {
                            createLineObjectDxf(newline);
                        }
                        else if (lwpoylineEntity is netDxf.Entities.Arc arcLwPolyline)
                        {
                            createArcObjectDxf(arcLwPolyline);
                        }
                    }
                }
                else
                {
                    createLwPolylineObjectDxf(lwpline1);
                }

            }
            #endregion

            #region HATCHDXF
            List<netDxf.Entities.EntityObject> boundary = new List<netDxf.Entities.EntityObject>();
            foreach (netDxf.Entities.Hatch tier in dxfReader.Hatches)
            {
                //netDxf.Collections.ObservableCollection<netDxf.Entities.HatchBoundaryPath> he = tier.BoundaryPaths;
                boundary = tier.UnLinkBoundary();

                foreach (netDxf.Entities.EntityObject e in boundary)
                {
                    if (e is netDxf.Entities.Line lines)
                    {
                        createLineObjectDxf(lines);
                    }


                }

            }
            #endregion

            #region TEXTDXF
            foreach (netDxf.Entities.Text txt in dxfReader.Texts)
            {
                createTextObjxf(txt);

            }
            #endregion

            #region ARCDXF
            foreach (netDxf.Entities.Arc newArc in dxfReader.Arcs)
            {
                createArcObjectDxf(newArc);
                
            }
            #endregion

            #region LINEACADSDXF
            foreach (netDxf.Entities.Line l in dxfReader.Lines)
            {
                createLineObjectDxf(l);
            }
            #endregion

            #region ELLIPSEACADDXF
            foreach (netDxf.Entities.Ellipse l in dxfReader.Ellipses)
            {
                createEllipseObjectDxf(l);
            }
            #endregion

            #region CIRCLEACADDXF
            foreach (netDxf.Entities.Circle circle in dxfReader.Circles)
            {
                createCircleObjDxf(circle);
            }
            #endregion

            #region JSONorMDBorACADDXFPolylines
            //checking for polyine for the json or mdb drawing converted in to dxf from APLAN
            foreach (netDxf.Entities.Polyline plyLine in dxfReader.Polylines)
            {

                if (plyLine.Layer.Name == "Entwurfselement_HO")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_HO = new();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_HO.Add(new() { Point = vertexPoint_HO });
                    }
                    CustomPolyLine newPolyline_HO = new CustomPolyLine();

                    newPolyline_HO.CustomPoints = pointCollection_HO;
                    newPolyline_HO.Color = new SolidColorBrush() { Color = Colors.DarkBlue };
                    Polyline_List.Add(newPolyline_HO);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_LA")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_LA = new();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_LA = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_LA.Add(new() { Point = vertexPoint_LA });
                    }
                    CustomPolyLine newPolyline_LA = new CustomPolyLine();

                    newPolyline_LA.CustomPoints = pointCollection_LA;
                    newPolyline_LA.Color = new SolidColorBrush() { Color = Colors.DarkRed };
                    Polyline_List.Add(newPolyline_LA);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_KM")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_KM = new();

                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_KM = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);
                        pointCollection_KM.Add(new() { Point = vertexPoint_KM });
                    }
                    CustomPolyLine newPolyline_KM = new CustomPolyLine();

                    newPolyline_KM.CustomPoints = pointCollection_KM;
                    newPolyline_KM.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                    Polyline_List.Add(newPolyline_KM);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_UH")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_UH = new();

                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_UH = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_UH.Add(new() { Point = vertexPoint_UH });
                    }
                    CustomPolyLine newPolyline_UH = new CustomPolyLine();

                    newPolyline_UH.CustomPoints = pointCollection_UH;
                    newPolyline_UH.Color = new SolidColorBrush() { Color = Colors.Green };
                    Polyline_List.Add(newPolyline_UH);
                }
                else if (plyLine.Layer.Name == "gleiskanten")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_gleiskanten = new();

                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_gleiskanten = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_gleiskanten.Add(new() { Point = vertexPoint_gleiskanten });


                    }
                    CustomPolyLine newPolyline_gleiskanten = new CustomPolyLine();

                    newPolyline_gleiskanten.CustomPoints = pointCollection_gleiskanten;
                    newPolyline_gleiskanten.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
                    Polyline_List.Add(newPolyline_gleiskanten);
                }
                else
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    List<CustomPoint> pointCollection_ACAD = new();
                    CustomPolyLine newPolyline_acad = new CustomPolyLine();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_gleiskanten = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_ACAD.Add(new() { Point = vertexPoint_gleiskanten });

                        newPolyline_acad.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "Points",
                            Value = "" + vertexPoint_gleiskanten + ""

                        });
                    }

                    newPolyline_acad.CustomPoints = pointCollection_ACAD;
                    newPolyline_acad.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
                    Polyline_List.Add(newPolyline_acad);
                }


            }
            foreach (netDxf.Entities.Point pnts in dxfReader.Points)
            {
                if (pnts.Layer.Name == "gleisknoten")
                {
                    CustomCircle node = new();
                    Point newPoint = new Point((((double)pnts.Position.X)), (((double)pnts.Position.Y)));
                    node.Center = new() { Point = newPoint };
                    Ellipse_List.Add(node);

                }
            }
            #endregion
             
        }
    }
}
