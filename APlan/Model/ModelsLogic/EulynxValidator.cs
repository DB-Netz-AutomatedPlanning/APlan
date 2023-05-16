using aplan.eulynx.validator;
using APLan.Commands;
using APLan.Model.ModelsLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using APLan.HelperClasses;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace APLan.Model.ModelsLogic
{
    public class EulynxValidator
    {
        public string Report_rules { get; set; }
        public async Task<string> validate(string xml,string Path)
        {
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
                createReportFile(validationReport, Path + "/" + nameof(validationReport) + ".txt");
            });
            Report_rules = validationReport;
            return validationReport;
        }
        /// <summary>
        /// Run the rule to be tested according to German book.
        /// </summary>
        /// <param name="euxmlPath"></param>
        /// <returns></returns>
        private async Task<bool> RulesValidate(string euxmlPath, string Path)
        {
            await Task.Run(() =>
            {
                string RulesReport = null;
                RulesValidator validator = new RulesValidator(euxmlPath);
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
        private void createReportFile(string report, string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(report);
                    fs.Write(title, 0, title.Length);
                }
            }

        }
    }
}
