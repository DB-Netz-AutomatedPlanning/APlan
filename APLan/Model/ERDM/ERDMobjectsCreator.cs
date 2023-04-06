using ERDM.Tier_0;
using ERDM.Tier_1;
using ERDM.Tier_2;
using ERDM.Tier_3;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ERDM_Implementation
{
    public class ERDMobjectsCreator
    {
        public ERDM.ERDMmodel createModel(string SegmentsPath, string GradientsPath, string NodesPath, string EdgesPath)
        {

            XLSreader reader = new XLSreader();

            var horizotanSegments = reader.ReadXLSContent(SegmentsPath, 0);
            var gradients = reader.ReadXLSContent(GradientsPath, 0);
            var nodes = reader.ReadXLSContent(NodesPath, 0); // simulated information till we get the correct one.
            var edges = reader.ReadXLSContent(EdgesPath, 0); // simulated information till we get the correct one.

            //return null if sth is wrong
            if (horizotanSegments == null || gradients == null || nodes == null || edges == null)
            {
                return null;
            }

            ERDM.Tier_0.Version version = new ERDM.Tier_0.Version();
            AreaOfControl areaOfControl = new AreaOfControl();

            version.id = Guid.NewGuid().ToString();
            version.version = 1;
            DateTime dateTime = DateTime.UtcNow;
            TimeSpan offset = TimeSpan.Zero;
            version.created = new ERDM.CustomDateTime(new DateTimeOffset(dateTime, offset));
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


            ERDM.ERDMmodel erdmModel = new();
            erdmModel.Tier0 = new();
            erdmModel.Tier1 = new();
            erdmModel.Tier2 = new();
            erdmModel.Tier3 = new();

            erdmModel.Tier0.Version.Add(version);
            erdmModel.Tier0.MapData.Add(mapData);
            erdmModel.Tier2.AreaOfControl.Add(areaOfControl);

            //trials of some attributes
            //erdmModel.Tier3.Balise.Add(new() { baliseTelegram = new ERDM.CustomBitArray(new bool[] { true, false, true }) });
            //erdmModel.Tier3.SegmentProfile.Add(new SegmentProfile() { utcTimeOffset = new ERDM.CustomTime() { value=TimeSpan.FromHours(-0.5) } });
            //LightSignal signal = new() { id = Guid.NewGuid().ToString() };
            //erdmModel.Tier3.LightSignal.Add(signal);
            //erdmModel.Tier3.TimingPoint.Add(new() {refersToSignal= signal.id });
            
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
        public void TrackNodesCreator(ArrayList xlsItems, MapData mapdata, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("KM [km]", out string KM);
                dict.TryGetValue("Pkt_East[m]", out string xx);
                dict.TryGetValue("Pkt_North [m]", out string yy);
                dict.TryGetValue("Pkt_Height [m]", out string zz);
                dict.TryGetValue("Node_ID", out string nodeID);

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
        public void TrackEdgesCreator(ArrayList xlsItems, ArrayList segmentsXlsItems, MapData mapdata, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {

           foreach (Dictionary<string, string> dict in xlsItems)
           {
                dict.TryGetValue("Edge_ID", out string edgeID);
                dict.TryGetValue("Start_Node", out string startNode);
                dict.TryGetValue("End_Node", out string endNode);
                edgeID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(edgeID);
                startNode = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(startNode);
                endNode = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(endNode);

                var start = erdmModel.Tier1?.TrackNode.Find(x => x.name.Equals(startNode) && x is TrackNode);
                var end = erdmModel.Tier1?.TrackNode.Find(x => x.name.Equals(endNode) && x is TrackNode);
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
        public void HorizontalSegmentsCreator(ArrayList xlsItems, MapData mapdata, ERDM.Tier_0.Version version, AreaOfControl areaOfControl,ERDM.ERDMmodel erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("Type", out var _type);
                dict.TryGetValue("KM_start [km]", out var _beginKm);
                dict.TryGetValue("KM_end [km]", out var _endKm);
                dict.TryGetValue("SegmentLength [m]", out var _length);
                dict.TryGetValue("Start ETRS89 X [m]", out var _x1);
                dict.TryGetValue("Start ETRS89 Y [m]", out var _y1);
                dict.TryGetValue("Start ETRS89 Z [m]", out var _z1);
                dict.TryGetValue("End ETRS89 X [m]", out var _x2);
                dict.TryGetValue("End ETRS89 Y [m]", out var _y2);
                dict.TryGetValue("End ETRS89 Z [m]", out var _z2);
                dict.TryGetValue("Segment ID", out var _segmentID);
                dict.TryGetValue("Track Edge", out var _trackEdge);

                dict.TryGetValue("Start Radius [m]", out var _radiusBegin);
                dict.TryGetValue("End Radius [m]", out var _radiusEnd);
                //dict.TryGetValue("Radius_Direction", out var _radiusDirection);


                dict.TryGetValue("initialLength [m]", out var _initialLength);
                dict.TryGetValue("Start Azimuth Angle [Â°]", out var _startAzimuth);
                dict.TryGetValue("End Azimuth Angle [Â°]", out var _endAzimuth);
                dict.TryGetValue("Arc Center ETRS89 X [m]", out var _arcCenterX);
                dict.TryGetValue("Arc Center ETRS89 Y [m]", out var _arcCenterY);
                dict.TryGetValue("Arc Center ETRS89 Z [m]", out var _arcCenterZ);
                dict.TryGetValue("Colthoiden param [m]", out var _clothoidParameter);

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
                var startAzimuth = ERDMhelperFunctions.parseDouble(_startAzimuth);
                var initialLength = ERDMhelperFunctions.parseDouble(_initialLength);

                var arcCenterX = ERDMhelperFunctions.parseDouble(_arcCenterX);
                var arcCenterY = ERDMhelperFunctions.parseDouble(_arcCenterY);
                var arcCenterZ = ERDMhelperFunctions.parseDouble(_arcCenterZ);
                var clothoidParameter = ERDMhelperFunctions.parseDouble(_clothoidParameter);

                TrackEdgeSection trackSection = ERDMhelperFunctions.CreateOrFindTrackEdgeSection(x1, y1, z1, beginKm, x2, y2, z2, _trackEdge, length, mapdata, version,erdmModel);
                areaOfControl?.consistsOfTrackEdgeSection?.Add(trackSection.id);

                switch (_type)
                {
                    case "Straight":
                        CreateSegmentLine(_segmentID, startAzimuth, trackSection,mapdata,version,erdmModel);
                        break;
                    case "Radius":
                        CreateCurveSegmentArc(radiusBegin,_segmentID, initialLength, arcCenterX, arcCenterY, arcCenterZ, trackSection, mapdata, version, erdmModel);
                        break;
                    case "Transition":
                        CreateCurveSegmentTransition(_segmentID, initialLength, arcCenterX, arcCenterY, arcCenterZ,clothoidParameter,startAzimuth, trackSection, mapdata, version, erdmModel);
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
        public void GradientSegmentsCreator(ArrayList xlsItems, MapData mapdata, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            foreach (Dictionary<string, string> dict in xlsItems)
            {
                dict.TryGetValue("Start ETRS89 X [m]", out var _startX);
                dict.TryGetValue("Start ETRS89 Y [m]", out var _startY);
                dict.TryGetValue("Start ETRS89 Z [m]", out var _startZ);
                dict.TryGetValue("End ETRS89 X [m]", out var _endX);
                dict.TryGetValue("End ETRS89 Y [m]", out var _endY);
                dict.TryGetValue("End ETRS89 Z [m]", out var _endZ);

                dict.TryGetValue("SegmentLength [m]", out var _segmentLength);
                dict.TryGetValue("Start_Alt [m]", out var _startAltitute);
                dict.TryGetValue("Gradient [per mill]", out var _gradient);
                dict.TryGetValue("Segment ID", out var segmentID);
                dict.TryGetValue("Start_KM [km]", out var _startKM);
                dict.TryGetValue("End_KM [km]", out var _endKM);
                dict.TryGetValue("Edge_ID", out var _edgeID);

                segmentID = ERDMhelperFunctions.ReplaceNonAlphaNumericalChar(segmentID);

                var startX = ERDMhelperFunctions.parseDouble(_startX);
                var startY = ERDMhelperFunctions.parseDouble(_startY);
                var startZ = ERDMhelperFunctions.parseDouble(_startZ);
                var endX = ERDMhelperFunctions.parseDouble(_endX);
                var endY = ERDMhelperFunctions.parseDouble(_endY);
                var endZ = ERDMhelperFunctions.parseDouble(_endZ);
                var gradient = ERDMhelperFunctions.parseDouble(_gradient);
                var startKM = ERDMhelperFunctions.parseDouble(_startKM);
                var endKM = ERDMhelperFunctions.parseDouble(_endKM);
                var startAltitute = ERDMhelperFunctions.parseDouble(_startAltitute);
                var segmentLength = ERDMhelperFunctions.parseDouble(_segmentLength);

                GradientSegmentLine GSL = new()
                {
                    id = Guid.NewGuid().ToString(),
                    name = segmentID,
                    startAltitude = startAltitute,
                    gradient = gradient,
                    appliesToTrackEdgeSection = new(),
                    version = version.id
                };

                var trackEdgeSectionslist = ERDMhelperFunctions.ExtractTrackEdgeSectionsForGradients(startKM,endKM,mapdata,erdmModel);

                //var section = ERDMhelperFunctions.CreateOrFindTrackEdgeSection(startX,startY,startZ, startKM,endX,endY,endZ, _edgeID, segmentLength,mapdata,version,erdmModel);

                GSL.appliesToTrackEdgeSection.AddRange(trackEdgeSectionslist);

                //this part is just a simulation and should be replaced when the issure is solved.
                if (trackEdgeSectionslist.Count == 0)
                {
                    GSL.appliesToTrackEdgeSection.Add(ERDMhelperFunctions.CreateOrFindTrackEdgeSection(startX, startY, startZ, startKM, endX, endY, endZ, _edgeID, segmentLength, mapdata, version, erdmModel).id); //simulated for validation
                }

                mapdata?.consistsOfTier3Objects?.Add(GSL.id);
                erdmModel?.Tier3?.GradientSegmentLine.Add(GSL);
            }

        }
        /// <summary>
        /// create a CurveSegmentLine which correspond to Straight type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateSegmentLine(string segmentID,double Azimuth,TrackEdgeSection trackEdgeSection, MapData mapdata, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
            CurveSegmentLine curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection?.id);
            curveSegment.version = version.id;
            curveSegment.azimuthAngle = Azimuth;
            //curveSegment.azimuthAngle = 0; //until it is calculated or given.

            curveSegment.name = segmentID;

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.CurveSegmentLine.Add(curveSegment);
        }
        /// <summary>
        /// Create a CurveSegmentArc which correspond to Radius type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateCurveSegmentArc(double radiusBegin,string segmentID,double initialLength,double arcCenterX, double arcCenterY, double arcCenterZ, TrackEdgeSection trackEdgeSection, MapData mapdata, ERDM.Tier_0.Version version,ERDM.ERDMmodel erdmModel)
        {
            CurveSegmentArc curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection.id);
            curveSegment.version = version.id;
            curveSegment.name = segmentID;
            curveSegment.radius = radiusBegin;
            curveSegment.initialArcLength = initialLength;
            curveSegment.hasCenterAtGeoCoordinates = ERDMhelperFunctions.CreatOrFindGeoCoordinates(arcCenterX, arcCenterY, arcCenterZ, mapdata, erdmModel).id;

            //curveSegment.initialArcLength = 0; // to be adjusted.
            //curveSegment.hasCenterAtGeoCoordinates = Guid.NewGuid().ToString(); //to be adjusted.

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.CurveSegmentArc.Add(curveSegment);
        }
    
        /// <summary>
        /// Create a CurveSegmentTransition which correspond to Transition type in the xls file
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mapdata"></param>
        private void CreateCurveSegmentTransition(string segmentID, double initialLength, double arcCenterX, double arcCenterY, double arcCenterZ, double clothoidParameter, double startAzimuth, TrackEdgeSection trackEdgeSection, MapData mapdata, ERDM.Tier_0.Version version, ERDM.ERDMmodel erdmModel)
        {
  

            CurveSegmentTransition curveSegment = new();
            curveSegment.id = Guid.NewGuid().ToString();
            curveSegment.appliesToTrackEdgeSection = new();
            curveSegment.appliesToTrackEdgeSection.Add(trackEdgeSection.id);
            curveSegment.version = version.id;
            curveSegment.name = segmentID;
            curveSegment.initialArcLength = initialLength;
            curveSegment.clothoidParameter = clothoidParameter;
            curveSegment.azimuthAngle = startAzimuth;
            curveSegment.hasCenterAtGeoCoordinates = ERDMhelperFunctions.CreatOrFindGeoCoordinates(arcCenterX, arcCenterY, arcCenterZ, mapdata, erdmModel).id;

            // curveSegment.initialArcLength = 0; // to be adjusted.
            // curveSegment.azimuthAngle = 0; // to be adjusted.
            // curveSegment.clothoidParameter = 0.001; //to be adjusted.
            // curveSegment.hasCenterAtGeoCoordinates = Guid.NewGuid().ToString();// to be adjusted.

            mapdata.consistsOfTier3Objects?.Add(curveSegment.id);
            erdmModel.Tier3?.CurveSegmentTransition.Add(curveSegment);
        }

    }
}
