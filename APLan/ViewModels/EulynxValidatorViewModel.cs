using aplan.eulynx.validator;
using APLan.Commands;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;

namespace APLan.ViewModels
{
    public class EulynxValidatorViewModel : INotifyPropertyChanged
    {
        #region Inotify essentials
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private string xml;
        private string path;
        private string report;
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
        public void ExecuteValidate(object parameter)
        {
            //define XSD validation version based on the imported xml.
            Report= validate(XML);
            //Schematron Validation
            //add % exit at the end to close the cmd after finishing
            //string command = $"/k cd validate & eulynx-validator.exe -s{xml} -o{path}";
            //var myProcess = Process.Start("cmd.exe", command);


            //Report += "Schematron Validation : " + "\n";

            //while (!File.Exists(path + "/Schematron Report.txt"))
            //{
            //    Thread.Sleep(1000);
            //}

            //Report += File.ReadAllText(path+ "/Schematron Report.txt");

            //File.Delete(path + "/Schematron Report.txt");
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
        public static string validate(string xml)
        {
            string validationReport="";
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
            
            return validationReport;
        }
        #endregion
    }
}
