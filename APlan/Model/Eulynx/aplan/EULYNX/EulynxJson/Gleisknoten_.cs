using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.Eulynx.aplan.EULYNX.EulynxJson
{
    public class KnotenFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public KnotenProperties? properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public KnotenGeometry? geometry { get; set; }
    }

    public class KnotenGeometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<double?>? coordinates { get; set; }
    }

    public class KnotenProperties
    {
        [JsonProperty("ID")]
        public string? ID { get; set; }

        [JsonProperty("KNOTENNAME")]
        public string? KNOTENNAME { get; set; }

        [JsonProperty("STW_BEZIRK")]
        public object? STW_BEZIRK { get; set; }

        [JsonProperty("OBJEKTNAME")]
        public int? OBJEKTNAME { get; set; }

        [JsonProperty("OBJEKTKENN")]
        public string? OBJEKTKENN { get; set; }

        [JsonProperty("KNOTENBESC")]
        public string? KNOTENBESC { get; set; }

        [JsonProperty("FREMDBEZ")]
        public object? FREMDBEZ { get; set; }

        [JsonProperty("KN_ID_AN")]
        public string? KN_ID_AN { get; set; }

        [JsonProperty("KNOTENNA_1")]
        public string? KNOTENNA_1 { get; set; }

        [JsonProperty("KN_ID_AB1")]
        public string? KN_ID_AB1 { get; set; }

        [JsonProperty("KNOTENNA_2")]
        public string? KNOTENNA_2 { get; set; }

        [JsonProperty("KN_ID_AB2")]
        public string? KN_ID_AB2 { get; set; }

        [JsonProperty("KNOTENNA_3")]
        public string? KNOTENNA_3 { get; set; }

        [JsonProperty("KN_ID_AB3")]
        public object? KN_ID_AB3 { get; set; }

        [JsonProperty("KNOTENNA_4")]
        public object? KNOTENNA_4 { get; set; }

        [JsonProperty("WA_ID")]
        public double? WA_ID { get; set; }

        [JsonProperty("WA_NUMMER")]
        public int? WA_NUMMER { get; set; }

        [JsonProperty("WA_RL100")]
        public string? WA_RL100 { get; set; }

        [JsonProperty("WA_DBDSTNA")]
        public string? WA_DBDSTNA { get; set; }

        [JsonProperty("WA_TECHN_P")]
        public string? WA_TECHN_P { get; set; }

        [JsonProperty("WA_BEZEICH")]
        public string? WA_BEZEICH { get; set; }

        [JsonProperty("PKT_ADRESS")]
        public string? PKT_ADRESS { get; set; }

        [JsonProperty("KN_TYP")]
        public string? KN_TYP { get; set; }

        [JsonProperty("KN_TYP_L")]
        public string? KN_TYP_L { get; set; }

        [JsonProperty("TECHN_PLAT")]
        public string? TECHN_PLAT { get; set; }

        [JsonProperty("AUFN_GENAU")]
        public string? AUFN_GENAU { get; set; }

        [JsonProperty("DBDSTNAME")]
        public string? DBDSTNAME { get; set; }

        [JsonProperty("RL100")]
        public string? RL100 { get; set; }

        [JsonProperty("NETZ_ID")]
        public int? NETZ_ID { get; set; }

        [JsonProperty("STELLENNUM")]
        public int? STELLENNUM { get; set; }

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

        [JsonProperty("STRECKENNU")]
        public int? STRECKENNU { get; set; }

        [JsonProperty("STRECKENKU")]
        public string? STRECKENKU { get; set; }

        [JsonProperty("RIKZ")]
        public string? RIKZ { get; set; }

        [JsonProperty("RIKZ_L")]
        public string? RIKZ_L { get; set; }

        [JsonProperty("ABBILDUNG")]
        public string? ABBILDUNG { get; set; }

        [JsonProperty("ABBILDUNG_")]
        public string? ABBILDUNG_ { get; set; }

        [JsonProperty("F_RIKZ")]
        public int? F_RIKZ { get; set; }

        [JsonProperty("F_RIKZ_L")]
        public string? F_RIKZ_L { get; set; }

        [JsonProperty("KM")]
        public string? KM { get; set; }

        [JsonProperty("KM_TEXT")]
        public string? KM_TEXT { get; set; }

        [JsonProperty("KM_KM")]
        public string? KM_KM { get; set; }

        [JsonProperty("KM_M")]
        public string? KM_M { get; set; }
    }

    public class GleisKnoten
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? name { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<KnotenFeature>? features { get; set; }
    }
}
