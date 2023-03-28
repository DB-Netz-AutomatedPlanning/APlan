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
    public partial class Validator : Window
    {
        #region attributes
        private SolidColorBrush themeColor;
        #endregion

        #region constructor
        public Validator()
        {
            InitializeComponent();
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }
        #endregion

        #region logic
        private void fileBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (projectType.SelectedItem.ToString().Contains("ERDM"))
            {
                if (File.Exists(textbox.Text))
                {
                    FileInfo info = new(textbox.Text);
                    if (info.Extension.Equals(".json") || info.Extension.Equals(".xml")) 
                    { 
                         textbox.Background = themeColor;
                    }
                    else
                    {
                         textbox.Background = Brushes.White;
                    }
                }
            }
            if (projectType.SelectedItem.ToString().Contains("EULYNX"))
            {
                if (File.Exists(textbox.Text))
                {
                    FileInfo info = new(textbox.Text);
                    if (info.Extension.Equals(".euxml") || info.Extension.Equals(".xml"))
                    {
                        textbox.Background = themeColor;
                    }
                    else
                    {
                        textbox.Background = Brushes.White;
                    }
                }
            }
            activateValidation();
        }

        private void outPutBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (Directory.Exists(textbox.Text))
            {
                textbox.Background = themeColor;
            }
            else
            {
                textbox.Background = Brushes.White;
            }
            activateValidation();
        }

        private void projectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileBox.Text = null;
            fileBox.Background = Brushes.White;
            if (((ComboBox)sender).SelectedItem.ToString().Contains("ERDM"))
            {
                if (infoPanel != null)
                {
                    infoPanel.Visibility = Visibility.Visible;
                    ERDMValidationNote.Visibility = Visibility.Visible;
                    EULYNXvalidationNote.Visibility = Visibility.Collapsed;

                    validationTab.Visibility = Visibility.Visible;
                    SchemaItem.Header = "JSON/XML vaildation";
                    RulesItem.Visibility = Visibility.Collapsed;
                }
            }

            if (((ComboBox)sender).SelectedItem.ToString().Contains("EULYNX"))
            {
                if (infoPanel != null)
                {
                    infoPanel.Visibility = Visibility.Visible;
                    EULYNXvalidationNote.Visibility = Visibility.Visible;
                    ERDMValidationNote.Visibility = Visibility.Collapsed;

                    validationTab.Visibility = Visibility.Visible;
                    SchemaItem.Header = "XSD vaildation";
                    RulesItem.Visibility = Visibility.Visible;
                }
            }
            activateValidation();
        }

        private void activateValidation()
        {
            if (fileBox.Background == themeColor && outputBox.Background == themeColor)
            {
                validate.IsEnabled = true;
            }
            else
            {
                validate.IsEnabled = false;
            }
                
        }
        #endregion
    }
}
