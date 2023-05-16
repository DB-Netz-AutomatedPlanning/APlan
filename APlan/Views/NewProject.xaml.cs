using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using APLan.ViewModels;
using System.IO;
using APLan.HelperClasses;
using System.Linq;
using Xceed.Wpf.AvalonDock.Controls;

namespace APLan.Views
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        #region attributes
        private readonly string[] requiredEULYNXjson = new string[] { "Entwurfselement_HO", "Entwurfselement_KM", "Entwurfselement_LA", "Entwurfselement_UH", "Gleiskanten", "Gleisknoten" };
        private readonly string[] requiredERDMxls = new string[] { "Edges", "Gradients", "Nodes", "Segments"};
        private SolidColorBrush themeColor;
        #endregion

        #region constructor
        public NewProject()
        {
            themeColor = System.Windows.Application.Current.FindResource("themeColor") as SolidColorBrush;
            InitializeComponent();
        }
        #endregion

        #region logic
        private void projetName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!projectNameBox.Text.Equals(""))
            {
                projectNameBox.Background = themeColor;
            }
            else
            {
                projectNameBox.Background = Brushes.White;
            }
            activateCreation();
        }
        private void fileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected="";
            if (((System.Windows.Controls.ComboBox)sender).SelectedItem!=null)
                    selected = ((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString();
            if(!selected.Equals(""))
                switch (selected)
                {
                    case ".json":
                        collapseOtherChoices(4);
                        break;
                    case ".mdb":
                        collapseOtherChoices(5);
                        break;
                    case ".euxml":
                        collapseOtherChoices(6);
                        break;
                    case ".ppxml":
                        collapseOtherChoices(7);
                        break;
                    case ".dxf":
                        collapseOtherChoices(8);
                        break;
                    case ".xls/.csv":
                        collapseOtherChoices(9);
                        break;
                    case ".xml":
                        collapseOtherChoices(10);
                        break;
                    case ".dwg":
                        collapseOtherChoices(11);
                        break;
                    default:
                        break;
            }
            activateCreation();         
        }
        private void projectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileType.SelectedItem = null;
            collapseOtherChoices(0);

            if (((ComboBox)sender).SelectedItem.ToString().Contains("Sweden"))
            {
                if (fileType != null)
                {
                    foreach (ComboBoxItem item in fileType.Items)
                    {
                        switch (item.Content)
                        {
                            case ".xml":
                                item.Visibility = Visibility.Visible;
                                break;
                            default:
                                item.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                }
            }

            if (((ComboBox)sender).SelectedItem.ToString().Contains("ERDM"))
            {
                if (fileType != null)
                {
                    foreach (ComboBoxItem item in fileType.Items)
                    {
                        switch (item.Content)
                        {
                            case ".xls/.csv":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".json":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".xml":
                                item.Visibility = Visibility.Visible;
                                break;
                            default:
                                item.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                }
            }

            if (((ComboBox)sender).SelectedItem.ToString().Contains("EULYNX"))
            {
                if (fileType != null)
                {
                    foreach (ComboBoxItem item in fileType.Items)
                    {
                        switch (item.Content)
                        {
                            case ".euxml":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".json":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".mdb":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".xls/.csv":
                                item.Visibility = Visibility.Visible;
                                break;
                            default:
                                item.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                }
            }

            if (((ComboBox)sender).SelectedItem.ToString().Contains("CAD"))
            {
                if (fileType != null)
                {
                    foreach (ComboBoxItem item in fileType.Items)
                    {
                        switch (item.Content)
                        {
                            case ".dxf":
                                item.Visibility = Visibility.Visible;
                                break;
                            case ".dwg":
                                item.Visibility = Visibility.Visible;
                                break;
                            default:
                                item.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                }
            }

            activateCreation();
        }
        private void directortPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(@$"{directortPathBox.Text}"))
            {
                directortPathBox.Background = themeColor;  
            }
            else
            {
                directortPathBox.Background = Brushes.White;
            }
            activateCreation();
        }
        private void Tmdbtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFileValidity("EULYNX", ((TextBox)sender), ".mdb");
        }
        private void euxmltext_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFileValidity("EULYNX", ((TextBox)sender), ".euxml");
        }
        private void dxftext_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFileValidity("CAD", ((TextBox)sender), ".dxf");
        }
        private void dwgtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFileValidity("CAD", ((TextBox)sender), ".dwg");
        }
        private void Json_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            if (projectType.SelectedItem!=null && projectType.SelectedItem.ToString().Contains("ERDM"))
            {
                checkFileValidity("ERDM", ((TextBox)sender), ".json");
            }
            if (projectType.SelectedItem != null &&  projectType.SelectedItem.ToString().Contains("EULYNX"))
            {
                if (checkJsonFiles())
                {
                    textBox.Background = themeColor;
                }
                else
                {
                    textBox.Background = Brushes.White;
                }
                activateCreation();
            }
           
        }
        private void xlsFilesBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = ((TextBox)sender);
            if (projectType.SelectedItem != null && (projectType.SelectedItem.ToString().Contains("ERDM") || projectType.SelectedItem.ToString().Contains("EULYNX")))
            {
                if (checkXlsFiles())
                {
                    textBox.Background = themeColor;
                }
                else
                {
                    textBox.Background = Brushes.White;
                }
            }
            activateCreation();
        }
        private void xmlFileBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFileValidity("ERDM", ((TextBox)sender), ".xml");
            checkFileValidity("Sweden", ((TextBox)sender), ".xml");
        }
        #endregion

        #region additional
        private void checkFileValidity(string model, TextBox textBox, string extension)
        {
            if (projectType.SelectedItem != null && projectType.SelectedItem.ToString().Contains($"{model}"))
            {
                if (File.Exists(@$"{textBox.Text}") && Path.GetExtension(@$"{textBox.Text}").EqualsIgnoreCase($"{extension}"))
                {
                    textBox.Background = themeColor;
                }
                else
                {
                    textBox.Background = Brushes.White;
                }
            }
            activateCreation();
        }
        private bool checkJsonFiles()
        {
            var check = true;
            var files = jsonFilesBox.Text.Split("+~+").ToList();
            files.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            for (int i = 0; i < files.Count; i++)
            {
                FileInfo info = new(files[i]);
                if (!File.Exists(files[i]) && !(info.Extension.Equals(".json") || info.Extension.Equals(".geojson")))
                {
                    return false;
                }
                else
                {
                    files[i] = info.Name.Replace(".json", "").Replace(".geojson", "");
                }
            }

            check = files.OrderBy(x => x).SequenceEqual(requiredEULYNXjson.OrderBy(x => x));

            if (check)
                jsonFilesBox.Background = themeColor;

            return check;
        }
        private bool checkXlsFiles()
        {
            var check = true;
            var files = xlsFilesBox.Text.Split("+~+").ToList();
            files.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            for (int i = 0; i < files.Count; i++)
            {
                FileInfo info = new(files[i]);
                if (!File.Exists(files[i]) && (!info.Extension.Equals(".xls")|| !info.Extension.Equals(".csv")))
                {
                    return false;
                }
                else
                {
                    files[i] = info.Name.Replace(".xls", "").Replace(".csv", "");
                }
            }

            check = files.OrderBy(x => x).SequenceEqual(requiredERDMxls.OrderBy(x => x));

            if (check)
                xlsFilesBox.Background = themeColor;

            return check;
        }
        /// <summary>
        /// activate the creation button whenever all conditions are met.
        /// </summary>
        public void activateCreation()
        {
            if (projectType == null || fileType == null || directortPathBox == null || projectNameBox == null || createProject == null)
                return;

            bool check = true;
            for (int i = 0; i < container.Children.Count - 1; i++)
            {
                if (container.Children[i] is Grid && container.RowDefinitions[Grid.GetRow(container.Children[i])].Height.Value != 0)
                {
                    for (int j = 0; j < (container.Children[i] as Grid).Children.Count - 1; j++)
                    {
                        if (((container.Children[i] as Grid).Children[j] is TextBox) && ((container.Children[i] as Grid).Children[j] as TextBox).Background != themeColor)
                        {
                            check = false;
                        }
                    }
                }
            }
            if ((projectType.SelectedItem == null || fileType.SelectedItem == null))
                check = false;

            if (projectNameBox.Background != themeColor || directortPathBox.Background != themeColor)
                check = false;

            if (createProject != null)
            {
                if (check == true)
                {
                    createProject.IsEnabled = true;
                }
                else
                {
                    createProject.IsEnabled = false;
                }
            }


        }
        /// <summary>
        /// collapse all rows except a specific one.
        /// </summary>
        /// <param name="itemRow"></param>
        private void collapseOtherChoices(int itemRow)
        {
            if (itemRow >= 0)
            {
                container.RowDefinitions[itemRow].Height = new GridLength(1, GridUnitType.Star);
            }
               
            for (int i = 4; i < container.RowDefinitions.Count - 1; i++)
            {
                if (i != itemRow)
                {
                    container.RowDefinitions[i].Height = new GridLength(0, GridUnitType.Star);
                }
            }
            if (itemRow < 0)
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
