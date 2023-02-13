using net.sf.saxon.functions;
using RCA_Model.Tier_0;
using RCA_Model.Tier_1;
using RCA_Model.Tier_2;
using RCA_Model.Tier_3;
using SD1_DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM_Implementation
{
    public class ERDMobjectsCreator
    {
        public ERDM createModel(string SegmentsPath, string GradientsPath, string NodesPath, string EdgesPath)
        {

            XLSreader reader = new XLSreader();

            var horizotanSegments = reader.ReadXLSContent(SegmentsPath, 0);
            var gradients = reader.ReadXLSContent(GradientsPath, 0);
            var nodes = reader.ReadXLSContent(NodesPath, 0); // simulated information till we get the correct one.
            var edges = reader.ReadXLSContent(EdgesPath, 0); // simulated information till we get the correct one.

            RCA_Model.Tier_0.Version version = new RCA_Model.Tier_0.Version();
            AreaOfControl areaOfControl = new AreaOfControl();

            version.id = Guid.NewGuid().ToString();
            version.version = 1;
            version.created = DateTimeOffset.UtcNow;
            version.status = LevelOfMaturity.validated;
            version.hashValue = ERDMhelperFunctions.CreateSHA512("helllo");

            areaOfControl.id = Guid.NewGuid().ToString();
            areaOfControl.name = "AreaOfControl1";
            areaOfControl.version = version.id;
            areaOfControl.consistsOfTrackEdgeSection = new();
            areaOfControl.areaOfControlIdentifier = "Area1";


            MapData mapData = new MapData();
            mapData.consistsOfTier0Objects = new();
            mapData.consistsOfTier1Objects = new();
            mapData.consistsOfTier2Objects = new();
            mapData.consistsOfTier3Objects = new();
            mapData.belongsToAreaOfControl = areaOfControl.id;
            mapData.version = version.id;
            mapData.name = "mapData1";
            mapData.id = Guid.NewGuid().ToString();

            mapData.consistsOfTier0Objects.Add(version.id);
            mapData.consistsOfTier2Objects.Add(areaOfControl.id);


            ERDM erdmModel = new();
            erdmModel.Tier0 = new() { version, mapData };
            erdmModel.Tier1 = new();
            erdmModel.Tier2 = new() { areaOfControl};
            erdmModel.Tier3 = new();

            ERDMobjectsCreator creator = new ERDMobjectsCreator();

            creator.TrackNodesCreator(nodes, mapData, version, erdmModel); //simulated data.
            creator.TrackEdgesCreator(edges, horizotanSegments, mapData, version, erdmModel); //simulated data.
            creator.HorizontalSegmentsCreator(horizotanSegments, mapData, version, areaOfControl, erdmModel);
            creator.GradientSegmentsCreator(gradients, mapData, version, erdmModel);

            return erdmModel;
        }
        /// <summary>
        /// create Nodes informations based on data extracted from xls file of Nodes.
        /// </summary>
        /// <param name="xlsItems"></param>
        /// <param name="mapdata"></param>
        public void TrackNodesCreator(ArrayList xlsItems, MapData mapdata, RCA_Model.Tier_0.Version version, ERDM erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("KM [km]", out string? KM);
                dict.TryGetValue("Pkt_East[m]", out string? xx);
                dict.TryGetValue("Pkt_North [m]", out string? yy);
                dict.TryGetValue("Pkt_Height [m]", out string? zz);
                dict.TryGetValue("Node_ID", out string? nodeID);

                var Gx = ERDMhelperFunctions.parseDouble(xx);
                var Gy = ERDMhelperFunctions.parseDouble(yy);
                var Gz = ERDMhelperFunctions.parseDouble(zz);

                nodeID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(nodeID);

                GeoCoordinates geoCoordiante = ERDMhelperFunctions.CreatOrFindGeoCoordinates(Gx, Gy, Gz, mapdata, erdmModel);

                ERDMhelperFunctions.CreateNewTrackNode(geoCoordiante, nodeID, mapdata, version, erdmModel);
            }
        }
        /// <summary>
        /// create edges informations based on data extracted from xls file of Edges. 
        /// </summary>
        /// <param name="xlsItems"></param>
        /// <param name="mapdata"></param>
        /// <param name="version"></param>
        public void TrackEdgesCreator(ArrayList xlsItems, ArrayList segmentsXlsItems, MapData mapdata, RCA_Model.Tier_0.Version version, ERDM erdmModel)
        {

           foreach (Dictionary<string, string> dict in xlsItems)
           {
                dict.TryGetValue("Edge_ID", out string? edgeID);
                dict.TryGetValue("Start_Node", out string? startNode);
                dict.TryGetValue("End_Node", out string? endNode);
                edgeID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(edgeID);
                startNode = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(startNode);
                endNode = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(endNode);

                var start = erdmModel.Tier1?.Find(x => x.name.Equals(startNode) && x is TrackNode);
                var end = erdmModel.Tier1?.Find(x => x.name.Equals(endNode) && x is TrackNode);
                var length = ERDMhelperFunctions.ExtractEdgeLengthFromSegmentsFile(edgeID, segmentsXlsItems);
                ERDMhelperFunctions.CreateNewTrackEdge(edgeID, length, start as TrackNode, end as TrackNode, mapdata, version, erdmModel);
           }
        }
        /// <summary>
        /// create HorizontalSegments based on data extracted from xls file of horizontalSegments
        /// the data from the xls file should be sorted topologically from start to end.
        /// </summary>
        /// <param name="xlsItems">List of dictionaries of attributes and values</param>
        /// <returns></returns>
        public void HorizontalSegmentsCreator(ArrayList xlsItems, MapData mapdata, RCA_Model.Tier_0.Version version, AreaOfControl areaOfControl,ERDM erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("Type", out var _type);
                dict.TryGetValue("KM_Begin [km]", out var _beginKm);
                dict.TryGetValue("KM_End [km]", out var _endKm);
                dict.TryGetValue("Length [km]", out var _length);
                dict.TryGetValue("Radius_Begin [m]", out var _radiusBegin);
                dict.TryGetValue("Radius_End [m]", out var _radiusEnd);
                dict.TryGetValue("Radius_Direction", out var _radiusDirection);
                dict.TryGetValue("St_Pkt_East[m]", out var _x1);
                dict.TryGetValue("St_Pkt_North [m]", out var _y1);
                dict.TryGetValue("St_Pkt_Height [m]", out var _z1);
                dict.TryGetValue("End_Pkt_East [m]", out var _x2);
                dict.TryGetValue("End_Pkt_North [m]", out var _y2);
                dict.TryGetValue("End_Pkt_Height [m]", out var _z2);
                dict.TryGetValue("Segment_ID", out var _segmentID);
                dict.TryGetValue("Track_Edge", out var _trackEdge);

                _segmentID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(_segmentID);
                _trackEdge = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(_trackEdge);

                var x1 = ERDMhelperFunctions.parseDouble(_x1);
                var y1 = ERDMhelperFunctions.parseDouble(_y1);
                var z1 = ERDMhelperFunctions.parseDouble(_z1);
                var x2 = ERDMhelperFunctions.parseDouble(_x2);
                var y2 = ERDMhelperFunctions.parseDouble(_y2);
                var z2 = ERDMhelperFunctions.parseDouble(_z2);
                var beginKm = ERDMhelperFunctions.parseDouble(_beginKm);
                var endKM = ERDMhelperFunctions.parseDouble(_endKm);
                var length = ERDMhelperFunctions.parseDouble(_length);
                var radiusBegin = ERDMhelperFunctions.parseDouble(_radiusBegin);
                var radiusEnd = ERDMhelperFunctions.parseDouble(_radiusEnd);

                TrackEdgeSection trackSection = ERDMhelperFunctions.CreateOrFindTrackEdgeSection(x1, y1, z1, beginKm, x2, y2, z2, endKM, _segmentID, _trackEdge, length, mapdata, version,erdmModel);
                areaOfControl?.consistsOfTrackEdgeSection?.Add(trackSection.id);

                switch (_type)
                {
                    case "Straight":
                        CreateSegmentLine(_segmentID, trackSection,mapdata,version,erdmModel);
                        break;
                    case "Radius":
                        CreateCurveSegmentArc(radiusBegin,_segmentID, trackSection, mapdata, version, erdmModel);
                        break;
                    case "Transition":
                        CreateCurveSegmentTransition(_segmentID,trackSection, mapdata, version, erdmModel);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// create gradient information based on data extracted from xls file of Gradients.
        /// the data from the xls file should be sorted topologically from start to end.
        /// </summary>
        public void GradientSegmentsCreator(ArrayList xlsItems, MapData mapdata, RCA_Model.Tier_0.Version version,ERDM erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("Start_height [m]", out var _altitute);
                dict.TryGetValue("Gradient [‰]", out var _gradient);
                dict.TryGetValue("Segment ID", out var segmentID);
                dict.TryGetValue("Start_KM [km]", out var _startKM);
                dict.TryGetValue("End_KM [km]", out var _endKM);

                segmentID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(segmentID);

                var altitude = ERDMhelperFunctions.parseDouble(_altitute);
                var gradient = ERDMhelperFunctions.parseDouble(_gradient);
                var startKM = ERDMhelperFunctions.parseDouble(_startKM);
                var endKM = ERDMhelperFunctions.parseDouble(_endKM);

                GradientSegmentLine GSL = new()
                {
                    id = Guid.NewGuid().ToString(),
                    name = segmentID,
                    startAltitude = altitude,
                    gradient = gradient,
                    appliesToTrackEdgeSection = new(),
                    version = version.id
                };

                var trackEdgeSectionslist = ERDMhelperFunctions.ExtractTrackEdgeSectionsForGradients(startKM,endKM,mapdata,erdmModel);

                GSL.appliesToTrackEdgeSection.AddRange(trackEdgeSectionslist);

                //this part is just a simulation and should be replaced when the issure is solved.
                if (trackEdgeSectionslist.Count==0)
                {
                    //GSL.appliesToTrackEdgeSection.Add(Guid.NewGuid().ToString()); //simulated for validation
                }

                mapdata?.consistsOfTier3Objects?.Add(GSL.id);
                erdmModel?.Tier3?.Add(GSL);
            }

        }
        /// <summary>
        /// create a CurveSegmentLine which correspond to Straight type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateSegmentLine(string? segmentID,TrackEdgeSection? trackEdgeSection, MapData mapdata, RCA_Model.Tier_0.Version version,ERDM erdmModel)
        {
            CurveSegmentLine curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection?.id);
            curveSegment.version = version.id;
            //curveSegment.azimuthAngle = 0; //until it is calculated or given.

            curveSegment.name = segmentID;

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.Add(curveSegment);
        }
        /// <summary>
        /// Create a CurveSegmentArc which correspond to Radius type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateCurveSegmentArc(double radiusBegin,string? segmentID,TrackEdgeSection trackEdgeSection, MapData mapdata, RCA_Model.Tier_0.Version version,ERDM erdmModel)
        {
            CurveSegmentArc curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection.id);
            curveSegment.version = version.id;
            curveSegment.name = segmentID;
            curveSegment.radius = radiusBegin;
            //curveSegment.initialArcLength = 0; // to be adjusted.
            //curveSegment.hasCenterAtGeoCoordinates = Guid.NewGuid().ToString(); //to be adjusted.

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.Add(curveSegment);
        }
    
        /// <summary>
        /// Create a CurveSegmentTransition which correspond to Transition type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateCurveSegmentTransition(string? segmentID,TrackEdgeSection trackEdgeSection, MapData mapdata, RCA_Model.Tier_0.Version version,ERDM erdmModel)
        {
  

            CurveSegmentTransition curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection.id);
            curveSegment.version = version.id;
            curveSegment.name = segmentID;

           // curveSegment.initialArcLength = 0; // to be adjusted.
           // curveSegment.azimuthAngle = 0; // to be adjusted.
           // curveSegment.clothoidParameter = 0.001; //to be adjusted.
           // curveSegment.hasCenterAtGeoCoordinates = Guid.NewGuid().ToString();// to be adjusted.

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.Add(curveSegment);
        }

    }
}
