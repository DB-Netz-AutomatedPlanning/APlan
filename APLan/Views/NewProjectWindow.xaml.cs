using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using APLan.ViewModels;
using System.IO;
using APLan.HelperClasses;
using System.Printing;

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
        private string DxfFileHint { get; set; }
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
            DxfFileHint = "please select single .dxf file";

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
                collapseOtherChoices(4);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".mdb"))
            {
                collapseOtherChoices(5);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".euxml"))
            {
                collapseOtherChoices(6);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".ppxml"))
            {
                collapseOtherChoices(7);
            }
            else if (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString().Equals(".dxf"))
            {
                collapseOtherChoices(8);
            }
            activateCreation();
        }

        private void directortPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(@$"{directortPathBox.Text}"))
            {
                directortPathBox.Foreground = Brushes.Black;
                directortPathBox.Background = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;   
            }
            else if (directortPathBox.Text.Equals(DirPathHint))
            {
                directortPathBox.Foreground = Brushes.Gray;
                directortPathBox.Background = Brushes.White;
            }
            else
            {
                directortPathBox.Foreground = Brushes.Black;
                directortPathBox.Background = Brushes.White;
            }
            activateCreation();
        }

        private void Tmdbtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{mdbFileBox.Text}") && Path.GetExtension(@$"{mdbFileBox.Text}").EqualsIgnoreCase(".mdb"))
            {
                mdbFileBox.Foreground = Brushes.Black;
                mdbFileBox.Background = themeColor;
                
            }
            else if(mdbFileBox.Text.Equals(MdbFileHint))
            {
                mdbFileBox.Foreground = Brushes.Gray;
                mdbFileBox.Background = Brushes.White;

            }
            else if (!File.Exists(@$"{mdbFileBox.Text}") || !Path.GetExtension(@$"{mdbFileBox.Text}").EqualsIgnoreCase(".mdb"))
            {
                mdbFileBox.Foreground = Brushes.Black;
                mdbFileBox.Background = Brushes.White;
            }
            activateCreation();
        }

        private void euxmltext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{euxmlFileBox.Text}") && Path.GetExtension(@$"{euxmlFileBox.Text}").EqualsIgnoreCase(".euxml"))
            {
                euxmlFileBox.Foreground = Brushes.Black;
                euxmlFileBox.Background = themeColor;  
            }
            else if (euxmlFileBox.Text.Equals(EuxmlFileHint))
            {
                euxmlFileBox.Foreground = Brushes.Gray;
                euxmlFileBox.Background = Brushes.White;
            }
            else
            {
                euxmlFileBox.Foreground = Brushes.Black;
                euxmlFileBox.Background = Brushes.White;   
            }
            activateCreation();
        }

        private void dxftext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (File.Exists(@$"{dxfFileBox.Text}") && Path.GetExtension(@$"{dxfFileBox.Text}").EqualsIgnoreCase(".dxf"))
            {
                euxmlFileBox.Foreground = Brushes.Black;
                euxmlFileBox.Background = themeColor;
            }
            else if (dxfFileBox.Text.Equals(DxfFileHint))
            {
                euxmlFileBox.Foreground = Brushes.Gray;
                euxmlFileBox.Background = Brushes.White;
            }
            else
            {
                euxmlFileBox.Foreground = Brushes.Black;
                euxmlFileBox.Background = Brushes.White;
            }
            activateCreation();
        }

        private void projetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!projectNameBox.Text.Equals(ProjectNameHint) && !projectNameBox.Text.Equals(""))
            {
                projectNameBox.Foreground = Brushes.Black;
                projectNameBox.Background = themeColor;     
            }
            else if (projectNameBox.Text.Equals(ProjectNameHint))
            {
                projectNameBox.Foreground = Brushes.Gray;
                projectNameBox.Background = Brushes.White;
            }
            else
            {
                projectNameBox.Foreground = Brushes.Black;
                projectNameBox.Background = Brushes.White; 
            }
            activateCreation();
        }

        private void Json_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!jsonFilesBox.Text.Equals(""))
            {
                bool check = true;
                string [] fileNames = jsonFilesBox.Text.Split("+~+");
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
                    jsonFilesBox.Foreground = Brushes.Black;
                    jsonFilesBox.Background = themeColor;  
                }
                else
                {
                    jsonFilesBox.Foreground = Brushes.Gray;
                    jsonFilesBox.Background = Brushes.White;     
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
            
            if (fileType!=null && jsonFilesBox != null && mdbFileBox != null && euxmlFileBox!=null && createProject!=null)
            {
                bool check = true;
                if (projectNameBox.Background != themeColor || directortPathBox.Background != themeColor)
                {
                    check = false;
                }
                if (fileType.SelectedIndex== 0  && jsonFilesBox.Background!=themeColor)
                {
                    check = false;
                }
                else if(fileType.SelectedIndex == 1  && mdbFileBox.Background != themeColor)
                {
                    check = false;
                }
                else if (fileType.SelectedIndex == 2  && euxmlFileBox.Background != themeColor)
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
            var box= (TextBox)sender;
            //check if the box has no input yet.
            if (box.Text.Equals(""))
            {
                switch (box.Name) {
                    case nameof(projectNameBox):
                        projectNameBox.Text = ProjectNameHint;
                        break;
                    case nameof(directortPathBox):
                        directortPathBox.Text = DirPathHint;
                        break;
                    case nameof(jsonFilesBox):
                        jsonFilesBox.Text = JsonFilesHint;
                        break;
                    case nameof(mdbFileBox):
                        mdbFileBox.Text = MdbFileHint;
                        break;
                    case nameof(euxmlFileBox):
                        euxmlFileBox.Text = EuxmlFileHint;
                        break;
                    case nameof(ppxmlFileBox):
                        ppxmlFileBox.Text = PpxmlFileHint;
                        break;
                    case nameof(dxfFileBox):
                        dxfFileBox.Text = DxfFileHint;
                        break;
                }
            }
        }
        /// <summary>
        /// reset the window hints and colors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProjectWindow_Loaded(object sender, System.EventArgs e)
        {
            //reset Hints
            newprojectClass.ProjectName = ProjectNameHint;
            newprojectClass.ProjectPath = DirPathHint;
            newprojectClass.JsonFiles = JsonFilesHint;
            newprojectClass.EUXML = EuxmlFileHint;
            newprojectClass.MDB = MdbFileHint;
            newprojectClass.PPXML = PpxmlFileHint;
            newprojectClass.DXF = DxfFileHint;

            //reset jsonFiles
            newprojectClass.Gleiskanten = null;
            newprojectClass.Gleisknoten = null;
            newprojectClass.Entwurfselement_KM = null;
            newprojectClass.Entwurfselement_HO = null;
            newprojectClass.Entwurfselement_LA = null;
            newprojectClass.Entwurfselement_UH = null;


            //reset inputBackground
            directortPathBox.Background = Brushes.White;
            projectNameBox.Background = Brushes.White;
            jsonFilesBox.Background = Brushes.White;
            mdbFileBox.Background = Brushes.White;
            euxmlFileBox.Background = Brushes.White;
            ppxmlFileBox.Background = Brushes.White;
            dxfFileBox.Background = Brushes.White;
        }
        
        /// <summary>
        /// collapse all rows except a specific one.
        /// </summary>
        /// <param name="itemRow"></param>
        private void collapseOtherChoices(int itemRow)
        {
            container.RowDefinitions[itemRow].Height = new GridLength(1, GridUnitType.Star);
            for (int i = 4; i < container.RowDefinitions.Count - 1; i++)
            {
                if (i != itemRow)
                {
                    container.RowDefinitions[i].Height = new GridLength(0, GridUnitType.Star);
                }  
            }
        }
        #endregion
    }
}
