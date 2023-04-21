using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.Eulynx.aplan.EULYNX.EulynxJson
{
    public class BuildJson
    {
        string edgeFile;
        string nodeFile;
        string gradientFile;
        string segmentFile;

        public BuildJson(string edgeFile, string nodeFile, string gradientFile, string segmentFile) 
        {
            this.edgeFile = edgeFile; this.nodeFile = nodeFile;
            this.gradientFile = gradientFile; this.segmentFile = segmentFile;
        } 
        public void CreateKanten(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            var gleisKanten = xLSreader.ReadXLSContent(edgeFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Edges.xls"
            var gleisKnoten = xLSreader.ReadXLSContent(nodeFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Nodes.xls"

            var kanten = new GleisKanten();
            kanten.name = "GleisKanten";
            kanten.type = "FeatureCollection";

            List<KantenFeature> features = new();


            //List<string> coordinate = new List<string>();
            //dict.TryGetValue("Track_Edge", out var edgeID);
            foreach (Dictionary<string, string> dict in gleisKanten)
            {
                // Get the coordinates for edge(s)
                List<List<double?>> coord = new List<List<double?>>();

                //string? ID = null;
                string? STATUS = null;
                string? KN_ID_V = null;
                string? KN_ID_B = null;
                string? LAENGE_ENT = null;
                string? RIKZ = null;
                string? KM_A_KM = null;
                string? KM_A_M = null;
                string? KM_A_TEXT = null;
                string? KM_E_KM = null;
                string? KM_E_M = null;
                string? KM_E_TEXT = null;
                dict.TryGetValue("Start_Node", out string? startNode);
                dict.TryGetValue("End_Node", out string? endNode);
                dict.TryGetValue("Edge_ID", out string? ID);

                foreach (Dictionary<string, string> dict2 in gleisKnoten)
                {
                    List<double?> start = new List<double?>();
                    List<double?> end = new List<double?>();
                    dict2.TryGetValue("Node_ID", out string? nodeId);

                    dict2.TryGetValue("Pkt_East[m]", out string? x1);
                    dict2.TryGetValue("Pkt_North [m]", out string? y1);
                    dict2.TryGetValue("Pkt_Height [m]", out string? z1);
                    if (nodeId == startNode)
                    {
                        start.Add(double.Parse(x1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," })); 
                        start.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                        start.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                        dict2.TryGetValue("KM [km]", out KM_A_KM);
                        coord.Add(start);
                        if (end.Count != 0) break;
                    }
                    else if (nodeId == endNode)
                    {
                        end.Add(double.Parse(x1));  //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                        end.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                        end.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                        dict2.TryGetValue("KM [km]", out KM_E_KM);
                        coord.Add(end);
                        if (start.Count != 0) break;
                    }
                }
                KantenGeometry geometry = new KantenGeometry() { type = "LineString", coordinates = coord };
                KantenProperties properties = new KantenProperties()
                {
                    ID = ID,
                    STATUS = STATUS,
                    KN_ID_V = KN_ID_V,
                    KN_ID_B = KN_ID_B,
                    LAENGE_ENT = LAENGE_ENT,
                    RIKZ = RIKZ,
                    KM_A_KM = KM_A_KM,
                    KM_E_KM = KM_E_KM,
                    KM_A_M = KM_A_M,
                    KM_E_M = KM_E_M,
                    KM_A_TEXT = KM_A_TEXT,
                    KM_E_TEXT = KM_E_TEXT
                };
                features.Add(new KantenFeature() { type = "Feature", properties = properties, geometry = geometry });
            }

            //KantenGeometry geometry = new KantenGeometry() { type = "LineString", coordinates = coord };
            //KantenProperties properties = new KantenProperties() { ID = ID, STATUS = STATUS, KN_ID_V = KN_ID_V, 
            //    KN_ID_B = KN_ID_B, LAENGE_ENT = LAENGE_ENT, RIKZ = RIKZ, KM_A_KM = KM_A_KM, KM_E_KM = KM_E_KM,
            //    KM_A_M = KM_A_M, KM_E_M = KM_E_M, KM_A_TEXT = KM_A_TEXT, KM_E_TEXT = KM_E_TEXT };



            //features.Add(new KantenFeature() { type = "Feature", properties = properties, geometry = geometry });

            kanten.features = features;

            string output = JsonConvert.SerializeObject(kanten);

            File.WriteAllText(filePath, output);
        }


        public void CreateKnoten(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            //var gleisKanten = xLSreader.ReadXLSContent("C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Edges.xls", 0);
            var gleisKnoten = xLSreader.ReadXLSContent(nodeFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Nodes.xls"

            var knoten = new GleisKnoten();
            knoten.name = "Gleisknoten";
            knoten.type = "FeatureCollection";

            List<KnotenFeature> features = new List<KnotenFeature>();

            //List<string> coordinate = new List<string>();
            //dict.TryGetValue("Track_Edge", out var edgeID);
            foreach (Dictionary<string, string> dict in gleisKnoten)
            {
                // Get the coordinates for edge(s)
                List<double?> coord = new List<double?>();

                string? ID = null;
                string? RIKZ = null;
                string? KN_ID_AN = null;
                string? KN_ID_AB1 = null;
                string? KN_ID_AB2 = null;
                string? KN_ID_AB3 = null;
                string? KM_KM = null;
                string? KM_M = null;
                string? KN_TYP = null;

                dict.TryGetValue("Node_ID", out ID);
                dict.TryGetValue("Pkt_East[m]", out string? x);
                dict.TryGetValue("Pkt_North [m]", out string? y);
                dict.TryGetValue("Pkt_Height [m]", out string? z);
                dict.TryGetValue("KM [km]", out KM_KM);

                coord.Add(double.Parse(x)); // , new NumberFormatInfo() { NumberDecimalSeparator = "," }))
                coord.Add(double.Parse(y)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }))
                coord.Add(double.Parse(z)); // , new NumberFormatInfo() { NumberDecimalSeparator = "," }))


                KnotenGeometry geometry = new KnotenGeometry() { type = "Point", coordinates = coord };
                KnotenProperties properties = new KnotenProperties()
                {
                    ID = ID,
                    RIKZ = RIKZ,
                    KN_ID_AN = KN_ID_AN,
                    KN_ID_AB1 = KN_ID_AB1,
                    KN_ID_AB2 = KN_ID_AB2,
                    KN_ID_AB3 = KN_ID_AB3,
                    KM_KM = KM_KM,
                    KM_M = KM_M,
                    KN_TYP = KN_TYP
                };
                features.Add(new KnotenFeature() { type = "Feature", properties = properties, geometry = geometry });
            }
            knoten.features = features;

            string output = JsonConvert.SerializeObject(knoten);

            File.WriteAllText(filePath, output);
        }


        public void CreateKMLine(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            var kmline = xLSreader.ReadXLSContent(segmentFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Segments.xls"

            var km = new KMLine();
            km.name = "Entwurfselement_KMLinie";
            km.type = "FeatureCollection";

            List<KMLineFeature> features = new List<KMLineFeature>();

            foreach (Dictionary<string, string> dict in kmline)
            {
                // Get the coordinates for edge(s)
                List<List<double?>> coord = new List<List<double?>>();
                List<double?> start = new List<double?>();
                List<double?> end = new List<double?>();

                string? ID = "";
                string? RIKZ = "";
                string? ELTYP = "";
                string? PARAM1 = "";
                string? PARAM2 = "";
                string? PARAM3 = "";
                string? PARAM4 = "";
                string? KM_A_KM = "";
                string? KM_A_M = "";
                string? KM_A = "";
                string? KM_A_TEXT = "";
                string? KM_E_KM = "";
                string? KM_E_M = "";
                string? KM_E = "";
                string? KM_E_TEXT = "";


                dict.TryGetValue("Segment ID", out ID);
                dict.TryGetValue("Type", out ELTYP);
                //dict.TryGetValue("KM_Begin [km]", out KM_A_KM);
                //dict.TryGetValue("KM_End [km]", out KM_E_KM);
                dict.TryGetValue("KM_start [km]", out KM_A_TEXT);
                dict.TryGetValue("KM_end [km]", out KM_E_TEXT);
                dict.TryGetValue("SegmentLength [m]", out PARAM1);
                dict.TryGetValue("Start Radius [m]", out PARAM2);
                dict.TryGetValue("End Radius [m]", out PARAM3);

                dict.TryGetValue("Start ETRS89 X [m]", out string? x1);
                dict.TryGetValue("Start ETRS89 Y [m]", out string? y1);
                dict.TryGetValue("Start ETRS89 Z [m]", out string? z1);

                start.Add(double.Parse(x1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(start);

                dict.TryGetValue("End ETRS89 X [m]", out string? x2);
                dict.TryGetValue("End ETRS89 Y [m]", out string? y2);
                dict.TryGetValue("End ETRS89 Z [m]", out string? z2);

                end.Add(double.Parse(x2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(y2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(z2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(end);

                KMLineGeometry kMLineGeometry = new KMLineGeometry() { type = "LineString", coordinates = coord };
                KMLineProperties properties = new KMLineProperties()
                {
                    ID = ID,
                    RIKZ = RIKZ,
                    ELTYP = ELTYP,
                    PARAM1 = PARAM1 == null ? PARAM1 : PARAM1.Replace(',', '.'),
                    PARAM2 = PARAM2 == null ? PARAM2 : PARAM2.Replace(',', '.'),
                    PARAM3 = PARAM3 == null ? PARAM3 : PARAM3.Replace(',', '.'),
                    PARAM4 = PARAM4 == null ? PARAM4 : PARAM4.Replace(',', '.'),
                    KM_A_KM = KM_A_KM == null ? KM_A_KM : KM_A_KM.Replace(',', '.'),
                    KM_E_KM = KM_E_KM == null ? KM_E_KM : KM_E_KM.Replace(',', '.'),
                    KM_A_M = KM_A_M == null ? KM_A_M : KM_A_M.Replace(',', '.'),
                    KM_E_M = KM_E_M == null ? KM_E_M : KM_E_M.Replace(',', '.'),
                    KM_A = KM_A == null ? KM_A : KM_A.Replace(',', '.'),
                    KM_E = KM_E == null ? KM_E : KM_E.Replace(',', '.'),
                    KM_A_TEXT = KM_A_TEXT == null ? KM_A_TEXT : KM_A_TEXT.Replace(',', '.'),
                    KM_E_TEXT = KM_E_TEXT == null ? KM_E_TEXT : KM_E_TEXT.Replace(',', '.')
                };


                features.Add(new KMLineFeature() { type = "Feature", properties = properties, geometry = kMLineGeometry });
            }

            km.features = features;
            string output = JsonConvert.SerializeObject(km);
            File.WriteAllText(filePath, output);
        }




        public void CreateHohe(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            var hohe = xLSreader.ReadXLSContent(gradientFile, 0);  //  "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Gradients.xls"

            var ho = new Hohe();
            ho.name = "Entwurfselement_Hoehe";
            ho.type = "FeatureCollection";

            List<HoheFeature> features = new List<HoheFeature>();

            foreach (Dictionary<string, string> dict in hohe)
            {
                // Get the coordinates for edge(s)
                List<List<double?>> coord = new List<List<double?>>();
                List<double?> start = new List<double?>();
                List<double?> end = new List<double?>();

                string? ID = null;
                string? RIKZ = null;
                string? ELTYP = null;
                string? ELTYP_L = null;
                string? PARAM1 = null;
                string? PARAM2 = null;
                string? PARAM3 = null;
                string? PARAM4 = null;
                string? KM_A_KM = null;
                string? KM_A_M = null;
                string? KM_A_TEXT = null;
                string? KM_E_KM = null;
                string? KM_E_M = null;
                string? KM_E_TEXT = null;
                string? HOEHE_A_R = null;
                string? HOEHE_E_R = null;


                dict.TryGetValue("Segment ID", out ID);
                //dict.TryGetValue("Type", out ELTYP);

                dict.TryGetValue("Start_KM [km]", out KM_A_KM);
                dict.TryGetValue("End_KM [km]", out KM_E_KM);
                dict.TryGetValue("Start_KM [km]", out KM_A_TEXT);
                dict.TryGetValue("End_KM [km]", out KM_E_TEXT);

                dict.TryGetValue("SegmentLength [m]", out PARAM1);

                dict.TryGetValue("Gradient [per mill]", out PARAM2);
                //dict.TryGetValue("Radius_End [m]", out PARAM3);

                dict.TryGetValue("Start ETRS89 X [m]", out string? x1);
                dict.TryGetValue("Start ETRS89 Y [m]", out string? y1);
                dict.TryGetValue("Start ETRS89 Z [m]", out string? z1);

                start.Add(double.Parse(x1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(start);

                dict.TryGetValue("End ETRS89 X [m]", out string? x2);
                dict.TryGetValue("End ETRS89 Y [m]", out string? y2);
                dict.TryGetValue("End ETRS89 Z [m]", out string? z2);

                end.Add(double.Parse(x2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(y2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(z2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(end);

                dict.TryGetValue("Start_Alt [m]", out HOEHE_A_R);
                dict.TryGetValue("End_Alt [m]", out HOEHE_E_R);

                HoheGeometry hoheGeometry = new HoheGeometry() { type = "LineString", coordinates = coord };
                HoheProperties properties = new HoheProperties()
                {
                    ID = ID,
                    RIKZ = RIKZ,
                    ELTYP = ELTYP,
                    ELTYP_L = ELTYP_L,
                    PARAM1 = PARAM1 == null ? PARAM1 : PARAM1.Replace(',', '.'),
                    PARAM2 = PARAM2 == null ? PARAM2 : PARAM2.Replace(',', '.'),
                    PARAM3 = PARAM3 == null ? PARAM3 : PARAM3.Replace(',', '.'),
                    PARAM4 = PARAM4 == null ? PARAM4 : PARAM4.Replace(',', '.'),
                    KM_A_KM = KM_A_KM == null ? KM_A_KM : KM_A_KM.Replace(',', '.'),
                    KM_E_KM = KM_E_KM == null ? KM_E_KM : KM_E_KM.Replace(',', '.'),
                    KM_A_M = KM_A_M == null ? KM_A_M : KM_A_M.Replace(',', '.'),
                    KM_E_M = KM_E_M == null ? KM_E_M : KM_E_M.Replace(',', '.'),

                    KM_A_TEXT = KM_A_TEXT == null ? KM_A_TEXT : KM_A_TEXT.Replace(',', '.'),
                    KM_E_TEXT = KM_E_TEXT == null ? KM_E_TEXT : KM_E_TEXT.Replace(',', '.'),
                    HOEHE_A_R = double.Parse(HOEHE_A_R), //, new NumberFormatInfo() { NumberDecimalSeparator = "," }),
                    HOEHE_E_R = double.Parse(HOEHE_E_R) //, new NumberFormatInfo() { NumberDecimalSeparator = "," })
                };


                features.Add(new HoheFeature() { type = "Feature", properties = properties, geometry = hoheGeometry });
            }

            ho.features = features;
            string output = JsonConvert.SerializeObject(ho);

            File.WriteAllText(filePath, output);
        }



        public void CreaeteLage(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            var _lage = xLSreader.ReadXLSContent(segmentFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Segments.xls"

            var lage = new Lage();
            lage.name = "Entwurfselement_LAGE";
            lage.type = "FeatureCollection";

            List<LageFeature> features = new List<LageFeature>();

            foreach (Dictionary<string, string> dict in _lage)
            {
                // Get the coordinates for edge(s)
                List<List<double?>> coord = new List<List<double?>>();
                List<double?> start = new List<double?>();
                List<double?> end = new List<double?>();

                string? ID = null;
                string? RIKZ = null;
                string? ELTYP = null;
                string? ELTYP_L = null;
                string? PARAM1 = null;
                string? PARAM2 = null;
                string? PARAM3 = null;
                string? PARAM4 = null;
                string? WINKEL_AN = null;
                string? KM_A_KM = null;
                string? KM_A_M = null;
                string? KM_A_TEXT = null;
                string? KM_E_KM = null;
                string? KM_E_M = null;
                string? KM_E_TEXT = null;

                dict.TryGetValue("Segment ID", out ID);
                dict.TryGetValue("KM_start [km]", out KM_A_KM);
                dict.TryGetValue("KM_end [km]", out KM_E_KM);
                dict.TryGetValue("KM_start [km]", out KM_A_TEXT);
                dict.TryGetValue("KM_end [km]", out KM_E_TEXT);
                dict.TryGetValue("SegmentLength [m]", out PARAM1);

                dict.TryGetValue("Start Radius [m]", out PARAM2);
                dict.TryGetValue("End Radius [m]", out PARAM3);
                dict.TryGetValue("Start Azimuth Angle [Â°]", out WINKEL_AN);

                dict.TryGetValue("Start ETRS89 X [m]", out string? x1);
                dict.TryGetValue("Start ETRS89 Y [m]", out string? y1);
                dict.TryGetValue("Start ETRS89 Z [m]", out string? z1);


                //dict.TryGetValue("Start ETRS89 Z [m]", out string? z1);




                start.Add(double.Parse(x1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(start);

                dict.TryGetValue("End ETRS89 X [m]", out string? x2);
                dict.TryGetValue("End ETRS89 Y [m]", out string? y2);
                dict.TryGetValue("End ETRS89 Z [m]", out string? z2);

                end.Add(double.Parse(x2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(y2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(z2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(end);

                LageGeometry lageGeometry = new LageGeometry() { type = "LineString", coordinates = coord };
                LageProperties properties = new LageProperties()
                {
                    ID = ID,
                    RIKZ = RIKZ,
                    ELTYP = ELTYP,
                    ELTYP_L = ELTYP_L,
                    PARAM1 = PARAM1 == null ? PARAM1 : PARAM1.Replace(',', '.'),
                    PARAM2 = PARAM2 == null ? PARAM2 : PARAM2.Replace(',', '.'),
                    PARAM3 = PARAM3 == null ? PARAM3 : PARAM3.Replace(',', '.'),
                    PARAM4 = PARAM4 == null ? PARAM4 : PARAM4.Replace(',', '.'),
                    WINKEL_AN = WINKEL_AN == null ? WINKEL_AN : WINKEL_AN.Replace(',', '.'),
                    KM_A_KM = KM_A_KM == null ? KM_A_KM : KM_A_KM.Replace(',', '.'),
                    KM_E_KM = KM_E_KM == null ? KM_E_KM : KM_E_KM.Replace(',', '.'),
                    KM_A_M = KM_A_M == null ? KM_A_M : KM_A_M.Replace(',', '.'),
                    KM_E_M = KM_E_M == null ? KM_E_M : KM_E_M.Replace(',', '.'),
                    KM_A_TEXT = KM_A_TEXT == null ? KM_A_TEXT : KM_A_TEXT.Replace(',', '.'),
                    KM_E_TEXT = KM_E_TEXT == null ? KM_E_TEXT : KM_E_TEXT.Replace(',', '.')

                };

                features.Add(new LageFeature() { type = "Feature", properties = properties, geometry = lageGeometry });
            }

            lage.features = features;
            string output = JsonConvert.SerializeObject(lage);

            File.WriteAllText(filePath, output);
        }



        public void CreaeteUH(string filePath)
        {
            XLSreader xLSreader = new XLSreader();
            var _uh = xLSreader.ReadXLSContent(segmentFile, 0);  // "C:\\Users\\DR-PHELZ\\Desktop\\Data\\ERDM_Input_output\\Scheibenberg_Final\\Segments.xls"

            var uh = new Uberhohung();
            uh.name = "Entwurfselement_Ueberhoehung";
            uh.type = "FeatureCollection";

            List<UberhohungFeature> features = new List<UberhohungFeature>();

            foreach (Dictionary<string, string> dict in _uh)
            {
                // Get the coordinates for edge(s)
                List<List<double?>> coord = new List<List<double?>>();
                List<double?> start = new List<double?>();
                List<double?> end = new List<double?>();

                string? ID = null;
                string? RIKZ = null;
                string? ELTYP = null;
                string? PARAM1 = null;
                string? PARAM2 = null;
                string? PARAM3 = null;
                string? PARAM4 = null;
                string? KM_A_KM = null;
                string? KM_A_M = null;
                string? KM_A_TEXT = null;
                string? KM_E_KM = null;
                string? KM_E_M = null;
                string? KM_E_TEXT = null;

                dict.TryGetValue("Segment ID", out ID);

                dict.TryGetValue("KM_start [km]", out KM_A_KM);
                dict.TryGetValue("KM_end [km]", out KM_E_KM);
                dict.TryGetValue("KM_start [km]", out KM_A_TEXT);
                dict.TryGetValue("KM_end [km]", out KM_E_TEXT);

                dict.TryGetValue("SegmentLength [m]", out PARAM1);
                //dict.TryGetValue("Radius_Begin [m]", out PARAM2);
                //dict.TryGetValue("Radius_End [m]", out PARAM3);

                dict.TryGetValue("Start ETRS89 X [m]", out string? x1);
                dict.TryGetValue("Start ETRS89 Y [m]", out string? y1);
                dict.TryGetValue("Start ETRS89 Z [m]", out string? z1);

                start.Add(double.Parse(x1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(y1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                start.Add(double.Parse(z1)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(start);


                dict.TryGetValue("End ETRS89 X [m]", out string? x2);
                dict.TryGetValue("End ETRS89 Y [m]", out string? y2);
                dict.TryGetValue("End ETRS89 Z [m]", out string? z2);

                end.Add(double.Parse(x2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(y2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                end.Add(double.Parse(z2)); //, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                coord.Add(end);

                UberhohungGeometry uberhohungGeometry = new UberhohungGeometry() { type = "LineString", coordinates = coord };
                UberhohungProperties properties = new UberhohungProperties()
                {
                    ID = ID,
                    RIKZ = RIKZ,
                    ELTYP = ELTYP,
                    PARAM1 = PARAM1 == null ? PARAM1 : PARAM1.Replace(',', '.'),
                    PARAM2 = PARAM2 == null ? PARAM2 : PARAM2.Replace(',', '.'),
                    PARAM3 = PARAM3 == null ? PARAM3 : PARAM3.Replace(',', '.'),
                    PARAM4 = PARAM4 == null ? PARAM4 : PARAM4.Replace(',', '.'),
                    KM_A_KM = KM_A_KM == null ? KM_A_KM : KM_A_KM.Replace(',', '.'),
                    KM_E_KM = KM_E_KM == null ? KM_E_KM : KM_E_KM.Replace(',', '.'),
                    KM_A_M = KM_A_M == null ? KM_A_M : KM_A_M.Replace(',', '.'),
                    KM_E_M = KM_E_M == null ? KM_E_M : KM_E_M.Replace(',', '.'),
                    KM_A_TEXT = KM_A_TEXT == null ? KM_A_TEXT : KM_A_TEXT.Replace(',', '.'),
                    KM_E_TEXT = KM_E_TEXT == null ? KM_E_TEXT : KM_E_TEXT.Replace(',', '.')
                };
                features.Add(new UberhohungFeature() { type = "Feature", properties = properties, geometry = uberhohungGeometry });
            }

            uh.features = features;
            string output = JsonConvert.SerializeObject(uh);
            File.WriteAllText(filePath, output);
        }
    }
}
