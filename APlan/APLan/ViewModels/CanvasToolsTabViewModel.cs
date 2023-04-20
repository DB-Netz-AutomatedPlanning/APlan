using APLan.Commands;
using APLan.Views;
using System.Windows.Input;
using System.Windows.Media;

namespace APLan.ViewModels
{
    public class CanvasToolsTabViewModel : BaseViewModel
    {
        #region attributes
        private Brush _selectBrush;
        private Brush _moveBrush;
        private Brush _dragBrush;
        private Brush _selectedBrush;
        private Brush _gridBrush;
        private Brush _copy;
        private Brush _paste;
        #endregion

        #region properties
        public Brush SelectBrush
        {
            get
            {
                return _selectBrush;
            }

            set
            {
                _selectBrush = value;
                OnPropertyChanged("SelectBrush");
            }
        }
        public Brush MoveBrush
        {
            get
            {
                return _moveBrush;
            }

            set
            {
                _moveBrush = value;
                OnPropertyChanged("MoveBrush");
            }
        }
        public Brush DragBrush
        {
            get
            {
                return _dragBrush;
            }

            set
            {
                _dragBrush = value;
                OnPropertyChanged("DragBrush");
            }
        }
        public Brush SelectedBrush
        {
            get
            {
                return _selectedBrush;
            }

            set
            {
                _selectedBrush = value;
                OnPropertyChanged("DragBrush");
            }
        }
        public Brush GridBrush
        {
            get
            {
                return _gridBrush;
            }

            set
            {
                _gridBrush = value;
                OnPropertyChanged("GridBrush");
            }
        }
        public Brush CopyBrush
        {
            get
            {
                return _copy;
            }

            set
            {
                _copy = value;
                OnPropertyChanged("CopyBrush");
            }
        }
        public Brush PasteBrush
        {
            get
            {
                return _paste;
            }

            set
            {
                _paste = value;
                OnPropertyChanged("PasteBrush");
            }
        }
        #endregion

        #region commands
        public ICommand SelectButton { get; set; }
        public ICommand MoveButton { get; set; }
        public ICommand DragButton { get; set; }
        public ICommand GridButton { get; set; }
        public ICommand CopyButton { get; set; }
        public ICommand PasteButton { get; set; }
        #endregion

        #region constructor
        public CanvasToolsTabViewModel()
        {
            MoveButton = new RelayCommand(ExecuteMoveButton);
            DragButton = new RelayCommand(ExecuteDragButton);
            SelectButton = new RelayCommand(ExecuteSelectButton);
            GridButton = new RelayCommand(ExecuteSelectGridButton);
            CopyButton = new RelayCommand(ExecuteCopyButton);
            PasteButton = new RelayCommand(ExecutePasteButton);
            SelectBrush = Brushes.White;
            MoveBrush = Brushes.White;
            DragBrush = Brushes.White;
            GridBrush = Brushes.White;
            CopyBrush = Brushes.White;
            PasteBrush = Brushes.White;

            SelectedBrush = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }
        #endregion

        #region  command logic 

        /// <summary>
        /// allow selection of grid
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecutePasteButton(object parameter)
        {
            var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.Paste)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.Paste;
                PasteBrush = SelectBrush;
                CopyBrush = Brushes.White;
                SelectBrush = Brushes.White;
                MoveBrush = Brushes.White;
                DragBrush = Brushes.White;
                GridBrush = Brushes.White;
                drawViewModel.drawLogic.pasteSelected(Clipboard, Lines,Ellipses,Arcs,Texts,UndoStack);

            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Paste)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                PasteBrush = Brushes.White;

            }

        }
        /// <summary>
        /// allow selection of grid
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteCopyButton(object parameter)
        {
            var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.Copy)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.Copy;                 
                CopyBrush = SelectedBrush;
                SelectBrush = Brushes.White;
                MoveBrush = Brushes.White;
                DragBrush = Brushes.White;
                GridBrush = Brushes.White;
                PasteBrush = Brushes.White;
                drawViewModel.drawLogic.copySelected(selected,Clipboard,Lines,Ellipses,Arcs,Texts); 

            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Copy)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                CopyBrush = Brushes.White;                

            }

        }
        /// <summary>
        /// allow selection of grid
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteSelectGridButton(object parameter)
        {
            var drawViewModel = System.Windows.Application.Current.FindResource("drawViewModel") as DrawViewModel;
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.Grid)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.Grid;
                 
                GridBrush = SelectedBrush;
                SelectBrush = Brushes.White;
                MoveBrush = Brushes.White;
                DragBrush = Brushes.White;
                CopyBrush = Brushes.White;
                PasteBrush = Brushes.White;
                DrawingBrush gridBrush = (DrawingBrush)drawViewModel.LayoutCanvas.TryFindResource("canvasGrid");                
                gridBrush.TileMode = TileMode.None;
                
            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Grid)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                GridBrush = Brushes.White; 
                DrawingBrush gridBrush = (DrawingBrush)drawViewModel.LayoutCanvas.TryFindResource("canvasGrid");
                gridBrush.TileMode = TileMode.Tile;
                 
            }

        }
        /// <summary>
        /// allow selection
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteSelectButton(object parameter)
        {
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.MultiSelect)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.MultiSelect;
                SelectBrush = SelectedBrush;
                MoveBrush = Brushes.White;
                DragBrush = Brushes.White;
                GridBrush = Brushes.White;
                CopyBrush = Brushes.White;
                PasteBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Cross;
                
            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.MultiSelect)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                SelectBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }

        }
        /// <summary>
        /// allow moving
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteMoveButton(object parameter)
        {
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.Move)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.Move;
                SelectBrush = Brushes.White;
                MoveBrush = SelectedBrush;
                DragBrush = Brushes.White;
                GridBrush = Brushes.White;
                CopyBrush = Brushes.White;
                PasteBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.SizeAll;
            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Move)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                MoveBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }

        }
        /// <summary>
        /// allow dragging
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteDragButton(object parameter)
        {
            if (DrawViewModel.tool != DrawViewModel.SelectedTool.Drag)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.Drag;
                SelectBrush = Brushes.White;
                MoveBrush = Brushes.White;
                DragBrush = SelectedBrush;
                GridBrush = Brushes.White;
                CopyBrush = Brushes.White;
                PasteBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Hand;
            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Drag)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                DragBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        /// <summary>
        /// allow draging for a text
        /// </summary>
        /// <param name="e"></param>
        #endregion
    }
}
