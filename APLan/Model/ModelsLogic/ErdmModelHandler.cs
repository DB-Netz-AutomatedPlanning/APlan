using System;
using System.Collections.Generic;
using System.Linq;
using ERDM.Tier_0;
using ERDM.Tier_3;
using ERDM.Tier_2;
using ERDM.Tier_1;
using System.IO;
using ERDM_Implementation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using APLan.Model.CustomObjects;
using APLan.Model.ModelsLogic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NPOI.HPSF;
using System.Xml.Serialization;
using System.Windows;
using System.Reflection;
using NPOI.Util.ArrayExtensions;

namespace APLan.ViewModels.ModelsLogic
{
    public class ErdmModelHandler
    {
       
        #region ERDM Model Logic
        private List<string> getFileNamesFromString(string filesString)
        {
            var files = filesString.Split("+~+").ToList();
            files.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            return files;
        }
        public async Task<ERDM.ERDMmodel> createERDMProject(string XLS)
        {
            ERDM.ERDMmodel erdModel = null;
            await Task.Run(() =>
            {
                var files = getFileNamesFromString(XLS);
                var SegmentsFilePath = files.Find(x => x.Contains("Segments"));
                var GradientsFilePath = files.Find(x => x.Contains("Gradients"));
                var NodesFilePath = files.Find(x => x.Contains("Nodes"));
                var EdgesFilePath = files.Find(x => x.Contains("Edges"));

                ERDMobjectsCreator ERDMcreator = new();
                erdModel = ERDMcreator.createModel(SegmentsFilePath, GradientsFilePath, NodesFilePath, EdgesFilePath);
            });
            return erdModel;
        }
        /// <summary>
        /// draw the ERDM informations.
        /// </summary>
        /// <param name="mapData"></param>
        public async void drawERDM(ERDM.ERDMmodel erdmModel,ObservableCollection<CustomPolyLine> PolyLines, ObservableCollection<CustomCircle> Circles)
        {
            List<CustomPolyLine> lines = new();
            List<CustomCircle> nodes = new();
            await Task.Run(() =>
            {
                var allMapData = erdmModel.Tier0.MapData.FindAll(t => t is MapData).ToList(); // get all MapData.
                allMapData.ForEach(mapData => {
                    var drawingContent = drawMapData((MapData)mapData, erdmModel);
                    lines.AddRange((List<CustomPolyLine>)drawingContent[0]);
                    nodes.AddRange((List<CustomCircle>)drawingContent[1]);
                    nodes.AddRange((List<CustomCircle>)drawingContent[2]);
                }); // draw each one.
            });
            lines.ForEach(x => PolyLines.Add(x));
            nodes.ForEach(x => Circles.Add(x));
        }
        /// <summary>
        /// draw a MapData informations.
        /// </summary>
        private object[] drawMapData(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            var lines = drawSegments(mapData, erdmModel); //Segments
            var nodes = drawNodes(mapData, erdmModel); //Nodes
            var lightSignals = drawLightSignal(mapData, erdmModel); //lightSignals

            return new object[] { lines, nodes,lightSignals };
        }
        /// <summary>
        /// add the segments to the LA list and the corresponding points.
        /// </summary>
        /// <param name="erdmModel"></param>
        private List<CustomPolyLine> drawSegments(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            var segments = getAllSegmentsOfMapData(mapData, erdmModel);
            foreach (CurveSegment segment in segments)
            {
                CustomPolyLine polyLine = new();
                TrackEdgeSection Section = erdmModel.Tier2.TrackEdgeSection.Find(x => (x is TrackEdgeSection) && segment.appliesToTrackEdgeSection.Contains(x.id)) as TrackEdgeSection;
                var trackEdgePoints = erdmModel.Tier2.TrackEdgePoint.FindAll(x => (x is TrackEdgePoint) && Section.hasStartTrackEdgePoint.Equals(x.id) || Section.hasEndTrackEdgePoint.Equals(x.id));
                var geoCoordinates = erdmModel.Tier0.GeoCoordinates.FindAll(x => (x is GeoCoordinates) && (x.id.Equals((trackEdgePoints[0]).isLocatedAtGeoCoordinates) || x.id.Equals((trackEdgePoints[1]).isLocatedAtGeoCoordinates)));

                var point1 = new System.Windows.Point((double)((GeoCoordinates)geoCoordinates[0]).xCoordinate, (double)((GeoCoordinates)geoCoordinates[0]).yCoordinate);
                var point2 = new System.Windows.Point((double)((GeoCoordinates)geoCoordinates[1]).xCoordinate, (double)((GeoCoordinates)geoCoordinates[1]).yCoordinate);

                if (ViewModels.DrawViewModel.GlobalDrawingPoint.X == 0)
                    ViewModels.DrawViewModel.GlobalDrawingPoint = point1;


                polyLine.Name = Section.name;
                polyLine.CustomPoints.Add(new() { Point = point1 });
                polyLine.CustomPoints.Add(new() { Point = point2 });
                polyLine.Color = System.Windows.Media.Brushes.Red;

                attachData(segment, polyLine.Data);
                lines.Add(polyLine);
            }
            return lines;
        }
        /// <summary>
        /// draw nodes of the ERDM model.
        /// </summary>
        /// <param name="erdmModel"></param>
        private List<CustomCircle> drawNodes(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            List<CustomCircle> Circles = new();
            var nodes = getAllNodesOfMapData(mapData, erdmModel);


            foreach (var node in nodes)
            {
                var geoCoordinates = erdmModel.Tier0.GeoCoordinates.Find(x => node.isLocatedAtGeoCoordinates.Equals(x.id));
                if (ViewModels.DrawViewModel.GlobalDrawingPoint.X == 0 && geoCoordinates != null)
                    ViewModels.DrawViewModel.GlobalDrawingPoint = new((double)geoCoordinates.xCoordinate, (double)geoCoordinates.yCoordinate);

                CustomCircle circle = new()
                {
                    Center = new() { Point = new System.Windows.Point((double)(geoCoordinates.xCoordinate), (double)(geoCoordinates.yCoordinate)) },
                    Color = System.Windows.Media.Brushes.Red,
                    RadiusX = 5,
                    RadiusY = 5,
                };

                attachData(node, circle.Data);
                Circles.Add(circle);

            } 
            return Circles;
        }
        private List<CustomCircle> drawLightSignal(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            List<CustomCircle> Circles = new();
            var lightSignals = getAllLightSignalsOfMapData(mapData, erdmModel);
            foreach (var lightSignal in lightSignals)
            {
                var trackEdgePoints = erdmModel.Tier2.TrackEdgePoint.Find(x=>lightSignal.appliesToTrackEdgePoint.Contains(x.id));

                var geoCoordinates = erdmModel.Tier0.GeoCoordinates.Find(x => trackEdgePoints.isLocatedAtGeoCoordinates.Equals(x.id));

                if (ViewModels.DrawViewModel.GlobalDrawingPoint.X == 0 && geoCoordinates != null)
                    ViewModels.DrawViewModel.GlobalDrawingPoint = new((double)geoCoordinates.xCoordinate, (double)geoCoordinates.yCoordinate);

                CustomCircle circle = new()
                {
                    Center = new() { Point = new System.Windows.Point((double)(geoCoordinates.xCoordinate), (double)(geoCoordinates.yCoordinate)) },
                    Color = System.Windows.Media.Brushes.Blue,
                    RadiusX = 5,
                    RadiusY = 5,
                };

                attachData(lightSignal, circle.Data);
                Circles.Add(circle);

            }  
            return Circles;
        }
        /// <summary>
        /// get all Segments of this specific MapData by the help of the ERDM object.
        /// </summary>
        /// <param name="mapData"></param>
        /// <param name="erdmModel"></param>
        /// <returns></returns>
        private List<CurveSegment> getAllSegmentsOfMapData(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            List<CurveSegment> curveSegments = new();

            erdmModel.Tier3.CurveSegmentArc.FindAll(x => mapData.consistsOfTier3Objects.Contains(x.id)).ForEach(x => curveSegments.Add(x));
            erdmModel.Tier3.CurveSegmentLine.FindAll(x => mapData.consistsOfTier3Objects.Contains(x.id)).ForEach(x => curveSegments.Add(x));
            erdmModel.Tier3.CurveSegmentTransition.FindAll(x => mapData.consistsOfTier3Objects.Contains(x.id)).ForEach(x => curveSegments.Add(x));


            return curveSegments;
        }
        /// <summary>
        /// get all Nodes of this specific MapData by the help of the ERDM object.
        /// </summary>
        /// <param name="mapData"></param>
        /// <param name="erdmModel"></param>
        /// <returns></returns>
        private List<TrackNode> getAllNodesOfMapData(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            var nodes = erdmModel.Tier1.TrackNode.FindAll(x => (x is TrackNode));
            var mapDataNodes = nodes.FindAll(x => mapData.consistsOfTier1Objects.Contains(x.id)).ToList();
            return mapDataNodes;
        }
        private List<LightSignal> getAllLightSignalsOfMapData(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
            var lightSignals = erdmModel.Tier3.LightSignal.FindAll(x => (x is LightSignal));
            var mapDataLightSignals = lightSignals.FindAll(x => mapData.consistsOfTier1Objects.Contains(x.id)).ToList();
            return mapDataLightSignals;
        }
        public async Task<ERDM.ERDMmodel> deserializeFromJSON(string JSON)
        {
            ERDM.ERDMmodel erdModel = null;
            await Task.Run(async () =>
            {
                ERDMvalidator validator = new();
                string report =  await validator.validate(JSON);
                try
                {
                    erdModel = JsonSerializer.Deserialize<ERDM.ERDMmodel>(File.ReadAllText(JSON), new JsonSerializerOptions() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message+"\nFile Cannot be deserialized to ERDM");
                }
               
            });
            return erdModel;
        }
        public async Task<ERDM.ERDMmodel> deserializeFromXML(string XML)
        {
            ERDM.ERDMmodel erdModel = null;
            await Task.Run(async () =>
            {
                ERDMvalidator validator = new();
                string report = await validator.validate(XML);

                XmlSerializer ser = new XmlSerializer(typeof(ERDM.ERDMmodel));
                using (XmlReader reader = XmlReader.Create(XML))
                {
                    if (ser.CanDeserialize(reader))
                    {
                        erdModel = (ERDM.ERDMmodel)ser.Deserialize(reader);
                    }
                    else
                    {
                        MessageBox.Show("File Cannot be deserialized to ERDM");
                    }
                        
                }
            });
            return erdModel;
        }
        private void attachData(object currentObject, ObservableCollection<HelperClasses.KeyValue> dataCollection)
        {
            Type type = currentObject.GetType();
   
            foreach (PropertyInfo prop in type.GetProperties())
            {
                string propName = prop.Name;
                object propValue = prop.GetValue(currentObject);
                dataCollection.Add(new() { Key = propName, Value = propValue!=null? propValue:"null" });
            }
        }
        #endregion

    }
}
