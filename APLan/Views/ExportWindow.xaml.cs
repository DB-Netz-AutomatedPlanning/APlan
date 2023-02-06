using APLan.ViewModels;
using Microsoft.Windows.Themes;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        private EulynxValidatorViewModel validatorViewModel;
        private SolidColorBrush themeColor;
        private string OutputPathHint { get; set; }
        public ExportWindow()
        {
            OutputPathHint = "please enter the output path";
            
            InitializeComponent();
            outputBox.Text = OutputPathHint;
            validatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as EulynxValidatorViewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
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
                box.Text = OutputPathHint;
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (Directory.Exists(textbox.Text))
            {
                textbox.Background = themeColor;
                textbox.Foreground = Brushes.Black;
                if (textbox.Background == themeColor)
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
        private void activeDeactiveValidation(bool active)
        {
            if (export != null)
            {
                if (active == true)
                {
                    export.IsEnabled = true;
                }
                else
                {
                    export.IsEnabled = false;
                }
            }

        }
    }
}
