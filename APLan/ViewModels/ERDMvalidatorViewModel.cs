using APLan.Commands;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

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
                    var MapDataSchemaPath = Directory.GetCurrentDirectory() + "\\JSON_schemas\\ERDM.json";
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
                //if we are at property with object value
                if (item.Type == JTokenType.Property && !regex.IsMatch(((JProperty)item).Value.ToString()))
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }

                if (item.Type == JTokenType.Object)
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }

                if (item.Type == JTokenType.Array)
                {
                    ValidateJSONreference(item, MapDataObject, attributeTypesObject, typesReport);
                }
                // if we found an attribute with a uuid as value and the name is not id
                if (item.Type == JTokenType.Property && !((JProperty)item).Name.Equals("id") && regex.IsMatch(((JProperty)item).Value.ToString()))
                {
                    checkTrueType(item, attributeTypesObject, typesReport);
                }

                // if the uuid is enclosed in a list of UUIDs
                if (item.Type == JTokenType.String && regex.IsMatch(item.ToString()))
                {
                    checkTrueType(item ,attributeTypesObject, typesReport);
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
        private bool checkTrueType(JToken item, JObject attributeTypesObject, ArrayList typesReport)
        {
            var uuid = "";
            if (item.Type== JTokenType.Property)
            {
                uuid = ((JProperty)item).Value.ToString();
            }else if (item.Type == JTokenType.String)
            {
                uuid = item.ToString();
            }

            var attributeName = getAttributeName(item);

            var targetClassName = getEnclosingObjectType(item);

            //find an object with that id in the whole file.
            JObject itemWithId = (JObject)item.Root.SelectToken($"$..[?(@id=='{uuid}')]");

            //if no object with that ID is found.
            if (item == null)
            {
                typesReport.Add($"The reference {uuid} is not related to any object");
                return false;
            }

            //attributesTypes json file property name.
           JToken validTypes = attributeTypesObject.SelectToken($"$.{targetClassName}_{attributeName}");

            //if no object with that ID is found.
            if (validTypes == null)
            {
                typesReport.Add($"{targetClassName}_{attributeName} has no registered type");
                return false;
            }

            if(!validateReference(validTypes, itemWithId))
            {
                typesReport.Add($"{item.Path} is not refering to the correct object class");
                return false;
            };
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

        /// <summary>
        /// get object name if the object is inside a list of objects as value of a property with the same name of the Type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string getEnclosingObjectType(JToken item)
        {
            var targetClass = item;
            while (targetClass.Type != JTokenType.Object)
            {
                targetClass = targetClass.Parent;
            }
            return ((JProperty)targetClass.Parent.Parent).Name; //according to the JSON file structure
        }
        /// <summary>
        /// get attribute name if the attribute value is not array
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string getAttributeName(JToken item)
        {
            var targetProperty = item;
            while (targetProperty.Type != JTokenType.Property)
            {
                targetProperty = targetProperty.Parent;
            }
            return ((JProperty)targetProperty).Name; // according to the JSON structure
        }
        /// <summary>
        /// validate references according to the attribute name and file containing maping between attributes and the valid reference types.
        /// </summary>
        /// <param name="validTypes"></param>
        /// <param name="itemWithId"></param>
        /// <returns></returns>
        private bool validateReference(JToken validTypes, JObject itemWithId)
        {
            bool validation = false;

            var itemObjectName = getEnclosingObjectType(itemWithId);

            // if the valid types are single
            if (validTypes.Type== JTokenType.String && itemObjectName!=null && itemObjectName.Equals(validTypes.ToString()))
            {
                return true;
            }

            //if we have array of valid types
            if (validTypes.Type == JTokenType.Array && itemObjectName != null)
            {
                foreach(var child in validTypes.Children())
                {
                    validation = child.ToString().Equals(itemObjectName)? true:false;

                    if (validation)
                        return validation;
                }
            }
            return validation;
        }
        #endregion
    }
}
