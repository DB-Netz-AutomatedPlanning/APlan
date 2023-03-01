using APLan.ViewModels;
using Microsoft.Windows.Themes;
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
using System.Windows.Shapes;
using System.IO;
namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for ERDMproject.xaml
    /// </summary>
    public partial class ERDMproject : Window
    {
        private ERDMviewModel eRDMnewProjectViewModel;
        private SolidColorBrush themeColor;
        //hints getters and setters
        private string ProjectNameHint { get; set; }
        private string DirPathHint { get; set; }
        private string SegmentsFilesHint { get; set; }
        private string GradientsFileHint { get; set; }
        private string NodesFileHint { get; set; }
        private string EdgesFileHint { get; set; }
        public ERDMproject()
        {
            //define the hint to the user.
            ProjectNameHint = "please enter a project name";
            DirPathHint = "please select a project directory";
            SegmentsFilesHint = "please select segments xls file";
            GradientsFileHint = "please select gradients xls file";
            NodesFileHint = "please select nodes xls file";
            EdgesFileHint = "please select edges xls file";

            eRDMnewProjectViewModel = System.Windows.Application.Current.FindResource("ERDMnewProjectViewModel") as ERDMviewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;

            InitializeComponent();

            projectNameBox.Text = ProjectNameHint;
            projectDirectoryBox.Text = DirPathHint;
            SegmentsFileBox.Text = SegmentsFilesHint;
            GradientsFileBox.Text = GradientsFileHint;
            NodesFileBox.Text = NodesFileHint;
            EdgesFileBox.Text = EdgesFileHint;
        }
        private void projectDirectoryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.IO.Directory.Exists(((TextBox)sender).Text))
            {
                ((TextBox)sender).Background = themeColor;
                activateCreate();
            }
            else
            {
                ((TextBox)sender).Background = Brushes.White;
                activateCreate();
            }
        }
        private void ExcelFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (System.IO.File.Exists(text))
            {
                FileInfo info = new(text);

                if (info.Extension.Equals(".xls"))
                {
                    ((TextBox)sender).Background = themeColor;
                    activateCreate();
                }
                else
                {
                    ((TextBox)sender).Background = Brushes.White;
                    activateCreate();
                }

            }
        }
        private void projectNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (text!=null && text!="" && !text.Equals(ProjectNameHint))
            {
                ((TextBox)sender).Background = themeColor;
                activateCreate();
            }
            else
            {
                ((TextBox)sender).Background = Brushes.White;
                activateCreate();
            }
        }
        /// <summary>
        /// activate create button if all infromation is satisfied.
        /// </summary>
        private void activateCreate()
        {
            if (projectNameBox!=null &&
                projectDirectoryBox != null &&
                SegmentsFileBox != null &&
                GradientsFileBox != null &&
                NodesFileBox != null &&
                EdgesFileBox != null && 
                projectNameBox.Background == themeColor
                && projectDirectoryBox.Background == themeColor
                && SegmentsFileBox.Background == themeColor
                && GradientsFileBox.Background == themeColor
                && NodesFileBox.Background == themeColor
                && EdgesFileBox.Background == themeColor)
            {
                if (createProject != null)
                {
                    createProject.IsEnabled = true;
                }
            }
            else
            {
                if (createProject!=null)
                {
                    createProject.IsEnabled = false;
                }
                
            }
        }
    }
}
