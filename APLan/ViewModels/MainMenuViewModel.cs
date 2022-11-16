using APLan.Commands;
using APLan.Views;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using APLan.HelperClasses;
using System.Windows.Media;
using System.Windows.Forms;
using aplan.eulynx;
using System.Windows.Shapes;

namespace APLan.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        public string SavePath
        {
            get;
            set;
        }
        #endregion

        #region commands
        public ICommand NewProject { get; set; }
        public ICommand AddData { get; set; }
        public ICommand PreviewData { get; set;}
        public ICommand RemoveData { get; set; }
        public ICommand Print { get; set; }
        public ICommand EulynxValidator { get; set; }
        public ICommand ExitProgram { get; set; }
        public ICommand AboutWPF { get; set; }
        public ICommand Save { get; set; }
        public ICommand SaveAs { get; set; }

        #endregion

        #region Constructor
        public MainMenuViewModel()
        {
            Save = new RelayCommand(ExecuteSave);
            SaveAs = new RelayCommand(ExecuteSaveAs);
            NewProject = new RelayCommand(ExecuteNewProjectWindow);
            AddData = new RelayCommand(ExecuteAddDataWindow);
            PreviewData = new RelayCommand(ExecutePreviewData);
            RemoveData = new RelayCommand(ExecuteRemoveData);
            Print = new RelayCommand(ExecutePrint);
            EulynxValidator = new RelayCommand(ExecuteEulynxValidator);
            AboutWPF = new RelayCommand(ExecuteAboutWPF);
            ExitProgram = new RelayCommand(ExecuteExitProgram);
            folderBrowserDialog1 = new();
        }
        #endregion

        #region logic
        private void ExecuteNewProjectWindow (object parameter)
        {
            
            NewProjectWindow newProject = new();
            newProject.ShowDialog();
        }
        private void ExecuteAddDataWindow(object parameter)
        {
            AddDataWindow addData = new();
            addData.ShowDialog();
        }
        private void ExecutePreviewData(object parameter)
        {
            PreviewDataWindow previewData = new ();
            previewData.ShowDialog();
        }
        private void ExecuteRemoveData(object parameter)
        {
            RemoveDataWindow removeData = new ();
            removeData.ShowDialog();
        }
        private void ExecutePrint(object parameter)
        {
            System.Windows.Controls.PrintDialog printDlg = new ();
            printDlg.ShowDialog();
        }
        private void ExecuteEulynxValidator(object parameter)
        {
            EulynxValidator validator = new ();
            validator.ShowDialog();
        }
        private void ExecuteAboutWPF(object parameter)
        {
            AboutWPF wpfinfo = new ();
            wpfinfo.ShowDialog();
        }
        private void ExecuteExitProgram(object parameter)
        {
            ((MainWindow)parameter).Close();
        }
        private void ExecuteSave(object parameter)
        {
            SavePath = NewProjectViewModel.currentProjectPath+"/"+ NewProjectViewModel.currentProjectName;
            saveAndSaveAs(SavePath);
            InfoExtractor.extractExtraInfo(SavePath, null);
        }
        private void ExecuteSaveAs(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            SavePath = folderBrowserDialog1.SelectedPath;
            if (!SavePath.Equals(""))
            {
                saveAndSaveAs(SavePath);
                InfoExtractor.extractExtraInfo(SavePath, null);
            } 
        }
        /// <summary>
        /// Preform saving action for Save or SaveAs.
        /// </summary>
        /// <param name="SavePath"></param>
        private void saveAndSaveAs(string SavePath)
        {
            if (Directory.Exists(SavePath))
            {
                List<CanvasObjectInformation> allInfo = new();
                foreach (UIElement element in DrawViewModel.toBeStored)
                {
                    Matrix transformation = element.RenderTransform.Value;
                    transformation.Translate(
                            Canvas.GetLeft(element), // initial from dragging when it comes to the canvas
                            Canvas.GetTop(element) // initial from dragging when it comes to the canvas

                        );
                    if (element is CustomCanvasSignal)
                    {
                        CanvasObjectInformation info = new()
                        {
                            Type = element.GetType().ToString(),
                            //Rotation = CustomProperites.GetRotation(element),
                            SignalImageSource = (((Image)element).Source).ToString(),
                            //LocationInCanvas = CustomProperites.GetLocation(element),
                            RenderTransformMatrix = transformation,
                            Scale = DrawViewModel.signalSizeForConverter
                        };
                        allInfo.Add(info);
                    }
                    else if (element is CustomCanvasText)
                    {
                        CanvasObjectInformation info = new()
                        {
                            Type = element.GetType().ToString(),
                            RenderTransformMatrix = transformation,
                            Scale = DrawViewModel.signalSizeForConverter,
                            IncludedText = ((CustomCanvasText)element).Text,
                            IncludedTextSize = ((CustomCanvasText)element).FontSize

                        };
                        allInfo.Add(info);
                    }
                }
                if (ModelViewModel.eulynx != null)
                {
                    var eulynxService = EulynxService.getInstance();
                    eulynxService.serialization(ModelViewModel.eulynx, "", SavePath);
                }


                //serialize
                BinaryFormatter bf = new();

                FileStream fsout = new(SavePath + "/save.APlan", FileMode.Create, FileAccess.Write, FileShare.None);

                using (fsout)
                {
                    bf.Serialize(fsout, allInfo);
                }
                fsout.Close();
                //File.Copy(NewProjectViewModel.currentProjectPath + "/" + NewProjectViewModel.currentProjectName + "/Aplan.db", SavePath + "/" + "/Aplan.db", true);
            }
            else
            {
                System.Windows.MessageBox.Show("Saving directory don't Exist. Please create a project.");
            }
        }
        #endregion
    }
}
