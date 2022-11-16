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

namespace APLan.ViewModels
{
    public class PlanningTabViewModel : BaseViewModel
    {
        #region attributes
        private string planType;
        private string exportType;
        private Visibility pdfDetailViewerVisibility;
        private Microsoft.Win32.SaveFileDialog safeFileDialog1;
       


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
       

        #endregion

        #region commands
        public ICommand PlanButton { get; set; }
        public ICommand ExportButton { get; set; }
        #endregion

        #region constructor
        public PlanningTabViewModel()
        {
            PlanButton = new RelayCommand(ExecutePlanButton);
            ExportButton = new RelayCommand(ExecuteExportButton);
            Signals = new ObservableCollection<Signalinfo>();
            safeFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            pdfDetailViewerVisibility = Visibility.Collapsed;
        }
        #endregion

        #region logic
        private void ExecutePlanButton(object parameter)
        {
            var EulynxObject= ModelViewModel.eulynx;
            var eulynxService = ModelViewModel.eulynxService;
            Database dataBase = ModelViewModel.db;

            bool etcs = false;
            if (EulynxObject == null)
            {
                MessageBox.Show("No current Eulynx object");

            }else if (Database.checkDB() == false)
            {
                MessageBox.Show("No database found for planning");
            }else if (ModelViewModel.db!=null)
            {
                var count = 0;
                using (var signals = ModelViewModel.db.accessDB())
                {
                    var collection= signals.GetCollection("Signals");
                    count=collection.Count();
                }
                if (count == 0)
                {
                    if (PlanType == "ETCS" && EulynxObject != null)
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
                else
                {
                    MessageBox.Show("Already Planned");
                }
            }
        }

        private void ExecuteExportButton(object parameter)
        {
            if (exportType == "Eulynx")
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
                foreach (netDxf.Entities.Polyline polylne in obsDxfpolyLine)
                {
                    dxf.AddEntity(polylne);
                }

                foreach (netDxf.Entities.Point point in obsDxfpoints)
                {
                    dxf.AddEntity(point);
                }
                dxf.AddEntity(globalPoint);

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
        #endregion
    }
}
