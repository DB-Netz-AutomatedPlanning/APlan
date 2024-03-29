﻿using ERDM.Tier_0;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;


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
    }
}
