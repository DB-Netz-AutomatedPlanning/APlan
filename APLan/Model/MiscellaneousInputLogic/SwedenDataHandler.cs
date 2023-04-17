using ERDM;
using ERDM.Tier_0;
using ERDM.Tier_1;
using ERDM.Tier_2;
using ERDM.Tier_3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;

namespace C_sharp_learning
{
    public class SwedenDataHandler
    {
        #region attributes
        string xmlFilePath;
        ERDMmodel erdmModel;
        XDocument doc;
        XNamespace ns = "http://www.trafikverket.se/FacilityData/2016/08";
        #endregion

        #region constructor
        public SwedenDataHandler(string filePath) {
            this.xmlFilePath = filePath;
            doc = XDocument.Load(filePath);
            erdmModel = new();
            erdmModel.Tier0.MapData = new() { new() {
                consistsOfTier0Objects = new(),
                consistsOfTier1Objects = new(),
                consistsOfTier2Objects = new(),
                consistsOfTier3Objects = new(),
            }};
            erdmModel.Tier1.TrackNode = new();
            erdmModel.Tier0.GeoCoordinates = new();
            erdmModel.Tier3.CurveSegmentLine = new();
            erdmModel.Tier1.TrackEdge = new();
        }
        #endregion

        #region ERDM handlers
        private TrackEdgePoint addTrackEdgePoint(GeoCoordinates coor, TrackEdge edge,double offset,ERDMmodel erdmModel,string EdgeID=null)
        {

            var TrackPoint =erdmModel.Tier2.TrackEdgePoint?.Find(x => x.isLocatedAtGeoCoordinates!=null && x.isLocatedAtGeoCoordinates.Equals(coor.id));
            if (TrackPoint != null)
                return TrackPoint;

            TrackEdgePoint point = new()
            {
                id = Guid.NewGuid().ToString(),
                name = coor.name,
                isLocatedAtGeoCoordinates = coor.id,
                offset=offset,
                isPositionedOnTrackEdge = edge.id
            };
            erdmModel.Tier0.MapData[0].consistsOfTier2Objects.Add(point.id);
            erdmModel.Tier2.TrackEdgePoint?.Add(point);
            return point;
        }
        private TrackEdgeSection addTrackEdgeSection(TrackEdgePoint startPoint, TrackEdgePoint endPoint, double length,TrackEdge edge,ERDMmodel erdmModel)
        {
            var TrackSection = erdmModel.Tier2.TrackEdgeSection?.Find(x => x.hasStartTrackEdgePoint != null && x.hasStartTrackEdgePoint.Equals(startPoint.id) && x.hasEndTrackEdgePoint != null && x.hasEndTrackEdgePoint.Equals(endPoint.id));
            if (TrackSection != null)
                return TrackSection;

            TrackEdgeSection trackSection = new()
            {
                id = Guid.NewGuid().ToString(),
                name = startPoint.name + endPoint.name,
                hasStartTrackEdgePoint = startPoint.id,
                hasEndTrackEdgePoint = endPoint.id,
                isPartOfTrackEdge = edge.id,
                length = length
            };
            erdmModel.Tier0.MapData[0].consistsOfTier2Objects.Add(trackSection.id);
            erdmModel.Tier2.TrackEdgeSection?.Add(trackSection);
            return trackSection;
        }
        private CurveSegmentLine addCurveSegmentLine(TrackEdgePoint startPoint, TrackEdgePoint endPoint, double azimuth, string universalID, ERDMmodel erdmModel)
        {
            var TrackSection = erdmModel.Tier2.TrackEdgeSection?.Find(x => x.hasStartTrackEdgePoint != null && x.hasStartTrackEdgePoint.Equals(startPoint.id) && x.hasEndTrackEdgePoint != null && x.hasEndTrackEdgePoint.Equals(endPoint.id));
            CurveSegmentLine curveSegmentLine = new()
            {
                id = Guid.NewGuid().ToString(),
                name = universalID,
                azimuthAngle = azimuth,
                appliesToTrackEdgeSection = TrackSection!=null && TrackSection.id!=null? new() { TrackSection.id } : null
            };
            erdmModel.Tier0.MapData[0].consistsOfTier3Objects.Add(curveSegmentLine.id);
            erdmModel.Tier3.CurveSegmentLine?.Add(curveSegmentLine);
            return curveSegmentLine;

        }
        private GeoCoordinates addCoordianteToERDM(List<string> coordiantes, ERDMmodel model)
        {
            var coord = model.Tier0.GeoCoordinates.Find(x =>(
            (x.xCoordinate!=null && x.yCoordinate!=null && x.zCoordinate!=null)&&
            (x.xCoordinate.ToString().Equals(coordiantes[0]) && x.yCoordinate.ToString().Equals(coordiantes[1]) && x.zCoordinate.ToString().Equals(coordiantes[2]))
            ));

            if (coord == null)
            {
                double.TryParse(coordiantes[0], out double x);
                double.TryParse(coordiantes[1], out double y);
                double.TryParse(coordiantes[2], out double z);

                GeoCoordinates coordinate = new()
                {
                    id = Guid.NewGuid().ToString(),
                    name = coordiantes[0] + coordiantes[1] + coordiantes[2],
                    xCoordinate = x,
                    yCoordinate = y,
                    zCoordinate = z,
                };
                erdmModel.Tier0.MapData[0].consistsOfTier0Objects.Add(coordinate.id);
                model.Tier0.GeoCoordinates.Add(coordinate);
                return coordinate;
            }
            return coord;
        }
        #endregion

