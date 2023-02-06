using aplan.eulynx.validator;
using APLan.Commands;
using javax.xml.validation;
using jdk.@internal.util.xml.impl;
using net.sf.saxon.expr.parser;
using net.sf.saxon.lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RCA_Model.Tier_0;
using sun.misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;

namespace APLan.ViewModels
{
    public class ERDMvalidatorViewModel:BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;
        private Regex regex = new Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
        private string _json;
        private string _path;
        private string _report;
        private string _report_rules;
        public string JSON
        {
            get { return _json; }
            set
            {
                _json = value;
                OnPropertyChanged();
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        public string Report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged();
            }
        }
        public string Report_rules
        {
            get { return _report_rules; }
            set
            {
                _report_rules = value;
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
        public ICommand ValidateJSON { get; set; }
        #endregion

        #region constructor
        public ERDMvalidatorViewModel()
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
        private void ExecuteFilePath(object parameter)
        {
            openFileDialog1.Filter = "Types (*.json)|*.json";
            openFileDialog1.ShowDialog();
            JSON = openFileDialog1.FileName;
        }
        private void ExecuteOutputPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            Path = folderBrowserDialog1.SelectedPath;
        }
        private async void ExecuteValidate(object parameter)
        {
            startLoading();
            //define XSD validation version based on the imported xml.
            await validate(JSON);
            //validate according to the rules in German book.
            //await RulesValidate(JSON);
            stopLoading();
        }
        private void ExecuteCancel(object parameter)
        {
            ((Window)parameter).Close();
        }
        /// <summary>
        /// validate an euxml file.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<string> validate(string json)
        {
            LoadingReport = "Validating against JSON-Schema";
            await Task.Run(() =>
            {
                if (File.Exists(json)) // if the file exists only.
                {
                    var jsonContent = File.ReadAllText(JSON);
                    var attributeTypes = File.ReadAllText(Directory.GetCurrentDirectory() + "\\JSON_schemas\\AttributeTypes.json");
                    var MapDataSchemaPath = Directory.GetCurrentDirectory() + "\\JSON_schemas\\MapData.json";
                    StreamReader MapDatasr = new StreamReader(MapDataSchemaPath);
                    JSchema MapDataSchema = JSchema.Parse(MapDatasr.ReadToEnd(), new JSchemaReaderSettings() { Resolver = new JSchemaUrlResolver(), BaseUri = new Uri(MapDataSchemaPath) });
                    JObject MapDataObject = JObject.Parse(jsonContent);
                    JObject attributeTypesObject = JObject.Parse(attributeTypes);
                    bool MapDataValidation = MapDataObject.IsValid(MapDataSchema, out IList<string> MapDataErrorMessages);

                    ArrayList typesReport = new();
                    ValidateJSONreference(MapDataObject,MapDataObject, attributeTypesObject, typesReport);

                    Report = ListReportToString(MapDataErrorMessages, typesReport);
                    File.WriteAllText($"{Path}/ERDMvalidation.txt", Report);
                }
                createReportFile(Report, Path + "/" + nameof(Report) + ".txt");
            });
            return Report;
        }
        /// <summary>
        /// Create a file to contain a report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        private void createReportFile(string report, string filePath)
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
        /// preform validation on the json file  to check for reference types.
        /// </summary>
        /// <param name="jsonToken"></param>
        /// <param name="MapDataObject"></param>
        /// <param name="attributeTypesObject"></param>
        /// <param name="typesReport"></param>
        private void ValidateJSONreference(JToken jsonToken, JObject MapDataObject, JObject attributeTypesObject, ArrayList typesReport)
        {
            foreach (JToken item in jsonToken.Children())
            {
                //if we are at object
                if (item.Type == JTokenType.Object)
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }
                // if we are at a property have array as value , go deeper.
                if (item.Type == JTokenType.Property && ((JProperty)item).Value.Type == JTokenType.Array)
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }
                // if we are at array , go deeper
                if (item.Type == JTokenType.Array)
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }
                // if we are inside an array of references.
                if (jsonToken.Type== JTokenType.Array && item.Type == JTokenType.String && regex.IsMatch(((JValue)item).Value.ToString()))
                {
                    checkTrueType(item.Parent.Parent, item.ToString(), ((JProperty)item.Parent.Parent).Name, MapDataObject, attributeTypesObject, typesReport);
                }
                // if we are at direct reference.
                if (item.Type == JTokenType.Property && !((JProperty)item).Name.Equals("id") && ((JProperty)item).Value.Type == JTokenType.String && regex.IsMatch(((JProperty)item).Value.ToString()))
                {
                    checkTrueType(item,((JProperty)item).Value.ToString(), ((JProperty)item).Name, MapDataObject, attributeTypesObject, typesReport);
                }
            }
            
        }
        /// <summary>
        /// check if the type of the referenced object is valid accordin the model.
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="id"></param>
        /// <param name="propertyName"></param>
        /// <param name="mapData"></param>
        /// <param name="attributeTypesObject"></param>
        /// <param name="typesReport"></param>
        /// <returns></returns>
        private bool checkTrueType(JToken currentItem, string id,string propertyName, JObject mapData, JObject attributeTypesObject,ArrayList typesReport)
        {
            //find an object with that id in the whoel file.
            JToken item = mapData.SelectToken($"$..[?(@id=='{id}')]");

            //if no object with that ID is found.
            if (item == null) {
                typesReport.Add($"The reference {id} is not related to any object");
                return false;
            }
            
            // the current object type.
            var current_type = ((JValue)currentItem.Parent.SelectToken(".$type"))?.Value.ToString();

            // the target type to be validated.
            string type = ((JValue)item?.SelectToken(".$type"))?.Value.ToString();

            // attributesTypes json file property name.
            JToken validTypes = attributeTypesObject.SelectToken($"$.{current_type}_{propertyName}");

            // if it supports any type => ex: Release_insertsMapDataObjects
            if (validTypes.Equals("MapDataObject"))
            {
                return true;
            }
            // if the target validation types is single type.
            if (validTypes is JValue && type.Equals(((JValue)validTypes).Value))
            {
                return true;
            }
            // if we have multiple types to validate against (Inheritance).
            else if (validTypes is JArray)
            {
                foreach (JValue value in validTypes as JArray)
                {
                    if (type.Equals(value.Value)) return true; 
                }
            }
            //wrong type is catched.
            typesReport.Add($"{((JProperty)currentItem).Name} at object with id {id} is not the correct type");
            return false;
        }
        /// <summary>
        /// combine all list of reports in a single string
        /// </summary>
        /// <param name="schema_validation_report"></param>
        /// <param name="referenceTypesValidationReport"></param>
        /// <returns></returns>
        private string ListReportToString(IList<string> schema_validation_report , ArrayList referenceTypesValidationReport)
        {
            var report = "Schema Report : " +"\n\n";
            foreach (var item in schema_validation_report)
            {
                report += item + "\n";
            }
            report += "References Types Report :" + "\n\n";
            foreach (var item in referenceTypesValidationReport)
            {
                report += item + "\n";
            }
            return report;
        }

        #endregion
    }
}
