using APLan.Commands;
using APLan.HelperClasses;
using APLan.Views;

using aplan.core;

using Models.TopoModels.EULYNX.rsmCommon;
using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.sig;
using Models.TopoModels.EULYNX.db;

using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Controls;
using System.Windows.Xps;
using netDxf;
using netDxf.Tables;
using netDxf.Entities;
using Image = System.Windows.Controls.Image;

namespace APLan.ViewModels
{
    public class PlanningTabViewModel : BaseViewModel
    {
        #region attributes
        private string planType;
        private string exportType;
        private string importType;
        private Visibility pdfDetailViewerVisibility;
        private Microsoft.Win32.SaveFileDialog safeFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;


        public Visibility PdfDetailViewerVisibility
        {
            get => pdfDetailViewerVisibility;
            set
            {
                pdfDetailViewerVisibility = value;
                OnPropertyChanged();
            }
        }
        public string PlanType
        {
            get => planType;
            set
            {
                planType = value.Split(':')[1].Trim();
                OnPropertyChanged();
            }
        }
        public string ExportType
        {
            get => exportType;
            set
            {
                exportType = value.Split(':')[1].Trim();
                OnPropertyChanged();
            }
        }
        public string ImportType
        {
            get => importType;
            set
            {
                importType = value.Split(':')[1].Trim();
                OnPropertyChanged();
            }
        }
        private static List<PositioningNetElement> VisitedElements;
        private static PositioningNetElement neededElement = null;
        #endregion

        #region commands
        public ICommand PlanButton { get; set; }
        public ICommand ExportButton { get; set; }

        public ICommand ImportButton { get; set; }
        #endregion

        #region constructor
        public PlanningTabViewModel()
        {
            PlanButton = new RelayCommand(ExecutePlanButton);
            ExportButton = new RelayCommand(ExecuteExportButton);
            ImportButton = new RelayCommand(ExecuteImportButton);
            Signals = new ObservableCollection<Signalinfo>();
            VisitedElements = new List<PositioningNetElement>();
            safeFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            pdfDetailViewerVisibility = Visibility.Collapsed;
        }
        #endregion

        #region logic
        private void ExecutePlanButton(object parameter)
        {
            var EulynxObject= ModelViewModel.eulynx;
            var eulynxService = ModelViewModel.eulynxService;
            Database dataBase = ModelViewModel.db;
            Signals.Clear();

            bool etcs = false;
            if (EulynxObject == null)
            {
                MessageBox.Show("No current Eulynx object");

            }else if (PlanType=="ETCS" && EulynxObject!=null)
            {
                etcs = true;
                eulynxService.plan(EulynxObject, dataBase, etcs);
                extractMainSignals(EulynxObject);
                extractOnTrackSignals(EulynxObject);
            }
            else
            {
                MessageBox.Show("Kindly Select a Plan type");
            }       
        }

