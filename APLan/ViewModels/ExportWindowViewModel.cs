using APLan.Commands;
using APLan.ERDMmodel;
using APLan.HelperClasses;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace APLan.ViewModels
{
    public class ExportWindowViewModel : BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private string _outputFolder;
        private string _successfull;
        private string _euxmlResult;
        private string _projectName;
        private string _loadingMessage;

        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                _loadingMessage = value;
                OnPropertyChanged();
            }
        }
        public string OutputFolder
        {
            get => _outputFolder;
            set
            {
                _outputFolder = value;
                OnPropertyChanged();
            }
        }
        public string Successfull
        {
            get { return _successfull; }
            set
            {
                _successfull = value;
                OnPropertyChanged();
            }
        }
        public string EuxmlResult
        {
            get { return _euxmlResult; }
            set
            {
                _euxmlResult = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand Cancel { get; set; }
        public ICommand Export { get; set; }
        public ICommand SelectFolder { get; set; }
        public ICommand Ok { get; set; }
        public ICommand ValidateXML { get; set; }
        #endregion

        #region constructor
        public ExportWindowViewModel()
        {
            Cancel = new RelayCommand(ExecuteCancelButton);
            Export = new RelayCommand(ExecuteExportButton);
            SelectFolder = new RelayCommand(ExecuteSelectFolderButton);
            Ok = new RelayCommand(ExecuteCancel);
            ValidateXML = new RelayCommand(ExecuteValidateXML);
            folderBrowserDialog1 = new FolderBrowserDialog();
            
            LoadingVisibility = Visibility.Collapsed;
            LoadingMessage = "Loading The Euxml...";
        }
        #endregion

        #region logic
        private void ExecuteCancelButton(object parameter)
        {
            ((Window)parameter).Close();
        }
        private async void ExecuteExportButton(object parameter)
        {
            startLoading();
            LoadingReport = "exporting to Euxml";
            var objects = (object[])parameter;
            var exportType = ((TextBlock)objects[0]).Text;
            var exportPath = ((System.Windows.Controls.TextBox)objects[1]).Text;
            var projectName = ((System.Windows.Controls.TextBlock)objects[2]).Text;
            this._projectName = projectName;
            var window = ((System.Windows.Window)objects[3]);

            if (exportType.Equals("JSON(ERDM)") && BaseViewModel.ERDMmodel != null)
            {
                ERDMserializer eRDMserializer = new(BaseViewModel.ERDMmodel);

                System.IO.File.WriteAllText(exportPath + "\\" + projectName + ".json", eRDMserializer.serializeERDM());
            }
            else
            if (exportType.Equals("Eulynx") && exportType != null && projectName != null && Directory.Exists(exportPath))
            {
                await exportToEuxml(projectName, exportPath);
                Views.ExportConfirmationAndValidation exportConfirmValidate = new();
                exportConfirmValidate.Show();
                startLoading();
                LoadingReport = "Loading the Euxml file.";
                Task<string> task = readingEuxmlAsText();
                EuxmlResult = await task;
                stopLoading();
                LoadingReport = "";
            }
            else
            {
                System.Windows.MessageBox.Show("Please check provided information");
            }
            stopLoading();
            window.Close();
        }
        private void ExecuteSelectFolderButton(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            OutputFolder= folderBrowserDialog1.SelectedPath;
        }
        private void ExecuteCancel(object parameter)
        {
            closeWindow(parameter);
        }
        private void ExecuteValidateXML(object parameter)
        {
            closeWindow(parameter);
            Views.EulynxValidator validator = new Views.EulynxValidator();
            validator.ShowDialog();
            LoadingReport = "loading Euxml";
        }
        
        private async Task<bool> exportToEuxml(string projectName, string exportPath)
        {
            await Task.Run(() =>
            {   
                ModelViewModel.eulynxService.serialization(ModelViewModel.eulynx, projectName, exportPath);
                InfoExtractor.extractExtraInfo(exportPath, projectName);
                Successfull = "Exporting to XML was Successfull";
                //XmlReader xmlReader = XmlReader.Create(OutputFolder + "/eulynx" + projectName + ".euxml");
            });
            
            
            return true;
        }
        /// <summary>
        /// close the window
        /// </summary>
        /// <param name="parameter"></param>
        private void closeWindow(object parameter)
        {
            var objects = (object[])parameter;
            var window = (System.Windows.Window)objects[0];
            window.Close();
        }
        /// <summary>
        /// read Euxml as text based on outputFolder and projectName.
        /// </summary>
        /// <returns></returns>
        private async Task<string> readingEuxmlAsText()
        {
            string text=null;
            await Task.Run(() =>
            {
                text = File.ReadAllText(OutputFolder + "/eulynx" + _projectName + ".euxml");
            });
            
            return text;
        }
        #endregion
    }
}
