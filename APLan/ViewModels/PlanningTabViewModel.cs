using APLan.Commands;
using APLan.Views;

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Controls;
using System.Windows.Xps;
using APLan.ViewModels.ModelsLogic;
using aplan.eulynx;
using APLan.Model.CustomObjects;
using netDxf.Tables;
using netDxf.Entities;
using netDxf;
using System.Collections.Generic;

using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using System.Drawing;
using System.Data;

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

            safeFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            pdfDetailViewerVisibility = Visibility.Collapsed;
        }
        #endregion

        #region command logic
        private void ExecutePlanButton(object parameter)
        {
         
            if (eulynxModel == null)
            {
                MessageBox.Show("No current Eulynx object");

            }else if (ProjectType != "EULYNX")
            {
                MessageBox.Show("Planning is only avalaibe for EULYNX project");
            }
            else if (PlanType != "ETCS")
            {
                MessageBox.Show("Kindly Select a Plan type");
            }
            else if (EulynxModelHandler.db == null)
            {
                MessageBox.Show("no database to preform planning");
            }else if (Signals.Count>0)
            {
                MessageBox.Show("already planned");
            }
            else
            {
                EulynxModelHandler eulynxModelHandler = new();
                var eulynxService = EulynxService.getInstance();

                eulynxService.plan(eulynxModel, EulynxModelHandler.db, true);
                eulynxModelHandler.extractMainSignals(eulynxModel,Signals);
                eulynxModelHandler.extractOnTrackSignals(eulynxModel,Signals);

                MessageBox.Show("planning was successfull");
            }
        }
        private void ExecuteImportButton(object parameter)
        {
            if (ImportType == "Image")
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
                    Images.Add(customImage);
                }
            }
        }
        private void ExecuteExportButton(object parameter)
        {
            if (exportType == "Eulynx" || exportType == "JSON(ERDM)")
            {
                ExportWindow export = new ExportWindow();
                export.ShowDialog();
            }
            else if (exportType == "Eulynx" || exportType == "XML(ERDM)")
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
            else if (exportType == "pdfwithtables")
            {
                PdfDocument doc = new PdfDocument();
                PdfPageBase page = doc.Pages.Add(PdfPageSize.A2);
                PdfTable table = new PdfTable();

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("   ");
                dataTable.Columns.Add("Bezeichnung Haupt- u. Vorsignale");
                dataTable.Columns.Add("[22]");
                dataTable.Columns.Add("[24]");
                dataTable.Columns.Add("[26]");
                dataTable.Columns.Add(" ");
                dataTable.Columns.Add("Vf");
                dataTable.Columns.Add("[Vff]");
                dataTable.Columns.Add("F");
                dataTable.Columns.Add("[FF]");
                dataTable.Columns.Add("  ");
                dataTable.Columns.Add("[ZU1]");
                dataTable.Columns.Add("[ZU2]");
                dataTable.Columns.Add("[ZU3]");


                dataTable.Rows.Add(new string[] { " ", "Bezeichnung Rangiersignale / Schutzsignale", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "1 ", "Bezeichnung Signale", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "2 ", "Standort [km] / Strecke", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "3 ", "Standort [km] / Strecke", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "4 ", "sonstige zulässige Anordnung", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "5 ", "obere Lichtpunkthöhe [mm]", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "6 ", "Rz S8000.5.6 Bild Nr.", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "7 ", "Rz S6250/2440 Blatt 15 Bild Nr.", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "8 ", "Signalsicht (Soll-/Mindestsignalsicht) [m]", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "9 ", "Richtpunktentfernung [m]", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "10 ", "Streuscheibe / Betriebsstellung", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });

                dataTable.Rows.Add(new string[] { "11 ", "Fundament Art / Höhe [mm] 2), 7)", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });
                dataTable.Rows.Add(new string[] { "12 ", "Sonderkonstruktion", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " });


                table.DataSource = dataTable;

                int rowsNumber = table.Rows.Count + 1;
                int colsNumber = table.Columns.Count;

                table.Style.ShowHeader = true;
                table.Style.CellPadding = 15;
                table.Style.HeaderSource = PdfHeaderSource.ColumnCaptions;
                table.Style.HeaderRowCount = 1;

                table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.LightSeaGreen;
                table.Style.HeaderStyle.Font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, System.Drawing.FontStyle.Bold));
                table.Style.HeaderStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Left);
                table.Style.HeaderStyle.TextBrush = PdfBrushes.White;

                float widthofPage = page.Canvas.ClientSize.Width;

                float heightofPage = page.Canvas.ClientSize.Height;

                float tableWidth = 0;
                tableWidth += table.Columns[0].Width;
                table.Columns[0].Width = widthofPage * 0.1f * widthofPage;

                table.Columns[1].StringFormat
                    = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                tableWidth += table.Columns[1].Width;
                table.Columns[1].Width = widthofPage * 0.32f * widthofPage;

                for (int i = 2; i < 14; i++)
                {
                    table.Columns[i].StringFormat
                    = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    tableWidth += table.Columns[i].Width;
                    table.Columns[i].Width = widthofPage * 0.1f * widthofPage;

                }


                table.BeginRowLayout += Table_BeginRowLayout;
                float y = 20f;
                PdfLayoutResult result = table.Draw(page, new RectangleF(0, y, 900, 1604));
                //PdfLayoutResult result = table.Draw(page, new PointF(0, y));
                y = y + (rowsNumber - 1) * (25f + 2 * table.Style.CellPadding);  //25f is the minimum row height--> Table_BeginRowLayout





                Spire.Pdf.Graphics.Layer.PdfLayer layer = doc.Layers.AddLayer("Image Layer");
                SizeF size = doc.Pages[0].Size;

                int pageCount = doc.Pages.Count;
                PdfCanvas canvas;
                for (int i = 0; (i < pageCount); i++)
                {
                    //Draw an image on the layer
                    var dbform = APLan.Properties.Resources.dblogo;
                    PdfImage pdfImage = PdfImage.FromFile(@"../../../Resources/Images/dbform2.png");

                    float widthImage = pdfImage.Width;
                    float heightImage = pdfImage.Height;
                    page = doc.Pages[i];
                    canvas = layer.CreateGraphics(page.Canvas);

                    canvas.DrawImage(pdfImage, 550, y, widthImage - 350, heightImage - 200);

                    ////Draw a line on the layer
                    //PdfPen pen = new PdfPen(PdfBrushes.DarkGray, 2);
                    //canvas.DrawLine(pen, x, (y + (height + 5)), (size.Width - x), (y + (height + 2)));
                }

                PdfBrush brush1 = PdfBrushes.Black;
                PdfTrueTypeFont font2 = new PdfTrueTypeFont(new Font("Times New Roman", 12f, System.Drawing.FontStyle.Underline));
                PdfTrueTypeFont font3 = new PdfTrueTypeFont(new Font("Times New Roman", 12f, System.Drawing.FontStyle.Regular));
                page.Canvas.DrawString("Erläuterungen", font2, brush1, 0, y);
                page.Canvas.DrawString("Anmerkungen:", font2, brush1, 250, y);
                y = y + 25;
                page.Canvas.DrawString("B/[m]    Beton/Fundamentlänge", font3, brush1, 0, y);
                page.Canvas.DrawString("1) Festlegung der verbindlichen Signalstandorte vor Ort", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("S        Sonderfundament", font3, brush1, 0, y);
                page.Canvas.DrawString("unter der Beachtung der Signalsicht gemäß Ril 819.0202", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("Mvk      Mastvorderkante", font3, brush1, 0, y);
                page.Canvas.DrawString("und der örtlichen Verhältnisse vornehmen.", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("Fvk      Fundamentvorderkante", font3, brush1, 0, y);
                page.Canvas.DrawString("Sonstige zulässige Anordnungen, die nicht", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("H        Mastschild Hauptsignal Ws/rt/ws", font3, brush1, 0, y);
                page.Canvas.DrawString("der Regelanordnung entsprechen, sind so anzugeben,", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("V        Mastschild Vorsignal", font3, brush1, 0, y);
                page.Canvas.DrawString("dass die Eintragungen in den betrieblichen Unterlagen", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("Sp       Mastschild", font3, brush1, 0, y);
                page.Canvas.DrawString("Sperrsignal des Fahrpersonals abgeleitet werden können.", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("O        Ortsfundament", font3, brush1, 0, y);
                page.Canvas.DrawString(" *)         Nichtgeltung für Fahrten auf dem Gegengleis", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("z        zugbedient", font3, brush1, 0, y);
                page.Canvas.DrawString(" **)        Anordnung des Signals rechts am Gleis", font3, brush1, 250, y);
                y = y + 20;
                page.Canvas.DrawString("w        wärterbedient", font3, brush1, 0, y);
                page.Canvas.DrawString(" ***)       Anordnung des Signals rechts am Gleis", font3, brush1, 250, y);





                // save and launch the file
                safeFileDialog1.Filter = "Data Files (*.pdf)|*.pdf";
                safeFileDialog1.DefaultExt = "pdf";
                safeFileDialog1.AddExtension = true;

                if (safeFileDialog1.ShowDialog() == true)
                {

                    doc.SaveToFile(safeFileDialog1.FileName);
                    doc.Close();
                }

                //System.Diagnostics.Process.Start("SimpleTable.pdf");


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


                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsPolylineList = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obslineList = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsLwPolylineList = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomCircle> obsEllipseList = Ellipses;
                System.Collections.ObjectModel.ObservableCollection<CustomArc> obsArcList = Arcs;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_LA_list = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_KM_list = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_HO_list = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_Entwurfselement_UH_list = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomPolyLine> obsCustomPolyline_gleiskantenList = Lines;
                System.Collections.ObjectModel.ObservableCollection<CustomCircle> obsCustomPolyline_gleisknotenList = Ellipses;
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
                foreach (CustomCircle customNode in obsCustomPolyline_gleisknotenList)
                {
                    netDxf.Vector3 vec = new netDxf.Vector3((double)customNode.Center.Point.X, (double)customNode.Center.Point.Y, 0.0);
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

                if (obsArcList != null)
                {
                    foreach (CustomArc newArc in obsArcList)
                    {
                        netDxf.Entities.Arc dxfArc = new netDxf.Entities.Arc();
                        dxfArc.Center = new netDxf.Vector3(newArc.Center.Point.X, newArc.Center.Point.Y, 0.0);
                        dxfArc.Radius = newArc.Radius;
                        dxfArc.EndAngle = (180 / Math.PI) * Math.Acos((newArc.EndPoint.Point.X - newArc.Center.Point.X) / newArc.Radius);
                        dxfArc.StartAngle = (180 / Math.PI) * Math.Acos((newArc.StartPoint.Point.X - newArc.Center.Point.X) / newArc.Radius);

                        dxfArc.Thickness = newArc.Thickness;

                        dxf.AddEntity(dxfArc);

                    }
                }

                if (obslineList != null)
                {
                    foreach (CustomPolyLine newLine in obslineList)
                    {
                        netDxf.Entities.Line dxfLine = new netDxf.Entities.Line();
                        dxfLine.StartPoint = new netDxf.Vector3(newLine.CustomPoints[0].Point.X, newLine.CustomPoints[0].Point.Y, 0.0);
                        dxfLine.EndPoint = new netDxf.Vector3(newLine.CustomPoints[1].Point.X, newLine.CustomPoints[1].Point.Y, 0.0);



                        dxf.AddEntity(dxfLine);

                    }
                }

                if (obsEllipseList != null)
                {
                    foreach (CustomCircle newEllipse in obsEllipseList)
                    {
                        netDxf.Entities.Ellipse dxfEllipse = new netDxf.Entities.Ellipse();
                        dxfEllipse.Center = new netDxf.Vector3(newEllipse.Center.Point.X, newEllipse.Center.Point.Y, 0.0);
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
                    if (element is System.Windows.Shapes.Line)
                    {
                        System.Windows.Shapes.Line canvasLine = (System.Windows.Shapes.Line)element;
                        netDxf.Vector3 vecStartPoint = new netDxf.Vector3((canvasLine.X1 + DrawViewModel.GlobalDrawingPoint.X) - DrawViewModel.sharedCanvasSize / 2, -canvasLine.Y1 + (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y, 0.0);
                        netDxf.Vector3 vecEndPoint = new netDxf.Vector3((canvasLine.X2 + DrawViewModel.GlobalDrawingPoint.X) - DrawViewModel.sharedCanvasSize / 2, -canvasLine.Y2 + (DrawViewModel.sharedCanvasSize) / 2 + DrawViewModel.GlobalDrawingPoint.Y, 0.0);
                        netDxf.Entities.Line dxfLine = new netDxf.Entities.Line();
                        dxfLine.StartPoint = vecStartPoint;
                        dxfLine.EndPoint = vecEndPoint;
                        dxfLine.Thickness = canvasLine.StrokeThickness;
                        dxf.AddEntity(dxfLine);
                    }
                    else if (element is System.Windows.Shapes.Ellipse)
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
                    else if (element is System.Windows.Shapes.Polyline)
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
        private static void Table_BeginRowLayout(object sender, BeginRowLayoutEventArgs args)
        {
            args.MinimalHeight = 25f;
        }
        #endregion
    }
}
