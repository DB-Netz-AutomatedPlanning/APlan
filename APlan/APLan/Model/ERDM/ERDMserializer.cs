using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Serialization;

namespace ERDM_Implementation
{
    public class ERDMserializer
    {
        private ERDM.ERDMmodel erdmModel;
        public ERDMserializer(ERDM.ERDMmodel erdmModel)
        {
            this.erdmModel = erdmModel;
        }
        /// <summary>
        /// serialize the ERDM model to JSON string.
        /// </summary>
        /// <returns></returns>
        public string serializeERDM()
        {
            if (this.erdmModel!=null)
            {
                string MapDataObjectJson = System.Text.Json.JsonSerializer.Serialize<ERDM.ERDMmodel>(erdmModel, new JsonSerializerOptions() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } });
                return MapDataObjectJson;
            }
            return null;
        }
        /// <summary>
        /// serialize the ERDM model to XML string.
        /// </summary>
        /// <param name="erdm"></param>
        /// <param name="outputPath"></param>
        public void serializeERDMtoXML(string outputPath)
        {
            if (this.erdmModel != null)
            {
                XmlSerializer XMLserializer = new XmlSerializer(typeof(ERDM.ERDMmodel));
                
                using (TextWriter writer = new StreamWriter(outputPath))
                {
                    XMLserializer.Serialize(writer, this.erdmModel);
                }
            }
        }
    }
}
