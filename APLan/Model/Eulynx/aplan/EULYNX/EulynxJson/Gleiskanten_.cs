using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.Eulynx.aplan.EULYNX.EulynxJson
{
    public class KantenFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public KantenProperties? properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public KantenGeometry? geometry { get; set; }
    }

    public class KantenGeometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<double?>>? coordinates { get; set; }
    }

    public class KantenProperties
    {
        [JsonProperty("ID")]
        public string? ID { get; set; }

        [JsonProperty("KN_ID_V")]
        public string? KN_ID_V { get; set; }

        [JsonProperty("KN_NAME")]
        public string? KN_NAME { get; set; }

        [JsonProperty("RL100_VON")]
        public string? RL100_VON { get; set; }

        [JsonProperty("NETZ_ID_VO")]
        public int? NETZ_ID_VO { get; set; }

        [JsonProperty("DBDSTNAME_")]
        public string? DBDSTNAME_ { get; set; }

        [JsonProperty("STRELLENNU")]
        public int? STRELLENNU { get; set; }

        [JsonProperty("GKN_MIG_ID")]
        public int? GKN_MIG_ID { get; set; }

        [JsonProperty("KN_ID_B")]
        public string? KN_ID_B { get; set; }

        [JsonProperty("KNOTENNA_1")]
        public string? KNOTENNA_1 { get; set; }

        [JsonProperty("RL100_BIS")]
        public string? RL100_BIS { get; set; }

        [JsonProperty("NETZ_ID_BI")]
        public int? NETZ_ID_BI { get; set; }

        [JsonProperty("DBDSTNAM_1")]
        public string? DBDSTNAM_1 { get; set; }

        [JsonProperty("STRELLEN_1")]
        public int? STRELLEN_1 { get; set; }

        [JsonProperty("GKN_MIG__1")]
        public int? GKN_MIG__1 { get; set; }

        [JsonProperty("FREMDBEZ")]
        public object? FREMDBEZ { get; set; }

        [JsonProperty("TECHN_PLAT")]
        public object? TECHN_PLAT { get; set; }

        [JsonProperty("AUFN_GENAU")]
        public string? AUFN_GENAU { get; set; }

        [JsonProperty("ERSTERFASS")]
        public string? ERSTERFASS { get; set; }

        [JsonProperty("ERSTERF_DA")]
        public string? ERSTERF_DA { get; set; }

        [JsonProperty("BEARBEITER")]
        public string? BEARBEITER { get; set; }

        [JsonProperty("BEARB_DATU")]
        public string? BEARB_DATU { get; set; }

        [JsonProperty("PROGRAMM")]
        public string? PROGRAMM { get; set; }

        [JsonProperty("AUFTRAG")]
        public string? AUFTRAG { get; set; }

        [JsonProperty("KOMMENTAR")]
        public string? KOMMENTAR { get; set; }

        [JsonProperty("LAENGE_ENT")]
        public string? LAENGE_ENT { get; set; }

        [JsonProperty("LAENGE_GK3")]
        public string? LAENGE_GK3 { get; set; }

        [JsonProperty("AUSPRAEGUN")]
        public string? AUSPRAEGUN { get; set; }

        [JsonProperty("AUSPRAEG_1")]
        public string? AUSPRAEG_1 { get; set; }

        [JsonProperty("DATENQUELL")]
        public string? DATENQUELL { get; set; }

        [JsonProperty("DATENQUE_1")]
        public string? DATENQUE_1 { get; set; }

        [JsonProperty("STATUS")]
        public string? STATUS { get; set; }

        [JsonProperty("STATUS_L")]
        public string? STATUS_L { get; set; }

        [JsonProperty("MIG_ID")]
        public int? MIG_ID { get; set; }

        [JsonProperty("GIS_SEGMEN")]
        public string? GIS_SEGMEN { get; set; }

        [JsonProperty("MIG_DATUM")]
        public string? MIG_DATUM { get; set; }

        [JsonProperty("GLS_STELLW")]
        public object GLS_STELLW { get; set; }

        [JsonProperty("GLS_GLEISN")]
        public int? GLS_GLEISN { get; set; }

        [JsonProperty("STRECKENNU")]
        public int? STRECKENNU { get; set; }

        [JsonProperty("STRECKENKU")]
        public string? STRECKENKU { get; set; }

        [JsonProperty("RIKZ")]
        public string? RIKZ { get; set; }

        [JsonProperty("RIKZ_L")]
        public string? RIKZ_L { get; set; }

        [JsonProperty("KM_VON")]
        public string? KM_VON { get; set; }

        [JsonProperty("KM_A_TEXT")]
        public string? KM_A_TEXT { get; set; }

        [JsonProperty("KM_A_KM")]
        public string? KM_A_KM { get; set; }

        [JsonProperty("KM_A_M")]
        public string? KM_A_M { get; set; }

        [JsonProperty("KM_BIS")]
        public string? KM_BIS { get; set; }

        [JsonProperty("KM_E_TEXT")]
        public string? KM_E_TEXT { get; set; }

        [JsonProperty("KM_E_KM")]
        public string? KM_E_KM { get; set; }

        [JsonProperty("KM_E_M")]
        public string? KM_E_M { get; set; }

        [JsonProperty("STATION_VO")]
        public int? STATION_VO { get; set; }

        [JsonProperty("STATION_BI")]
        public string? STATION_BI { get; set; }

        [JsonProperty("LAGERICHTI")]
        public int? LAGERICHTI { get; set; }
    }

    public class GleisKanten
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? name { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<KantenFeature>? features { get; set; }
    }
}
