using aplan.eulynx.validator;
using APLan.Commands;
using APLan.Model.ModelsLogic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace APLan.ViewModels
{
    public class ValidatorViewModel : BaseViewModel
    { 
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private string _projectType;
        private string _file;
        private string _path;
        private string _report;
        private string _report_rules;
        #endregion

        #region properties
        public new string ProjectType
        {
            get => _projectType;
            set
            {
                if (_projectType != value)
                {
                    _projectType = value.Split(":").Length > 1 ? value.Split(":")[1].Trim() : value;
                    OnPropertyChanged();                
                }
            }
        }
        public string InputFile
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged();
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        public string Report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged();
            }
        }
        public string Report_rules
        {
            get { return _report_rules; }
            set
            {
                _report_rules = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand FilePath { get; set; }
        public ICommand OutputPath { get; set; }
        public ICommand Validate { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Ok { get; set; }
        public ICommand ValidateXML { get; set; }
        #endregion

        #region constructor
        public ValidatorViewModel()
        {
            
            FilePath = new RelayCommand(ExecuteFilePath);
            OutputPath = new RelayCommand(ExecuteOutputPath);
            Validate = new RelayCommand(ExecuteValidate);
            Cancel = new RelayCommand(ExecuteCancel);
            folderBrowserDialog1 = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();

            LoadingVisibility = Visibility.Collapsed;
        }
        #endregion

        #region command logic
        private void ExecuteFilePath(object parameter)
        {

            switch (ProjectType)
            {
                case "ERDM":
                    openFileDialog1.Filter = "Types (*.json;*.xml)|*.json;*.xml";
                    break;
                case "EULYNX":
                    openFileDialog1.Filter = "Types (*.xml;*.euxml)|*.xml;*.euxml";
                    break;
                default:
                    break;
            }
            openFileDialog1.ShowDialog();
            InputFile = openFileDialog1.FileName;

        }
        private void ExecuteOutputPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            Path = folderBrowserDialog1.SelectedPath;
        }
        private async void ExecuteValidate(object parameter)
        {
            Report = null;
            switch (ProjectType)
            {
                case "ERDM":
                    await validateERDM();
                    break;
                case "EULYNX":
                    await validateEULYNX();
                    break;
                default:
                    break;  
            }
        }
        private void ExecuteCancel(object parameter)
        {
            ((Window)parameter).Close();
        }
        #endregion
        
        #region validationlogic
        private async Task<string> validateERDM()
        {
            ERDMvalidator erdmValidator = new();
            startLoading();
            //define XSD validation version based on the imported xml.
            var report = await erdmValidator.validate(InputFile, Path);
            Report = report;
            if (string.IsNullOrEmpty(report))
                Report = "validation was sucessfull";
            //validate according to the rules in German book.
            //await RulesValidate(JSON);
            stopLoading();
            return erdmValidator.Report;
        }
        private async Task<string> validateEULYNX()
        {
            EulynxValidator eulynxValidator = new();
            startLoading();
            //define XSD validation version based on the imported xml.
            var report = await eulynxValidator.validate(InputFile, Path);
            if (string.IsNullOrEmpty(report))
                Report = "validation was sucessfull";
            //validate according to the rules in German book.
            //await RulesValidate(JSON);
            stopLoading();
            return "";
        }
        #endregion

    }
}
