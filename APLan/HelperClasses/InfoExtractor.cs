using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace APLan.HelperClasses
{
    public class InfoExtractor
    {
        private static Dictionary<string, Feature> AllInfo = new Dictionary<string, Feature>();
        public static Dictionary<string, Feature> getAllInfo()
        {
            return AllInfo;
        }
        /// <summary>
        /// replace the database item id with the Eulynx element id to use later.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newId"></param>
        public static void ChangeID(string id, string newId)
        {
            if (AllInfo!=null&& id!=null && AllInfo.Keys.Contains(id))
            {
                Feature value = AllInfo[id];
                AllInfo.Remove(id);
                AllInfo[newId] = value;
            }
        }
        /// <summary>
        /// attaching properties to specific visualization object based on Eulynx element id.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public static void attachProperties(CustomItem item, string id)
        {
            if (!AllInfo.ContainsKey(id))
            {
                return;
            }
            Feature value = AllInfo[id];
            foreach (var data in value?.Properties)
            {
                item.ExtraInfo.Add(new KeyValue()
                {
                    Key = data.Key,
                    Value = data.Value?.ToString()
                });
            }
        }
        /// <summary>
        /// write the extra information to an xml file to be used later.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="projectName"></param>
        public static void extractExtraInfo(string outputPath,string projectName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(outputPath+ "/" + projectName + "Additional.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Additionals", "");
                foreach (string key in AllInfo.Keys)
                {
                    writer.WriteStartElement("Element");
                    writer.WriteAttributeString("id", key);
                    foreach (string key2 in AllInfo[key].Properties.Keys)
                    {
                        writer.WriteElementString(key2, AllInfo[key].Properties[key2]?.ToString());
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }
        /// <summary>
        /// store the MDB extra information to be mapped later.  
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="id"></param>
        public static void attachMDBextraInfo(OleDbDataReader reader, string id)
        {
            Dictionary<string, object> mydic = new Dictionary<string, object>();
            Feature f = new Feature(null, mydic);
            for (int i = 0; i < reader.VisibleFieldCount; i++)
            {
                mydic.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
            }
            AllInfo.Add(id, f);
        }
        /// <summary>
        /// load additional information from an xml mapped to Eulynx Object elements.
        /// </summary>
        /// <param name="xmlPath"></param>
        public static void loadExtraInfo(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(xmlPath);

            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='Element']");
            foreach (XmlNode xn in xnList)
            {
                Dictionary<string, object> mydic = new Dictionary<string, object>();
                Feature feature = new Feature(null, mydic);

                foreach (XmlNode item in xn.ChildNodes)
                {
                    if (item.NodeType == XmlNodeType.Element)
                    {
                        mydic.Add(item.Name,item.InnerText);
                    }
               }
                AllInfo.Add(xn.Attributes[0]?.Value, feature);
            }
        }
    }
}
