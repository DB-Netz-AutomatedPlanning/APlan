using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.Eulynx.aplan.EULYNX.EulynxJson
{
    public class KMLineFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public KMLineProperties? properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public KMLineGeometry? geometry { get; set; }
    }

    public class KMLineGeometry
    {
        [JsonProperty("type")]
        public string? type { get; set; }

        [JsonProperty("coordinates")]
        public List<List<double?>>? coordinates { get; set; }
    }

    public class KMLineProperties
    {
        [JsonProperty("LADE_ID")]
        public double? LADE_ID { get; set; }

        [JsonProperty("ID")]
        public string? ID { get; set; }

        [JsonProperty("RIKZ")]
        public string? RIKZ { get; set; }

        [JsonProperty("EELK_ID")]
        public double? EELK_ID { get; set; }

        [JsonProperty("STRECKENR")]
        public string? STRECKENR { get; set; }

        [JsonProperty("KM_A")]
        public string? KM_A { get; set; }

        [JsonProperty("KM_A_KM")]
        public string? KM_A_KM { get; set; }

        [JsonProperty("KM_E_KM")]
        public string? KM_E_KM { get; set; }

        [JsonProperty("KM_E")]
        public string? KM_E { get; set; }

        [JsonProperty("KM_A_M")]
        public string? KM_A_M { get; set; }

        [JsonProperty("KM_E_M")]
        public string? KM_E_M { get; set; }

        [JsonProperty("VON_KM_I")]
        public double? VON_KM_I { get; set; }

        [JsonProperty("KM_A_TEXT")]
        public string? KM_A_TEXT { get; set; }

        [JsonProperty("BIS_KM_I")]
        public double? BIS_KM_I { get; set; }

        [JsonProperty("KM_E_TEXT")]
        public string? KM_E_TEXT { get; set; }

        [JsonProperty("STR_ZUORD")]
        public double? STR_ZUORD { get; set; }

        [JsonProperty("ELTYP")]
        public string? ELTYP { get; set; }

        [JsonProperty("PARAM1")]
        public string? PARAM1 { get; set; }

        [JsonProperty("PARAM2")]
        public string? PARAM2 { get; set; }

        [JsonProperty("PARAM3")]
        public string? PARAM3 { get; set; }

        [JsonProperty("EELK_LSYS_")]
        public string? EELK_LSYS_ { get; set; }

        [JsonProperty("PARAM4")]
        public string? PARAM4 { get; set; }

        [JsonProperty("EELK_EKPAR")]
        public string? EELK_EKPAR { get; set; }

        [JsonProperty("GEO_VALID")]
        public string? GEO_VALID { get; set; }
    }

    public class KMLine
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? name { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<KMLineFeature>? features { get; set; }
    }
}
