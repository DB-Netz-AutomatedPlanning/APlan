using ERDM.Tier_0;
using ERDM.Tier_1;
using ERDM.Tier_2;
using ERDM.Tier_3;
using MathNet.Numerics;
using Org.BouncyCastle.Tls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERDM_Implementation
{
    public class ERDMhelperFunctions
    {
        #region create or find geoCoordinate
        public static GeoCoordinates FindGeoCoordinate(double x, double y, double z, ERDM.ERDMmodel erdmModel)
        {
            var doubles = SixDecimalsDoubles(x,y,z);
            double xx = doubles[0];
            double yy = doubles[1];
            double zz = doubles[2];
            //search for existance.
            var geoCoordiante = erdmModel.Tier0.GeoCoordinates.Find(coor => coor.xCoordinate == xx && coor.yCoordinate == yy && coor.zCoordinate == zz);
            
            if (geoCoordiante != null)
                return geoCoordiante;

            return null;
        }

        public static GeoCoordinates CreateGeoCoordinate(double x, double y, double z, MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            var doubles = SixDecimalsDoubles(x, y, z);
            double xx = doubles[0];
            double yy = doubles[1];
            double zz = doubles[2];

            var newGeoCoordinate = new GeoCoordinates()
            {
                id = Guid.NewGuid().ToString(),
                name = "geoCoordinate",
                xCoordinate = xx,
                yCoordinate = yy,
                zCoordinate = zz,
            };
            // add to Tier0
            mapData.consistsOfTier0Objects?.Add(newGeoCoordinate.id);
            erdmModel.Tier0?.GeoCoordinates.Add(newGeoCoordinate);
            return newGeoCoordinate;
        }
        #endregion

        #region create or find TrackEdgePoint
        public static TrackEdgePoint FindTrackEdgePoint(double x, double y, double z, ERDM.ERDMmodel erdmModel)
        {
            var doubles = SixDecimalsDoubles(x, y, z);
            double xx = doubles[0];
            double yy = doubles[1];
            double zz = doubles[2];
            //search for existing geoCoordinate
            var geoCoordiante = FindGeoCoordinate(xx, yy, zz, erdmModel);

            if (geoCoordiante == null)
                return null;

            var trackEdgePoint = erdmModel?.Tier2?.TrackEdgePoint.Find(x => x is TrackEdgePoint && x.isLocatedAtGeoCoordinates.Equals(geoCoordiante.id));

            //if found
            if (trackEdgePoint != null)
                return trackEdgePoint;

            return null;
        }

        private static TrackEdgePoint CreateTrackEdgePoint(double x, double y, double z, double offset, TrackEdge trackEdge, MapData mapData, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            var doubles = SixDecimalsDoubles(x, y, z);
            double xx = doubles[0];
            double yy = doubles[1];
            double zz = doubles[2];

            var geoCoordiante = FindGeoCoordinate(xx, yy, zz, erdmModel);

            if (geoCoordiante == null)
                geoCoordiante = CreateGeoCoordinate(xx, yy, zz, mapData, erdmModel);

            TrackEdgePoint trackEdgePoint = new TrackEdgePoint()
            {
                id = Guid.NewGuid().ToString(),
                isLocatedAtGeoCoordinates = geoCoordiante.id,
                offset = offset,
                name = geoCoordiante.name,
                version = version.id,
                isPositionedOnTrackEdge = trackEdge.id
            };
            mapData.consistsOfTier2Objects?.Add(trackEdgePoint.id);
            erdmModel.Tier2?.TrackEdgePoint.Add(trackEdgePoint);
            return trackEdgePoint;
        }
        #endregion

        #region create or find trackEdgeSection
        public static TrackEdgeSection FindTrackEdgeSection(double x1, double y1, double z1, double x2, double y2, double z2, double length, ERDM.ERDMmodel erdmModel)
        {
            var doubles1 = SixDecimalsDoubles(x1, y1, z1);
            double xx1 = doubles1[0];
            double yy1 = doubles1[1];
            double zz1 = doubles1[2];
            var doubles2 = SixDecimalsDoubles(x2, y2, z2);
            double xx2 = doubles2[0];
            double yy2 = doubles2[1];
            double zz2 = doubles2[2];

            var point1 = FindTrackEdgePoint(xx1, yy1, zz1,erdmModel);
            var point2 = FindTrackEdgePoint(xx1, yy1, zz1, erdmModel);

            if (point1 == null || point2 == null)
                return null;

            var trackEdgeSection = erdmModel.Tier2.TrackEdgeSection.Find(section => section.hasStartTrackEdgePoint.Equals(point1.id) && section.hasEndTrackEdgePoint.Equals(point2.id) && section.length == length);

            if (trackEdgeSection != null)
                return trackEdgeSection;

            return null;
        }
        
        public static TrackEdgeSection CreateTrackEdgeSection(double x1, double y1, double z1, double offset1, double x2, double y2, double z2,string trackEdgeID,double length,MapData mapData, ERDM.Tier_0.Version verison, ERDM.ERDMmodel erdmModel)
        {
            var xx1 = ((double)Math.Truncate((decimal)x1 * 1000000m) / 1000000.0);
            var yy1 = ((double)Math.Truncate((decimal)y1 * 1000000m) / 1000000.0);
            var zz1 = ((double)Math.Truncate((decimal)z1 * 1000000m) / 1000000.0);
            var xx2 = ((double)Math.Truncate((decimal)x2 * 1000000m) / 1000000.0);
            var yy2 = ((double)Math.Truncate((decimal)y2 * 1000000m) / 1000000.0);
            var zz2 = ((double)Math.Truncate((decimal)z2 * 1000000m) / 1000000.0);

            var edgeID = ReplaceNonAlphaNumericalChar(trackEdgeID);
            TrackEdge trackEdge = erdmModel?.Tier1?.TrackEdge.Find(x => x is TrackEdge && x.name.Equals(edgeID));

            var startTrackEdge = FindTrackEdgePoint(xx1, yy1, zz1,erdmModel);
            if (startTrackEdge == null)
                startTrackEdge=CreateTrackEdgePoint(xx1, yy1, zz1, offset1, trackEdge, mapData, verison, erdmModel);

            var endTrackEdge = FindTrackEdgePoint(xx2, yy2, zz2, erdmModel);
            if (endTrackEdge == null)
                endTrackEdge=CreateTrackEdgePoint(xx2, yy2, zz2, offset1, trackEdge, mapData, verison, erdmModel);

            TrackEdgeSection TES = new() {
                id = Guid.NewGuid().ToString(),
                name = "EdgeSection",
                version = verison.id,
                length = length,
                hasStartTrackEdgePoint = startTrackEdge.id,
                hasEndTrackEdgePoint = endTrackEdge.id,
                isPartOfTrackEdge = trackEdge?.id
            };

            mapData?.consistsOfTier2Objects?.Add(TES.id);
            erdmModel?.Tier2?.TrackEdgeSection.Add(TES);

            return TES;
        }
        #endregion

        #region create TrackEdge
        public static void CreateTrackEdge(string name, double length, TrackNode startNode, TrackNode endNode, MapData mapData, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            //Create new one if not found.
            var trackEdge = new TrackEdge()
            {
                id = Guid.NewGuid().ToString(),
                name = name,
                version = version.id,
                length = length,
                gauge = new List<TrackEdgeGauge?>() { TrackEdgeGauge._750 }, //introduced for validation.
                hasStartTrackNode = startNode?.id,
                hasEndTrackNode = endNode?.id
            };
            // add to Tier0
            mapData.consistsOfTier1Objects?.Add(trackEdge.id);
            erdmModel.Tier1?.TrackEdge.Add(trackEdge);
        }
        #endregion

        #region create Node
        public static TrackNode CreateNewTrackNode(GeoCoordinates geoCoordiante, string name, MapData mapData, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            TrackNode node = new TrackNode()
            {
                id = Guid.NewGuid().ToString(),
                name = name,
                isLocatedAtGeoCoordinates = geoCoordiante.id,
                version = version.id,
                nodeType = TrackNodeType.EndOfTrack
            };
            mapData?.consistsOfTier1Objects?.Add(node.id);
            erdmModel?.Tier1?.TrackNode.Add(node);
            return node;
        }
        #endregion

        #region assist
        public static List<string> ExtractTrackEdgeSectionsForGradients(double startKM, double endKM,MapData mapData,ERDM.ERDMmodel erdmModel)
        {
            var trackEdgeSections = new List<string>();
            var result = erdmModel.Tier2.TrackEdgePoint.FindAll(x => x is TrackEdgePoint && (((TrackEdgePoint)x).offset >= startKM && ((TrackEdgePoint)x).offset <= endKM) && (mapData.consistsOfTier2Objects.Contains(x.id)));

            foreach (var item in erdmModel.Tier2.TrackEdgeSection)
            {
                if (item is TrackEdgeSection)
                {
                    var trackEdgeSection = (TrackEdgeSection)item;

                    var sections = result.FindAll(x => x.id.Equals(trackEdgeSection.hasStartTrackEdgePoint) || x.id.Equals(trackEdgeSection.hasEndTrackEdgePoint));
                    if (sections.Count!=0)
                    {
                        trackEdgeSections.Add(trackEdgeSection.id);
                    }
                }

            }
            return trackEdgeSections;
        }
        #endregion
        
        #region misc
        private static double[] SixDecimalsDoubles(double x , double y , double z)
        {
            return new  double [] { ((double)Math.Truncate((decimal)x * 1000000m) / 1000000.0),((double)Math.Truncate((decimal)y * 1000000m) / 1000000.0), ((double)Math.Truncate((decimal)z * 1000000m) / 1000000.0) };
        }
        public static double parseDouble(string value)
        {
            double.TryParse(value, out var x);
            return x;
        }
        /// <summary>
        /// create SHA512 value from string input
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string CreateSHA512(string strData)
        {
            var message = Encoding.UTF8.GetBytes(strData);
            using (var alg = SHA512.Create())
            {
                string hex = "";
                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                // Truncate the string to 128 characters
                hex = hex.Substring(0, 128);
                return hex;
            }
        }
        /// <summary>
        /// extract length of track edge from the xls file of horizontal segments
        /// </summary>
        /// <param name="edgeID"></param>
        /// <param name="segmentsXlsItems"></param>
        /// <returns></returns>
        public static double ExtractEdgeLengthFromSegmentsFile(string edgeID,ArrayList segmentsXlsItems)
        {
            double length=0.1;

            foreach (Dictionary<string, string> dict in segmentsXlsItems)
            {
                dict.TryGetValue("Track_Edge", out string edge);
                if (edge!=null && edge.Equals(edgeID))
                {
                    dict.TryGetValue("Length [km]", out string segmentLength);
                    var len = parseDouble(segmentLength);
                    length += len*1000; //file unit is km
                }
            }

            return length;
        }
        /// <summary>
        /// replace non alphanumerical charachters with AaA
        /// </summary>
        /// <returns></returns>
        public static string ReplaceNonAlphaNumericalChar(string input) {

            string output="";
            
            foreach(char a in input.ToArray())
            {
                if (!char.IsLetterOrDigit(a))
                {
                    output += "AaA";
                }
                else
                {
                    output += a;
                }
            }
            return output;
        }
        #endregion
    }
}
