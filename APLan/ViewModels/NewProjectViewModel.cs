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

            DxfDocument dxfReader = DxfDocument.Load(dxf);

            foreach (netDxf.Entities.Point pnts in dxfReader.Points)
            {
                if (pnts.Layer.Name == "GlobalDrawingPoint")
                {
                    System.Windows.Point globalPoint = new System.Windows.Point((pnts.Position.X), (pnts.Position.Y));

                    ViewModels.DrawViewModel.GlobalDrawingPoint = globalPoint;
                }
            }

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

                if (plyLine.Layer.Name == "Entwurfselement_LA")
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

                if (plyLine.Layer.Name == "Entwurfselement_KM")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_KM = new PointCollection();
                    //int i = 0;
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

                if (plyLine.Layer.Name == "Entwurfselement_UH")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_UH = new PointCollection();
                    int i = 0;
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

                if (plyLine.Layer.Name == "gleiskanten")
                {
                    netDxf.Collections.ObservableCollection<netDxf.Entities.PolylineVertex> vertexCollection = plyLine.Vertexes;
                    PointCollection pointCollection_gleiskanten = new PointCollection();
                    int i = 0;
                    foreach (netDxf.Entities.PolylineVertex singleVertex in vertexCollection)
                    {
                        System.Windows.Point vertexPoint_gleiskanten = new System.Windows.Point(singleVertex.Position.X, singleVertex.Position.Y);

                        pointCollection_gleiskanten.Add(vertexPoint_gleiskanten);
                        if (i == 0)
                        {
                            ViewModels.DrawViewModel.GlobalDrawingPoint = vertexPoint_gleiskanten;
                            i++;
                        }

                    }
                    CustomPolyLine newPolyline_gleiskanten = new CustomPolyLine();

                    newPolyline_gleiskanten.Points = pointCollection_gleiskanten;
                    newPolyline_gleiskanten.Color = new SolidColorBrush() { Color = Colors.DarkOrange };
                    gleiskantenList.Add(newPolyline_gleiskanten);
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
