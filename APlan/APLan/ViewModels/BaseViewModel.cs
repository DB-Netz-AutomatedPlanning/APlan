using aplan.core;
using APLan.HelperClasses;
using APLan.Model.CustomObjects;
using APLan.Views;
using Models.TopoModels.EULYNX.generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace APLan.ViewModels
{
    public class BaseViewModel : Loading
    {
        #region staticPropertyChanged
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        #region fields
        private static string projectType;
        private static string projectPath;
        private static string projectName;
        public static ERDM.ERDMmodel erdmModel;
        public static EulynxDataPrepInterface eulynxModel;


        public static Loading loadingObject;
        private static bool listsCreated = false; // to lock creation of new lists for binding.
        private Visibility _welcomeVisibility = Visibility.Visible;
        #endregion

        #region properites
        public Visibility WelcomeVisibility
        {
            get { return _welcomeVisibility; }
            set
            {
                _welcomeVisibility = value;
                OnPropertyChanged();
            }
        }
        public static string ProjectName
        {
            get => projectName;
            set
            {
                if (projectName != value)
                {
                    projectName = value;
                    RaiseStaticPropertyChanged(nameof(ProjectName));// reports this property
                }
            }
        }
        public static string ProjectPath
        {
            get => projectPath;
            set
            {
                if (projectPath != value)
                {
                    projectPath = value;
                    RaiseStaticPropertyChanged(nameof(ProjectPath));// reports this property
                }
            }
        }
        public static string ProjectType
        {
            get {
                return projectType;
            }
            set
            {
                if (projectType != value)
                {
                    projectType = value.Split(":").Length>1? value.Split(":")[1].Trim():value;
                    RaiseStaticPropertyChanged(nameof(ProjectType));// reports this property
                }
            }
        }
        public static List<UIElement> toBeStored
        {
            get;
            set;
        }
        public static ObservableCollection<UIElement> selected
        {
            get;
            set;
        }
        public static ObservableCollection<Signalinfo> Signals
        {
            get;
            set;
        }
        public static ObservableCollection<CanvasObjectInformation> loadedObjects
        {
            get;
            set;
        }
        public static ObservableCollection<CustomPolyLine> Lines
        {
            get;
            set;
        }
        public static ObservableCollection<CustomCircle> Ellipses
        {
            get;
            set;
        }
        public static ObservableCollection<CustomArc> Arcs
        {
            get;
            set;
        }
        public static ObservableCollection<CustomBezierCurve> BezierCurves
        {
            get;
            set;
        }
        public static ObservableCollection<CustomTextBlock> Texts
        {
            get;
            set;
        }
        public static ObservableCollection<CustomImage> Images
        {
            get; set;
        }


        public ObservableCollection<CustomRectangle> Rectangle_Shape_points_List
        {
            get;
            set;
        }
        public static Stack<object> UndoStack
        {
            get;
            set;
        }
        public static Stack<object> RedoStack
        {
            get;set;
        }

        
        public static ObservableCollection<SymbolObject> items
        {
            get;
            set;
        }

        public static ObservableCollection<CustomArrowLine> Arrows
        { 
            get;
            set;
        }
        
        public static ObservableCollection<object> Clipboard
        {
            get;
            set;
        }
        #endregion
        
        #region constructor
        public BaseViewModel()
        {
            if(listsCreated == false) {
                Lines = new();
                Ellipses = new();
                Texts = new();
                Arcs = new();
                Signals = new();

                Rectangle_Shape_points_List = new();
                BezierCurves = new();
                Images = new();
       
                loadedObjects = new();
                items = new();
                listsCreated = true;
                UndoStack = new();
                RedoStack = new();
                Arrows = new();
                Clipboard = new();
            }
        }
        #endregion
        
        #region logic
        /// <summary>
        /// cancel any selection.
        /// </summary>
        public static void CancelSelected()
        {
            ClearSelection(selected);
        }
        public static void ClearSelection(ObservableCollection<UIElement> selected)
        {
            ResetPreSelectionColor(selected);
            selected.Clear();
        }
        public static void ResetToolSelection()
        {
            DrawViewModel.tool = DrawViewModel.SelectedTool.None;
            var canvasToolsTabViewModel = System.Windows.Application.Current.Resources["canvasToolsTabViewModel"] as CanvasToolsTabViewModel;
            canvasToolsTabViewModel.SelectBrush = Brushes.White;
            canvasToolsTabViewModel.MoveBrush = Brushes.White;
            canvasToolsTabViewModel.DragBrush = Brushes.White;
        }
        public static void ResetPreSelectionColor(ObservableCollection<UIElement> selected)
        {
            foreach (UIElement selec in selected)
            {
                selec.Opacity = 1;
            }
        }
        /// <summary>
        /// clear all data drawn previously.
        /// </summary>
        public static void clearDrawings()
        {
            //clear old data.
            Lines.Clear();
            Ellipses.Clear();
            Arcs.Clear();
            BezierCurves.Clear();
            Texts.Clear();
            Images.Clear();

            Signals.Clear();
            toBeStored.Clear();
            UndoStack.Clear();
            RedoStack.Clear();
            Arrows.Clear();
            Clipboard.Clear();

            ViewModels.DrawViewModel.GlobalDrawingPoint = new(0, 0);
        }
        /// <summary>
        /// update the binded collections after project creation.
        /// </summary>
        public static void updateBindedCollections()
        {
            CollectionViewSource.GetDefaultView(Lines).Refresh();
            CollectionViewSource.GetDefaultView(Ellipses).Refresh();
            CollectionViewSource.GetDefaultView(Arcs).Refresh();
            CollectionViewSource.GetDefaultView(Texts).Refresh();
            CollectionViewSource.GetDefaultView(Arrows).Refresh();
            CollectionViewSource.GetDefaultView(Clipboard).Refresh();

        }
        #endregion
    }
}
