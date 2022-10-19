using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using APLan.ViewModels;
using System.Collections;
using System.IO;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        private NewProjectViewModel newprojectClass;
        private SolidColorBrush themeColor;
        public NewProjectWindow()
        {
            newprojectClass = System.Windows.Application.Current.FindResource("newProjectViewModel") as NewProjectViewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
            InitializeComponent();
            fileType.SelectedIndex = 0;


        }
        private void fileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (createProject != null)
                createProject.IsEnabled = false;
            if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".json"))
            {
                container.RowDefinitions[5].Height = new GridLength(0);
                container.RowDefinitions[6].Height = new GridLength(0);
                container.RowDefinitions[7].Height = new GridLength(0);
                for (int i = 4; i < container.RowDefinitions.Count - 4; i++)
                {
                    container.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
                }
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".mdb"))
            {
                for (int i = 4; i < container.RowDefinitions.Count - 1; i++)
                {
                    container.RowDefinitions[i].Height = new GridLength(0);
                }
                container.RowDefinitions[5].Height = new GridLength(1, GridUnitType.Star);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".euxml"))
            {
                for (int i = 4; i < container.RowDefinitions.Count - 1; i++)
                {
                    container.RowDefinitions[i].Height = new GridLength(0);
                }
                container.RowDefinitions[6].Height = new GridLength(1, GridUnitType.Star);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".ppxml"))
            {
                for (int i = 4; i < container.RowDefinitions.Count - 1; i++)
                {
                    container.RowDefinitions[i].Height = new GridLength(0);
                }
                container.RowDefinitions[7].Height = new GridLength(1, GridUnitType.Star);
            }
            activateCreation();
        }

        private void directortPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(@$"{directortPath.Text}"))
            {
                directortPath.Background = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
                newprojectClass.HelperHintProjectPath = Brushes.Black;
            }
            else
            {
                newprojectClass.HelperHintProjectPath = Brushes.Gray;
                directortPath.Background = Brushes.White;
            }
            activateCreation();
        }

        private void Tmdbtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{Tmdbtext.Text}"))
            {
                newprojectClass.HelperHintMdb = Brushes.Black;
                Tmdbtext.Background = themeColor;
                
            }
            else
            {
                newprojectClass.HelperHintMdb = Brushes.Gray;
                Tmdbtext.Background = Brushes.White;
            }
            activateCreation();
        }

        private void euxmltext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{euxmltext.Text}"))
            {
                euxmltext.Background = themeColor;
                newprojectClass.HelperHintEuxml = Brushes.Black;
            }
            else
            {
                euxmltext.Background = Brushes.White;
                newprojectClass.HelperHintEuxml = Brushes.Gray;
            }
            activateCreation();
        }

        private void projetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!projetName.Text.Equals("please enter a project name") && !projetName.Text.Equals(""))
            {
                projetName.Background = themeColor;
                newprojectClass.HelperHintProjectName = Brushes.Black;
            }
            else
            {
                projetName.Background = Brushes.White;
                newprojectClass.HelperHintProjectName = Brushes.Gray;
            }
            activateCreation();
        }

        private void Json_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Json.Text.Equals(""))
            {
                bool check = true;
                string [] fileNames = Json.Text.Split("+~+");
                foreach (string file in fileNames)
                {
                    if (!file.Equals("") && !File.Exists(file))
                    {
                        check = false;
                    }
                    else
                    {
                        switch (System.IO.Path.GetFileNameWithoutExtension(file))
                        {
                            case "Entwurfselement_HO":
                                newprojectClass.Entwurfselement_HO = file;
                                break;

                            case "Entwurfselement_KM":
                                newprojectClass.Entwurfselement_KM = file;
                                break;
                            case "Entwurfselement_LA":
                                newprojectClass.Entwurfselement_LA = file;
                                break;

                            case "Entwurfselement_UH":
                                newprojectClass.Entwurfselement_UH = file;
                                break;
                            case "Gleiskanten":
                                newprojectClass.Gleiskanten = file;
                                break;

                            case "Gleisknoten":
                                newprojectClass.Gleisknoten = file;
                                break;

                            default:

                                break;
                        }
                    }
                    
                }
                check = checkAllJson();
                if (check==true)
                {
                    Json.Background = themeColor;
                    newprojectClass.HelperHintJsonFiles = Brushes.Black;
                }
                else
                {
                    Json.Background = Brushes.White;
                    newprojectClass.HelperHintJsonFiles = Brushes.Gray;
                }  
            }
            activateCreation();
        }
        /// <summary>
        /// check that all files are attached correctly.
        /// </summary>
        /// <returns></returns>
        public bool checkAllJson()
        {
            bool check = true;
            if (newprojectClass.Entwurfselement_HO==null)
            {
                check = false;
            }
            if (newprojectClass.Entwurfselement_KM == null)
            {
                check = false;
            }
            if (newprojectClass.Entwurfselement_LA == null)
            {
                check = false;
            }
            if (newprojectClass.Entwurfselement_UH == null)
            {
                check = false;
            }
            if (newprojectClass.Gleiskanten == null)
            {
                check = false;
            }
            if (newprojectClass.Gleisknoten == null)
            {
                check = false;
            }

            return check;
        }
        /// <summary>
        /// activate the creation button whenever all conditions are met.
        /// </summary>
        public void activateCreation()
        {
            
            if (fileType!=null && Json!=null && Tmdbtext!=null && euxmltext!=null && createProject!=null)
            {
                bool check = true;
                if (projetName.Background != themeColor || directortPath.Background != themeColor)
                {
                    check = false;
                }
                if (fileType.SelectedIndex== 0  && Json.Background!=themeColor)
                {
                    check = false;
                }
                else if(fileType.SelectedIndex == 1  && Tmdbtext.Background != themeColor)
                {
                    check = false;
                }
                else if (fileType.SelectedIndex == 2  && euxmltext.Background != themeColor)
                {
                    check = false;
                }
                if (check==true)
                {
                    createProject.IsEnabled = true;
                }
                else
                {
                    createProject.IsEnabled = false;
                }
            }
        }

    }
}
