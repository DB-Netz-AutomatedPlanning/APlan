using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using APLan.Commands;
using APLan.HelperClasses;
using System.IO;
using aplan.eulynx;
using aplan.core;
using System;
using Point = System.Windows.Point;
using APLan.ViewModels.ModelsLogic;
using APLan.Model.MiscellaneousInputLogic;
using APLan.Model.HelperClasses;
using File = System.IO.File;
using C_sharp_learning;

namespace APLan.ViewModels
{
    public class NewProjectViewModel : BaseViewModel
    {
        #region attributes
        public static Point firspoint = new System.Windows.Point(0, 0);
        #endregion

        #region fields
        private EulynxModelHandler eulynxModelHandler;
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;
        private string welcomeInfo;
        private string newProjectName;
        private string newProjectPath;
        private string country;
        private string format;
        private string json;
        private string mdb;
        private string xml;
        private string ppxml;
        private string dxf;
        private string dwg;
        private string xls;
        #endregion

        #region properties
        public string NewProjectName
        {
            get { return newProjectName; }
            set
            {
                newProjectName = value;
                OnPropertyChanged();
            }
        }
        public string NewProjectPath
        {
            get { return newProjectPath; }
            set
            {
                newProjectPath = value;
                OnPropertyChanged();
            }
        }
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }
        public string Format
        {
            get { return format; }
            set
            {
                format = (value!=null && value.Split(":").Length > 1) ? value.Split(":")[1].Trim():value;
                OnPropertyChanged();
            }
        }
        public string Json
        {
            get { return json; }
            set
            {
                json = value;
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
        public string XML
        {
            get { return xml; }
            set
            {
                xml = value;
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
        public string DWG
        {
            get { return dwg; }
            set
            {
                dwg = value;
                OnPropertyChanged();
            }
        }
        public string XLS
        {
            get { return xls; }
            set
            {
                xls = value;
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
        #endregion

        #region commands
        public ICommand KantenPoints { get; set; }
        public ICommand AddPath { get; set; }
        public ICommand BrowseJson { get; set; }
        public ICommand BrowseMDB { get; set; }
        public ICommand BrowsePpxml { get; set; }
        public ICommand BrowseDxf { get; set; }
        public ICommand BrowseDwg { get; set; }
        public ICommand BrowseXML { get; set; }
        public ICommand BrowseXLS { get; set; }
        public ICommand Create { get; set; }
        public ICommand Cancel { get; set; }

        #endregion

        #region constructor
        public NewProjectViewModel()
        {
            initializeCommands();
            initializeNeededObjects();
        }
        #endregion

        #region constructor logic
        private void initializeCommands()
        {
            AddPath = new RelayCommand(ExecuteAddPath);
            BrowseJson = new RelayCommand(ExecuteBrowseJson);
            BrowseMDB = new RelayCommand(ExecuteBrowseMDB);
            BrowseXML = new RelayCommand(ExecuteBrowseXml);
            BrowsePpxml = new RelayCommand(ExecuteBrowsePpxml);
            BrowseDxf = new RelayCommand(ExecuteBrowseDxf);
            BrowseDwg = new RelayCommand(ExecuteBrowseDwg);
            BrowseXLS = new RelayCommand(ExecuteBrowseXls);
            Create = new RelayCommand(ExecuteCreate);
            Cancel = new RelayCommand(ExecuteCancel);
        }
        private void initializeNeededObjects()
        {

            loadingObject = System.Windows.Application.Current.FindResource("globalLoading") as Loading;
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = true;
            openFileDialog1 = new OpenFileDialog();

            WelcomeVisibility = Visibility.Visible;
            loadingObject.LoadingReport = "Welcome";
        }
        #endregion

        #region commands logic
        private void ExecuteAddPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                NewProjectPath = folderBrowserDialog1.SelectedPath;
            }
        }
        private void ExecuteBrowseJson(object parameter)
        {
            switch (ProjectType)
            {
                case "EULYNX":
                    openFileDialog1.Filter = "Types (*.geojson;*.json)|*.json;*.geojson";
                    openFileDialog1.Multiselect = true;
                    break;
                case "ERDM":
                    openFileDialog1.Filter = "Types (*.json)|*.json";
                    break;
                default:
                    break;
            }

            openFileDialog1.ShowDialog();
            Json = openFileDialog1.FileName;

            if (ProjectType.Equals("EULYNX") && (openFileDialog1.FileNames.Length != 6 && openFileDialog1.FileNames.Length >= 1))
            {
                System.Windows.MessageBox.Show("project Creation needs all the files");
            }
            else if(ProjectType.Equals("EULYNX") && openFileDialog1.FileNames.Length == 6)
            {
                var temp = "";
                foreach (string file in openFileDialog1.FileNames)
                {
                    temp += file;
                    temp += "+~+";
                }
                Json = temp;
            }
        }
        private void ExecuteBrowseMDB(object parameter)
        {
            switch (ProjectType)
            {
                case "EULYNX":
                    openFileDialog1.Filter = "Types (*.MDB)|*.MDB";
                    break;
                default:
                    break;
            }

            openFileDialog1.ShowDialog();
            MDB = openFileDialog1.FileName;
        }
        private void ExecuteBrowseXml(object parameter)
        {
            switch (ProjectType)
            {
                case "EULYNX":
                    openFileDialog1.Filter = "Types (*.euxml)|*.euxml";
                    break;
                case "ERDM":
                    openFileDialog1.Filter = "Types (*.xml)|*.xml";
                    break;
                case "Sweden":
                    openFileDialog1.Filter = "Types (*.xml)|*.xml";
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            XML = openFileDialog1.FileName;
         
        }
        private void ExecuteBrowsePpxml(object parameter)
        {
            switch (ProjectType)
            {
                case "EULYNX":
                    openFileDialog1.Filter = "Types (*.ppxml)|*.ppxml";
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            PPXML = openFileDialog1.FileName;
        }
        public void ExecuteBrowseDxf(object parameter)
        {
            switch (ProjectType)
            {
                case "CAD":
                    openFileDialog1.Filter = "Types (*.dxf)|*.dxf";
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            DXF = openFileDialog1.FileName;
        }
        public void ExecuteBrowseDwg(object parameter)
        {
            switch (ProjectType)
            {
                case "CAD":
                    openFileDialog1.Filter = "Types (*.dwg)|*.dwg";
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            DWG = openFileDialog1.FileName;
        }
        public void ExecuteBrowseXls(object parameter)
        {
            switch (ProjectType)
            {
                case "ERDM":
                    openFileDialog1.Filter = "Excel and CSV files (*.xls;*.csv)|*.xls;*.csv";
                    openFileDialog1.Multiselect = true;
                    break;
                // Eulynx case
                case "EULYNX":
                    openFileDialog1.Filter = "Excel and CSV files (*.xls;*.csv)|*.xls;*.csv";
                    openFileDialog1.Multiselect = true;
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            XLS = openFileDialog1.FileName;

            if (ProjectType.Equals("ERDM") && (openFileDialog1.FileNames.Length != 4 && openFileDialog1.FileNames.Length >= 1))
            {
                System.Windows.MessageBox.Show("project Creation needs all the files");
            }
            else if (ProjectType.Equals("ERDM") && openFileDialog1.FileNames.Length == 4)
            {
                var temp = "";
                foreach (string file in openFileDialog1.FileNames)
                {
                    temp += file;
                    temp += "+~+";
                }
                XLS = temp;
            }
        }
        private void ExecuteCreate(object parameter)
        {
            var parameters = (object[])parameter;

            try
            {
                if (creatProjectFolder())
                {
                    ((Window)parameters[1]).Close();
                }

            } catch (Exception e) { System.Windows.MessageBox.Show("creation of project folder failed : \n" +e.Message);}


            try
            {
                ProjectName = NewProjectName;
                ProjectPath = NewProjectPath;
                clearModelsParameters();
                createProject(Format);
                MainMenuViewModel.activateButtons();
            }
            catch  (Exception e) { System.Windows.MessageBox.Show("creation of project model failed : \n" + e.Message); }


        }
        private void ExecuteCancel(object parameter)
        {
            (parameter as Window)?.Close();
        }
        #endregion

        #region general logic
        /// <summary>
        /// choose which model creation according to selected file type.
        /// </summary>
        /// <param name="format"></param>
        private async void createProject(string format)
        {
            clearDrawings(); //clear previous drawing data.

            loadingObject.startLoading();
            if (ProjectType.Equals("Sweden"))
            {
                loadingObject.LoadingReport = "Creating ERDM Object...";

                if (format.Contains(".xml"))
                {
                    SwedenDataHandler swedenHandler = new(XML);
                    ErdmModelHandler erdmHandler = new();

                    BaseViewModel.erdmModel = await swedenHandler.getInfraStructure();
                    erdmHandler.drawERDM(erdmModel, Lines, Ellipses);
                }
            }
            if (ProjectType.Equals("EULYNX"))
            {
                eulynxModelHandler = new();
                
                loadingObject.LoadingReport = "Creating Eulynx Object...";
                if (format.Contains(".json"))
                {
                    BaseViewModel.eulynxModel = await eulynxModelHandler.createJSONproject(Json, Format, ProjectName, ProjectPath);
                    await eulynxModelHandler.DrawEulyxObject(Lines, Ellipses, Signals);
                }
                else if (format.Contains(".mdb"))
                {
                    BaseViewModel.eulynxModel = await eulynxModelHandler.createMDBproject(MDB, Format, ProjectName, ProjectPath);
                    await eulynxModelHandler.DrawEulyxObject(Lines, Ellipses, Signals);
                }
                else if (format.Contains(".euxml"))
                {
                    BaseViewModel.eulynxModel = await eulynxModelHandler.loadEuxml(XML, Lines, Ellipses, Signals);
                }
                else if (format.Contains(".xls"))
                {
                    eulynxModelHandler.CreateJSONFilesFromXLS(XLS, ProjectName, ProjectPath);
                }
            }
            if (ProjectType.Equals("ERDM"))
            {
                //eulynxModelHandler = new();
                ErdmModelHandler erdmHandler = new();
                loadingObject.LoadingReport = "Creating ERDM Object...";
                if (format.Contains(".json"))
                {
                    BaseViewModel.erdmModel = await erdmHandler.deserializeFromJSON(Json);
                }
                else if (format.Contains(".xml"))
                {
                    BaseViewModel.erdmModel = await erdmHandler.deserializeFromXML(XML);
                }
                else if (format.Contains(".xls"))
                {
                    //eulynxModelHandler.CreateJSONFilesFromXLS(XLS, ProjectName, ProjectPath);
                    BaseViewModel.erdmModel = await erdmHandler.createERDMProject(XLS);
                }
                if (BaseViewModel.erdmModel != null)
                    erdmHandler.drawERDM(erdmModel, Lines, Ellipses);
            }
            if (ProjectType.Equals("CAD"))
            {
                
                if (format.Contains(".dxf"))
                {
                    DxfHandler dxfHandler = new(firspoint,Lines, Ellipses, Texts, Arcs);
                    dxfHandler.createDxfProject(DXF);
                    DrawViewModel.GlobalDrawingPoint = dxfHandler.GlobalDrawingPoint;
                    updateBindedCollections();
                }
                else if (format.Contains(".dwg"))
                {
                    DwgHandler dwgHandler = new(firspoint, Lines, Ellipses, Texts, Arcs);
                    dwgHandler.createDwgProject(DWG);
                    DrawViewModel.GlobalDrawingPoint = dwgHandler.GlobalDrawingPoint;
                    updateBindedCollections();
                }
            }

            loadingObject.LoadingReport = "Finished";
            loadingObject.stopLoading();
            WelcomeVisibility = Visibility.Collapsed;
        }
        /// <summary>
        /// create project folder for the new project.
        /// </summary>
        /// <returns></returns>
        private bool creatProjectFolder()
        {
            bool flag = true;
            string targetFolderPath = $"{NewProjectPath}/{NewProjectName}";
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
            return flag;
        }
        /// <summary>
        /// transfer the selected file for project creation to the project path.
        /// </summary>
        /// <param name="path"></param>
        private void transferFilesToPath(string path)
        {
            if (ProjectType.Equals("EULYNX"))
            {
                if (Format.Contains(".json"))
                {
                    HelperFunctions.copyFilesInString(Json, path);
                }
                if (Format.Contains(".mdb"))
                {
                    File.Copy(MDB, $"{path}/{Path.GetFileName(MDB)}", true);
                }
                if (Format.Contains(".euxml"))
                {
                    File.Copy(XML, $"{path}/{Path.GetFileName(XML)}", true);
                }
                if (Format.Contains(".xls"))
                {
                    File.Copy(XLS, $"{path}/{Path.GetFileName(XLS)}", true);
                }
            }
            if (ProjectType.Equals("ERDM"))
            {
                if (Format.Contains(".json"))
                {
                    File.Copy(Json, $"{path}/{Path.GetFileName(Json)}", true);
                }
                if (Format.Contains(".xls"))
                {
                    HelperFunctions.copyFilesInString(XLS, path);
                }
                if (Format.Contains(".xml"))
                {
                    File.Copy(XML, $"{path}/{Path.GetFileName(XML)}", true);
                }
            }
        }
        #endregion
    }
}
