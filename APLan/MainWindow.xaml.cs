using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace APLan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static ColumnDefinition c0;
        public static ColumnDefinition c1;
        public static ColumnDefinition c2;
        public static ColumnDefinition c3;

        public static RowDefinition r0;
        public static RowDefinition r1;
        public static RowDefinition r2;
        public static RowDefinition r3;
        public static RowDefinition r4;

        public static UserControl drawing;
        public static UserControl visualized_Data;
        public static UserControl Canvas_Content;

        public static UserControl aplanCADViewer;

        public static Canvas basCanvas;
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;

            c0 = this.C0;
            c1 = this.C1;
            c2 = this.C2;
            c3 = this.C3;

            r0 = this.R0;
            r1 = this.R1;
            r2 = this.R2;
            r3 = this.R3;
            r4 = this.R4;

            drawing = this.drawingTab;
            basCanvas = this.baseCanvas;

            visualized_Data = this.visualizedData;
            Canvas_Content = this.CanvasContent;
            aplanCADViewer = this.AplanCADViewer;



        }

        private void drawing_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //   pdfDetailsView.container.Width = e.NewSize.Width;
        //   pdfDetailsView.container.Height = e.NewSize.Height;

        //   double xChange = 1, yChange = 1;

        //   if (e.PreviousSize.Width != 0)
        //       xChange = (e.NewSize.Width / e.PreviousSize.Width);

        //   if (e.PreviousSize.Height != 0)
        //       yChange = (e.NewSize.Height / e.PreviousSize.Height);

        //   ScaleTransform scale = new ScaleTransform(pdfDetailsView.container.LayoutTransform.Value.M11 * xChange, pdfDetailsView.container.LayoutTransform.Value.M22 * yChange);
        //   pdfDetailsView.container.LayoutTransform = scale;
        //   pdfDetailsView.container.UpdateLayout();
        //}
    }
}
