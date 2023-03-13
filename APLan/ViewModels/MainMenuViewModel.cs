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
using org.apache.xml.resolver.helpers;
using Dsafa.WpfColorPicker;
using aplan.database;
using System.Windows.Media.Imaging;
using java.util;
using System;
using java.nio.file;
using Spire.Pdf;

using net.sf.saxon.functions;
using Patagames.Pdf.Net;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace APLan.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private Microsoft.Win32.SaveFileDialog safeFileDialog1;
        private OpenFileDialog openFileDialog1;
        private List<String> pdfFiles = null;
        public string SavePath
        {
            get;
            set;
        }
        public List<String> PDFFiles
        {
            get { return pdfFiles; }
            set
            {
                pdfFiles = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands      


        public ICommand NewProject { get; set; }
        public ICommand AddData { get; set; }
        public ICommand PreviewData { get; set;}
        public ICommand RemoveData { get; set; }
        public ICommand Print { get; set; }
        public ICommand EulynxValidator { get; set; }
        public ICommand ERDMvalidator { get; set; }
        public ICommand ExitProgram { get; set; }
        public ICommand AboutWPF { get; set; }
        public ICommand Save { get; set; }
        public ICommand SaveAs { get; set; }
        
        public ICommand Undo { get; set; }

        public ICommand Redo { get; set; }

        public ICommand ColorPicker { get; set; }

        public ICommand AcadDrawing { get; set; }

        public ICommand ChooseProjectType { get; set; }

        public ICommand ERDM { get; set; }

        public ICommand Snapshot { get; set; }

        public ICommand MergePdfFiles { get; set; }

        public ICommand PdfViewer { get; set; }




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
            ERDMvalidator = new RelayCommand(ExecuteERDMvalidator);
            AboutWPF = new RelayCommand(ExecuteAboutWPF);
            ExitProgram = new RelayCommand(ExecuteExitProgram);
            folderBrowserDialog1 = new();
            safeFileDialog1 = new Microsoft.Win32.SaveFileDialog();

            Undo = new RelayCommand(ExecuteUndoProgram);
            Redo = new RelayCommand(ExecuteRedoProgram);
            AcadDrawing = new RelayCommand(ExecuteAcadDrawing);
            ColorPicker = new RelayCommand(ExecuteColorPickingView);
            ChooseProjectType = new RelayCommand(ChooseProject);
            ERDM = new RelayCommand(ERDMmodel);
            Snapshot = new RelayCommand(ExecuteSnapshot);
            MergePdfFiles = new RelayCommand(ExecuteMergePdfFiles);
            openFileDialog1 = new OpenFileDialog();
            PDFFiles = new List<String>();
            PdfViewer = new RelayCommand(ExecutePDFViewer);


        }
        #endregion

        #region logic


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

            for(int i=1;i<PDFFiles.Count;i++)
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

        private void ChooseProject(object parameter)
        {
            ChooseProject choose = new();
            choose.ShowDialog();
        }

        private void ERDMmodel(object parameter)
        {
            ((Window)parameter).Close();

            ERDMproject choose = new();
            choose.ShowDialog();
        }


        private void ExecuteColorPickingView(object parameter)
        {
            var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel ;
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

                    Arc_List.Add((CustomArc)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomPolyLine))
                {
                    Polyline_List.Add((CustomPolyLine)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomLine))
                {
                    Line_List.Add((CustomLine)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomEllipse))
                {
                    Ellipse_List.Add((CustomEllipse)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomBezierCurve))
                {
                    Bezier_Curve_List.Add((CustomBezierCurve)newObject);
                    RedoStack.Pop();
                    UndoStack.Push(newObject);
                }
            }
        }
        private void ExecuteUndoProgram(object parameter)
        {
            if(UndoStack.Count > 0)
            {
                object newObject = UndoStack.Peek();
                if(newObject.GetType() == typeof(CustomArc))
                {

                    Arc_List.Remove((CustomArc)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if(newObject.GetType() == typeof(CustomPolyLine))
                {
                    Polyline_List.Remove((CustomPolyLine)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if(newObject.GetType() == typeof(CustomLine))
                {
                    Line_List.Remove((CustomLine)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if(newObject.GetType() == typeof(CustomEllipse))
                {
                    Ellipse_List.Remove((CustomEllipse)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
                else if (newObject.GetType() == typeof(CustomBezierCurve))
                {
                    Bezier_Curve_List.Remove((CustomBezierCurve)newObject);
                    UndoStack.Pop();
                    RedoStack.Push(newObject);
                }
            }
        }
        private void ExecuteNewProjectWindow (object parameter)
        {
            ((Window)parameter).Close(); //close choosing window.
            NewProjectWindow newProject = new();
            newProject.ShowDialog();
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
        private void ExecuteERDMvalidator(object parameter)
        {
            ERDMvalidator validator = new();
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
            }
            else
            {
                System.Windows.MessageBox.Show("Saving directory don't Exist. Please create a project.");
            }
        }

        #endregion
    }
}
