using RCA_Model.Tier_0;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace APLan.ERDMmodel
{
    public class ERDMserializer
    {
        private MapData mapData;
        public ERDMserializer(MapData mapData)
        {
            this.mapData = mapData;
        }
        /// <summary>
        /// serialize the ERDM model to JSON string.
        /// </summary>
        /// <returns></returns>
        public string serializeERDM()
        {
            if (this.mapData!=null)
            {
                string MapDataObjectJson = System.Text.Json.JsonSerializer.Serialize<MapData>(mapData, new JsonSerializerOptions() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } });
                return MapDataObjectJson;
            }
            return null;
        }
    }
}
