using APLan.HelperClasses;
using APLan.ViewModels;
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

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for ERDMvalidator.xaml
    /// </summary>
    public partial class ERDMvalidator : Window
    {
        #region attributes
        private EulynxValidatorViewModel validatorViewModel;
        private SolidColorBrush themeColor;
        private string FilePathHint { get; set; }
        private string OutputPathHint { get; set; }
        #endregion
        public ERDMvalidator()
        {
            FilePathHint = "please enter the path for the .json file";
            OutputPathHint = "please select the output path for the validation report";
            InitializeComponent();
            validatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as EulynxValidatorViewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;

            fileBox.Text = FilePathHint;
            outputBox.Text = OutputPathHint;
            reportTextBox.Text = "";
        }
        #region logic
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (File.Exists(textbox.Text) && System.IO.Path.GetExtension(textbox.Text).EqualsIgnoreCase(".json"))
            {
                textbox.Background = themeColor;
                textbox.Foreground = Brushes.Black;
                if (outputBox?.Background == themeColor)
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
            if (validate != null)
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
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = (TextBox)sender;
            //remove content only if a hint is there.
            if (box.Text.Contains("please"))
            {
                ((TextBox)sender).Text = "";
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = (TextBox)sender;
            //check if the box has no input yet.
            if (box.Text.Equals(""))
            {
                switch (box.Name)
                {
                    case nameof(fileBox):
                        fileBox.Text = FilePathHint;
                        break;
                    case nameof(outputBox):
                        outputBox.Text = OutputPathHint;
                        break;
                }
            }
        }
        #endregion
    }
}
