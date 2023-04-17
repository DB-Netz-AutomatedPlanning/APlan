using APLan.Commands;
using APLan.Converters;
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
        private double _drawingScale;
        #endregion

        #region properties
        public double DrawingScale
        {
            get
            {
                return _drawingScale;
            }

            set
            {
                _drawingScale =value>0?value:1.0;
                OnPropertyChanged("DrawingScale");
            }
        }
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
        #endregion

        #region commands
        public ICommand SelectButton { get; set; }
        public ICommand MoveButton { get; set; }
        public ICommand DragButton { get; set; }
        public ICommand ScaleDrawing { get; set; }
        #endregion

        #region constructor
        public CanvasToolsTabViewModel()
        {
            MoveButton = new RelayCommand(ExecuteMoveButton);
            DragButton = new RelayCommand(ExecuteDragButton);
            SelectButton = new RelayCommand(ExecuteSelectButton);
            ScaleDrawing = new RelayCommand(ExecuteScaleDrawing);
            SelectBrush = Brushes.White;
            MoveBrush = Brushes.White;
            DragBrush = Brushes.White;

            SelectedBrush = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }
        #endregion
        
        #region  command logic 
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
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Hand;
            }
            else if (DrawViewModel.tool == DrawViewModel.SelectedTool.Drag)
            {
                DrawViewModel.tool = DrawViewModel.SelectedTool.None;
                DragBrush = Brushes.White;
                System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void ExecuteScaleDrawing(object parameter)
        {
            CoordinatesConverter.scaleValue = DrawingScale;
            DrawViewModel.GlobalDrawingPoint = new();
            updateBindedCollections();
        }
        #endregion
    }
}
