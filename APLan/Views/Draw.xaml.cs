using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using APLan.ViewModels;
using Point = System.Windows.Point;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for Draw.xaml
    /// </summary>
    public partial class Draw : UserControl
    {
        Point InitialDragPoint = new Point(-1, -1);
        public static Canvas drawing;
        public static Canvas hostToDraw;
        public static Canvas baseToDraw;
        public static ScrollViewer drawingScrollViewer;
        public static ItemsControl mycontrol;


        public static ColumnDefinition visualizedDataColumn;
        public static RowDefinition signalresultsRow;

        public static ItemsControl myKantenLines;

        public static SolidColorBrush canvasGridColor;
        public Draw()
        {
            InitializeComponent();
            drawing = mycanvas;
            drawingScrollViewer = DrawingViewer;
            signalresultsRow = this.SignalOutput;
            myKantenLines = KantenLines;
            canvasGridColor = System.Windows.Application.Current.Resources["gridColor"] as SolidColorBrush;
        }
        private void drawingCanvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(drawing);
            object data = e.Data.GetData(DataFormats.Serializable);
          
            if (data is UIElement element && element.GetType() != typeof(Canvas))
            {
                // element.MouseMove += drag;
                
                if (drawing.Children.Contains(element))
                {
                    Canvas.SetLeft(element, dropPosition.X - InitialDragPoint.X);
                    Canvas.SetTop(element, dropPosition.Y - InitialDragPoint.Y);
                    CustomProperites.SetLocation(element,new Point(dropPosition.X - InitialDragPoint.X, dropPosition.Y - InitialDragPoint.Y));
                }
                else
                {
                    Canvas.SetLeft(element, dropPosition.X);
                    Canvas.SetTop(element, dropPosition.Y);
                    if (drawing.Children.Contains(element) == false)
                    {
                        DrawViewModel.toBeStored.Add(element);
                        
                        drawing.Children.Add(element);
                        
                    }

                }
            }
        }

        private void drag(object sender, MouseEventArgs e)
        {
            if (e.Source.GetType() != typeof(Canvas))
            {
                //dargging is active
                if (DrawViewModel.tool == DrawViewModel.SelectedTool.Drag)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        //dragged = (UIElement)sender;
                        if (InitialDragPoint.X == -1)
                        {
                            InitialDragPoint = e.GetPosition((UIElement)sender);
                        }
                        //preform drag and drop.
                        DragDrop.DoDragDrop((UIElement)sender, new DataObject(DataFormats.Serializable, (UIElement)sender), DragDropEffects.Move);
                    }
                    if (e.LeftButton == MouseButtonState.Released)
                    {
                        InitialDragPoint = new Point(-1, -1);
                    }
                }
            }
        }
    }
}
