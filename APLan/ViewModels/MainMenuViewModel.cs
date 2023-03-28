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
using Dsafa.WpfColorPicker;
using aplan.core;
using System.Threading.Tasks;
using System;
using System.Windows.Media.Imaging;
using Patagames.Pdf.Net;
using PdfDocument = Spire.Pdf.PdfDocument;
using Spire.Pdf;
using APLan.Model.CustomObjects;

namespace APLan.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        #region fields
        private FolderBrowserDialog folderBrowserDialog1;
        private Microsoft.Win32.SaveFileDialog safeFileDialog1;
        private OpenFileDialog openFileDialog1;
        private List<String> pdfFiles = null;
        private static bool saveButtonActive;
        private static bool saveAsButtonActive;
        #endregion

        #region properties
        public List<String> PDFFiles
        {
            get { return pdfFiles; }
            set
            {
                pdfFiles = value;
                OnPropertyChanged();
            }
        }
        private string OpenProjectPath { get; set; }
        public string SavePath
        {
            get;
            set;
        }
        public static bool SaveButtonActive
        {
            get { return saveButtonActive; }
            set
            {
                saveButtonActive = value;
                RaiseStaticPropertyChanged("SaveButtonActive");
            }
        }
        public static bool SaveAsButtonActive
        {
            get { return saveAsButtonActive; }
            set
            {
                saveAsButtonActive = value;
                RaiseStaticPropertyChanged("SaveAsButtonActive");
            }
        }
        #endregion
        
        #region commands      
        public ICommand Print { get; set; }
        public ICommand EulynxValidator { get; set; }
        public ICommand ERDMvalidator { get; set; }
        public ICommand ExitProgram { get; set; }
        public ICommand AboutWPF { get; set; }
        public ICommand Save { get; set; }
        public ICommand SaveAs { get; set; }
        public ICommand Undo { get; set; }
        public ICommand Redo { get; set; }
        public ICommand Open { get; set; }
        public ICommand ColorPicker { get; set; }
        public ICommand AcadDrawing { get; set; }
        public ICommand Snapshot { get; set; }

        public ICommand MergePdfFiles { get; set; }

        public ICommand PdfViewer { get; set; }

        #endregion

        #region Constructor
        public MainMenuViewModel()
        {
            loadingObject = System.Windows.Application.Current.FindResource("globalLoading") as Loading;
            Save = new RelayCommand(ExecuteSave);
            SaveAs = new RelayCommand(ExecuteSaveAs);
            Print = new RelayCommand(ExecutePrint);
            Open = new RelayCommand(ExecuteOpen);
            Undo = new RelayCommand(ExecuteUndoProgram);
            Redo = new RelayCommand(ExecuteRedoProgram);
            AcadDrawing = new RelayCommand(ExecuteAcadDrawing);
            ColorPicker = new RelayCommand(ExecuteColorPickingView);

            Snapshot = new RelayCommand(ExecuteSnapshot);
            MergePdfFiles = new RelayCommand(ExecuteMergePdfFiles);
            PdfViewer = new RelayCommand(ExecutePDFViewer);

            folderBrowserDialog1 = new();
            openFileDialog1 = new();
            safeFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            PDFFiles = new List<String>();

        }
        #endregion

        #region command logic
        private void ExecutePDFViewer(object parameter)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Types (*.pdf;*.pdf)|*.pdf;*.pdf";
            openFileDialog1.ShowDialog();
            PdfCommon.Initialize();

            //Open and load a PDF document from a file.
            PdfViewer pdf = new();
            pdf.pdfview.LoadDocument(openFileDialog1.FileName);
            pdf.ShowDialog();
        }
        private void ExecuteSnapshot(object parameter)
        {
            int width = Convert.ToInt32(MainWindow.basCanvas.ActualWidth);
            int height = Convert.ToInt32(MainWindow.basCanvas.ActualHeight);
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(MainWindow.basCanvas);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            safeFileDialog1.Filter = "Data Files (*.png)|*.png";
            safeFileDialog1.DefaultExt = "png";
            safeFileDialog1.AddExtension = true;

            if (safeFileDialog1.ShowDialog() == true)
            {
                using (Stream filestream = File.Create(safeFileDialog1.FileName))
                {
                    pngImage.Save(filestream);
                }
            }
        }
        private void ExecuteMergePdfFiles(object parameter)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Types (*.pdf;*.pdf)|*.pdf;*.pdf";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length > 1)
            {

                foreach (string file in openFileDialog1.FileNames)
                {
                    PDFFiles.Add(file);

                }
            }

            //open pdf documents            
            PdfDocument[] docs = new PdfDocument[PDFFiles.Count];
            for (int i = 0; i < PDFFiles.Count; i++)
            {
                docs[i] = new PdfDocument(PDFFiles[i]);

            }

            for (int i = 1; i < PDFFiles.Count; i++)
            {
                docs[0].AppendPage(docs[i]);
            }

            safeFileDialog1.Filter = "Data Files (*.pdf)|*.pdf";
            safeFileDialog1.DefaultExt = "pdf";
            safeFileDialog1.AddExtension = true;

            if (safeFileDialog1.ShowDialog() == true)
            {

                docs[0].SaveToFile(safeFileDialog1.FileName);

                //Initialize the SDK library
                //You have to call this function before you can call any PDF processing functions.


            }
            //PdfCommon.Initialize();

            ////Open and load a PDF document from a file.
            //PdfViewer pdf = new();
            //pdf.pdfview.LoadDocument(safeFileDialog1.FileName);
            //pdf.ShowDialog();

            foreach (PdfDocument doc in docs)
            {
                doc.Close();
            }
            //PDFDocumentViewer("MergeDocuments.pdf");
        }
        private void ExecuteColorPickingView(object parameter)
        {
            var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;
            var initialColor = drawViewModel.SelectedColorForACAD;
            var dialog = new ColorPickerDialog(initialColor);
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                drawViewModel.SelectedColorForACAD = dialog.Color;
            }
        }
        private void ExecuteRedoProgram(object parameter)
        {
            if (RedoStack.Count > 0)
            {
                object newObject = RedoStack.Peek();
                if (newObject.GetType() == typeof(CustomArc))
                {

                    Arcs.Add((CustomArc)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomPolyLine))
                {
                    Lines.Add((CustomPolyLine)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }         
                else if (newObject.GetType() == typeof(CustomCircle))
                {
                    Ellipses.Add((CustomCircle)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomBezierCurve))
                {
                    BezierCurves.Add((CustomBezierCurve)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
            }
        }
        private void ExecuteUndoProgram(object parameter)
        {
            if (UndoStack.Count > 0)
            {
                object newObject = UndoStack.Peek();
                if (newObject.GetType() == typeof(CustomArc))
                {

                    Arcs.Remove((CustomArc)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomPolyLine))
                {
                    Lines.Remove((CustomPolyLine)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomCircle))
                {
                    Ellipses.Remove((CustomCircle)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomBezierCurve))
                {
                    BezierCurves.Remove((CustomBezierCurve)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
            }
        }
        private void ExecuteAcadDrawing(object parameter)
        {
            var NewProjectViewModel = System.Windows.Application.Current.FindResource("newProjectViewModel") as NewProjectViewModel;
            NewProjectViewModel.WelcomeVisibility = Visibility.Collapsed;
            object resourceCanvasGrid = Draw.drawing.TryFindResource("canvasGrid");
            DrawingBrush gridBrush = (DrawingBrush)resourceCanvasGrid;
            gridBrush.TileMode = TileMode.None;
            gridBrush.Viewport = new Rect(0, 0, 0, 0);

        }
        private void ExecutePrint(object parameter)
        {
            System.Windows.Controls.PrintDialog printDlg = new();
            printDlg.ShowDialog();
        }
        private void ExecuteSave(object parameter)
        {
            SavePath = ProjectPath + "/" + ProjectName;
            saveAndSaveAs(SavePath);
            InfoExtractor.extractExtraInfo(SavePath, null);
        }
        private void ExecuteOpen(object parameter)
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
                        //bool finished = await loadEuxml(f);
                    }
                }
                ProjectName = OpenProjectPath.Split("\\")[^1];
                activateButtons();
                WelcomeVisibility = Visibility.Collapsed;
            }

            loadingObject.LoadingReport = "Finished...";
            loadingObject.stopLoading();
        }
        private void ExecuteSaveAs(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            SavePath = folderBrowserDialog1.SelectedPath;
            if (!SavePath.Equals(""))
            {
                saveAndSaveAs(SavePath);
            }
        }
        #endregion
        
        #region logic

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
                if (eulynxModel != null)
                {
                    var eulynxService = EulynxService.getInstance();
                    eulynxService.serialization(eulynxModel, "", SavePath);
                }


                //serialize
                BinaryFormatter bf = new();

                FileStream fsout = new(SavePath + "/save.APlan", FileMode.Create, FileAccess.Write, FileShare.None);

                using (fsout)
                {
                    bf.Serialize(fsout, allInfo);
                }
                fsout.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Saving directory don't Exist. Please create a project.");
            }
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

        /// <summary>
        /// activate the buttons like save, save as ... that are not allowed to be activated in the beginning.
        /// </summary>
        public static void activateButtons()
        {
            SaveButtonActive = true;
            SaveAsButtonActive = true;
        }

        #endregion
    }
}
