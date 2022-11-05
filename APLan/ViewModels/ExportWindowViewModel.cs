using APLan.Commands;
using APLan.HelperClasses;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using System.Xml;

namespace APLan.ViewModels
{
    public class ExportWindowViewModel : BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private string outputFolder;
        private string successfull;
        private string euxmlResult;
        private string projectName;
        private string loadingMessage;

        public string LoadingMessage
        {
            get => loadingMessage;
            set
            {
                loadingMessage = value;
                OnPropertyChanged();
            }
        }
        public string OutputFolder
        {
            get => outputFolder;
            set
            {
                outputFolder = value;
                OnPropertyChanged();
            }
        }
        public string Successfull
        {
            get { return successfull; }
            set
            {
                successfull = value;
                OnPropertyChanged();
            }
        }
        public string EuxmlResult
        {
            get { return euxmlResult; }
            set
            {
                euxmlResult = value;
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
        public void ExecuteCancelButton(object parameter)
        {
            ((Window)parameter).Close();
        }
        public async void ExecuteExportButton(object parameter)
        {
            startLoading();
            LoadingReport = "exporting to Euxml";
            var objects = (object[])parameter;
            var exportType = ((TextBlock)objects[0]).Text;
            var exportPath = ((System.Windows.Controls.TextBox)objects[1]).Text;
            var projectName = ((System.Windows.Controls.TextBlock)objects[2]).Text;
            this.projectName = projectName;
            var window = ((System.Windows.Window)objects[3]);
            if (exportType!=null && projectName!=null && Directory.Exists(exportPath))
            {
                await exportToEuxml(projectName,exportPath);
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
        public void ExecuteSelectFolderButton(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            OutputFolder= folderBrowserDialog1.SelectedPath;
        }
        public void ExecuteCancel(object parameter)
        {
            closeWindow(parameter);
        }
        public void ExecuteValidateXML(object parameter)
        {
            closeWindow(parameter);
            Views.EulynxValidator validator = new Views.EulynxValidator();
            validator.ShowDialog();
            LoadingReport = "loading Euxml";
        }
        public async Task<bool> exportToEuxml(string projectName, string exportPath)
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
        public void closeWindow(object parameter)
        {
            var objects = (object[])parameter;
            var window = (System.Windows.Window)objects[0];
            window.Close();
        }
        public async Task<string> readingEuxmlAsText()
        {
            string text=null;
            await Task.Run(() =>
            {
                text = File.ReadAllText(OutputFolder + "/eulynx" + projectName+ ".euxml");
            });
            
            return text;
        }
        #endregion
    }
}
