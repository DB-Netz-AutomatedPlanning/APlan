using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using APLan.ViewModels;
using System.IO;
using APLan.HelperClasses;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        #region attributes
        private NewProjectViewModel newprojectClass;
        private SolidColorBrush themeColor;

        //hints getters and setters
        private string ProjectNameHint { get; set;}
        private string DirPathHint { get; set; }
        private string JsonFilesHint { get; set; }
        private string EuxmlFileHint { get; set; }
        private string MdbFileHint { get; set; }
        private string PpxmlFileHint { get; set; }
        #endregion

        #region constructor
        public NewProjectWindow()
        {

            //define the hint to the user.
            ProjectNameHint = "please enter a project name";
            DirPathHint = "please select a project directory";
            JsonFilesHint = "please select all .json/.geojson files";
            EuxmlFileHint = "please select .Euxml file";
            MdbFileHint = "please select single .Mdb file";
            PpxmlFileHint = "please select single .PPXML file";

            newprojectClass = System.Windows.Application.Current.FindResource("newProjectViewModel") as NewProjectViewModel;
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
            InitializeComponent();
            fileType.SelectedIndex = 0;
        }
        #endregion

        #region logic
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
                directortPath.Foreground = Brushes.Black;
                directortPath.Background = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;   
            }
            else if (directortPath.Text.Equals(DirPathHint))
            {
                directortPath.Foreground = Brushes.Gray;
                directortPath.Background = Brushes.White;
            }
            else
            {
                directortPath.Foreground = Brushes.Black;
                directortPath.Background = Brushes.White;
            }
            activateCreation();
        }

        private void Tmdbtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{Tmdbtext.Text}") && Path.GetExtension(@$"{Tmdbtext.Text}").EqualsIgnoreCase(".mdb"))
            {
                Tmdbtext.Foreground = Brushes.Black;
                Tmdbtext.Background = themeColor;
                
            }
            else if(Tmdbtext.Text.Equals(MdbFileHint))
            {
                Tmdbtext.Foreground = Brushes.Gray;
                Tmdbtext.Background = Brushes.White;

            }
            else if (!File.Exists(@$"{Tmdbtext.Text}") || !Path.GetExtension(@$"{Tmdbtext.Text}").EqualsIgnoreCase(".mdb"))
            {
                Tmdbtext.Foreground = Brushes.Black;
                Tmdbtext.Background = Brushes.White;
            }
            activateCreation();
        }

        private void euxmltext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{euxmltext.Text}") && Path.GetExtension(@$"{euxmltext.Text}").EqualsIgnoreCase(".euxml"))
            {
                euxmltext.Foreground = Brushes.Black;
                euxmltext.Background = themeColor;  
            }
            else if (euxmltext.Text.Equals(EuxmlFileHint))
            {
                euxmltext.Foreground = Brushes.Gray;
                euxmltext.Background = Brushes.White;
            }
            else
            {
                euxmltext.Foreground = Brushes.Black;
                euxmltext.Background = Brushes.White;   
            }
            activateCreation();
        }

        private void projetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!projetName.Text.Equals(ProjectNameHint) && !projetName.Text.Equals(""))
            {
                projetName.Foreground = Brushes.Black;
                projetName.Background = themeColor;     
            }
            else if (projetName.Text.Equals(ProjectNameHint))
            {
                projetName.Foreground = Brushes.Gray;
                projetName.Background = Brushes.White;
            }
            else
            {
                projetName.Foreground = Brushes.Black;
                projetName.Background = Brushes.White; 
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
                    if (!file.Equals("") && !File.Exists(file) && (Path.GetExtension(file).EqualsIgnoreCase(".json")|| Path.GetExtension(file).EqualsIgnoreCase(".geojson")))
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
                    Json.Foreground = Brushes.Black;
                    Json.Background = themeColor;  
                }
                else
                {
                    Json.Foreground = Brushes.Gray;
                    Json.Background = Brushes.White;     
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
        private void newProjectWindow_Loaded(object sender, System.EventArgs e)
        {
            //reset Hints
            newprojectClass.ProjectName = ProjectNameHint;
            newprojectClass.ProjectPath = DirPathHint;
            newprojectClass.JsonFiles = JsonFilesHint;
            newprojectClass.EUXML = EuxmlFileHint;
            newprojectClass.MDB = MdbFileHint;
            newprojectClass.PPXML = PpxmlFileHint;

            //reset inputBackground
            directortPath.Background = Brushes.White;
            projetName.Background = Brushes.White;
            Json.Background = Brushes.White;
            Tmdbtext.Background = Brushes.White;
            euxmltext.Background = Brushes.White;
            ppxmltext.Background = Brushes.White;
        }

        #endregion
    }
}
