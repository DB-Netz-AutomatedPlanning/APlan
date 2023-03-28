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
using System.Xml.Schema;
using System.Xml;

namespace APLan.Model.ModelsLogic
{
    public class ERDMvalidator
    {
        public string Report { get; set; }
        private Regex regex = new Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
        #region logic
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
                    checkTrueType(item, attributeTypesObject, typesReport);
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
            if (item.Type == JTokenType.Property)
            {
                uuid = ((JProperty)item).Value.ToString();
            }
            else if (item.Type == JTokenType.String)
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

            if (!validateReference(validTypes, itemWithId))
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
        private string ListReportToString(IList<string> schema_validation_report, ArrayList referenceTypesValidationReport)
        {
            var report = "Schema Report : " + "\n\n";
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
            if (validTypes.Type == JTokenType.String && itemObjectName != null && itemObjectName.Equals(validTypes.ToString()))
            {
                return true;
            }

            //if we have array of valid types
            if (validTypes.Type == JTokenType.Array && itemObjectName != null)
            {
                foreach (var child in validTypes.Children())
                {
                    validation = child.ToString().Equals(itemObjectName) ? true : false;

                    if (validation)
                        return validation;
                }
            }
            return validation;
        }
        /// <summary>
        /// validate ERDM xml file against xsd schema.
        /// </summary>
        private void validateXML(string xmlFile)
        {
            Report = "";
            XmlReaderSettings settings = new XmlReaderSettings();

            settings.Schemas.Add("", $"{Directory.GetCurrentDirectory()}\\ERDMxsd\\ERDM.xsd");

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.ValidationEventHandler += xmlValidationEventHandler;

            XmlReader books = XmlReader.Create(xmlFile, settings);

            while (books.Read()) { }
        }

        /// <summary>
        /// event handler for xml validation process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xmlValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {

            if (e.Severity == XmlSeverityType.Warning)
            {
                Report += "WARNING: ";
                Report += e.Message + "\n";
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Report += "ERROR: ";
                Report += e.Message + "\n";
            }
        }
        #endregion
        #region async logic
        /// <summary>
        /// validate an euxml file.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<string> validate(string erdmFile, string outputPath=null)
        {
            FileInfo info = new(erdmFile);
            await Task.Run(() =>
            {
                try { 
                if (File.Exists(erdmFile) && info.Extension.Equals(".json")) // if the file exists only.
                {
                    var jsonContent = File.ReadAllText(erdmFile);
                    var attributeTypes = File.ReadAllText(Directory.GetCurrentDirectory() + "\\JSON_schemas\\AttributeTypes.json");
                    var MapDataSchemaPath = Directory.GetCurrentDirectory() + "\\JSON_schemas\\ERDM.json";
                    StreamReader MapDatasr = new StreamReader(MapDataSchemaPath);
                    JSchema MapDataSchema = JSchema.Parse(MapDatasr.ReadToEnd(), new JSchemaReaderSettings() { Resolver = new JSchemaUrlResolver(), BaseUri = new Uri(MapDataSchemaPath) });
                    JObject MapDataObject = JObject.Parse(jsonContent);
                    JObject attributeTypesObject = JObject.Parse(attributeTypes);
                    bool MapDataValidation = MapDataObject.IsValid(MapDataSchema, out IList<string> MapDataErrorMessages);

                    ArrayList typesReport = new();
                    ValidateJSONreference(MapDataObject, MapDataObject, attributeTypesObject, typesReport);

                    Report = ListReportToString(MapDataErrorMessages, typesReport);
                    if (outputPath!=null)
                    {
                        File.WriteAllText($"{outputPath}/ERDMvalidation.txt", Report);
                    }
                }
                else if (File.Exists(erdmFile) && info.Extension.Equals(".xml"))
                {
                    validateXML(erdmFile);
                }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
                createReportFile(Report, outputPath + "/" + nameof(Report) + ".txt");
            });
            return Report;
        }
        #endregion
     
    }
}
