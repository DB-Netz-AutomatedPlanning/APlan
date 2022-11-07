using GeoJSON.Net.Feature;
using java.io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static void ChangeID(string id, string newId)
        {
            if (AllInfo!=null&& id!=null && AllInfo.Keys.Contains(id))
            {
                Feature value = AllInfo[id];
                AllInfo.Remove(id);
                AllInfo[newId] = value;
            }
        }
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
        public static void extractExtraInfo(string outputPath,string projectName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(outputPath+ "/" + projectName + "Additional.euxml", settings))
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
    }
}
