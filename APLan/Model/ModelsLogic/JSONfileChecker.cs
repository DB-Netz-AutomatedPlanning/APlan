using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoJSON;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;

namespace APLan.Model.ModelsLogic
{
    public class JSONfileChecker
    {
        private string mileageFilePath;
        private string edgesFilePath;
        private string nodesFilePath;
        private string horizontalAlignmentsFilePath;
        private string verticalAlignmentsFilePath;
        private string cantAlignmentsFilePath;
        public JSONfileChecker(string mileageFilePath, string edgesFilePath, string nodesFilePath, string horizontalAlignmentsFilePath, string verticalAlignmentsFilePath, string cantAlignmentsFilePath)
        {
            this.mileageFilePath = mileageFilePath;
            this.edgesFilePath = edgesFilePath;
            this.nodesFilePath = nodesFilePath;
            this.horizontalAlignmentsFilePath = horizontalAlignmentsFilePath;
            this.verticalAlignmentsFilePath = verticalAlignmentsFilePath;
            this.cantAlignmentsFilePath = cantAlignmentsFilePath;
        }
        /// <summary>
        /// make sure that content of the mileage file contains what is needed.
        /// </summary>
        private KeyValuePair<bool, string> checkMileageFile()
        {
            KeyValuePair<bool, string> check = KeyValuePair.Create(true, "");
            FeatureCollection collection;
            try { collection = JsonConvert.DeserializeObject<FeatureCollection>(mileageFilePath); } catch { return KeyValuePair.Create(true, "couldn't read the MileageFile as FeatureCollection"); };

            return check;
        }
        /// <summary>
        /// make sure that content of the EdgesFile file contains what is needed.
        /// </summary>
        private void checkEdgesFile()
        {

        }
        /// <summary>
        /// make sure that content of the EdgesFile file contains what is needed.
        /// </summary>
        private void checkMileageFilePath()
        {

        }
        /// <summary>
        /// make sure that content of the NodesFile contains what is needed.
        /// </summary>
        private void checkNodesFile()
        {

        }
        /// <summary>
        /// make sure that content of the HorizontalAlignmentsFile contains what is needed.
        /// </summary>
        private void checkHorizontalAlignmentsFile()
        {

        }
        /// <summary>
        /// make sure that content of the VerticalAlignmentsFile contains what is needed.
        /// </summary>
        private void checkVerticalAlignmentsFile()
        {

        }
        /// <summary>
        /// make sure that content of the cantAlignmentsFile contains what is needed.
        /// </summary>
        private void checkcantAlignmentsFile()
        {

        }
    }
}
