using System;
using System.Collections.Generic;
using System.IO;
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
using APLan.HelperClasses;
using APLan.ViewModels;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for EulynxValidator.xaml
    /// </summary>
    public partial class EulynxValidator : Window
    {
        #region attributes
        private EulynxValidatorViewModel validatorViewModel;
        private SolidColorBrush themeColor;
        private string FilePathHint { get; set; }
        private string OutputPathHint { get; set; }
        #endregion

        #region constructor
        public EulynxValidator()
        {
            FilePathHint= "please enter the path for the .euxml file";
            OutputPathHint = "please select the output path for the validation report";
            InitializeComponent();
            validatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as EulynxValidatorViewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }
        #endregion

        #region logic
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (File.Exists(textbox.Text)&& System.IO.Path.GetExtension(textbox.Text).EqualsIgnoreCase(".euxml"))
            {
                textbox.Background = themeColor;
                textbox.Foreground = Brushes.Black;
                if (outputBox?.Background== themeColor)
                {
                    activeDeactiveValidation(true);
                }
            }
            else if (textbox.Text.Equals(FilePathHint))
            {
                textbox.Foreground = Brushes.Gray;
                textbox.Background = Brushes.White;
                activeDeactiveValidation(false);
            }
            else
            {
                textbox.Background = Brushes.White;
                textbox.Foreground = Brushes.Black;
                activeDeactiveValidation(false);
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (Directory.Exists(textbox.Text))
            {
                textbox.Background = themeColor;
                textbox.Foreground = Brushes.Black;
                if (fileBox.Background == themeColor)
                {
                    activeDeactiveValidation(true);
                }
            }
            else if (textbox.Text.Equals(OutputPathHint))
            {
                textbox.Foreground = Brushes.Gray;
                textbox.Background = Brushes.White;
                activeDeactiveValidation(false);
            }
            else
            {
                textbox.Background = Brushes.White;
                textbox.Foreground = Brushes.Black;
                activeDeactiveValidation(false);
            }
        }
        /// <summary>
        /// activate and deactivate the validation button according to validitiy of input paths
        /// </summary>
        /// <param name="active"></param>
        private void activeDeactiveValidation(bool active)
        {
            if (validate!=null)
            {
                if (active == true)
                {
                    validate.IsEnabled = true;
                }
                else
                {
                    validate.IsEnabled = false;
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            //reset hints 
            validatorViewModel.XML = FilePathHint;
            validatorViewModel.Path = OutputPathHint;
            //reset reports
            validatorViewModel.Report = "";
            validatorViewModel.Report_rules = "";
        }
        #endregion
    }
}