        #region xml handlers
        public XmlDocument fetchFile()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            return xmlDocument;
        }
        public async Task<ERDMmodel> getInfraStructure()
        {
            ERDM.ERDMmodel erdModel = null;
            await Task.Run(() =>
            {
                XmlDocument doc = fetchFile();
                XmlNode infrastructureNode = doc.SelectSingleNode(".//*[local-name()='Infrastructure']");
                var links = infrastructureNode != null ? infrastructureNode.SelectSingleNode(".//*[local-name()='Links']") : null;
                var nodes = infrastructureNode != null ? infrastructureNode.SelectSingleNode(".//*[local-name()='Nodes']") : null;
                XmlNode signalingFacilities = doc.SelectSingleNode(".//*[local-name()='SignalingFacilities']");
                var informationPoints = this.doc.Descendants(ns + "InformationPoint");
                if (nodes != null)
                    extractNodes(nodes);
                if (links != null)
                    extractLinks(links);
                if (informationPoints != null)
                    extractInformationPoints(informationPoints);
                erdModel = this.erdmModel;
            });
            return erdModel;
        }
        private void extractLinks(XmlNode linkes)
        {
            Dictionary<string, object> data = new();

            foreach (XmlNode item in linkes.ChildNodes)
            {
                var UniversalId = item != null ? item.SelectSingleNode(".//*[local-name()='UniversalId']")?.InnerText : null;
                var VersionId = item != null ? item.SelectSingleNode(".//*[local-name()='VersionId']")?.InnerText : null;

                var LinkID = getLinkID(item);

                var coordiantes = getLinkCoordinates(item);
                var fromNodeId = item != null ? item.SelectSingleNode(".//*[local-name()='FromNodeId']")?.InnerText : null;
                var toNodeId = item != null ? item.SelectSingleNode(".//*[local-name()='ToNodeId']")?.InnerText : null;
                var length = item != null ? item.SelectSingleNode(".//*[local-name()='Length']")?.InnerText : null;

                double.TryParse(length, out double len);

                TrackEdge edge = new()
                {
                    id = Guid.NewGuid().ToString(),
                    hasStartTrackNode = fromNodeId,
                    hasEndTrackNode = toNodeId,
                    length = len,
                    name= UniversalId
                };
                erdmModel.Tier1.TrackEdge.Add(edge);
                erdmModel.Tier0.MapData[0].consistsOfTier1Objects.Add(edge.id);

                double offset1 = 0;
                double offset2 = 0;
                for (int i = 0; i < coordiantes.Count - 1; i++)
                {
                    var coor1 = addCoordianteToERDM(coordiantes[i], erdmModel);
                    var coor2 = addCoordianteToERDM(coordiantes[i + 1], erdmModel);
                    //update offsets of coor1 and coor2 on the edge
                    offset1 += offset2;
                    offset2 += distanceBetweenTwoCoordinates2D(coor1, coor2);

                    var point1 = addTrackEdgePoint(coor1, edge, offset1, erdmModel);
                    var point2 = addTrackEdgePoint(coor2, edge, offset2, erdmModel);

                    var section = addTrackEdgeSection(point1, point2, offset2 - offset1, edge, erdmModel);

                    var azimuth = calculateAzimuthAngleFromTwoPoints(coor1, coor2);

                    addCurveSegmentLine(point1, point2, azimuth, $"{UniversalId} {coordiantes[i][0]} {coordiantes[i][1]} {coordiantes[i + 1][0]} {coordiantes[i + 1][1]}", erdmModel);
                }

            }
        }
        private void extractNodes(XmlNode nodes)
        {
            erdmModel.Tier1.TrackNode = new List<ERDM.Tier_1.TrackNode>();
            foreach (XmlNode item in nodes.ChildNodes)
            {
                var UniversalId = item != null ? item.SelectSingleNode(".//*[local-name()='UniversalId']")?.InnerText : null;
                var VersionId = item != null ? item.SelectSingleNode(".//*[local-name()='VersionId']")?.InnerText : null;

                var LocationNumber = item != null ? item.SelectSingleNode("//*[local-name()='LocationNumber']")?.InnerText : null;
                var NodeNumber = item != null ? item.SelectSingleNode("//*[local-name()='NodeNumber']")?.InnerText : null;

                var coordiantes = getCoordiantes(item);

                var type = item != null ? item.SelectSingleNode("//*[local-name()='Type']")?.InnerText : null;

                var erdmCoordiante = addCoordianteToERDM(coordiantes, erdmModel);

                TrackNode node = new()
                {
                    id = Guid.NewGuid().ToString(),
                    name = UniversalId,
                    isLocatedAtGeoCoordinates = erdmCoordiante.id
                };
                erdmModel.Tier0.MapData[0].consistsOfTier1Objects.Add(node.id);
                erdmModel.Tier1.TrackNode.Add(node);
            }
        }
        private void extractInformationPoints(IEnumerable<XElement> informatioPoints)
        {
            foreach (var item in informatioPoints)
            {
                var universalId= from globalID in item.Descendants(ns+ "Identification").Descendants(ns + "GlobalId")
                                    select globalID.Element(ns + "UniversalId").Value;

                var edgeGlobalID = from globalID in item.Descendants(ns + "LinkPosition").Descendants(ns + "GlobalId")
                                    select globalID.Element(ns + "UniversalId").Value;

                var coordaintes = from Coordinate in item.Descendants(ns + "Coordinate")
                                    select new List<string>(){ Coordinate.Element(ns + "Easting").Value, Coordinate.Element(ns + "Northing").Value, Coordinate.Element(ns + "HeightOverSea").Value };

                var offset = from StartDistanceFromNode in item.Descendants(ns+ "StartDistanceFromNode")
                                  select StartDistanceFromNode.Value;

                var direction = from Direction in item.Descendants(ns + "Direction")
                             select Direction.Value;

                var type = from Signal in item.Descendants(ns + "Signal")
                                select Signal.Element(ns + "Type").Value;

                var geoCoor = addCoordianteToERDM(coordaintes.FirstOrDefault(), erdmModel);
                var trackEdge = erdmModel.Tier1.TrackEdge.Find(x => x.name.Equals(edgeGlobalID.FirstOrDefault()));

                LightSignal lightSignal = new()
                {
                    id = Guid.NewGuid().ToString(),
                    name = universalId.FirstOrDefault(),
                    appliesToTrackEdgePoint = new() { addTrackEdgePoint(geoCoor, trackEdge, double.Parse(offset.FirstOrDefault()), erdmModel).id }
                };

                erdmModel.Tier3.LightSignal.Add(lightSignal);
                erdmModel.Tier0.MapData[0].consistsOfTier1Objects.Add(lightSignal.id);
            }
        }
        private List<string> getCoordiantes(XmlNode node)
        {
            var northing = node != null ? node.SelectSingleNode(".//*[local-name()='Northing']")?.InnerText : null;
            var easting = node != null ? node.SelectSingleNode(".//*[local-name()='Easting']")?.InnerText : null;
            var heightOverSeaype = node != null ? node.SelectSingleNode(".//*[local-name()='HeightOverSea']")?.InnerText : null;
            return new() { easting, northing, heightOverSeaype };
        }
        private List<List<string>> getLinkCoordinates(XmlNode node)
        {
            List<List<string>> coordiantes = new();
            var coordiantesNode = node != null ? node.SelectNodes(".//*[local-name()='Coordinate']") : null;
            foreach (XmlNode item in coordiantesNode)
            {
                coordiantes.Add(getCoordiantes(item));
            }
            return coordiantes;
        }
        private Dictionary<string, string> getLinkID(XmlNode node)
        {
            Dictionary<string, string> data = new();
            var NodeFromLocationNumber = node != null ? node.SelectSingleNode(".//*[local-name()='NodeFrom']")?.SelectSingleNode(".//*[local-name()='LocationNumber']")?.InnerText : null;
            var NodeFromNodeNumber = node != null ? node.SelectSingleNode(".//*[local-name()='NodeFrom']")?.SelectSingleNode(".//*[local-name()='NodeNumber']")?.InnerText : null;
            var NodeToLocationNumber = node != null ? node.SelectSingleNode(".//*[local-name()='NodeTo']")?.SelectSingleNode(".//*[local-name()='LocationNumber']")?.InnerText : null;
            var NodeToNodeNumber = node != null ? node.SelectSingleNode(".//*[local-name()='NodeTo']")?.SelectSingleNode(".//*[local-name()='NodeNumber']")?.InnerText : null;

            data.TryAdd(nameof(NodeFromLocationNumber), NodeFromLocationNumber);
            data.TryAdd(nameof(NodeFromNodeNumber), NodeFromNodeNumber);
            data.TryAdd(nameof(NodeToLocationNumber), NodeToLocationNumber);
            data.TryAdd(nameof(NodeToNodeNumber), NodeToNodeNumber);

            return data;
        }
        private string nodeTypeToERDM(string type)
        {
            //to be revised. points in the Trafikverket can be represented by several points in ERDM
            switch (type)
            {
                case "SwitchOrCrossing":
                    return "Point";
                case "BufferStop":
                    return "System Border";
                case "Turntable":
                    return "Point";
                case "Fictisous":
                    return "Point";
                case "CountryBorder":
                    return "System Border";
                case "LocationBorder":
                    return "Point";
                case "InfrastructureManagementBorder":
                    return "Point";
                case "TrackEnd":
                    return "End of Track";
            }
            return null;
        }
        #endregion

        #region helper functions
        private double calculateAzimuthAngleFromTwoPoints(GeoCoordinates c1, GeoCoordinates c2)
        {
            if (c1 != null && c2 != null && c1.xCoordinate != null && c2.xCoordinate != null && c1.yCoordinate != null && c2.yCoordinate != null)
            {
                double azimuth = Math.Atan2((double)c2.yCoordinate - (double)c1.yCoordinate, (double)c2.xCoordinate - (double)c1.xCoordinate);
                // convert to degrees
                return azimuth * 180 / Math.PI;
            }
            return 0;
        }
        private double distanceBetweenTwoCoordinates2D(GeoCoordinates coor1, GeoCoordinates coor2)
        {
            if (coor2.xCoordinate != null && coor1.xCoordinate != null && coor2.yCoordinate != null && coor1.yCoordinate != null)
            {
                return Math.Sqrt(Math.Pow((double)coor2.xCoordinate - (double)coor1.xCoordinate, 2) + Math.Pow((double)coor2.yCoordinate - (double)coor1.yCoordinate, 2));
            }
            return 0;

        }
        #endregion
    }
}
