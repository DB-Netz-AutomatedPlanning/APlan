using APLan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for pdfDetail.xaml
    /// </summary>
    public partial class pdfDetail : UserControl
    {
        public static Canvas pdfdetailCan;
        public static UserControl pdfDetailsControl;
        public pdfDetail()
        {
            InitializeComponent();
            pdfdetailCan = this.pdfdetailCanva;
            pdfDetailsControl = this.pdfdetails;
            ScaleTransform scale = new ScaleTransform(0.5, 0.5);
            pdfdetailCan.LayoutTransform = scale;
            pdfdetailCan.UpdateLayout();
            pdfdetailCan.Visibility = Visibility.Collapsed;
            //pdfdetailCan.Visibility = Visibility.Collapsed;

        }
    }
}
