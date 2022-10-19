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
    /// Interaction logic for EulynxValidator.xaml
    /// </summary>
    public partial class EulynxValidator : Window
    {
        private SolidColorBrush themeColor;
        public EulynxValidator()
        {
            InitializeComponent();
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (File.Exists(textbox.Text))
            {
                textbox.Background = themeColor;
                if (outputBox.Background== themeColor)
                {
                    activeDeactiveValidation(true);
                }
            }
            else
            {
                textbox.Background = Brushes.White;
                activeDeactiveValidation(false);
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = ((TextBox)sender);
            if (Directory.Exists(textbox.Text))
            {
                textbox.Background = themeColor;
                if (fileBox.Background == themeColor)
                {
                    activeDeactiveValidation(true);
                }
            }
            else
            {
                textbox.Background = Brushes.White;
                activeDeactiveValidation(false);
            }
        }
        /// <summary>
        /// activate and deactivate the validation button according to validitiy of input paths
        /// </summary>
        /// <param name="active"></param>
        private void activeDeactiveValidation(bool active)
        {
            if (active==true)
            {
                validate.IsEnabled = true;
            }
            else
            {
                validate.IsEnabled = false;
            }
        }
    }
}
