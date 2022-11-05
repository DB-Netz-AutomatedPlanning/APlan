using aplan.eulynx.validator;
using APLan.Commands;
//using java.nio.file.attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Timers;

namespace APLan.ViewModels
{
    public class EulynxValidatorViewModel : BaseViewModel
    { 
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private string xml;
        private string path;
        private string report;
        private string report_rules;
        public string XML
        {
            get { return xml; }
            set
            {
                xml = value;
                OnPropertyChanged();
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged();
            }
        }
        public string Report
        {
            get { return report; }
            set
            {
                report = value;
                OnPropertyChanged();
            }
        }
        public string Report_rules
        {
            get { return report_rules; }
            set
            {
                report_rules = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand FilePath { get; set; }
        public ICommand OutputPath { get; set; }
        public ICommand Validate { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Ok { get; set; }
        public ICommand ValidateXML { get; set; }
        #endregion

        #region constructor
        public EulynxValidatorViewModel()
        {
            
            FilePath = new RelayCommand(ExecuteFilePath);
            OutputPath = new RelayCommand(ExecuteOutputPath);
            Validate = new RelayCommand(ExecuteValidate);
            Cancel = new RelayCommand(ExecuteCancel);
            folderBrowserDialog1 = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();

            LoadingVisibility = Visibility.Collapsed;
        }
        #endregion

        #region logic
        public void ExecuteFilePath(object parameter)
        { 
            openFileDialog1.Filter = "Types (*.xml;*.euxml)|*.xml;*.euxml";
            openFileDialog1.ShowDialog();
            XML = openFileDialog1.FileName;
        }
        public void ExecuteOutputPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            Path = folderBrowserDialog1.SelectedPath;
        }
        public async void ExecuteValidate(object parameter)
        {
            startLoading();
            //define XSD validation version based on the imported xml.
            await validate(XML);
            //validate according to the rules in German book.
            await RulesValidate(XML);
            stopLoading();
        }
        public void ExecuteCancel(object parameter)
        {
            ((Window)parameter).Close();
        }
        /// <summary>
        /// validate an euxml file.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public async Task<string> validate(string xml)
        {
            LoadingReport = "Validating against XSD";
            string validationReport = "";
            await Task.Run(() =>
            {
                if (File.Exists(xml)) // if the file exists only.
                {
                    XmlTextReader reader = new XmlTextReader(xml);
                    ArrayList nameSpaces = new ArrayList();
                    while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        string name = reader.NamespaceURI;

                        if (!nameSpaces.Contains(name))
                        {
                            nameSpaces.Add(name);
                        }
                    }
                }
                    //XSD validation
                    EulynxXmlValidator validator = EulynxXmlValidator.getInstance();
                    ArrayList ValidationVersion_NameSpace = validator.XSDvalidationVersionCheck(nameSpaces);
                    validationReport = "XSD Validation : ";
                    if (ValidationVersion_NameSpace.Count != 0)
                    {
                        validationReport += "Validation Version is " + ValidationVersion_NameSpace[0].ToString() + "\n";
                        validationReport += validator.validate(xml, ValidationVersion_NameSpace[0].ToString(), (List<string>)ValidationVersion_NameSpace[1]) + "\n";
                    }
                    else
                    {
                        validationReport += "File don't contain the required name spaces";
                    }

                }
                createReportFile(validationReport, Path+"/"+nameof(validationReport)+".txt");
            });
            Report = validationReport;
            return validationReport;
        }
        /// <summary>
        /// Run the rule to be tested according to German book.
        /// </summary>
        /// <param name="euxmlPath"></param>
        /// <returns></returns>
        public async Task<bool> RulesValidate(string euxmlPath)
        {
            LoadingReport = "Validating against Rules";
            await Task.Run(() =>
            {
                string RulesReport = null;
                HelperClasses.RulesValidator validator = new HelperClasses.RulesValidator(euxmlPath);
                RulesReport = validator.runRulesTesting();
                createReportFile(RulesReport, Path + "/" + nameof(RulesReport) + ".txt");
                Report_rules = RulesReport;
            });
            return true;
        }
        /// <summary>
        /// Create a file to contain a report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        public void createReportFile(string report, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                using (FileStream fs = File.Create(filePath))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(report);
                    fs.Write(title, 0, title.Length);
                }
            }

        }
        /// <summary>
        /// stop the loading sign.
        /// </summary>
        /// <param name="stop"></param>
        #endregion
    }
}
