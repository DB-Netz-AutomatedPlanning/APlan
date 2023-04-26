using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.Eulynx.aplan.EULYNX.EulynxJson
{
    public class UberhohungFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public UberhohungProperties? properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public UberhohungGeometry? geometry { get; set; }
    }

    public class UberhohungGeometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<double?>>? coordinates { get; set; }
    }

    public class UberhohungProperties
    {
        [JsonProperty("primaryind")]
        public double? primaryind { get; set; }

        [JsonProperty("ID")]
        public string? ID { get; set; }

        [JsonProperty("PAD_A")]
        public string? PAD_A { get; set; }

        [JsonProperty("PAD_E")]
        public string? PAD_E { get; set; }

        [JsonProperty("ELTYP")]
        public string? ELTYP { get; set; }

        [JsonProperty("ELTYP_L")]
        public string? ELTYP_L { get; set; }

        [JsonProperty("PARAM1")]
        public string? PARAM1 { get; set; }

        [JsonProperty("PARAM2")]
        public string? PARAM2 { get; set; }

        [JsonProperty("PARAM3")]
        public string? PARAM3 { get; set; }

        [JsonProperty("PARAM4")]
        public string? PARAM4 { get; set; }

        [JsonProperty("AUFN_GENAU")]
        public double? AUFN_GENAU { get; set; }

        [JsonProperty("U_EBENE")]
        public string? U_EBENE { get; set; }

        [JsonProperty("QUELLE")]
        public string? QUELLE { get; set; }

        [JsonProperty("QUELLE_L")]
        public string? QUELLE_L { get; set; }

        [JsonProperty("MIG_ID")]
        public double? MIG_ID { get; set; }

        [JsonProperty("GIS_SEG")]
        public string? GIS_SEG { get; set; }

        [JsonProperty("MIG_DATUM")]
        public string? MIG_DATUM { get; set; }

        [JsonProperty("STRECKENR")]
        public string? STRECKENR { get; set; }

        [JsonProperty("STRECKKN")]
        public string? STRECKKN { get; set; }

        [JsonProperty("RIKZ")]
        public string? RIKZ { get; set; }

        [JsonProperty("RIKZ_L")]
        public string? RIKZ_L { get; set; }

        [JsonProperty("ABBILD")]
        public string? ABBILD { get; set; }

        [JsonProperty("ABBILD_L")]
        public string? ABBILD_L { get; set; }

        [JsonProperty("F_RIKZ")]
        public string? F_RIKZ { get; set; }

        [JsonProperty("F_RIKZ_L")]
        public string? F_RIKZ_L { get; set; }

        [JsonProperty("KM_A")]
        public double? KM_A { get; set; }

        [JsonProperty("KM_A_TEXT")]
        public string? KM_A_TEXT { get; set; }

        [JsonProperty("KM_A_KM")]
        public string? KM_A_KM { get; set; }

        [JsonProperty("KM_A_M")]
        public string? KM_A_M { get; set; }

        [JsonProperty("KM_E")]
        public double? KM_E { get; set; }

        [JsonProperty("KM_E_TEXT")]
        public string? KM_E_TEXT { get; set; }

        [JsonProperty("KM_E_KM")]
        public string? KM_E_KM { get; set; }

        [JsonProperty("KM_E_M")]
        public string? KM_E_M { get; set; }

        [JsonProperty("LAGERICHT")]
        public int? LAGERICHT { get; set; }

        [JsonProperty("PD")]
        public string? PD { get; set; }

        [JsonProperty("PLANADRES")]
        public string? PLANADRES { get; set; }

        [JsonProperty("NL")]
        public string? NL { get; set; }
    }

    public class Uberhohung
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? name { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<UberhohungFeature>? features { get; set; }
    }
}
