using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace APLan.HelperClasses
{
    public class JSONfileChecker
    {
        #region attributes
        private string mileageFile;
        private string edgesFile;
        private string nodesFile;
        private string horizontalAlignmentsFile;
        private string verticalAlignmentsFile;
        private string cantAlignmentsFile;

        private Dictionary<string, object[]> fileNames;

        private List<string> necessaryMielageAttributes;
        private List<string> necessaryHorizontalAlignmentsAttributes;
        private List<string> necessaryVerticalAlignmentsAttributes;
        private List<string> necessaryCantAlignmentsAttributes;
        private List<string> necessaryEdgesAttributes;
        private List<string> necessaryNodesAttributes;
        #endregion

        #region constructor
        public JSONfileChecker(string mileageFilePath, string horizontalAlignmentsFile, string verticalAlignmentsFile,string cantAlignmentsFile,string edgesFile,string nodesFile) {
            this.mileageFile = mileageFilePath;
            this.edgesFile = edgesFile;
            this.nodesFile = nodesFile;
            this.horizontalAlignmentsFile = horizontalAlignmentsFile;
            this.verticalAlignmentsFile = verticalAlignmentsFile;
            this.cantAlignmentsFile = cantAlignmentsFile;
            assignNecessaryAttributes();
            assignFilesNames();
            
        }

        #endregion

        #region logic
        /// <summary>
        /// make sure that content of the mileage file contains what is needed.
        /// </summary>
        public string checkAllFiles()
        {
            string result="";
            foreach (var file in fileNames)
            {
                var resultOfCheck=CheckFile(file.Key, (string)file.Value[0], (List<string>)file.Value[1]);
                result += resultOfCheck.Value;
                if (!resultOfCheck.Value.Equals(""))
                {
                    result += "\n";
                }
            }
            return result;
        }
        /// <summary>
        /// Check the content of a file and report the result if needed info is missing.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="necessaryAttributes"></param>
        /// <returns></returns>
        private KeyValuePair<bool, string> CheckFile(string fileName, string filePath, List<string> necessaryAttributes)
        {
            var collection = readAsCollection(filePath);
            if (collection.Key == false)
                return KeyValuePair.Create(false, $"{fileName} : Failed to read the file");

            foreach (var feature in ((FeatureCollection)collection.Value).Features)
            {
                // if there is no geometry attribute
                if (feature.Properties == null)
                    return KeyValuePair.Create(false, $"{fileName} :Feature should contain a properties");

                var checkP = checkProperties(fileName,feature.Properties, necessaryAttributes);
                if (!checkP.Key)
                    return checkP;

                // if there is no geometry attribute
                if (feature.Geometry == null)
                    return KeyValuePair.Create(false, $"{fileName} :Feature should contain a Geometry");
                //else check geometry
                var checkG = checkGeometry(fileName,feature.Geometry);
                if (!checkG.Key)
                    return checkG;
            }
            return KeyValuePair.Create(true, "");
        }
        private KeyValuePair<bool, string> checkGeometry(string fileName, GeoJSON.Net.Geometry.IGeometryObject geometry)
        {
            KeyValuePair<bool, string> check = KeyValuePair.Create(true, "");
            if (geometry.Type!= GeoJSON.Net.GeoJSONObjectType.LineString && geometry.Type != GeoJSON.Net.GeoJSONObjectType.Point)
                return KeyValuePair.Create(false, $"{fileName} : Geometry is not of type LineString or Point");
            else
            {
                var lineString = geometry as GeoJSON.Net.Geometry.LineString;
                 if(lineString!=null && lineString.Coordinates.Count<2)
                    return KeyValuePair.Create(false, $"{fileName} :LineString has less than 2 coordiantes");
            }
            return check;
        }
        private KeyValuePair<bool, string> checkProperties(string fileName,IDictionary<string,object> properties, List<string> targetFileAttributes)
        {
            KeyValuePair<bool, string> check = KeyValuePair.Create(true, "");
            foreach (string key in targetFileAttributes)
            {
                if (!properties.ContainsKey(key))
                    return KeyValuePair.Create(false, $"{fileName} : {key} is missing from the properties");
                else if (!properties.TryGetValue(key, out object output))
                    return KeyValuePair.Create(false, $"{fileName} : value is missing from the properties");
            }
            return check;
        }
        /// <summary>
        /// assign file names.
        /// </summary>
        private void assignFilesNames()
        {
            fileNames = new Dictionary<string, object[]>();
            fileNames.Add(nameof(mileageFile), new object[] { mileageFile, necessaryMielageAttributes });
            fileNames.Add(nameof(edgesFile), new object[] { edgesFile, necessaryEdgesAttributes });
            fileNames.Add(nameof(nodesFile), new object[] { nodesFile, necessaryNodesAttributes });
            fileNames.Add(nameof(horizontalAlignmentsFile), new object[] { horizontalAlignmentsFile, necessaryHorizontalAlignmentsAttributes });
            fileNames.Add(nameof(verticalAlignmentsFile), new object[] { verticalAlignmentsFile, necessaryVerticalAlignmentsAttributes });
            fileNames.Add(nameof(cantAlignmentsFile), new object[] { cantAlignmentsFile, necessaryCantAlignmentsAttributes });
        }
        /// <summary>
        /// assign necessary attributes to be found for each file.
        /// </summary>
        private void assignNecessaryAttributes()
        {
            //mielage file neccesary information.
            necessaryMielageAttributes = new List<string>();
            necessaryMielageAttributes.Add("ID");
            necessaryMielageAttributes.Add("ELTYP");
            necessaryMielageAttributes.Add("PARAM1");
            necessaryMielageAttributes.Add("PARAM2");
            necessaryMielageAttributes.Add("PARAM3");
            necessaryMielageAttributes.Add("KM_A_TEXT");
            necessaryMielageAttributes.Add("KM_E_TEXT");

            necessaryHorizontalAlignmentsAttributes = new List<string>();
            necessaryHorizontalAlignmentsAttributes.Add("ID");
            necessaryHorizontalAlignmentsAttributes.Add("RIKZ");
            necessaryHorizontalAlignmentsAttributes.Add("ELTYP");
            necessaryHorizontalAlignmentsAttributes.Add("PARAM1");
            necessaryHorizontalAlignmentsAttributes.Add("PARAM2");
            necessaryHorizontalAlignmentsAttributes.Add("PARAM3");
            necessaryHorizontalAlignmentsAttributes.Add("KM_A_TEXT");
            necessaryHorizontalAlignmentsAttributes.Add("KM_E_TEXT");
            necessaryHorizontalAlignmentsAttributes.Add("WINKEL_AN");


            necessaryVerticalAlignmentsAttributes = new List<string>();
            necessaryVerticalAlignmentsAttributes.Add("ID");
            necessaryVerticalAlignmentsAttributes.Add("RIKZ");
            necessaryVerticalAlignmentsAttributes.Add("ELTYP");
            necessaryVerticalAlignmentsAttributes.Add("PARAM1");
            necessaryVerticalAlignmentsAttributes.Add("PARAM2");
            necessaryVerticalAlignmentsAttributes.Add("PARAM3");
            necessaryVerticalAlignmentsAttributes.Add("KM_A_TEXT");
            necessaryVerticalAlignmentsAttributes.Add("KM_E_TEXT");
            necessaryVerticalAlignmentsAttributes.Add("HOEHE_A_R");
            necessaryVerticalAlignmentsAttributes.Add("HOEHE_E_R");


            necessaryCantAlignmentsAttributes = new List<string>();
            necessaryCantAlignmentsAttributes.Add("ID");
            necessaryCantAlignmentsAttributes.Add("RIKZ");
            necessaryCantAlignmentsAttributes.Add("ELTYP");
            necessaryCantAlignmentsAttributes.Add("PARAM1");
            necessaryCantAlignmentsAttributes.Add("PARAM2");
            necessaryCantAlignmentsAttributes.Add("PARAM3");
            necessaryCantAlignmentsAttributes.Add("KM_A_TEXT");
            necessaryCantAlignmentsAttributes.Add("KM_E_TEXT");


            necessaryEdgesAttributes = new List<string>();
            necessaryEdgesAttributes.Add("ID");
            necessaryEdgesAttributes.Add("STATUS");
            necessaryEdgesAttributes.Add("KN_ID_V");
            necessaryEdgesAttributes.Add("KN_ID_B");
            necessaryEdgesAttributes.Add("LAENGE_ENT");
            necessaryEdgesAttributes.Add("RIKZ");
            necessaryEdgesAttributes.Add("KM_A_TEXT");
            necessaryEdgesAttributes.Add("KM_E_TEXT");


            necessaryNodesAttributes = new List<string>();
            necessaryNodesAttributes.Add("ID");
            necessaryNodesAttributes.Add("RIKZ");
            necessaryNodesAttributes.Add("KN_ID_AN");
            necessaryNodesAttributes.Add("KM_TEXT");
            necessaryNodesAttributes.Add("KN_TYP");
        }
        /// <summary>
        /// read file as collection.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public KeyValuePair<bool, object> readAsCollection(string filePath)
        {
            string file;
            FeatureCollection? collection;
            try { file = File.ReadAllText(filePath); } catch { return KeyValuePair.Create<bool,object>(false, null); };
            try { collection = JsonConvert.DeserializeObject<FeatureCollection>(file); } catch { return KeyValuePair.Create<bool, object>(false, null); };
            return KeyValuePair.Create<bool, object>(true, collection);
        }
        #endregion
    }
}