        private void ExecuteImportButton(object parameter)
        {
            if(ImportType == "Image")
            {
                var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;
                openFileDialog1.InitialDirectory = @"c:\\";
                openFileDialog1.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = openFileDialog1.FileName;
                   
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFileName);
                    bitmap.EndInit();
                    //Image img = new Image();
                    //img.Source = bitmap;
                    CustomImage customImage = new CustomImage();
                    customImage.Name = bitmap;
                    customImage.Height = drawViewModel.RecGeometry.Rect.Height;
                    customImage.Width = drawViewModel.RecGeometry.Rect.Width;
                    //img.Stretch = Stretch.Fill;
                    //double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
                    //double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;
                    //System.Windows.Controls.Canvas.SetLeft(img,drawViewModel.RecGeometry.Rect.X+xCoordinatesAdjust);
                    //System.Windows.Controls.Canvas.SetTop(img, -drawViewModel.RecGeometry.Rect.Y+yCoordinatesAdjust);
                    customImage.SetLeft = drawViewModel.RecGeometry.Rect.Left;
                    customImage.SetTop = drawViewModel.RecGeometry.Rect.Top;
                    NewProjectViewModel.Image_List.Add(customImage);
                }
            }
        }

        private void ExecuteExportButton(object parameter)
        {
            if (exportType == "Eulynx" || exportType == "JSON(ERDM)")
            {
                ExportWindow export = new ExportWindow();
                export.ShowDialog();
            }else if (exportType == "Eulynx" || exportType == "XML(ERDM)")
            {
                ExportWindow export = new ExportWindow();
                export.ShowDialog();
            }
            else if (exportType == "pdf")
            {

                string fileName = System.IO.Path.GetRandomFileName();

                try
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;

                    PdfDetailViewerVisibility = Visibility.Visible;
                    //getting static resource to remove the tile in canvas                  
                    object resourceCanvasGrid = Draw.drawing.TryFindResource("canvasGrid");
                    DrawingBrush gridBrush = (DrawingBrush)resourceCanvasGrid;
                    gridBrush.TileMode = TileMode.None;
                    gridBrush.Viewport = new Rect(0, 0, 0, 0);



                    // write the XPS document
                    using (XpsDocument doc = new XpsDocument(fileName, FileAccess.ReadWrite))
                    {
                        XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                        writer.Write(MainWindow.basCanvas);
                    }

                    // Read the XPS document into a dynamically generated
                    // preview Window 
                    using (XpsDocument doc = new XpsDocument(fileName, FileAccess.Read))
                    {
                        FixedDocumentSequence fds = doc.GetFixedDocumentSequence();

                        var window = new Window();

                        window.Content = new DocumentViewer { Document = fds };

                        window.ShowDialog();

                    }



                    //adding the tile in canvas again

                    gridBrush.TileMode = TileMode.Tile;
                    gridBrush.Viewport = new Rect(0, 0, 100, 100);

                }
                finally
                {
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            File.Delete(fileName);
                        }
                        catch (Exception exep)
                        {
                            Console.WriteLine(exep.Message);
                        }
                    }
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    PdfDetailViewerVisibility = Visibility.Collapsed;
                } 
            }
            else if (exportType == "dxf")
            {
                DxfDocument dxf = new DxfDocument();
                Layer layer1_LA = new Layer("Entwurfselement_LA");
                Layer layer2_KM = new Layer("Entwurfselement_KM");
                Layer layer3_HO = new Layer("Entwurfselement_HO");
                Layer layer4_UH = new Layer("Entwurfselement_UH");
                Layer layer5_gleiskanten = new Layer("gleiskanten");
                Layer layer6_gleisknoten = new Layer("gleisknoten");
                Layer layer7_symbolImage = new Layer("symbolImage");
                Layer layer8_GlobalPoint = new Layer("GlobalDrawingPoint");
                netDxf.Entities.Polyline p1 = new netDxf.Entities.Polyline();
                PolylineVertex polylineVertex = new PolylineVertex();
                List<netDxf.Vector3> polylineVertexList = new List<netDxf.Vector3>();


                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsPolylineList = Polyline_List;
                System.Collections.ObjectModel.ObservableCollection<CustomLine> obslineList = Line_List;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsLwPolylineList = Polyline_LW_list;
                System.Collections.ObjectModel.ObservableCollection<CustomEllipse> obsEllipseList = Ellipse_List;
                System.Collections.ObjectModel.ObservableCollection<CustomArc> obsArcList = Arc_List;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_LA_list = Entwurfselement_LA_list;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_KM_list = Entwurfselement_KM_list;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_HO_list = Entwurfselement_HO_list;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_UH_list = Entwurfselement_UH_list;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_gleiskantenList = gleiskantenList;
                System.Collections.ObjectModel.ObservableCollection<CustomNode> obsCustomPolyline_gleisknotenList = gleisknotenList;
                System.Collections.ObjectModel.ObservableCollection<netDxf.Entities.Polyline> obsDxfpolyLine = new System.Collections.ObjectModel.ObservableCollection<netDxf.Entities.Polyline>();
                System.Collections.ObjectModel.ObservableCollection<netDxf.Entities.Point> obsDxfpoints = new System.Collections.ObjectModel.ObservableCollection<netDxf.Entities.Point>();

                netDxf.Entities.Point globalPoint = new netDxf.Entities.Point(DrawViewModel.GlobalDrawingPoint.X, DrawViewModel.GlobalDrawingPoint.Y, 0.0);
                globalPoint.Layer = layer8_GlobalPoint;


                foreach (CustomPolyLine polyLne in obsCustomPolyline_Entwurfselement_LA_list)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    dxfPolyline.Layer = layer1_LA;

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }

                foreach (CustomPolyLine polyLne in obsCustomPolyline_Entwurfselement_KM_list)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    dxfPolyline.Layer = layer2_KM;

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }
                foreach (CustomPolyLine polyLne in obsCustomPolyline_Entwurfselement_HO_list)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    dxfPolyline.Layer = layer3_HO;

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }
                foreach (CustomPolyLine polyLne in obsCustomPolyline_Entwurfselement_UH_list)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    dxfPolyline.Layer = layer4_UH;

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }
                foreach (CustomPolyLine polyLne in obsCustomPolyline_gleiskantenList)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    dxfPolyline.Layer = layer5_gleiskanten;

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }
                foreach (CustomNode customNode in obsCustomPolyline_gleisknotenList)
                {
                    netDxf.Vector3 vec = new netDxf.Vector3((double)customNode.NodePoint.X, (double)customNode.NodePoint.Y, 0.0);
                    netDxf.Entities.Point point = new netDxf.Entities.Point(vec);

                    point.Layer = layer6_gleisknoten;

                    obsDxfpoints.Add(point);
                }

                //foreach(var d in DrawViewModel.toBeStored)
                //{
                //    if(d is CustomCanvasSignal element && element.GetType() == typeof(CustomCanvasSignal))
                //    { 

                //        System.Windows.Media.ImageSource img = element.Source;                        
                //        System.Drawing.Image dimg = ImageWpfToGDI(img);                        
                //        dimg.Save(@"D:\\somepath.png");
                //        //using (FileStream file = File.Create(@"D:\\somepath.png"))
                //        //{
                //        //    tempImage = System.Drawing.Image.FromStream(file);

                //        //}                        
                //        string imgFile = @"D:\\somepath.png";
                //        //System.Drawing.Image img = System.Drawing.Image.FromFile(imgFile);
                //        netDxf.Objects.ImageDefinition imageDefinition = new netDxf.Objects.ImageDefinition("MyImage", imgFile, 100, 10, 100, 10, ImageResolutionUnits.Inches);
                //        netDxf.Entities.Image image = new netDxf.Entities.Image(imageDefinition, netDxf.Vector3.Zero, 10, 10);
                //        image.Layer = layer7_symbolImage;
                //        dxf.AddEntity(image);
                //        //netDxf.Entities.Image img = new netDxf.Entities.Image(element,);

                //    }

                //}
                

                foreach (netDxf.Entities.Point point in obsDxfpoints)
                {
                    dxf.AddEntity(point);
                }
                dxf.AddEntity(globalPoint);

                if(obsArcList != null)
                {
                    foreach (CustomArc newArc in obsArcList)
                    {
                        netDxf.Entities.Arc dxfArc = new netDxf.Entities.Arc();
                        dxfArc.Center = new netDxf.Vector3(newArc.Center.X, newArc.Center.Y, 0.0);
                        dxfArc.Radius = newArc.Radius;
                        dxfArc.EndAngle = (180 / Math.PI) * Math.Acos((newArc.EndPoint.X - newArc.Center.X) / newArc.Radius);
                        dxfArc.StartAngle = (180 / Math.PI) * Math.Acos((newArc.StartPoint.X - newArc.Center.X) / newArc.Radius);
                        
                        dxfArc.Thickness = newArc.Thickness;

                        dxf.AddEntity(dxfArc);

                    }
                }

                if (obslineList != null)
                {
                    foreach (CustomLine newLine in obslineList)
                    {
                        netDxf.Entities.Line dxfLine = new netDxf.Entities.Line();
                        dxfLine.StartPoint = new netDxf.Vector3(newLine.StartingPoint.X, newLine.StartingPoint.Y, 0.0);
                        dxfLine.EndPoint = new netDxf.Vector3(newLine.Endpoint.X, newLine.Endpoint.Y, 0.0);              

                        

                        dxf.AddEntity(dxfLine);

                    }
                }

                if (obsEllipseList != null)
                {
                    foreach (CustomEllipse newEllipse in obsEllipseList)
                    {
                        netDxf.Entities.Ellipse dxfEllipse = new netDxf.Entities.Ellipse();
                        dxfEllipse.Center = new netDxf.Vector3(newEllipse.EllipseVertexCenter.X, newEllipse.EllipseVertexCenter.Y, 0.0);
                        dxfEllipse.MajorAxis = newEllipse.RadiusX * 2;
                        dxfEllipse.MinorAxis = newEllipse.RadiusX * 2;
                        dxfEllipse.Thickness = newEllipse.Thickness; ;
                        dxf.AddEntity(dxfEllipse);

                    }
                }
               
                
                 foreach (CustomPolyLine polyLne in obsPolylineList)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                     

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }

                foreach (CustomPolyLine polyLne in obsLwPolylineList)
                {
                    PointCollection points = polyLne.Points;
                    foreach (System.Windows.Point vertexPoint in points)
                    {
                        netDxf.Vector3 vec = new netDxf.Vector3((double)vertexPoint.X, (double)vertexPoint.Y, 0.0);
                        //polylineVertex.Position = vec;
                        polylineVertexList.Add(vec);
                    }
                    netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);
                    

                    obsDxfpolyLine.Add(dxfPolyline);
                    polylineVertexList.Clear();
                }

                

                foreach (UIElement element in DrawViewModel.toBeStored)
                {
                    if(element is System.Windows.Shapes.Line)
                    {
                        System.Windows.Shapes.Line canvasLine = (System.Windows.Shapes.Line)element;
                        netDxf.Vector3 vecStartPoint = new netDxf.Vector3((canvasLine.X1 + DrawViewModel.GlobalDrawingPoint.X) - DrawViewModel.sharedCanvasSize/2 , -canvasLine.Y1+ (DrawViewModel.sharedCanvasSize)/2 + DrawViewModel.GlobalDrawingPoint.Y , 0.0);
                        netDxf.Vector3 vecEndPoint = new netDxf.Vector3((canvasLine.X2 + DrawViewModel.GlobalDrawingPoint.X) - DrawViewModel.sharedCanvasSize / 2, -canvasLine.Y2 + (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y, 0.0);
                        netDxf.Entities.Line dxfLine = new netDxf.Entities.Line();
                        dxfLine.StartPoint = vecStartPoint;
                        dxfLine.EndPoint = vecEndPoint;
                        dxfLine.Thickness = canvasLine.StrokeThickness;
                        dxf.AddEntity(dxfLine);
                    }
                    else if(element is System.Windows.Shapes.Ellipse)
                    {
                        System.Windows.Shapes.Ellipse canvasEllipse = (System.Windows.Shapes.Ellipse)element;
                        double leftCanvasEllipse = System.Windows.Controls.Canvas.GetLeft(element);
                        double topCanvasEllipse = System.Windows.Controls.Canvas.GetTop(element);
                        double ellipseVertexX = leftCanvasEllipse + ((canvasEllipse.Width) / 2);
                        double ellipseVertexY = topCanvasEllipse + ((canvasEllipse.Height) / 2);
                        netDxf.Vector3 vecCenterPoint = new netDxf.Vector3((ellipseVertexX + DrawViewModel.GlobalDrawingPoint.X) - DrawViewModel.sharedCanvasSize / 2, -ellipseVertexY + (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y, 0.0);
                        netDxf.Entities.Ellipse dxfEllipse = new netDxf.Entities.Ellipse();
                        dxfEllipse.Center = vecCenterPoint;
                        dxfEllipse.MajorAxis = canvasEllipse.Width;
                        dxfEllipse.MinorAxis = canvasEllipse.Height;
                        dxfEllipse.Thickness = canvasEllipse.StrokeThickness;
                        dxf.AddEntity(dxfEllipse);
                    }
                    else if(element is System.Windows.Shapes.Polyline)
                    {
                        System.Windows.Shapes.Polyline canvasPolyline = (System.Windows.Shapes.Polyline)element;
                        PointCollection polylinePoints = canvasPolyline.Points;
                        double xCoordinatesAdjust = DrawViewModel.GlobalDrawingPoint.X - (DrawViewModel.sharedCanvasSize / 2);
                        double yCoordinatesAdjust = (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y;
                        foreach (System.Windows.Point vertexPoint in polylinePoints)
                        {
                            netDxf.Vector3 vec = new netDxf.Vector3((double)(vertexPoint.X + xCoordinatesAdjust), (double)(-vertexPoint.Y + yCoordinatesAdjust), 0.0);
                            
                            polylineVertexList.Add(vec);
                        }
                        netDxf.Entities.Polyline dxfPolyline = new netDxf.Entities.Polyline(polylineVertexList);                        

                        obsDxfpolyLine.Add(dxfPolyline);
                        polylineVertexList.Clear();

                    }
                    
                }
                foreach (netDxf.Entities.Polyline polylne in obsDxfpolyLine)
                {
                    dxf.AddEntity(polylne);
                }

                safeFileDialog1.Filter = "Data Files (*.dxf)|*.dxf";
                safeFileDialog1.DefaultExt = "dxf";
                safeFileDialog1.AddExtension = true;
                
                if (safeFileDialog1.ShowDialog() == true)
                {
                    //File.WriteAllText(safeFileDialog1.FileName, txtEditor.Text);
                    dxf.Save(safeFileDialog1.FileName);
                }

            }
        }

        /// <summary>
        /// Extract the onTrack signals from the Eulynx object.
        /// </summary>
        /// <param name="EulynxObject"></param>
        public static void extractOnTrackSignals(EulynxDataPrepInterface EulynxObject)
        {
            var dataPrepEntities = EulynxObject.hasDataContainer.First().ownsDataPrepEntities;
            List<Unit> units = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesUnit;
            List<Models.TopoModels.EULYNX.rsmSig.OnTrackSignallingDevice> onTrackSignals = EulynxObject.hasDataContainer.First().ownsRsmEntities.ownsOnTrackSignallingDevice;
            List<BaseLocation> locations = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesLocation;
            List<PositioningSystemCoordinate> PSCoordinates = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate;
            List<IntrinsicCoordinate> intrCoordinates = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate;
            List<PositioningNetElement> netElements = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTrackTopology.usesNetElement;
            foreach (Models.TopoModels.EULYNX.rsmSig.OnTrackSignallingDevice signal in onTrackSignals)
            {
                //dp function
                var euBalise = dataPrepEntities.ownsTrackAsset.First(x => x is EtcsBalise && ((EtcsBalise)x).refersToRsmTpsDevice.@ref == signal.id) as EtcsBalise;
                var euBaliseGroupId = euBalise.implementsTpsDataTxSystem.@ref;
                var euBaliseProperty = dataPrepEntities.ownsTpsDataTransmissionSystemProperties.First(x => x is EtcsBaliseGroupLevel2 && ((EtcsBaliseGroupLevel2)x).appliesToTpsDataTxSystem.@ref == euBaliseGroupId) as EtcsBaliseGroupLevel2;
                var euBaliseFunction = euBaliseProperty.implementsFunction.First().type;


                // get axle counter's location and print it
                var spotLoc = locations.Find(item => item.id == signal.locations.First().@ref) as Models.TopoModels.EULYNX.rsmCommon.SpotLocation; // dereference
                var tpsDeviceIntrinsicCoordinateRef = spotLoc.associatedNetElements.First().bounds.First();
                var tpsDeviceIntrinsicCoordinate = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate.First(item => item.id == tpsDeviceIntrinsicCoordinateRef.@ref);
                var tpsDeviceCoordinate = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.First(item => item.id == tpsDeviceIntrinsicCoordinate.coordinates.First().@ref) as LinearCoordinate;
                var tpsDeviceKmValue = tpsDeviceCoordinate.measure.value.Value;


                AssociatedNetElement assElement = spotLoc.associatedNetElements[0];
                tElementWithIDref bound = assElement.bounds[0];
                IntrinsicCoordinate intcoord = intrCoordinates.Find(x => x.id.Equals(bound.@ref));
                LinearCoordinate SignalIntrensic = PSCoordinates.Find(x => x.id.Equals(intcoord.coordinates[0].@ref)) as LinearCoordinate;



                //get relatedNetElement
                var element = spotLoc.associatedNetElements[0].netElement;
                PositioningNetElement netElement = netElements.Find(x => x.id.Equals(element.@ref));
                aplan.database.NetElement dataBaseElement = null;
                using (var liteDB = ModelViewModel.db.accessDB())
                {
                    dataBaseElement = (liteDB).GetCollection<aplan.database.NetElement>("NetElements").Find(x => x.uuid== element.@ref).FirstOrDefault();
                }
                netElement = netElements.Find(x => x.id.Equals(dataBaseElement.uuid));

                List<IntrinsicCoordinate> elementIntrensics_initial = netElement.associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate elementStartKm_initial = PSCoordinates.Find(x => x.id.Equals(elementIntrensics_initial[0].coordinates[0].@ref)) as LinearCoordinate;
                LinearCoordinate elementEndKm_initial = PSCoordinates.Find(x => x.id.Equals(elementIntrensics_initial[1].coordinates[0].@ref)) as LinearCoordinate;


                // if the km value is not between the the NetElement range.
                if ((double)intcoord.value > 1 || SignalIntrensic.measure.value<elementStartKm_initial.measure.value)
                {
                    getNeighbors(netElement, EulynxObject, SignalIntrensic);
                    if (neededElement != null)
                    {
                        netElement = neededElement;
                    }
                }

                Unit elementLenthunit = units.Find(x => x.id.Equals(((LinearElementWithLength)netElement).elementLength.quantiy[0].unit.@ref));
                List<IntrinsicCoordinate> elementIntrensics = netElement.associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate elementStartKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[0].coordinates[0].@ref)) as LinearCoordinate;
                LinearCoordinate elementEndKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[1].coordinates[0].@ref)) as LinearCoordinate;
                List<System.Windows.Point> points = getNetElementCartesianCoordinates(netElement, PSCoordinates);
                var newSignal = new Signalinfo()
                {
                    SignalImageSource = new BitmapImage(new Uri(@"/Resources/SymbolsImages/BalisengruppeGesteuertTri.png", UriKind.RelativeOrAbsolute)),
                    LongName = signal.longname,
                    Name = signal.name,
                    Function = euBaliseFunction,
                    IntrinsicValue = tpsDeviceKmValue,
                    AttachedToElementname = netElement.name,
                    Side = assElement.isLocatedToSide.ToString(),
                    Direction = assElement.appliesInDirection.ToString(),
                    Coordinates = points,
                    AttachedToElementLength = ((Length)((LinearElementWithLength)netElement).elementLength.quantiy[0]).value,
                    LateralDistance = 3.1
                    
                };
                calculateSignalLocation(newSignal, SignalIntrensic, elementStartKm, elementEndKm);
                Signals.Add(newSignal);
            }
        }

        /// <summary>
        /// Extract the main signals from the Eulynx object.
        /// </summary>
        /// <param name="EulynxObject"></param>
        public static void extractMainSignals(EulynxDataPrepInterface EulynxObject)
        {
            List<Unit> units = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesUnit;
            List<Models.TopoModels.EULYNX.sig.SignalFrame> signalFrames = EulynxObject.hasDataContainer.First().ownsDataPrepEntities.ownsSignalFrame;
            List<PositioningSystemCoordinate> PSCoordinates = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate;
            List<PositioningNetElement> netElements = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTrackTopology.usesNetElement;
            List<BaseLocation> locations = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesLocation;
            List<IntrinsicCoordinate> intrCoordinates = EulynxObject.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate;
            List<Models.TopoModels.EULYNX.rsmSig.Signal> mainSignals = EulynxObject.hasDataContainer.First().ownsRsmEntities.ownsSignal;

            foreach (Models.TopoModels.EULYNX.rsmSig.Signal signal in mainSignals)
            {

                tElementWithIDref loc = signal.locations[0];
                Models.TopoModels.EULYNX.rsmCommon.SpotLocation spotLoc = (Models.TopoModels.EULYNX.rsmCommon.SpotLocation)locations.Find(x => x.id.Equals(loc.@ref));
                AssociatedNetElement assElement = spotLoc.associatedNetElements[0];
                tElementWithIDref bound = assElement.bounds[0];
                IntrinsicCoordinate intcoord = intrCoordinates.Find(x => x.id.Equals(bound.@ref));
                LinearCoordinate SignalIntrensic = PSCoordinates.Find(x => x.id.Equals(intcoord.coordinates[0].@ref)) as LinearCoordinate;
                PositioningNetElement element = netElements.Find(x => x.id.Equals(assElement.netElement.@ref));
                if ((double)intcoord.value>1)
                {
                    getNeighbors(element, EulynxObject, SignalIntrensic);
                    if (neededElement != null)
                    {
                        element = neededElement;
                    } 
                }
                
                Unit elementLenthunit = units.Find(x => x.id.Equals(((LinearElementWithLength)element).elementLength.quantiy[0].unit.@ref));
                List<IntrinsicCoordinate> elementIntrensics = element.associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate elementStartKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[0].coordinates[0].@ref)) as LinearCoordinate;
                LinearCoordinate elementEndKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[1].coordinates[0].@ref)) as LinearCoordinate;
                List<System.Windows.Point> points = getNetElementCartesianCoordinates(element, PSCoordinates);


                //signal type.
                var typeFunction = extractSignalTypeAndFunction(signal, EulynxObject);

                var newSignal = new Signalinfo()
                {
                    SignalImageSource = new BitmapImage(new Uri(@"/Resources/SymbolsImages/mehraZugoderStellwerksbedient.png", UriKind.RelativeOrAbsolute)),
                    LongName = signal.longname,
                    Name = signal.name,
                    Type= typeFunction[0],
                    Function = typeFunction[1],
                    AttachedToElementname = element.name,
                    IntrinsicValue = (double)SignalIntrensic.measure.value,
                    Side = assElement.isLocatedToSide.ToString(),
                    Direction = assElement.appliesInDirection.ToString(),
                    Coordinates = points,
                    AttachedToElementLength = ((Length)((LinearElementWithLength)element).elementLength.quantiy[0]).value,
                    LateralDistance = 3.1
        };
                calculateSignalLocation(newSignal, SignalIntrensic, elementStartKm, elementEndKm);
                Signals.Add(newSignal);
                //extract lateral distance
                //foreach (SignalFrame s in signalFrames)
                //{

                //}
            }

        }

        /// <summary>
        /// get neighboring elements according to relations between the elements.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="EulynxObject"></param>
        /// <param name="SignalIntrensic"></param>
        private static void getNeighbors(PositioningNetElement element, EulynxDataPrepInterface EulynxObject, LinearCoordinate SignalIntrensic)
        {
            VisitedElements.Add(element);
            PositioningNetElement hostElement =null;
            List<PositionedRelation> relations=  EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesPositionedRelation;
            List<PositionedRelation> targetRelations= relations.FindAll(x => ((x.elementA.@ref== element.id) || (x.elementB.@ref == element.id)));
            List<PositioningNetElement> relatedElements = new List<PositioningNetElement>();
            foreach (PositionedRelation relation in targetRelations)
            {
                PositioningNetElement elementA = EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesNetElement.Find(x=>x.id.Equals(relation.elementA.@ref));
                PositioningNetElement elementB = EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesNetElement.Find(x => x.id.Equals(relation.elementB.@ref));

                if (!relatedElements.Contains(elementA) && !VisitedElements.Contains(elementA))
                {
                    relatedElements.Add(elementA);
                }
                if (!relatedElements.Contains(elementB) && !VisitedElements.Contains(elementB))
                {
                    relatedElements.Add(elementB);
                }
            }
            for ( int i=0; i< relatedElements.Count; i++)
            {
                List<IntrinsicCoordinate> intCoordiante= relatedElements[i].associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate startKm =(LinearCoordinate) EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.Find(pos => pos.id.Equals(intCoordiante[0].coordinates[0].@ref));
                LinearCoordinate endKm = (LinearCoordinate)EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.Find(pos => pos.id.Equals(intCoordiante[1].coordinates[0].@ref));
                if (startKm.measure.value!=null)
                {
                    if (startKm.measure.value<= SignalIntrensic.measure.value && endKm.measure.value >= SignalIntrensic.measure.value)
                    {
                        neededElement = relatedElements[i];
                        hostElement= relatedElements[i];
                        break;
                    }
                }
                else
                {
                    relatedElements.Remove(relatedElements[i]);
                    i--;
                }
            }

            if (hostElement==null)
            {
                foreach (PositioningNetElement elem in relatedElements)

                {
                    getNeighbors(elem, EulynxObject, SignalIntrensic);
                }
            }
        }
        private static  List<System.Windows.Point> getNetElementCartesianCoordinates(PositioningNetElement positioningNetElements,List<PositioningSystemCoordinate> PSCoordinates)
        {
            List<System.Windows.Point> points = new List<System.Windows.Point>();
            AssociatedPositioning associatedPositionings = positioningNetElements.associatedPositioning[1];
            List<IntrinsicCoordinate> intrinsicCoordinates = associatedPositionings.intrinsicCoordinates;
            foreach (IntrinsicCoordinate intrinsicCoordinate in intrinsicCoordinates)
            {
                List<tElementWithIDref> tElementWithIDrefs = intrinsicCoordinate.coordinates;
                foreach (tElementWithIDref tElementWithIDref in tElementWithIDrefs)
                {
                    CartesianCoordinate cartCoordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(tElementWithIDref.@ref));
                    System.Windows.Point newPoint = new System.Windows.Point((((double)cartCoordinate.x)), ((double)cartCoordinate.y));
                    points.Add(newPoint);
                }
            }
            return points;
        }
        private static void calculateSignalLocation(Signalinfo signal, LinearCoordinate signalIntrensic, LinearCoordinate elementStart, LinearCoordinate elementEnd)
        {

            double? targetValue = (signalIntrensic.measure.value - elementStart.measure.value)*1000 / signal.AttachedToElementLength;
       
            double? targetLengthLocation = targetValue * signal.AttachedToElementLength;
            
            double currentLength = 0;
            for (int i=0; i<signal.Coordinates.Count-1; i++)
            {
                currentLength += Math.Sqrt(Math.Pow((signal.Coordinates[i].X - signal.Coordinates[i + 1].X), 2.0) + Math.Pow((signal.Coordinates[i].Y - signal.Coordinates[i + 1].Y),2.0));
                if (currentLength>targetLengthLocation)
                {
                    double? factor = targetLengthLocation / currentLength;
                    double xdiff = signal.Coordinates[i+1].X - signal.Coordinates[i].X;
                    double ydiff = signal.Coordinates[i+1].Y - signal.Coordinates[i].Y;
                    applyRotation(signal, xdiff, ydiff);
                    signal.LocationCoordinate = new System.Windows.Point((double)(signal.Coordinates[i].X+ xdiff* factor), (double)(signal.Coordinates[i].Y +ydiff * factor));
                    applyHorizontalOffset(signal, signal.Coordinates[i + 1]);
                    applyDirection(signal);
                    break;
                }
            }
            if (signal.LocationCoordinate.X == 0)
            {
                signal.LocationCoordinate = new System.Windows.Point((double)(signal.Coordinates[signal.Coordinates.Count - 1].X) - DrawViewModel.GlobalDrawingPoint.X, (double)(signal.Coordinates[signal.Coordinates.Count - 1].Y) - DrawViewModel.GlobalDrawingPoint.Y);
            }
        }
        /// <summary>
        /// apply rotation on signal
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="xdiff"></param>
        /// <param name="ydiff"></param>
        private static void applyRotation(Signalinfo signal, double xdiff, double ydiff)
        {
            //negative sign for drawing
            if (ydiff>=0 && xdiff>=0)
            {
                signal.Orientation = -Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI);
            }else if (ydiff < 0 && xdiff < 0)
            {
                signal.Orientation = -Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI)-180;
            }
            else if (ydiff > 0 && xdiff < 0)
            {
                signal.Orientation = -(180-Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI));
            }
            else if (ydiff < 0 && xdiff > 0)
            {
                signal.Orientation = Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI);
            }

        }
        /// <summary>
        /// apply the horizontal offset required for a signal.
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="backward"></param>
        private static void applyHorizontalOffset(Signalinfo signal, System.Windows.Point backward)
        {
            var x = signal.LocationCoordinate.X;
            var y = signal.LocationCoordinate.Y;
            var shift = 10;

            var shiftx = 0.0;
            var shifty = 0.0;
            if (signal.Side == "right")
            {
                if (Math.Abs(signal.Orientation) <= 90)
                {
                    shiftx = shift * Math.Sin(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                    shifty = -shift * Math.Cos(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 180 && Math.Abs(signal.Orientation) > 90)
                {
                    shiftx = shift * Math.Cos((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                    shifty = shift * Math.Sin((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 270 && Math.Abs(signal.Orientation) > 180)
                {

                    shiftx = -shift * Math.Sin((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                    shifty = shift * Math.Cos((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 360 && Math.Abs(signal.Orientation) > 270)
                {
                    shiftx = -shift * Math.Cos((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                    shifty = -shift * Math.Sin((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                }

                }
                else if (signal.Side == "left")
                {
                if (Math.Abs(signal.Orientation) <= 90)
                {
                    shiftx = -shift * Math.Sin(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                    shifty = shift * Math.Cos(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 180 && Math.Abs(signal.Orientation) > 90)
                {
                    shiftx = -shift * Math.Cos((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                    shifty = -shift * Math.Sin((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 270 && Math.Abs(signal.Orientation) > 180)
                {

                    shiftx = shift * Math.Sin((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                    shifty = -shift * Math.Cos((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 360 && Math.Abs(signal.Orientation) > 270)
                {
                    shiftx = shift * Math.Cos((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                    shifty = shift * Math.Sin((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                }

            }
            x += shiftx;
            y += shifty;

            signal.LocationCoordinate = new System.Windows.Point(x, y);
        }
        /// <summary>
        /// apply the direction to the signal
        /// </summary>
        /// <param name="signal"></param>
        private static void applyDirection(Signalinfo signal)
        {
            if (signal.Direction.Equals("2") || signal.Direction.Equals("reverse"))
            {
                signal.Orientation = signal.Orientation-180;
            }
        }
        private static string [] extractSignalTypeAndFunction(Models.TopoModels.EULYNX.rsmSig.Signal signal , EulynxDataPrepInterface EulynxObject)
        {
            string type = "notFound" ;
            string function= "notFound";

            var ownsSignal = EulynxObject.hasDataContainer[0].ownsRsmEntities.ownsSignal;
            var ownsSignalType = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsSignalType;
            var ownsSignalFunction = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsSignalFunction;
            var ownsTrackAsset = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsTrackAsset;

            LightSignalTyped trackAsset=null;
            foreach (Models.TopoModels.EULYNX.db.SignalType signalType in ownsSignalType)
            {
                trackAsset = ownsTrackAsset.Find(x => x.id.Equals(signalType.appliesToSignal.@ref)) as LightSignalTyped;
                Models.TopoModels.EULYNX.rsmSig.Signal current_signal = ownsSignal.Find(x => x.id.Equals(trackAsset.refersToRsmSignal.@ref));
                if (current_signal.id == signal.id)
                {
                    foreach (Models.TopoModels.EULYNX.db.SignalFunction signalFunction in ownsSignalFunction)
                    {
                        if (signalFunction.appliesToSignal.@ref.Equals(signalType.appliesToSignal.@ref))
                        {
                            function = signalFunction.isOfSignalFunctionType?.ToString();
                        }
                    }
                    type = signalType.isOfSignalTypeType?.ToString();
                }
            }

            return  new string[2] { type, function };
        }
        #endregion
    }
}
