using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using APLan.Commands;
using System.Collections.ObjectModel;
using APLan.HelperClasses;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using aplan.eulynx;
using System.Windows.Media;
using aplan.core;
using APLan.Views;
using System.Threading.Tasks;
using netDxf;
using sun.misc;
using netDxf.Tables;
using System;
using Models.TopoModels.EULYNX.rsmCommon;
using net.sf.saxon.expr.instruct;
using System.Linq;
using System.Net;



namespace APLan.ViewModels
{
    public class NewProjectViewModel : BaseViewModel
    {
        #region attributes

        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;
        private Loading loadingObject;
        private bool saveButtonActive;
        private bool saveAsButtonActive;
        private string welcomeInfo;
        private string country;
        private string format;
        private string jsonFiles = null;
        private string entwurfselement_KM = null;
        private string gleiskanten = null;
        private string gleisknoten = null;
        private string entwurfselement_LA = null;
        private string entwurfselement_HO = null;
        private string entwurfselement_UH = null;
        private string mdb = null;
        private string euxml = null;
        private string ppxml = null;
        private string projectPath = null;
        private string projectName = null;
        private string dxf = null;
        
        public static string currentProjectPath = null; //this would be used to know our project path.
        public static string currentProjectName = null; //this would be used to know our project path.


        private string OpenProjectPath { get; set; }
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                currentProjectName = value;
                OnPropertyChanged();
            }
        }
        public string ProjectPath
        {
            get { return projectPath; }
            set
            {
                projectPath = value;
                currentProjectPath = value;
                OnPropertyChanged();
            }
        }
        public string Country
        {
            get { return country; }
            set
            {
                country = value.Split(':')[1].Trim();
                OnPropertyChanged();
            }
        }
        public string Format
        {
            get { return format; }
            set
            {
                format = value.Split(':')[1].Trim();
                OnPropertyChanged();
            }
        }
        public string JsonFiles
        {
            get { return jsonFiles; }
            set
            {
                jsonFiles = value;
                OnPropertyChanged();
            }
        }
        public string Entwurfselement_KM
        {
            get { return entwurfselement_KM; }
            set
            {
                entwurfselement_KM = value;
                OnPropertyChanged();
            }
        }
        public string Gleiskanten
        {
            get { return gleiskanten; }
            set
            {
                gleiskanten = value;
                OnPropertyChanged();
            }
        }
        public string Gleisknoten
        {
            get { return gleisknoten; }
            set
            {
                gleisknoten = value;
                OnPropertyChanged();
            }
        }
        public string Entwurfselement_LA
        {
            get { return entwurfselement_LA; }
            set
            {
                entwurfselement_LA = value;
                OnPropertyChanged();
            }
        }
        public string Entwurfselement_HO
        {
            get { return entwurfselement_HO; }
            set
            {
                entwurfselement_HO = value;
                OnPropertyChanged();
            }
        }
        public string Entwurfselement_UH
        {
            get { return entwurfselement_UH; }
            set
            {
                entwurfselement_UH = value;
                OnPropertyChanged();
            }
        }
        public string MDB
        {
            get { return mdb; }
            set
            {
                mdb = value;
                OnPropertyChanged();
            }
        }
        public string EUXML
        {
            get { return euxml; }
            set
            {
                euxml = value;
                OnPropertyChanged();
            }
        }
        public string PPXML
        {
            get { return ppxml; }
            set
            {
                ppxml = value;
                OnPropertyChanged();
            }
        }
        public string DXF
        {
            get { return dxf; }
            set
            {
                dxf = value;
                OnPropertyChanged();
            }
        }
        public bool SaveButtonActive
        {
            get { return saveButtonActive; }
            set
            {
                saveButtonActive = value;
                OnPropertyChanged();
            }
        }
        public bool SaveAsButtonActive
        {
            get { return saveAsButtonActive; }
            set
            {
                saveAsButtonActive = value;
                OnPropertyChanged();
            }
        }
        public string WelcomeInfo
        {
            get { return welcomeInfo; }
            set
            {
                welcomeInfo = value;
                OnPropertyChanged();
            }
        }
        
        private Visibility _welcomeVisibility;
        public Visibility WelcomeVisibility
        {
            get { return _welcomeVisibility; }
            set
            {
                _welcomeVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand KantenPoints { get; set; }
        public ICommand AddPath { get; set; }
        public ICommand BrowseJson { get; set; }
        public ICommand BrowseMDB { get; set; }
        public ICommand BrowseEuxml { get; set; }
        public ICommand BrowsePpxml { get; set; }
        public ICommand Create { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Open { get; set; }
        public ICommand BrowseDxf { get; set; }
        #endregion

        #region constructor
        public NewProjectViewModel()
        {
            loadingObject = System.Windows.Application.Current.FindResource("globalLoading") as Loading;
            AddPath = new RelayCommand(ExecuteAddPath);
            BrowseJson = new RelayCommand(ExecuteBrowseJson);
            BrowseMDB = new RelayCommand(ExecuteBrowseMDB);
            BrowseEuxml = new RelayCommand(ExecuteBrowseEuxml);
            BrowsePpxml = new RelayCommand(ExecuteBrowsePpxml);
            Create = new RelayCommand(ExecuteCreate);
            Cancel = new RelayCommand(ExecuteCancel);
            Open = new RelayCommand(ExecuteOpen);
            BrowseDxf = new RelayCommand(ExecuteBrowseDxf);
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton=true;
            openFileDialog1 = new OpenFileDialog();

            loadedObjects = new ObservableCollection<CanvasObjectInformation>();

            gleiskantenList = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_LA_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_KM_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_HO_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_UH_list = new ObservableCollection<CustomPolyLine>();
            

            gleisknotenList = new ObservableCollection<CustomNode>();

            gleiskantenPointsList = new ObservableCollection<Point>();
            Entwurfselement_LAPointsList = new ObservableCollection<Point>();
            Entwurfselement_KMPointsList = new ObservableCollection<Point>();
            Entwurfselement_HOPointsList = new ObservableCollection<Point>();
            Entwurfselement_UHPointsList = new ObservableCollection<Point>();

            Rectangle_Shape_points_List = new ObservableCollection<CustomRectangle>();
            Ellipse_List = new ObservableCollection<CustomEllipse>();
            Polyline_List = new ObservableCollection<CustomPolyLine>();
            Polyline_LW_list = new ObservableCollection<CustomPolyLine>();
            Circle_List = new ObservableCollection<CustomCircle>();
            Arc_List = new ObservableCollection<CustomArc>();
            Line_List = new ObservableCollection<CustomLine>();
            Bezier_Curve_List = new ObservableCollection<CustomBezierCurve>();
            Image_List = new ObservableCollection<CustomImage>();


            Text_List = new ObservableCollection<CustomTextBlock>();
            


            WelcomeVisibility = Visibility.Visible;
            loadingObject.LoadingReport = "Welcome";
            
        }
        #endregion

        #region logic
        public void ExecuteBrowseDxf(object parameter)
        {
            clearOldSelectedFiles();
            openFileDialog1.Filter = "Types (*.dxf)|*.dxf";
            openFileDialog1.ShowDialog();
            dxf = openFileDialog1.FileName;
            if (dxf != null)
            {
                ((System.Windows.Controls.Button)parameter).IsEnabled = true;
            }
        }
        private void ExecuteAddPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                ProjectPath = folderBrowserDialog1.SelectedPath;
            }
            else
            {
                ProjectPath = "please select a project directory";
            }
        }
        private void ExecuteBrowseJson(object parameter)
        {
            //clearOldSelectedFiles();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Types (*.geojson;*.json)|*.json;*.geojson";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length<6 && openFileDialog1.FileNames.Length>=1)
            {
                System.Windows.MessageBox.Show("project Creation needs all the files");
            }
            else if(openFileDialog1.FileNames.Length > 1)
            {
                JsonFiles = "";
                foreach (string file in openFileDialog1.FileNames)
                {
                    JsonFiles += file;
                    JsonFiles += "+~+";
                }
            }
        }
        private void ExecuteBrowseMDB(object parameter)
        {
            clearOldSelectedFiles();
            openFileDialog1.Filter = "Types (*.MDB)|*.MDB";
            openFileDialog1.ShowDialog();
            MDB = openFileDialog1.FileName;
            if (MDB!=null && File.Exists(MDB) && MDB != "")
            {
                ((System.Windows.Controls.Button)parameter).IsEnabled = true;
            }
            else
            {
                MDB = "please select single .mdb file";
            }
        }
        private void ExecuteBrowseEuxml(object parameter)
        {
            clearOldSelectedFiles();
            openFileDialog1.Filter = "Types (*.euxml)|*.euxml";
            openFileDialog1.ShowDialog();
            EUXML = openFileDialog1.FileName;
            if (EUXML != null && File.Exists(EUXML) && EUXML!="")
            {
                ((System.Windows.Controls.Button)parameter).IsEnabled = true;
            }
            else
            {
                EUXML = "please select single .euxml file";
            }
        }
        private void ExecuteBrowsePpxml(object parameter)
        {
            clearOldSelectedFiles();
            openFileDialog1.Filter = "Types (*.ppxml)|*.ppxml";
            openFileDialog1.ShowDialog();
            PPXML = openFileDialog1.FileName;
            if (PPXML != null)
            {
                ((System.Windows.Controls.Button)parameter).IsEnabled = true;
            }

        }
        private void ExecuteCreate(object parameter)
        {
            var parameters = (object[])parameter;
            if (creatProjectFolder() == true)
            {
                ((Window)parameters[1]).Close();
                //WelcomeVisibility = Visibility.Collapsed;
                activateButtons();
                createModel(parameters[0].ToString());
            }   
        }
        private async void ExecuteOpen(object parameter)
        {
            loadingObject.startLoading();
            folderBrowserDialog1.SelectedPath = null;
            folderBrowserDialog1.ShowDialog();
            OpenProjectPath = folderBrowserDialog1.SelectedPath;

            if (Directory.Exists(OpenProjectPath))
            {
                Database.setDBPath(OpenProjectPath);
                foreach (string f in Directory.GetFiles(OpenProjectPath))
                {
                    if (System.IO.Path.GetExtension(f) == ".APlan")
                    {
                        loadAPlanFile(f);
                    }
                    else if (System.IO.Path.GetExtension(f) == ".xml" && f.Contains("Additional"))
                    {
                        InfoExtractor.getAllInfo().Clear();
                        InfoExtractor.loadExtraInfo(f);
                    }
                    else if (System.IO.Path.GetExtension(f) == ".euxml")
                    {
                        loadingObject.LoadingReport = "Loading Eulynx Object...";
                        bool finished = await loadEuxml(f);
                    }
                }
                CurrentProjectNameBind = OpenProjectPath.Split("\\")[^1];
                activateButtons();
                WelcomeVisibility = Visibility.Collapsed;
            }
            
            loadingObject.LoadingReport = "Finished...";
            loadingObject.stopLoading();
        }
        private void ExecuteCancel(object parameter)
        {
            (parameter as Window)?.Close();
        }
        
        private void clearOldSelectedFiles()
        {
            Entwurfselement_KM = null;
            entwurfselement_LA = null;
            entwurfselement_HO = null;
            entwurfselement_UH = null;
            Gleiskanten = null;
            Gleisknoten = null;
            //MDB = null;
            //EUXML = null;
            //PPXML = null;
        }
        private bool creatProjectFolder()
        {
            bool flag = true;
            string targetFolderPath = projectPath + "/" + $"{projectName}";
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Overrider Project?", "Project Exists", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            DirectoryInfo directory = new DirectoryInfo(targetFolderPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            transferFilesToPath(targetFolderPath);
            CurrentProjectNameBind = projectName;
            return flag;
        }
        /// <summary>
        /// transfer the selected file for project creation to the project path.
        /// </summary>
        /// <param name="path"></param>
        private void transferFilesToPath(string path)
        {
            if(Entwurfselement_KM != null)
            {
                File.Copy(Entwurfselement_KM, path+"/"+Path.GetFileName(Entwurfselement_KM), true);
            }
            if (entwurfselement_LA != null)
            {
                File.Copy(Entwurfselement_LA, path + "/" + Path.GetFileName(Entwurfselement_LA), true);
            }
            if (entwurfselement_HO != null)
            {
                File.Copy(Entwurfselement_HO, path + "/" + Path.GetFileName(Entwurfselement_HO), true);
            }
            if (Entwurfselement_UH != null)
            {
                File.Copy(Entwurfselement_UH, path + "/" + Path.GetFileName(Entwurfselement_UH), true);
            }
            if (Gleiskanten != null)
            {
                File.Copy(Gleiskanten, path + "/" + Path.GetFileName(Gleiskanten), true);
            }
            if (Gleisknoten != null)
            {
                File.Copy(Gleisknoten, path + "/" + Path.GetFileName(Gleisknoten), true);
            }
            if (MDB != null && File.Exists(MDB))
            {
                File.Copy(MDB, path + "/" + Path.GetFileName(MDB), true);
            }
            if (EUXML != null && File.Exists(EUXML))
            {
                File.Copy(EUXML, path + "/" + Path.GetFileName(EUXML), true);
            }
            if (PPXML != null && File.Exists(PPXML))
            {
                File.Copy(PPXML, path + "/" + Path.GetFileName(PPXML), true);
            }
            if (DXF != null && File.Exists(DXF))
            {
                File.Copy(DXF, path + "/" + Path.GetFileName(DXF), true);
            }
        }
        /// <summary>
        /// activate the buttons like save, save as ... that are not allowed to be activated in the beginning.
        /// </summary>
        private void activateButtons()
        {
            SaveButtonActive = true;
            SaveAsButtonActive = true;
        }
        /// <summary>
        /// load .APlan file which represent the saved information.
        /// </summary>
        /// <param name="f"></param>
        private void loadAPlanFile(string f)
        {
            loadedObjects.Clear(); //clear the previously loaded items.

            List<CanvasObjectInformation> importedObjects = new List<CanvasObjectInformation>();
            BinaryFormatter bfDeserialize = new BinaryFormatter();
            FileStream fsin = new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.None);
            fsin.Position = 0;
            importedObjects = (List<CanvasObjectInformation>)bfDeserialize.Deserialize(fsin);
            foreach (CanvasObjectInformation ObjectInfo in importedObjects)
            {
                loadedObjects.Add(ObjectInfo);
            }
            fsin.Close();
        }
        public void createDxfProject()
        {
          
            netDxf.Header.DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(DXF);
            // netDxf is only compatible with AutoCad2000 and higher DXF versions
            if (dxfVersion < netDxf.Header.DxfVersion.AutoCad2000) return;
            DxfDocument dxfReader = DxfDocument.Load(DXF);       
            List<netDxf.Entities.EntityObject> boundary2 = new List<netDxf.Entities.EntityObject>();
            #region POINTSDXF
            if(dxfReader.Points.Count() > 0)
            {
                foreach (netDxf.Entities.Point pnts in dxfReader.Points)
                {
                    if (pnts.Layer.Name == "GlobalDrawingPoint")
                    {
                        System.Windows.Point globalPoint = new System.Windows.Point((pnts.Position.X), (pnts.Position.Y));

                        ViewModels.DrawViewModel.GlobalDrawingPoint = globalPoint;
                        ModelViewModel.firspoint.X = globalPoint.X;
                        ModelViewModel.firspoint.Y = globalPoint.Y;
                    }
                }
            }
           
            #endregion

            // cheking for the Inserts entity and the entities made for it
            #region INSERTSDXF
            if (dxfReader.Inserts.Count() > 0)
            {
                foreach (netDxf.Entities.Insert lwpline in dxfReader.Inserts)
                {
                    //splitting the Inserts entities in to netDXF recognizable entities
                    boundary2 = lwpline.Explode();                    
                    foreach (netDxf.Entities.EntityObject e in boundary2)
                    {
                        // if the entities is circle type
                        if (e is netDxf.Entities.Circle circle)
                        {
                            CustomCircle newEllipse = new CustomCircle();
                            double radius = circle.Radius;
                            double thickness = circle.Thickness;
                            System.Windows.Point centerVertex = new System.Windows.Point(circle.Center.X, circle.Center.Y);
                            if(ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = centerVertex.X;
                                ModelViewModel.firspoint.Y = centerVertex.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                            }

                            newEllipse.Radius = radius;
                            newEllipse.Thickness = thickness;
                            newEllipse.EllipseVertexCenter = centerVertex;
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
                            Circle_List.Add(newEllipse);

                        }
                        //if the entiites is text type
                        if (e is netDxf.Entities.Text txt)
                        {
                            if (e.Layer.Name != "0" && txt.IsVisible == true && txt.Value.Length > 0 && txt.Value.Length < 9)
                            {
                                CustomTextBlock textBlock = new CustomTextBlock();
                                Point newPoint = new Point((((double)txt.Position.X)), (((double)txt.Position.Y)));
                                textBlock.NodePoint = newPoint;
                                textBlock.Name = txt.Value;
                                textBlock.Height = txt.Height;
                                textBlock.Width = txt.Width;
                                textBlock.RotationAngle = -(txt.Rotation);
                                if (ModelViewModel.firspoint.X == 0)
                                {
                                    ModelViewModel.firspoint.X = newPoint.X;
                                    ModelViewModel.firspoint.Y = newPoint.Y;
                                    DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                                }

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
                                textBlock.Width = txt.Width;
                                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                                {
                                    Key = "Width",
                                    Value = "" + txt.Width + ""

                                });

                                textBlock.ShapeAttributeInfo.Add(new KeyValue()
                                {
                                    Key = "TextAlignment",
                                    Value = "" + txt.Alignment + ""

                                });


                                Text_List.Add(textBlock);
                            }


                        }
                        // if the entities is of image type
                        if (e is netDxf.Entities.Image Im)
                        {

                        }
                        // if the entities is of arc type
                        if (e is netDxf.Entities.Arc arc)
                        {
                            
                            System.Windows.Point endPoint = new System.Windows.Point((arc.Center.X + Math.Cos(arc.EndAngle * Math.PI / 180) * arc.Radius), (arc.Center.Y + Math.Sin(arc.EndAngle * Math.PI / 180) * arc.Radius));
                            System.Windows.Point startPoint = new System.Windows.Point((arc.Center.X + Math.Cos(arc.StartAngle * Math.PI / 180) * arc.Radius), (arc.Center.Y + Math.Sin(arc.StartAngle * Math.PI / 180) * arc.Radius));
                            double sweep = 0.0;
                            if (arc.EndAngle < arc.StartAngle)
                                sweep = (360 + arc.EndAngle) - arc.StartAngle;
                            else sweep = Math.Abs(arc.EndAngle - arc.StartAngle);
                            bool IsLargeArc = sweep >= 180;

                            Size size = new System.Windows.Size(arc.Radius, arc.Radius);
                            SweepDirection sweepDirection = arc.Normal.Z > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

                            CustomArc newArc = new CustomArc();
                            newArc.StartPoint = startPoint;
                            newArc.EndPoint = endPoint;
                            newArc.Radius = arc.Radius;
                            newArc.SweepDirection = sweepDirection;
                            newArc.Normal = arc.Normal;
                            newArc.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                            newArc.Size = new Size(newArc.Radius, newArc.Radius);
                            if (ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = startPoint.X;
                                ModelViewModel.firspoint.Y = startPoint.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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
                        
                        //if the entities is of lwpolyline
                        if (e is netDxf.Entities.LwPolyline lwpline1)
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
                                        double X1 = newline.StartPoint.X;
                                        double Y1 = newline.StartPoint.Y;
                                        double X2 = newline.EndPoint.X;
                                        double Y2 = newline.EndPoint.Y;

                                        CustomPolyLine newpolylinewpf = new CustomPolyLine();

                                        System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
                                        System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
                                        PointCollection pc = new PointCollection();
                                        pc.Add(startpoint);
                                        pc.Add(endpoint);
                                        newpolylinewpf.Points = pc;
                                        if (ModelViewModel.firspoint.X == 0)
                                        {
                                            ModelViewModel.firspoint.X = startpoint.X;
                                            ModelViewModel.firspoint.Y = startpoint.Y;
                                            DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                                        }
                                        newpolylinewpf.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                                        newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
                                        {
                                            Key = "EndPoint",
                                            Value = "" + endpoint + ""

                                        });
                                        newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
                                        {
                                            Key = "StartPoint",
                                            Value = "" + startpoint + ""

                                        });
                                        Polyline_List.Add(newpolylinewpf);
                                    }
                                    else if (lwpoylineEntity is netDxf.Entities.Arc arcLwPolyline)
                                    {
                                        System.Windows.Point endPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.EndAngle * Math.PI / 180) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.EndAngle * Math.PI / 180) * arcLwPolyline.Radius));
                                        System.Windows.Point startPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.StartAngle * Math.PI / 180) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.StartAngle * Math.PI / 180) * arcLwPolyline.Radius));
                                        double sweep = 0.0;
                                        if (arcLwPolyline.EndAngle < arcLwPolyline.StartAngle)
                                            sweep = (360 + arcLwPolyline.EndAngle) - arcLwPolyline.StartAngle;
                                        else sweep = Math.Abs(arcLwPolyline.EndAngle - arcLwPolyline.StartAngle);
                                        bool IsLargeArc = sweep >= 180;

                                        Size size = new System.Windows.Size(arcLwPolyline.Radius, arcLwPolyline.Radius);
                                        SweepDirection sweepDirection = arcLwPolyline.Normal.Z > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

                                        CustomArc newArc = new CustomArc();
                                        newArc.StartPoint = startPoint;
                                        newArc.EndPoint = endPoint;
                                        newArc.Radius = arcLwPolyline.Radius;
                                        newArc.Normal = arcLwPolyline.Normal;
                                        newArc.SweepDirection = sweepDirection;
                                        newArc.Size = new Size(newArc.Radius, newArc.Radius);
                                        newArc.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                                        if (ModelViewModel.firspoint.X == 0)
                                        {
                                            ModelViewModel.firspoint.X = startPoint.X;
                                            ModelViewModel.firspoint.Y = startPoint.Y;
                                            DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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
                                }
                            }
                            else
                            {
                                List<netDxf.Entities.LwPolylineVertex> vertexCollection = lwpline1.Vertexes;
                                PointCollection pointCollection_HO = new PointCollection();

                                CustomPolyLine newPolyline_lw = new CustomPolyLine();
                                foreach (netDxf.Entities.LwPolylineVertex singleVertex in vertexCollection)
                                {
                                    System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                                    if (ModelViewModel.firspoint.X == 0)
                                    {
                                        ModelViewModel.firspoint.X = vertexPoint_HO.X;
                                        ModelViewModel.firspoint.Y = vertexPoint_HO.Y;
                                        DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                                    }

                                    pointCollection_HO.Add(vertexPoint_HO);
                                    newPolyline_lw.ShapeAttributeInfo.Add(new KeyValue()
                                    {
                                        Key = "Points",
                                        Value = "" + vertexPoint_HO + ""

                                    });


                                }
                                
                                newPolyline_lw.Points = pointCollection_HO;
                               
                                newPolyline_lw.Color = new SolidColorBrush() { Color = Colors.DarkBlue };
                                Polyline_LW_list.Add(newPolyline_lw);
                            }

                        }
                        //if entities is of type line
                        if (e is netDxf.Entities.Line lines)
                        {

                            double X1 = lines.StartPoint.X;
                            double Y1 = lines.StartPoint.Y;
                            double X2 = lines.EndPoint.X;
                            double Y2 = lines.EndPoint.Y;

                            CustomPolyLine newpolylinewpf = new CustomPolyLine();

                            System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
                            System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
                            PointCollection pc = new PointCollection();
                            pc.Add(startpoint);
                            pc.Add(endpoint);
                            newpolylinewpf.Points = pc;
                            if (ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = startpoint.X;
                                ModelViewModel.firspoint.Y = startpoint.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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
                            Polyline_List.Add(newpolylinewpf);
                        }

                        if (e is netDxf.Entities.Ellipse l)
                        {
                            CustomEllipse newCustomEllipse = new CustomEllipse();
                            System.Windows.Point centerVertex = new System.Windows.Point(l.Center.X, l.Center.Y);

                            newCustomEllipse.RadiusX = (l.MajorAxis / 2);
                            newCustomEllipse.RadiusY = (l.MinorAxis / 2);
                            newCustomEllipse.Thickness = l.Thickness;
                            newCustomEllipse.EllipseVertexCenter = centerVertex;
                            if (ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = centerVertex.X;
                                ModelViewModel.firspoint.Y = centerVertex.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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

                }
            }
            #endregion
            // checking is the entities is of type lwpolyline
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
                            double X1 = newline.StartPoint.X;
                            double Y1 = newline.StartPoint.Y;
                            double X2 = newline.EndPoint.X;
                            double Y2 = newline.EndPoint.Y;

                            CustomPolyLine newpolylinewpf = new CustomPolyLine();

                            System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
                            System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
                            PointCollection pc = new PointCollection();
                            pc.Add(startpoint);
                            pc.Add(endpoint);
                            newpolylinewpf.Points = pc;
                            if (ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = startpoint.X;
                                ModelViewModel.firspoint.Y = startpoint.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                            }
                            newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
                            {
                                Key = "EndPoint",
                                Value = "" + endpoint + ""

                            });
                            newpolylinewpf.ShapeAttributeInfo.Add(new KeyValue()
                            {
                                Key = "StartPoint",
                                Value = "" + startpoint + ""

                            });

                            newpolylinewpf.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                            Polyline_List.Add(newpolylinewpf);
                        }
                        else if (lwpoylineEntity is netDxf.Entities.Arc arcLwPolyline)
                        {
                            System.Windows.Point endPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.EndAngle * Math.PI / 180) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.EndAngle * Math.PI / 180) * arcLwPolyline.Radius));
                            System.Windows.Point startPoint = new System.Windows.Point((arcLwPolyline.Center.X + Math.Cos(arcLwPolyline.StartAngle * Math.PI / 180) * arcLwPolyline.Radius), (arcLwPolyline.Center.Y + Math.Sin(arcLwPolyline.StartAngle * Math.PI / 180) * arcLwPolyline.Radius));
                            double sweep = 0.0;
                            if (arcLwPolyline.EndAngle < arcLwPolyline.StartAngle)
                                sweep = (360 + arcLwPolyline.EndAngle) - arcLwPolyline.StartAngle;
                            else sweep = Math.Abs(arcLwPolyline.EndAngle - arcLwPolyline.StartAngle);
                            bool IsLargeArc = sweep >= 180;

                            Size size = new System.Windows.Size(arcLwPolyline.Radius, arcLwPolyline.Radius);
                            SweepDirection sweepDirection = arcLwPolyline.Normal.Z > 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

                            CustomArc newArc = new CustomArc();
                            newArc.StartPoint = startPoint;
                            newArc.EndPoint = endPoint;
                            newArc.Radius = arcLwPolyline.Radius;
                            newArc.Size = new Size(newArc.Radius, newArc.Radius);
                            newArc.Normal = arcLwPolyline.Normal;
                            newArc.SweepDirection = sweepDirection;
                            newArc.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                            if (ModelViewModel.firspoint.X == 0)
                            {
                                ModelViewModel.firspoint.X = startPoint.X;
                                ModelViewModel.firspoint.Y = startPoint.Y;
                                DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                            }
                            Arc_List.Add(newArc);


                        }
                    }
                }
                else
                {
                    List<netDxf.Entities.LwPolylineVertex> vertexCollection = lwpline1.Vertexes;
                    PointCollection pointCollection_HO = new PointCollection();

                    CustomPolyLine newPolyline_lw = new CustomPolyLine();
                    foreach (netDxf.Entities.LwPolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_HO.Add(vertexPoint_HO);
                      
                        newPolyline_lw.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "Points",
                            Value = "" + vertexPoint_HO + ""

                        });
                        if (ModelViewModel.firspoint.X == 0)
                        {
                            ModelViewModel.firspoint.X = vertexPoint_HO.X;
                            ModelViewModel.firspoint.Y = vertexPoint_HO.Y;
                            DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                        }

                    }
                    
                    newPolyline_lw.Points = pointCollection_HO;
                    newPolyline_lw.Color = new SolidColorBrush() { Color = Colors.DarkBlue };
                    Polyline_LW_list.Add(newPolyline_lw);
                }



                
            }
            #endregion

            //checking is entity is of type hatch
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
                        double X1 = lines.StartPoint.X;
                        double Y1 = lines.StartPoint.Y;
                        double X2 = lines.EndPoint.X;
                        double Y2 = lines.EndPoint.Y;

                        CustomPolyLine newpolylinewpf = new CustomPolyLine();

                        System.Windows.Point startpoint = new System.Windows.Point(X1, Y1);
                        System.Windows.Point endpoint = new System.Windows.Point(X2, Y2);
                        PointCollection pc = new PointCollection();
                        pc.Add(startpoint);
                        pc.Add(endpoint);
                        newpolylinewpf.Points = pc;

                        newpolylinewpf.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                        Polyline_List.Add(newpolylinewpf);
                    }

                    
                }

            }
            #endregion

             

            // entities of type Solid need to be implemented nicely
            //foreach (netDxf.Entities.Solid circ in dxfReader.Solids)
            //{
            //    CustomRectangle cr = new CustomRectangle();
            //    Point firstPostion = new Point(circ.FirstVertex.X, circ.FirstVertex.Y);
            //    Point secondPostion = new Point(circ.SecondVertex.X, circ.SecondVertex.Y);
            //    Point thirdPostion = new Point(circ.ThirdVertex.X, circ.ThirdVertex.Y);
            //    Point fourthPostion = new Point(circ.FourthVertex.X, circ.FourthVertex.Y);

            //    cr.Points.Add(firstPostion);
            //    cr.Points.Add(secondPostion);
            //    cr.Points.Add(thirdPostion);
            //    cr.Points.Add(fourthPostion);
            //    cr.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
            //    Rectangle_Shape_points_List.Add(cr);

            //}

            // if eneitites is of type text
            #region TEXTDXF
            foreach (netDxf.Entities.Text txt in dxfReader.Texts)
            {
                if (txt.IsVisible == true && txt.Value.Length > 0)
                {
                    CustomTextBlock textBlock = new CustomTextBlock();
                    Point newPoint = new Point((((double)txt.Position.X)), (((double)txt.Position.Y)));
                    textBlock.NodePoint = newPoint;
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
                    textBlock.Height = txt.Height ;
                    textBlock.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Height",
                        Value = "" + txt.Height + ""

                    });
                    textBlock.Width = txt.Width;
                    textBlock.ShapeAttributeInfo.Add(new KeyValue()
                    {
                        Key = "Width",
                        Value = "" + txt.Width + ""

                    });
                    textBlock.RotationAngle = -(txt.Rotation);                     

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
                        Value = "" + txt.Alignment + ""

                    });
                    Text_List.Add(textBlock);
                }
               
            }
            #endregion
            
           

            #region ARCDXF
            foreach(netDxf.Entities.Arc newArc in dxfReader.Arcs)
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
                newArc2.StartPoint = startPoint;
                newArc2.EndPoint = endPoint;
                newArc2.Radius = newArc.Radius;
                newArc2.SweepDirection = sweepDirection;
                newArc2.Thickness = newArc.Thickness;
                newArc2.Normal = newArc.Normal;
                newArc2.IsLargeArc = false;
                newArc2.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                newArc2.Size = new Size(newArc.Radius, newArc.Radius);
                if (ModelViewModel.firspoint.X == 0)
                {
                    ModelViewModel.firspoint.X = startPoint.X;
                    ModelViewModel.firspoint.Y = startPoint.Y;
                    DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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
            #endregion



            //if entities of type lines of netDxf
            #region LINEACADSDXF
            foreach (netDxf.Entities.Line l in dxfReader.Lines)
            {
                netDxf.Entities.Line newline = new netDxf.Entities.Line();
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
                PointCollection pc = new PointCollection();
                pc.Add(startpoint);
                pc.Add(endpoint);
                newpolylinewpf.Points = pc;
                if (ModelViewModel.firspoint.X == 0)
                {
                    ModelViewModel.firspoint.X = startpoint.X;
                    ModelViewModel.firspoint.Y = startpoint.Y;
                    DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                }

                newpolylinewpf.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                Polyline_List.Add(newpolylinewpf);
            }
            #endregion

            #region ELLIPSEACADDXF
            foreach(netDxf.Entities.Ellipse l in dxfReader.Ellipses)
            {
                CustomEllipse newCustomEllipse = new CustomEllipse();         
                System.Windows.Point centerVertex = new System.Windows.Point(l.Center.X, l.Center.Y);

                newCustomEllipse.RadiusX = (l.MajorAxis/2);
                newCustomEllipse.RadiusY = (l.MinorAxis/2);
                newCustomEllipse.Thickness = l.Thickness;
                newCustomEllipse.EllipseVertexCenter = centerVertex;
                if (ModelViewModel.firspoint.X == 0)
                {
                    ModelViewModel.firspoint.X = centerVertex.X;
                    ModelViewModel.firspoint.Y = centerVertex.Y;
                    DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
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
            #endregion

            #region CIRCLEACADDXF
            foreach (netDxf.Entities.Circle circle in dxfReader.Circles)
            {
                CustomCircle newEllipse = new CustomCircle();
                double radius = circle.Radius;
                double thickness = circle.Thickness;
                System.Windows.Point centerVertex = new System.Windows.Point(circle.Center.X, circle.Center.Y);
                if (ModelViewModel.firspoint.X == 0)
                {
                    ModelViewModel.firspoint.X = centerVertex.X;
                    ModelViewModel.firspoint.Y = centerVertex.Y;
                    DrawViewModel.GlobalDrawingPoint = ModelViewModel.firspoint;
                }

                newEllipse.Radius = radius;
                newEllipse.Thickness = thickness;
                newEllipse.EllipseVertexCenter = centerVertex;
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
                Circle_List.Add(newEllipse);
            }
            #endregion




            #region JSONorMDBorACADDXFPolylines
            //checking for polyine for the json or mdb drawing converted in to dxf from APLAN
            foreach (netDxf.Entities.Polyline plyLine in dxfReader.Polylines)
            {
                               
                if (plyLine.Layer.Name == "Entwurfselement_HO")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_HO = new PointCollection();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_HO = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_HO.Add(vertexPoint_HO);
                    }
                    CustomPolyLine newPolyline_HO = new CustomPolyLine();

                    newPolyline_HO.Points = pointCollection_HO;
                    newPolyline_HO.Color = new SolidColorBrush() { Color = Colors.DarkBlue };
                    Entwurfselement_HO_list.Add(newPolyline_HO);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_LA")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_LA = new PointCollection();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_LA = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_LA.Add(vertexPoint_LA);
                    }
                    CustomPolyLine newPolyline_LA = new CustomPolyLine();

                    newPolyline_LA.Points = pointCollection_LA;
                    newPolyline_LA.Color = new SolidColorBrush() { Color = Colors.DarkRed };
                    Entwurfselement_LA_list.Add(newPolyline_LA);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_KM")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_KM = new PointCollection();
                    
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_KM = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);
                        pointCollection_KM.Add(vertexPoint_KM);
                    }
                    CustomPolyLine newPolyline_KM = new CustomPolyLine();

                    newPolyline_KM.Points = pointCollection_KM;
                    newPolyline_KM.Color = new SolidColorBrush() { Color = Colors.DarkViolet };
                    Entwurfselement_KM_list.Add(newPolyline_KM);
                }
                else if (plyLine.Layer.Name == "Entwurfselement_UH")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_UH = new PointCollection();
                    
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_UH = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_UH.Add(vertexPoint_UH);
                    }
                    CustomPolyLine newPolyline_UH = new CustomPolyLine();

                    newPolyline_UH.Points = pointCollection_UH;
                    newPolyline_UH.Color = new SolidColorBrush() { Color = Colors.Green };
                    Entwurfselement_UH_list.Add(newPolyline_UH);
                }
                else if (plyLine.Layer.Name == "gleiskanten")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_gleiskanten = new PointCollection();
                    
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_gleiskanten = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_gleiskanten.Add(vertexPoint_gleiskanten);
                       

                    }
                    CustomPolyLine newPolyline_gleiskanten = new CustomPolyLine();

                    newPolyline_gleiskanten.Points = pointCollection_gleiskanten;
                    newPolyline_gleiskanten.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
                    gleiskantenList.Add(newPolyline_gleiskanten);
                }
                else
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_ACAD = new PointCollection();
                    CustomPolyLine newPolyline_acad = new CustomPolyLine();
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_gleiskanten = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_ACAD.Add(vertexPoint_gleiskanten);

                        newPolyline_acad.ShapeAttributeInfo.Add(new KeyValue()
                        {
                            Key = "Points",
                            Value = "" + vertexPoint_gleiskanten + ""

                        });
                    }
                    
                    newPolyline_acad.Points = pointCollection_ACAD;
                    newPolyline_acad.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
                    Polyline_List.Add(newPolyline_acad);
                }


            }
            foreach (netDxf.Entities.Point pnts in dxfReader.Points)
            {
                if (pnts.Layer.Name == "gleisknoten")
                {
                    CustomNode node = new CustomNode();
                    Point newPoint = new Point((((double)pnts.Position.X)), (((double)pnts.Position.Y)));


                    node.NodePoint = newPoint;
                    gleisknotenList.Add(node);

                }
            }
            #endregion
        }

        /// <summary>
        /// choose which model creation according to selected file type.
        /// </summary>
        /// <param name="format"></param>
        private async void createModel(string format)
        {
            loadingObject.LoadingReport = "Creating Eulynx Object...";
            loadingObject.startLoading();
            if (format.Equals(".json"))
            {
                await createJSONproject();
            }
            else if (format.Equals(".mdb"))
            {
                await createMDBproject();
            }
            else if (format.Equals(".euxml"))
            {
                await loadEuxml(EUXML);
            }
            else if (format.Equals(".dxf"))
            {
                createDxfProject();
            }
            loadingObject.LoadingReport = "Finished";
            loadingObject.stopLoading();
            WelcomeVisibility = Visibility.Collapsed;
        }
        private async Task<bool> createJSONproject()
        {
            Task<bool> taskFinished1 = CreateJSONeulyxObject();
            bool report1 = await taskFinished1;
            Task<bool> taskFinished2 = DrawEulyxObject();
            bool report2 = await taskFinished2;
            return true;
        }
        private async Task<bool> createMDBproject()
        {
            WelcomeInfo = "Creating Eulynx Object...";
            Task<bool> taskFinished1 = CreateMDBeulyxObject();
            bool report1 = await taskFinished1;

            WelcomeInfo = "Drawing...";
            Task<bool> taskFinished2 = DrawEulyxObject();
            bool report2 = await taskFinished2;
            return true;
        }
        /// <summary>
        /// load .euxml file representing the saved Eulynx model.
        /// </summary>
        /// <param name="f"></param>
        private async Task<bool> loadEuxml(string f)
        {
            loadingObject.LoadingReport = "Validating Euxml...";
            var EulynxValidatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as EulynxValidatorViewModel;
            Task<string> reportTask = EulynxValidatorViewModel.validate(f);
            string report = await reportTask;
            if (report.Contains("Validation is Successful"))
            {
                await deserializeEuxml(f);

                ModelViewModel model = new();
                await model.drawObject(ViewModels.DrawViewModel.sharedCanvasSize);

                var planningTabViewModel = System.Windows.Application.Current.FindResource("planTabViewModel") as PlanningTabViewModel;
            }
            else
            {
                if (System.Windows.MessageBox.Show("Euxml file is Invalid, Show details?",
                    "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    EulynxValidatorViewModel validatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as EulynxValidatorViewModel;
                    validatorViewModel.Report = report;
                    EulynxValidator validatorWindow = new EulynxValidator();
                    validatorWindow.ShowDialog();
                }
                else
                {
                    // close the window 
                }
            }

            return true;
        }
        /// <summary>
        /// create a Eulynx object from JSON files async.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CreateJSONeulyxObject()
        {
            ((Loading)System.Windows.Application.Current.FindResource("globalLoading")).LoadingReport = "Creating Eulynx Object...";
            await Task.Run(() =>
            {
                APLan.ViewModels.DrawViewModel.model = new ModelViewModel(
                            country,
                            format,
                            entwurfselement_KM,
                            gleiskanten,
                            gleisknoten,
                            entwurfselement_LA,
                            entwurfselement_HO,
                            entwurfselement_UH,
                            null,
                            ProjectPath + "/" + projectName
                            );
            });
            return true;
        }
        /// <summary>
        /// create a Eulynx object from MDB file async.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CreateMDBeulyxObject()
        {
            ((Loading)System.Windows.Application.Current.FindResource("globalLoading")).LoadingReport = "Creating Eulynx Object...";
            await Task.Run(() =>
            {
                APLan.ViewModels.DrawViewModel.model = new ModelViewModel(
                country,
                format,
                null,
                null,
                null,
                null,
                null,
                null,
                mdb,
                ProjectPath + "/" + projectName
                );
            });
            return true;
        }
        /// <summary>
        /// draw the created Eulynx object async.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> DrawEulyxObject()
        {
           await DrawViewModel.model.drawObject(ViewModels.DrawViewModel.sharedCanvasSize);

            return true;
        }
        /// <summary>
        /// create a Eulynx object from a .euxml file async.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<bool> deserializeEuxml(string file)
        {
            await Task.Run(() =>
            {
                var eulynxService = EulynxService.getInstance();
                ModelViewModel.eulynx = eulynxService.deserialization(file);
            });
            return true;
        }
        #endregion 
    }
}
