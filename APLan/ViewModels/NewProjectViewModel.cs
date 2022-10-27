using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using APLan.Commands;
using System.Collections.ObjectModel;
using APLan.HelperClasses;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using aplan.eulynx;
using System;
using System.Windows.Media;
using aplan.core;
using APLan.Views;

namespace APLan.ViewModels
{
    public class NewProjectViewModel : INotifyPropertyChanged
    {
        #region Inotify Essentials
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region attributes

        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private bool saveButtonActive;
        private bool saveAsButtonActive;
        private bool printButtonActive;
        private bool importButtonActive;
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
        public static string currentProjectPath = null; //this would be used to know our project path.
        public static string currentProjectName = null; //this would be used to know our project path.
        private string projectName = null;

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
        public bool PrintButtonActive
        {
            get { return printButtonActive; }
            set
            {
                printButtonActive = value;
                OnPropertyChanged();
            }
        }
        public bool ImportButtonActive
        {
            get { return importButtonActive; }
            set
            {
                importButtonActive = value;
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
        
        private Visibility welcomeVisibility;
        private Visibility _gleisKantenPointsVisibility;
        public Visibility GleisKantenPointsVisibility
        {
            get => _gleisKantenPointsVisibility;
            set
            {
                _gleisKantenPointsVisibility = value;
                OnPropertyChanged();
            }

        }
        public Visibility WelcomeVisibility
        {
            get { return welcomeVisibility; }
            set
            {
                welcomeVisibility = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Point> KantenPointsHoaX
        {
            get;
            set;
        }
        public ObservableCollection<CanvasObjectInformation> loadedObjects
        {
            get;
            set;
        }
        public ObservableCollection<CustomPolyLine> gleiskantenList
        {
            get;
            set;
        }
        public ObservableCollection<Point> gleiskantenPointsList
        {
            get;
            set;
        }
        public ObservableCollection<CustomPolyLine> Entwurfselement_LA_list
        {
            get;
            set;
        }
        public ObservableCollection<Point> Entwurfselement_LAPointsList
        {
            get;
            set;
        }
        public ObservableCollection<CustomPolyLine> Entwurfselement_KM_list
        {
            get;
            set;
        }
        public ObservableCollection<Point> Entwurfselement_KMPointsList
        {
            get;
            set;
        }
        public ObservableCollection<CustomPolyLine> Entwurfselement_HO_list
        {
            get;
            set;
        }
        public ObservableCollection<Point> Entwurfselement_HOPointsList
        {
            get;
            set;
        }
        public ObservableCollection<CustomPolyLine> Entwurfselement_UH_list
        {
            get;
            set;
        }
        public ObservableCollection<Point> Entwurfselement_UHPointsList
        {
            get;
            set;
        }
        public ObservableCollection<CustomNode> gleisknotenList
        {
            get;
            set;
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
        #endregion
        
        #region constructor
        public NewProjectViewModel()
        {
            AddPath = new RelayCommand(ExecuteAddPath);
            BrowseJson = new RelayCommand(ExecuteBrowseJson);
            BrowseMDB = new RelayCommand(ExecuteBrowseMDB);
            BrowseEuxml = new RelayCommand(ExecuteBrowseEuxml);
            BrowsePpxml = new RelayCommand(ExecuteBrowsePpxml);
            Create = new RelayCommand(ExecuteCreate);
            Cancel = new RelayCommand(ExecuteCancel);
            Open = new RelayCommand(ExecuteOpen);
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton=true;
            openFileDialog1 = new OpenFileDialog();

            loadedObjects = new ObservableCollection<CanvasObjectInformation>(); //binded to view

            gleiskantenList = new ObservableCollection<CustomPolyLine>();
            gleiskantenPointsList = new ObservableCollection<Point>();

            Entwurfselement_LA_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_KM_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_HO_list = new ObservableCollection<CustomPolyLine>();
            Entwurfselement_UH_list = new ObservableCollection<CustomPolyLine>();

            gleisknotenList = new ObservableCollection<CustomNode>();
            Entwurfselement_LAPointsList = new ObservableCollection<Point>();
            Entwurfselement_KMPointsList = new ObservableCollection<Point>();
            Entwurfselement_HOPointsList = new ObservableCollection<Point>();
            Entwurfselement_UHPointsList = new ObservableCollection<Point>();

            KantenPointsHoaX = new ObservableCollection<Point>();

            KantenPoints = new RelayCommand(ExecuteKantenPoints);

            WelcomeInfo = "Welcome";
        }
        #endregion
        
        #region logic
        public void createModel(string format)
        {

            WelcomeInfo = "Creating Eulynx Object...";

            if (format.Equals(".json"))
            {
                createJSONproject();
            }
            else if (format.Equals(".mdb"))
            {
                createMDBproject();
            }else if (format.Equals(".euxml"))
            {
                loadEuxml(EUXML);
            }
            WelcomeVisibility = Visibility.Collapsed;   
        }
        public void ExecuteAddPath(object parameter)
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
        public void ExecuteBrowseJson(object parameter)
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
        public void ExecuteBrowseMDB(object parameter)
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
        public void ExecuteBrowseEuxml(object parameter)
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
        public void ExecuteBrowsePpxml(object parameter)
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
        public void ExecuteCreate(object parameter)
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
        public void ExecuteCancel(object parameter)
        {
            (parameter as Window)?.Close();
        }
        public void clearOldSelectedFiles()
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
        public bool checkProjectNameAndPath()
        {
            bool flag = true;
            if (ProjectName==null)
            {
                flag = false;
                System.Windows.MessageBox.Show("please enter a project name");
            }else if (ProjectPath == null)
            {
                flag = false;
                System.Windows.MessageBox.Show("please select a project path");
            }
            return flag;
        }
        public bool creatProjectFolder()
        {
            bool flag = true;
            string targetFolderPath = projectPath + "/" + $"{projectName}"+"/"+"files";
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
        public void transferFilesToPath(string path)
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
        public void activateButtons()
        {
            SaveButtonActive = true;
            SaveAsButtonActive = true;
            //ImportButtonActive = true;
            //PrintButtonActive = true;
        }
        public void ExecuteKantenPoints(object parameter)
        {
            System.Windows.Controls.CheckBox box = ((System.Windows.Controls.CheckBox)parameter);
            if (box.IsChecked==true)
            {
                //for (int i = 0; i < gleiskantenPointsList.Count; i++)
                //{
                //    KantenPointsHoaX.Add(gleiskantenPointsList[i]);
                //}
                KantenPointsHoaX = gleiskantenPointsList;
                System.Windows.MessageBox.Show("Hello");
            }
            else
            {
                KantenPointsHoaX.Clear();
            }
        }
        public void ExecuteOpen(object parameter)
        {
            folderBrowserDialog1.SelectedPath = null;
            folderBrowserDialog1.ShowDialog();
            OpenProjectPath = folderBrowserDialog1.SelectedPath;

            if (Directory.Exists(OpenProjectPath))
            {
                foreach (string f in Directory.GetFiles(OpenProjectPath))
                {
                    if (System.IO.Path.GetExtension(f) == ".APlan")
                    {

                        loadAPlanFile(f);
                    }
                    else if (System.IO.Path.GetExtension(f) == ".euxml")
                    {
                        loadEuxml(f);
                    }
                }
            }
        }
        public void createJSONproject()
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
                     null
                     );
            DrawViewModel.model.drawObject(ViewModels.DrawViewModel.sharedCanvasSize,
            gleiskantenList,
            gleiskantenPointsList,
            Entwurfselement_LA_list,
            Entwurfselement_LAPointsList,
            Entwurfselement_KM_list,
            Entwurfselement_KMPointsList,
            Entwurfselement_HO_list,
            Entwurfselement_HOPointsList,
            Entwurfselement_UH_list,
            Entwurfselement_UHPointsList,
            gleisknotenList);
        }
        public void createMDBproject()
        {
            WelcomeInfo = "Creating Eulynx Object...";
            APLan.ViewModels.DrawViewModel.model = new ModelViewModel(
            country,
            format,
            null,
            null,
            null,
            null,
            null,
            null,
            mdb
            );

            WelcomeInfo = "Drawing...";
            DrawViewModel.model.drawObject(ViewModels.DrawViewModel.sharedCanvasSize,
           gleiskantenList,
           gleiskantenPointsList,
           Entwurfselement_LA_list,
           Entwurfselement_LAPointsList,
           Entwurfselement_KM_list,
           Entwurfselement_KMPointsList,
           Entwurfselement_HO_list,
           Entwurfselement_HOPointsList,
           Entwurfselement_UH_list,
           Entwurfselement_UHPointsList,
           gleisknotenList);
        }
        /// <summary>
        /// load .euxml file representing the saved Eulynx model.
        /// </summary>
        /// <param name="f"></param>
        public void loadEuxml(string f)
        {
            string report = EulynxValidatorViewModel.validate(f);
            if (report.Contains("Validation is Successful"))
            {
                var eulynxService = EulynxService.getInstance();
                ModelViewModel.eulynx = eulynxService.deserialization(f);

                ModelViewModel model = new();
                model.drawObject(ViewModels.DrawViewModel.sharedCanvasSize,
                gleiskantenList,
                gleiskantenPointsList,
                Entwurfselement_LA_list,
                Entwurfselement_LAPointsList,
                Entwurfselement_KM_list,
                Entwurfselement_KMPointsList,
                Entwurfselement_HO_list,
                Entwurfselement_HOPointsList,
                Entwurfselement_UH_list,
                Entwurfselement_UHPointsList,
                gleisknotenList);
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
            
        }
        /// <summary>
        /// load an APlan binary file representing the saved items.
        /// </summary>
        /// <param name="f"></param>
        public void loadAPlanFile(string f)
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
        #endregion
    }
}
