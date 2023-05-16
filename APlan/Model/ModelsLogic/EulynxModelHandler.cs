using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.TopoModels.EULYNX.generic;
using Models.TopoModels.EULYNX.rsmCommon;
using aplan.eulynx;
using aplan.core;
using System.Windows;
using APLan.Views;
using APLan.HelperClasses;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Linq;
using Models.TopoModels.EULYNX.sig;
using Models.TopoModels.EULYNX.db;
using Point = System.Windows.Point;
using APLan.Model.ModelsLogic;
using APLan.Model.CustomObjects;
using APLan.Model.HelperClasses;
using APLan.Model.Eulynx.aplan.EULYNX.EulynxJson;

namespace APLan.ViewModels.ModelsLogic
{
    public class EulynxModelHandler
    {
        #region fields
        private  List<PositioningNetElement> VisitedElements = new();
        private  PositioningNetElement neededElement = null;
        private System.Windows.Point firstPoint;
        private bool start = false;
        #endregion

        #region properties
        public  EulynxService eulynxService { get; set; }
        public static Database db { get; set; }
        private RsmEntities RsmEntities { get; set; }
        private List<DataContainer> DataContainer { get; set; }
        private List<IntrinsicCoordinate> IntrensicCoordinates { get; set; }
        private List<PositioningNetElement> PositioningNetElements { get; set; }
        private List<PositioningSystemCoordinate> PositioningSystemCoordinates { get; set; }
        private List<Unit> units { get; set; }
        #endregion

        #region eulynx Model Logic 
        public EulynxDataPrepInterface createEulynxModel(string country, string format,
                string mileageFilePath, string edgesFilePath, string nodesFilePath,
                string horizontalAlignmentsFilePath, string verticalAlignmentsFilePath, string cantAlingnmentsFilePath,
                string mdbFilePath, string projectPath)
        {
            Globals.routeNumber = 6624;
            eulynxService = EulynxService.getInstance();

            db = Database.getInstance();
            Database.setDBPath(projectPath);

            eulynxService.inputHandling(
           format, db,
           mileageFilePath, edgesFilePath, nodesFilePath,
           horizontalAlignmentsFilePath, verticalAlignmentsFilePath, cantAlingnmentsFilePath,
           mdbFilePath);

            // eulynx object creation
            return  (EulynxDataPrepInterface)eulynxService.objectsCreation(country, db);

        }


        /// <summary>
        /// draw the Eulynx Object
        /// </summary>
        /// <param name="canvasSize"></param>
        /// <param name="gleiskantenList"></param>
        /// <param name="gleiskantenPointsList"></param>
        /// <param name="Entwurfselement_LA_list"></param>
        /// <param name="Entwurfselement_LAPointsList"></param>
        /// <param name="Entwurfselement_KM_list"></param>
        /// <param name="Entwurfselement_KMPointsList"></param>
        /// <param name="Entwurfselement_HO_list"></param>
        /// <param name="Entwurfselement_HOList"></param>
        /// <param name="Entwurfselement_UH_list"></param>
        /// <param name="Entwurfselement_UHPointsList"></param>
        /// <param name="gleisknotenList"></param>
        public async Task<bool> drawObject(double canvasSize, EulynxDataPrepInterface eulynxModel, ObservableCollection<CustomPolyLine> PolyLines, ObservableCollection<CustomCircle> Circles, ObservableCollection<Signalinfo> Signals)
        {
            firstPoint = new System.Windows.Point(0, 0); //reset the first point in each drawing call. very important.

            DataContainer = eulynxModel.hasDataContainer;
            RsmEntities = DataContainer[0].ownsRsmEntities;
            PositioningNetElements = RsmEntities.usesTrackTopology.usesNetElement;
            PositioningSystemCoordinates = RsmEntities.usesTopography.usesPositioningSystemCoordinate;
            IntrensicCoordinates = RsmEntities.usesTopography.usesIntrinsicCoordinate;
            units = RsmEntities.usesUnit;

            var kanten = await DrawNetElement(PositioningNetElements, PositioningSystemCoordinates, canvasSize, units);
            fetchLinesForBinding(PolyLines,kanten);
            var HO = await DrawVerticalAlignment(RsmEntities, PositioningSystemCoordinates, canvasSize, units);
            fetchLinesForBinding(PolyLines, HO);
            var LA = await DrawHorizontalAlignment(RsmEntities, PositioningSystemCoordinates, canvasSize, units);
            fetchLinesForBinding(PolyLines, LA);
            var KM = await DrawMielage(RsmEntities, PositioningSystemCoordinates, canvasSize, units);
            fetchLinesForBinding(PolyLines, KM);
            var UH = await DrawCantlAlignment(RsmEntities, PositioningSystemCoordinates, canvasSize, units);
            fetchLinesForBinding(PolyLines, UH);
            var nodes = await DrawNodes(RsmEntities, PositioningSystemCoordinates, IntrensicCoordinates, canvasSize, units);
            fetchNodesForBinding(Circles, nodes);

            if (db != null)
            {
                extractMainSignals(eulynxModel, Signals);
                extractOnTrackSignals(eulynxModel, Signals);
            }
            else {
                System.Windows.MessageBox.Show(" Database is required to extract OnTrack and Main planned signals");
            }
            return true;
        }
        /// <summary>
        /// extract the NetElements in the EulynxObject async
        /// </summary>
        /// <param name="positioningNetElements"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="customPolylines"></param>
        /// <param name="units"></param>
        private async Task<List<CustomPolyLine>> DrawNetElement(List<PositioningNetElement> positioningNetElements,
            List<PositioningSystemCoordinate> PSCoordinates,
            double canvasSize,
            List<Unit> units)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            await Task.Run(() =>
            {
                List<PositioningSystemCoordinate> positioningSystemCoordinates = new List<PositioningSystemCoordinate>();
                PositioningSystemCoordinate positioningSystemCoordinate = new PositioningSystemCoordinate();
                foreach (PositioningNetElement positioningNetElement in positioningNetElements)
                {

                    CustomPolyLine polyline = new CustomPolyLine();
                    polyline.LineType = nameof(CustomItem.dataModelsTypes.GleisKanten);
                    polyline.Name = positioningNetElement.name;
                    polyline.Data.Add(new KeyValue() { Key = "Name", Value = positioningNetElement.name });
                    polyline.Data.Add(new KeyValue()
                    {
                        Key = "Length",
                        Value =
                        ((Length)((LinearElementWithLength)positioningNetElement).elementLength.quantiy[0]).value
                        .ToString() + " " +
                        units.Find(x => x.id.Equals(((LinearElementWithLength)positioningNetElement).elementLength.quantiy[0].unit.@ref)).name
                    });

                    List<AssociatedPositioning> associatedPositionings = positioningNetElement.associatedPositioning;
                    foreach (AssociatedPositioning associatedPositioning in associatedPositionings)
                    {
                        List<IntrinsicCoordinate> intrinsicCoordinates = associatedPositioning.intrinsicCoordinates;
                        foreach (IntrinsicCoordinate intrinsicCoordinate in intrinsicCoordinates)
                        {
                            List<tElementWithIDref> tElementWithIDrefs = intrinsicCoordinate.coordinates;
                            foreach (tElementWithIDref tElementWithIDref in tElementWithIDrefs)
                            {
                                positioningSystemCoordinate = PSCoordinates.Find(x => x.id.Equals(tElementWithIDref.@ref));

                                if (positioningSystemCoordinate is CartesianCoordinate)
                                {

                                    CartesianCoordinate cartesianCoordinate = positioningSystemCoordinate as CartesianCoordinate;
                                    if (firstPoint.X == 0)
                                    {
                                        firstPoint.X = (((double)cartesianCoordinate.x));
                                        firstPoint.Y = (((double)cartesianCoordinate.y));
                                        ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                                    }
                                    polyline.GlobalPoint = new Point(firstPoint.X, firstPoint.Y);
                                    Point newPoint = new Point((((double)cartesianCoordinate.x)), (((double)cartesianCoordinate.y)));
                                    polyline.CustomPoints.Add(new() { Point = newPoint, PointType = nameof(CustomItem.dataModelsTypes.GleisKantenPoints) });
                                }
                                else if (positioningSystemCoordinate is LinearCoordinate)
                                {
                                    extractStartEnd((LinearCoordinate)positioningSystemCoordinate, polyline, units, PSCoordinates);
                                }
                            }
                        }
                    }
                    InfoExtractor.attachProperties(polyline, positioningNetElement.id);
                    polyline.Color = new SolidColorBrush() { Color = Colors.Red };
                    polyline.Color.Freeze();
                    polyline.Points.Freeze();
                    lines.Add(polyline);
                }
            });
            return lines;
        }

        /// <summary>
        /// extract the VerticalAlignment in the EulynxObject async
        /// </summary>
        /// <param name="ownsRsmEntity"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="customPolylines"></param>
        /// <param name="units"></param>
        private async Task<List<CustomPolyLine>> DrawVerticalAlignment(RsmEntities ownsRsmEntity,
            List<PositioningSystemCoordinate> PSCoordinates,
            double canvasSize,
            List<Unit> units)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            await Task.Run(() =>
            {
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmCommon.VerticalAlignmentSegment>
                usesVerticalAlignemntSegment = ownsRsmEntity.usesTopography.usesVerticalAlignmentSegment;

                foreach (Models.TopoModels.EULYNX.rsmCommon.VerticalAlignmentSegment segment in usesVerticalAlignemntSegment)
                {
                    CustomPolyLine polyline = new CustomPolyLine();
                    polyline.LineType = nameof(CustomItem.dataModelsTypes.Entwurfselement_HO);
                    verticalElementExtractProperties(segment, polyline, ownsRsmEntity);
                    polyline.Name = null;
                    PolyLine poly = segment.hasLinearLocation.polyLines[1];
                    foreach (tElementWithIDref element in poly.coordinates)
                    {
                        CartesianCoordinate coordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(element.@ref));
                        if (firstPoint.X == 0)
                        {

                            firstPoint.X = (((double)coordinate.x));
                            firstPoint.Y = (((double)coordinate.y));
                            ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                        }
                        polyline.GlobalPoint = new Point(firstPoint.X, firstPoint.Y);
                        Point newPoint = new Point((((double)coordinate.x)), (((double)coordinate.y)));
                        polyline.CustomPoints.Add(new() { Point = newPoint, PointType = nameof(CustomItem.dataModelsTypes.Entwurfselement_HO_Points) });
                        //extractBoundary(newPoint);
                    }
                    InfoExtractor.attachProperties(polyline, segment.id);
                    polyline.Color = new SolidColorBrush() { Color = Colors.Red };
                    polyline.Color.Freeze();
                    polyline.Points.Freeze();
                    lines.Add(polyline);
                }
            });
            return lines;
        }

        /// <summary>
        /// extract the HorizontalAlignment in the EulynxObject async
        /// </summary>
        /// <param name="ownsRsmEntity"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="customPolylines"></param>
        /// <param name="units"></param>
        private async Task<List<CustomPolyLine>> DrawHorizontalAlignment(RsmEntities ownsRsmEntity,
           List<PositioningSystemCoordinate> PSCoordinates,
           double canvasSize,
           List<Unit> units)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            await Task.Run(() =>
            {
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmCommon.HorizontalAlignmentSegment>
                usesHorizontalAlignemntSegment = ownsRsmEntity.usesTopography.usesHorizontalAlignmentSegment;

                foreach (Models.TopoModels.EULYNX.rsmCommon.HorizontalAlignmentSegment segment in usesHorizontalAlignemntSegment)
                {
                    if (segment.initialAzimuth != null)
                    {
                        CustomPolyLine polyline = new CustomPolyLine();
                        polyline.LineType = nameof(CustomItem.dataModelsTypes.Entwurfselement_LA);
                        HorizontalElementExtractProperties(segment, polyline, ownsRsmEntity);
                        polyline.Name = null;
                        PolyLine poly = segment.hasLinearLocation.polyLines[1];
                        foreach (tElementWithIDref element in poly.coordinates)
                        {
                            CartesianCoordinate coordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(element.@ref));
                            //polyline.Points.Add(new Point((double)coordinate.x, (double)coordinate.y));
                            if (firstPoint.X == 0)
                            {

                                firstPoint.X = (((double)coordinate.x));
                                firstPoint.Y = (((double)coordinate.y));
                                ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                            }
                            polyline.GlobalPoint = new Point(firstPoint.X, firstPoint.Y);
                            Point newPoint = new Point((((double)coordinate.x)), (((double)coordinate.y)));
                            polyline.CustomPoints.Add(new() { Point = newPoint, PointType = nameof(CustomItem.dataModelsTypes.Entwurfselement_LA_Points) });
                            //extractBoundary(newPoint);
                        }
                        InfoExtractor.attachProperties(polyline, segment.id);
                        polyline.Color = new SolidColorBrush() { Color = Colors.Red };
                        polyline.Color.Freeze();
                        polyline.Points.Freeze();
                        lines.Add(polyline);
                    }
                }
            });
            return lines;
        }

        /// <summary>
        /// extract the Mielage in the EulynxObject async
        /// </summary>
        /// <param name="ownsRsmEntity"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="customPolylines"></param>
        /// <param name="units"></param>
        private async Task<List<CustomPolyLine>> DrawMielage(RsmEntities ownsRsmEntity,
            List<PositioningSystemCoordinate> PSCoordinates,
            double canvasSize,
            List<Unit> units)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            await Task.Run(() =>
            {
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmCommon.HorizontalAlignmentSegment>
                usesHorizontalAlignemntSegment = ownsRsmEntity.usesTopography.usesHorizontalAlignmentSegment;

                foreach (Models.TopoModels.EULYNX.rsmCommon.HorizontalAlignmentSegment segment in usesHorizontalAlignemntSegment)
                {
                    if (segment.initialAzimuth == null)
                    {
                        CustomPolyLine polyline = new CustomPolyLine();
                        polyline.LineType = nameof(CustomItem.dataModelsTypes.Entwurfselement_KM);
                        HorizontalElementExtractProperties(segment, polyline, ownsRsmEntity);
                        polyline.Name = null;
                        //PolyLine polyIntrensic = segment.hasLinearLocation.polyLines[0];
                        PolyLine poly = segment.hasLinearLocation.polyLines[1];
                        foreach (tElementWithIDref element in poly.coordinates)
                        {
                            CartesianCoordinate coordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(element.@ref));
                            if (firstPoint.X == 0)
                            {

                                firstPoint.X = (((double)coordinate.x));
                                firstPoint.Y = (((double)coordinate.y));
                                ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                            }
                            polyline.GlobalPoint = new Point(firstPoint.X, firstPoint.Y);
                            Point newPoint = new Point((((double)coordinate.x)), (((double)coordinate.y)));
                            polyline.CustomPoints.Add(new() { Point = newPoint, PointType = nameof(CustomItem.dataModelsTypes.Entwurfselement_KM_Points) });
                            //extractBoundary(newPoint);
                        }
                        InfoExtractor.attachProperties(polyline, segment.id);
                        polyline.Color = new SolidColorBrush() { Color = Colors.Red };
                        polyline.Color.Freeze();
                        polyline.Points.Freeze();
                        lines.Add(polyline);
                    }

                }
            });
            return lines;
        }
        /// <summary>
        /// extract the CantAlignment in the EulynxObject async
        /// </summary>
        /// <param name="ownsRsmEntity"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="customPolylines"></param>
        /// <param name="units"></param>
        private async Task<List<CustomPolyLine>> DrawCantlAlignment(RsmEntities ownsRsmEntity,
           List<PositioningSystemCoordinate> PSCoordinates,
           double canvasSize,
           List<Unit> units)
        {
            List<CustomPolyLine> lines = new List<CustomPolyLine>();
            await Task.Run(() =>
            {
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmCommon.AlignmentCantSegment>
                usesHorizontalAlignemntSegment = ownsRsmEntity.usesTopography.usesAlignmentCantSegment;

                foreach (Models.TopoModels.EULYNX.rsmCommon.AlignmentCantSegment segment in usesHorizontalAlignemntSegment)
                {
                    CustomPolyLine polyline = new CustomPolyLine();
                    polyline.LineType = nameof(CustomItem.dataModelsTypes.Entwurfselement_UH);
                    CantElementExtractProperties(segment, polyline, ownsRsmEntity);
                    polyline.Name = null;
                    PolyLine poly = segment.hasLinearLocation.polyLines[1];
                    foreach (tElementWithIDref element in poly.coordinates)
                    {
                        CartesianCoordinate coordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(element.@ref));
                        //polyline.Points.Add(new Point((double)coordinate.x, (double)coordinate.y));
                        if (firstPoint.X == 0)
                        {
                            firstPoint.X = (((double)coordinate.x));
                            firstPoint.Y = (((double)coordinate.y));
                            ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                        }
                        polyline.GlobalPoint = new Point(firstPoint.X, firstPoint.Y);
                        Point newPoint = new Point((((double)coordinate.x)), (((double)coordinate.y)));
                        polyline.CustomPoints.Add(new() { Point = newPoint, PointType = nameof(CustomItem.dataModelsTypes.Entwurfselement_UH_Points) });
                        //extractBoundary(newPoint);
                    }
                    InfoExtractor.attachProperties(polyline, segment.id);
                    polyline.Color = new SolidColorBrush() { Color = Colors.Red };
                    polyline.Color.Freeze();
                    polyline.Points.Freeze();
                    lines.Add(polyline);
                }
            });
            return lines;
        }

        /// <summary>
        /// extract the Nodes in the EulynxObject async
        /// </summary>
        /// <param name="ownsRsmEntity"></param>
        /// <param name="PSCoordinates"></param>
        /// <param name="IntrensicCoordinates"></param>
        /// <param name="canvasSize"></param>
        /// <param name="allNodes"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        private async Task<List<CustomCircle>> DrawNodes(RsmEntities ownsRsmEntity,
            List<PositioningSystemCoordinate> PSCoordinates,
            List<IntrinsicCoordinate> IntrensicCoordinates,
            double canvasSize,
            List<Unit> units)
        {
            List<CustomCircle> nodes = new List<CustomCircle>();
            await Task.Run((Action)(() =>
            {
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmTrack.Turnout>
                Nodes = ownsRsmEntity.ownsPoint;
                System.Collections.Generic.List<Models.TopoModels.EULYNX.rsmTrack.VehicleStop>
                    VehicleNodes = ownsRsmEntity.ownsVehicleStop;
                List<CartesianCoordinate> AllCoordinates = new List<CartesianCoordinate>();


                //turn outs.
                foreach (Models.TopoModels.EULYNX.rsmTrack.Turnout turn in Nodes)
                {

                    Models.TopoModels.EULYNX.rsmCommon.tElementWithIDref loc = turn.locations[0];
                    SpotLocation spot = (SpotLocation)ownsRsmEntity.usesLocation.Find(x => x.id.Equals(loc.@ref));
                    List<AssociatedNetElement> AssElement = spot.associatedNetElements;

                    CustomCircle node = new CustomCircle();
                    node.CircleType = nameof(CustomItem.dataModelsTypes.GleisKnoten);
                    node.RadiusX = 5;
                    node.RadiusY = 5;
                    TurnOutElementExtractProperties(turn, node, ownsRsmEntity);
                    node.Name = null;
                    foreach (AssociatedNetElement ele in AssElement)
                    {
                        IntrinsicCoordinate intCoordinate = (IntrinsicCoordinate)IntrensicCoordinates.Find(x => x.id.Equals(ele.bounds[0].@ref));
                        CartesianCoordinate cartCoordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(intCoordinate.coordinates[1].@ref));
                        if (!AllCoordinates.Contains(cartCoordinate))
                        {
                            AllCoordinates.Add(cartCoordinate);
                            if (firstPoint.X == 0)
                            {
                                firstPoint.X = (((double)cartCoordinate.x));
                                firstPoint.Y = (((double)cartCoordinate.y));
                                ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                            }
                            Point newPoint = new Point((((double)cartCoordinate.x)), (((double)cartCoordinate.y)));
                            node.Center = new() { Point = newPoint };
                            node.Color = Brushes.Red;
                            node.Color.Freeze();
                            nodes.Add(node);
                            //extractBoundary(newPoint);   
                        }
                    }
                    InfoExtractor.attachProperties(node, turn.id);
                }
                //Vehicle Stops
                foreach (Models.TopoModels.EULYNX.rsmTrack.VehicleStop Vstop in VehicleNodes)
                {

                    Models.TopoModels.EULYNX.rsmCommon.tElementWithIDref locVstop = Vstop.locations[0];
                    SpotLocation spotVstop = (SpotLocation)ownsRsmEntity.usesLocation.Find(x => x.id.Equals(locVstop.@ref));
                    List<AssociatedNetElement> AssElementVstop = spotVstop.associatedNetElements;
                    CustomCircle node = new CustomCircle();
                    node.CircleType = nameof(CustomItem.dataModelsTypes.GleisKnoten);
                    node.RadiusX = 5;
                    node.RadiusY = 5;
                    VehicleStopElementExtractProperties(Vstop, node, ownsRsmEntity);
                    node.Name = null;
                    foreach (AssociatedNetElement ele in AssElementVstop)
                    {
                        IntrinsicCoordinate intCoordinate = (IntrinsicCoordinate)IntrensicCoordinates.Find(x => x.id.Equals(ele.bounds[0].@ref));
                        CartesianCoordinate cartCoordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(intCoordinate.coordinates[1].@ref));
                        if (!AllCoordinates.Contains(cartCoordinate))
                        {
                            AllCoordinates.Add(cartCoordinate);
                            if (firstPoint.X == 0)
                            {

                                firstPoint.X = (((double)cartCoordinate.x));
                                firstPoint.Y = (((double)cartCoordinate.y));
                                ViewModels.DrawViewModel.GlobalDrawingPoint = firstPoint;
                            }
                            Point newPoint = new Point((((double)cartCoordinate.x)), (((double)cartCoordinate.y)));
                            node.Center = new() { Point = newPoint };
                            node.Color = Brushes.Red;
                            node.Color.Freeze();
                            nodes.Add(node);
                            //extractBoundary(newPoint);
                        }

                    }
                    InfoExtractor.attachProperties(node, Vstop.id);
                }
            }));
            return nodes;
        }

        /// <summary>
        /// extract the stard and end kilometer value of a polyline.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="polyLine"></param>
        /// <param name="units"></param>
        /// <param name="PSCoordinates"></param>
        private void extractStartEnd(LinearCoordinate coordinate, CustomPolyLine polyLine, List<Unit> units, List<PositioningSystemCoordinate> PSCoordinates)
        {
            if (start == false)
            {
                polyLine.Data.Add(new KeyValue() { Key = "Start", Value = coordinate.measure.value?.ToString() + "  " + units.Find(x => x.id.Equals(coordinate.measure.unit.@ref)).name });
                start = true;
            }
            else
            {
                polyLine.Data.Add(new KeyValue() { Key = "End", Value = coordinate.measure.value?.ToString() + "  " + units.Find(x => x.id.Equals(coordinate.measure.unit.@ref)).name });
                start = false;
            }
        }
        /// <summary>
        /// extract the points used to draw polylines.
        /// </summary>
        /// <param name="polylines"></param>
        /// <param name="pointsContainer"></param>
        private void verticalElementExtractProperties(VerticalAlignmentSegment element, CustomPolyLine polyline, RsmEntities ownsRsmEntity)
        {
            //double? initialSegmentValue = element.initialSegment.value;
            //string? initialSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.initialSegment.unit.@ref)).name;

            //double? endSegmentValue= element.endSegment.value;
            //string? endSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x=>x.id.Equals(element.endSegment.unit.@ref)).name;

            Gradient initialElevation = ownsRsmEntity.usesTopography.usesElevationAndInclination.Find(x => x.id.Equals(element.initialElevation.@ref)).inclination;

            var initialElevationValue = initialElevation.value;
            var initialElevationUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(initialElevation.unit.@ref)).name;

            var finalElevationValue = initialElevation.value;
            var finalElevationUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(initialElevation.unit.@ref)).name;

            var length = element.length.value;
            var lengthUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.length.unit.@ref)).name;

            var type = element.type;

            LinearCoordinate startKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
            x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[0].@ref)) as LinearCoordinate);

            LinearCoordinate endKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
             x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[1].@ref)) as LinearCoordinate);

            var startKMvalue = startKM.measure.value;
            var startKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(startKM.measure.unit.@ref)).name;

            var endKMvalue = endKM.measure.value;
            var endKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(endKM.measure.unit.@ref)).name;


            polyline.Data.Add(new KeyValue() { Key = "startKM", Value = startKMvalue.ToString() + " " + startKMunit });
            polyline.Data.Add(new KeyValue() { Key = "endSegmentValue", Value = endKMvalue.ToString() + " " + endKMunit });
            polyline.Data.Add(new KeyValue() { Key = "initialElevationValue", Value = initialElevationValue.ToString() + " " + initialElevationUnit });
            polyline.Data.Add(new KeyValue() { Key = "finalElevationValue", Value = finalElevationValue.ToString() + " " + finalElevationUnit });
            polyline.Data.Add(new KeyValue() { Key = "length", Value = length.ToString() + " " + lengthUnit });
            //polyline.Data.Add(new KeyValue() { Key = "startKMvalue", Value = startKMvalue.ToString() + " " + startKMunit });
            //polyline.Data.Add(new KeyValue() { Key = "endKMvalue", Value = endKMvalue.ToString() + " " + endKMunit });
            polyline.Data.Add(new KeyValue() { Key = "type", Value = type });

        }
        private void HorizontalElementExtractProperties(Models.TopoModels.EULYNX.rsmCommon.HorizontalAlignmentSegment element, CustomPolyLine polyline, RsmEntities ownsRsmEntity)
        {
            //double? initialSegmentValue = element.initialSegment.value;
            //string? initialSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.initialSegment.unit.@ref)).name;

            //double? endSegmentValue = element.endSegment.value;
            //string? endSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.endSegment.unit.@ref)).name;

            var initialAzimuth = element.initialAzimuth?.value;
            var initialAzimuthUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.initialAzimuth?.unit.@ref))?.name;

            double? radius = null;
            string radiusUnit = null;

            double? initialRadius = null;
            string initialRadiusUnit = null;

            double? finalRadius = null;
            string finalRadiusUnit = null;

            string type = null;


            if (element is HorizontalSegmentArc)
            {
                radius = ((HorizontalSegmentArc)element).radius.value;
                radiusUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(((HorizontalSegmentArc)element).radius.unit.@ref)).name;
            }
            else if (element is HorizontalSegmentTransition)
            {

                initialRadius = ((HorizontalSegmentTransition)element).initialRadius.value;
                initialRadiusUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(((HorizontalSegmentTransition)element).initialRadius.unit.@ref)).name;

                finalRadius = ((HorizontalSegmentTransition)element).finalRadius.value;
                finalRadiusUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(((HorizontalSegmentTransition)element).finalRadius.unit.@ref)).name;

                type = ((HorizontalSegmentTransition)element).type;
            }

            var length = element.length.value;
            var lengthUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.length.unit.@ref)).name;


            LinearCoordinate startKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
            x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[0].@ref)) as LinearCoordinate);

            LinearCoordinate endKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
             x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[1].@ref)) as LinearCoordinate);

            var startKMvalue = startKM.measure.value;
            var startKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(startKM.measure.unit.@ref)).name;

            var endKMvalue = endKM.measure.value;
            var endKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(endKM.measure.unit.@ref)).name;

            if (startKMvalue != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "initialSegmentValue", Value = startKMvalue.ToString() + " " + startKMunit });
                polyline.Data.Add(new KeyValue() { Key = "endSegmentValue", Value = endKMvalue.ToString() + " " + endKMunit });
            }
            if (initialAzimuth != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "initialAzimuth", Value = initialAzimuth.ToString() + " " + initialAzimuthUnit });
            }
            if (initialRadius != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "initialRadius", Value = initialRadius.ToString() + " " + initialRadiusUnit });
            }
            if (finalRadius != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "finalRadius", Value = finalRadius.ToString() + " " + finalRadiusUnit });
            }
            if (radius != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "radius", Value = radius.ToString() + " " + radiusUnit });
            }
            if (type != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "type", Value = type });
            }

            polyline.Data.Add(new KeyValue() { Key = "length", Value = length.ToString() + " " + lengthUnit });
            //polyline.Data.Add(new KeyValue() { Key = "startKMvalue", Value = startKMvalue.ToString() + " " + startKMunit });
            //polyline.Data.Add(new KeyValue() { Key = "endKMvalue", Value = endKMvalue.ToString() + " " + endKMunit });


        }
        private void CantElementExtractProperties(Models.TopoModels.EULYNX.rsmCommon.AlignmentCantSegment element, CustomPolyLine polyline, RsmEntities ownsRsmEntity)
        {
            //double? initialSegmentValue = element.initialSegment.value;
            //string? initialSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.initialSegment.unit.@ref)).name;

            //double? endSegmentValue = element.endSegment.value;
            //string? endSegmentValueUnit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(element.endSegment.unit.@ref)).name;

            string transitionType = null;
            if (element is SegmentCantTransition)
            {
                transitionType = ((SegmentCantTransition)element).transitionShape.isOfTransitionType.Value.ToString();
            }
            LinearCoordinate startKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
            x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[0].@ref)) as LinearCoordinate);

            LinearCoordinate endKM = (ownsRsmEntity.usesTopography.usesPositioningSystemCoordinate.Find(x =>
             x.id.Equals(element.hasLinearLocation.polyLines[0].coordinates[1].@ref)) as LinearCoordinate);

            var startKMvalue = startKM.measure.value;
            var startKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(startKM.measure.unit.@ref)).name;

            var endKMvalue = endKM.measure.value;
            var endKMunit = ownsRsmEntity.usesUnit.Find(x => x.id.Equals(endKM.measure.unit.@ref)).name;
            if (startKMvalue != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "initialSegmentValue", Value = startKMvalue.ToString() + " " + startKMunit });
                polyline.Data.Add(new KeyValue() { Key = "endSegmentValue", Value = endKMvalue.ToString() + " " + endKMunit });
            }
            if (transitionType != null)
            {
                polyline.Data.Add(new KeyValue() { Key = "transitionType", Value = transitionType });
            }



            //polyline.Data.Add(new KeyValue() { Key = "startKMvalue", Value = startKMvalue.ToString() + " " + startKMunit });
            //polyline.Data.Add(new KeyValue() { Key = "endKMvalue", Value = endKMvalue.ToString() + " " + endKMunit });


        }
        private void TurnOutElementExtractProperties(Models.TopoModels.EULYNX.rsmTrack.Turnout element, CustomCircle node, RsmEntities ownsRsmEntity)
        {
            string longname = element.longname;
            string name = element.name;

            node.Data.Add(new KeyValue() { Key = "longname", Value = longname.ToString() });
            node.Data.Add(new KeyValue() { Key = "name", Value = name.ToString() });
        }
        private void VehicleStopElementExtractProperties(Models.TopoModels.EULYNX.rsmTrack.VehicleStop element, CustomCircle node, RsmEntities ownsRsmEntity)
        {
            var name = element.name;
            node.Data.Add(new KeyValue() { Key = "name", Value = name.ToString() });
        }

        /// <summary>
        /// fetch the lines to the UI on the Main thread
        /// </summary>
        /// <param name="bindedList"></param>
        /// <param name="lines"></param>
        private void fetchLinesForBinding(ObservableCollection<CustomPolyLine> PolyLines, List<CustomPolyLine> lines)
        {
            foreach (CustomPolyLine line in lines)
            {
                PolyLines.Add(line);
            }
        }

        /// <summary>
        /// fetch the nodes to the UI on the Main thread
        /// </summary>
        /// <param name="bindedList"></param>
        /// <param name="nodes"></param>
        private void fetchNodesForBinding(ObservableCollection<CustomCircle> Circles,List<CustomCircle> nodes)
        {
            foreach (CustomCircle node in nodes)
            {
                Circles.Add(node);
            }
        }
        #endregion
        
        #region eulynx planning 
        /// <summary>
        /// Extract the onTrack signals from the Eulynx object.
        /// </summary>
        /// <param name="EulynxObject"></param>
        public  void extractOnTrackSignals(EulynxDataPrepInterface eulynxModel, ObservableCollection<Signalinfo> Signals)
        {
            var dataPrepEntities = eulynxModel.hasDataContainer.First().ownsDataPrepEntities;
            List<Unit> units = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesUnit;
            List<Models.TopoModels.EULYNX.rsmSig.OnTrackSignallingDevice> onTrackSignals = eulynxModel.hasDataContainer.First().ownsRsmEntities.ownsOnTrackSignallingDevice;
            List<BaseLocation> locations = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesLocation;
            List<PositioningSystemCoordinate> PSCoordinates = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate;
            List<IntrinsicCoordinate> intrCoordinates = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate;
            List<PositioningNetElement> netElements = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTrackTopology.usesNetElement;
            foreach (Models.TopoModels.EULYNX.rsmSig.OnTrackSignallingDevice signal in onTrackSignals)
            {
                //dp function
                var euBalise = dataPrepEntities.ownsTrackAsset.First(x => x is EtcsBalise && ((EtcsBalise)x).refersToRsmTpsDevice.@ref == signal.id) as EtcsBalise;
                var euBaliseGroupId = euBalise.implementsTpsDataTxSystem.@ref;
                var euBaliseProperty = dataPrepEntities.ownsTpsDataTransmissionSystemProperties.First(x => x is EtcsBaliseGroupLevel2 && ((EtcsBaliseGroupLevel2)x).appliesToTpsDataTxSystem.@ref == euBaliseGroupId) as EtcsBaliseGroupLevel2;
                var euBaliseFunction = euBaliseProperty.implementsFunction.First().type;


                // get axle counter's location and print it
                var spotLoc = locations.Find(item => item.id == signal.locations.First().@ref) as Models.TopoModels.EULYNX.rsmCommon.SpotLocation; // dereference
                var tpsDeviceIntrinsicCoordinateRef = spotLoc.associatedNetElements.First().bounds.First();
                var tpsDeviceIntrinsicCoordinate = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate.First(item => item.id == tpsDeviceIntrinsicCoordinateRef.@ref);
                var tpsDeviceCoordinate = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.First(item => item.id == tpsDeviceIntrinsicCoordinate.coordinates.First().@ref) as LinearCoordinate;
                var tpsDeviceKmValue = tpsDeviceCoordinate.measure.value.Value;


                AssociatedNetElement assElement = spotLoc.associatedNetElements[0];
                tElementWithIDref bound = assElement.bounds[0];
                IntrinsicCoordinate intcoord = intrCoordinates.Find(x => x.id.Equals(bound.@ref));
                LinearCoordinate SignalIntrensic = PSCoordinates.Find(x => x.id.Equals(intcoord.coordinates[0].@ref)) as LinearCoordinate;



                //get relatedNetElement
                var element = spotLoc.associatedNetElements[0].netElement;
                PositioningNetElement netElement = netElements.Find(x => x.id.Equals(element.@ref));
                aplan.database.NetElement dataBaseElement = null;
                using (var liteDB = db.accessDB())
                {
                    dataBaseElement = (liteDB).GetCollection<aplan.database.NetElement>("NetElements").Find(x => x.uuid == element.@ref).FirstOrDefault();
                }

                if (dataBaseElement!=null)
                {
                    netElement = netElements.Find(x => x.id.Equals(dataBaseElement.uuid));

                    List<IntrinsicCoordinate> elementIntrensics_initial = netElement.associatedPositioning[0].intrinsicCoordinates;
                    LinearCoordinate elementStartKm_initial = PSCoordinates.Find(x => x.id.Equals(elementIntrensics_initial[0].coordinates[0].@ref)) as LinearCoordinate;
                    LinearCoordinate elementEndKm_initial = PSCoordinates.Find(x => x.id.Equals(elementIntrensics_initial[1].coordinates[0].@ref)) as LinearCoordinate;


                    // if the km value is not between the the NetElement range.
                    if ((double)intcoord.value > 1 || SignalIntrensic.measure.value < elementStartKm_initial.measure.value)
                    {
                        getNeighbors(netElement, eulynxModel, SignalIntrensic);
                        if (neededElement != null)
                        {
                            netElement = neededElement;
                        }
                    }

                    Unit elementLenthunit = units.Find(x => x.id.Equals(((LinearElementWithLength)netElement).elementLength.quantiy[0].unit.@ref));
                    List<IntrinsicCoordinate> elementIntrensics = netElement.associatedPositioning[0].intrinsicCoordinates;
                    LinearCoordinate elementStartKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[0].coordinates[0].@ref)) as LinearCoordinate;
                    LinearCoordinate elementEndKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[1].coordinates[0].@ref)) as LinearCoordinate;
                    List<System.Windows.Point> points = getNetElementCartesianCoordinates(netElement, PSCoordinates);
                    var newSignal = new Signalinfo()
                    {
                        SignalImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Resources/SymbolsImages/BalisengruppeGesteuertTri.png", UriKind.RelativeOrAbsolute)),
                        LongName = signal.longname,
                        Name = signal.name,
                        Function = euBaliseFunction,
                        IntrinsicValue = tpsDeviceKmValue,
                        AttachedToElementname = netElement.name,
                        Side = assElement.isLocatedToSide.ToString(),
                        Direction = assElement.appliesInDirection.ToString(),
                        Coordinates = points,
                        AttachedToElementLength = ((Length)((LinearElementWithLength)netElement).elementLength.quantiy[0]).value,
                        LateralDistance = 3.1

                    };
                    calculateSignalLocation(newSignal, SignalIntrensic, elementStartKm, elementEndKm);
                    Signals.Add(newSignal);
                }
                
            }
        }

        /// <summary>
        /// Extract the main signals from the Eulynx object.
        /// </summary>
        /// <param name="EulynxObject"></param>
        public  void extractMainSignals(EulynxDataPrepInterface eulynxModel, ObservableCollection<Signalinfo> Signals)
        {
            List<Unit> units = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesUnit;
            List<Models.TopoModels.EULYNX.sig.SignalFrame> signalFrames = eulynxModel.hasDataContainer.First().ownsDataPrepEntities.ownsSignalFrame;
            List<PositioningSystemCoordinate> PSCoordinates = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate;
            List<PositioningNetElement> netElements = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTrackTopology.usesNetElement;
            List<BaseLocation> locations = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesLocation;
            List<IntrinsicCoordinate> intrCoordinates = eulynxModel.hasDataContainer.First().ownsRsmEntities.usesTopography.usesIntrinsicCoordinate;
            List<Models.TopoModels.EULYNX.rsmSig.Signal> mainSignals = eulynxModel.hasDataContainer.First().ownsRsmEntities.ownsSignal;

            foreach (Models.TopoModels.EULYNX.rsmSig.Signal signal in mainSignals)
            {

                tElementWithIDref loc = signal.locations[0];
                Models.TopoModels.EULYNX.rsmCommon.SpotLocation spotLoc = (Models.TopoModels.EULYNX.rsmCommon.SpotLocation)locations.Find(x => x.id.Equals(loc.@ref));
                AssociatedNetElement assElement = spotLoc.associatedNetElements[0];
                tElementWithIDref bound = assElement.bounds[0];
                IntrinsicCoordinate intcoord = intrCoordinates.Find(x => x.id.Equals(bound.@ref));
                LinearCoordinate SignalIntrensic = PSCoordinates.Find(x => x.id.Equals(intcoord.coordinates[0].@ref)) as LinearCoordinate;
                PositioningNetElement element = netElements.Find(x => x.id.Equals(assElement.netElement.@ref));
                if ((double)intcoord.value > 1)
                {
                    getNeighbors(element, eulynxModel, SignalIntrensic);
                    if (neededElement != null)
                    {
                        element = neededElement;
                    }
                }

                Unit elementLenthunit = units.Find(x => x.id.Equals(((LinearElementWithLength)element).elementLength.quantiy[0].unit.@ref));
                List<IntrinsicCoordinate> elementIntrensics = element.associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate elementStartKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[0].coordinates[0].@ref)) as LinearCoordinate;
                LinearCoordinate elementEndKm = PSCoordinates.Find(x => x.id.Equals(elementIntrensics[1].coordinates[0].@ref)) as LinearCoordinate;
                List<System.Windows.Point> points = getNetElementCartesianCoordinates(element, PSCoordinates);


                //signal type.
                var typeFunction = extractSignalTypeAndFunction(signal, eulynxModel);

                var newSignal = new Signalinfo()
                {
                    SignalImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/Resources/SymbolsImages/mehraZugoderStellwerksbedient.png", UriKind.RelativeOrAbsolute)),
                    LongName = signal.longname,
                    Name = signal.name,
                    Type = typeFunction[0],
                    Function = typeFunction[1],
                    AttachedToElementname = element.name,
                    IntrinsicValue = (double)SignalIntrensic.measure.value,
                    Side = assElement.isLocatedToSide.ToString(),
                    Direction = assElement.appliesInDirection.ToString(),
                    Coordinates = points,
                    AttachedToElementLength = ((Length)((LinearElementWithLength)element).elementLength.quantiy[0]).value,
                    LateralDistance = 3.1
                };
                calculateSignalLocation(newSignal, SignalIntrensic, elementStartKm, elementEndKm);
                Signals.Add(newSignal);
                //extract lateral distance
                //foreach (SignalFrame s in signalFrames)
                //{

                //}
            }

        }

        /// <summary>
        /// get neighboring elements according to relations between the elements.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="EulynxObject"></param>
        /// <param name="SignalIntrensic"></param>
        private void getNeighbors(PositioningNetElement element, EulynxDataPrepInterface EulynxObject, LinearCoordinate SignalIntrensic)
        {
            VisitedElements.Add(element);
            PositioningNetElement hostElement = null;
            List<PositionedRelation> relations = EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesPositionedRelation;
            List<PositionedRelation> targetRelations = relations.FindAll(x => ((x.elementA.@ref == element.id) || (x.elementB.@ref == element.id)));
            List<PositioningNetElement> relatedElements = new List<PositioningNetElement>();
            foreach (PositionedRelation relation in targetRelations)
            {
                PositioningNetElement elementA = EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesNetElement.Find(x => x.id.Equals(relation.elementA.@ref));
                PositioningNetElement elementB = EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTrackTopology.usesNetElement.Find(x => x.id.Equals(relation.elementB.@ref));

                if (!relatedElements.Contains(elementA) && !VisitedElements.Contains(elementA))
                {
                    relatedElements.Add(elementA);
                }
                if (!relatedElements.Contains(elementB) && !VisitedElements.Contains(elementB))
                {
                    relatedElements.Add(elementB);
                }
            }
            for (int i = 0; i < relatedElements.Count; i++)
            {
                List<IntrinsicCoordinate> intCoordiante = relatedElements[i].associatedPositioning[0].intrinsicCoordinates;
                LinearCoordinate startKm = (LinearCoordinate)EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.Find(pos => pos.id.Equals(intCoordiante[0].coordinates[0].@ref));
                LinearCoordinate endKm = (LinearCoordinate)EulynxObject.hasDataContainer[0].ownsRsmEntities.usesTopography.usesPositioningSystemCoordinate.Find(pos => pos.id.Equals(intCoordiante[1].coordinates[0].@ref));
                if (startKm.measure.value != null)
                {
                    if (startKm.measure.value <= SignalIntrensic.measure.value && endKm.measure.value >= SignalIntrensic.measure.value)
                    {
                        neededElement = relatedElements[i];
                        hostElement = relatedElements[i];
                        break;
                    }
                }
                else
                {
                    relatedElements.Remove(relatedElements[i]);
                    i--;
                }
            }

            if (hostElement == null)
            {
                foreach (PositioningNetElement elem in relatedElements)

                {
                    getNeighbors(elem, EulynxObject, SignalIntrensic);
                }
            }
        }
        private  List<System.Windows.Point> getNetElementCartesianCoordinates(PositioningNetElement positioningNetElements, List<PositioningSystemCoordinate> PSCoordinates)
        {
            List<System.Windows.Point> points = new List<System.Windows.Point>();
            AssociatedPositioning associatedPositionings = positioningNetElements.associatedPositioning[1];
            List<IntrinsicCoordinate> intrinsicCoordinates = associatedPositionings.intrinsicCoordinates;
            foreach (IntrinsicCoordinate intrinsicCoordinate in intrinsicCoordinates)
            {
                List<tElementWithIDref> tElementWithIDrefs = intrinsicCoordinate.coordinates;
                foreach (tElementWithIDref tElementWithIDref in tElementWithIDrefs)
                {
                    CartesianCoordinate cartCoordinate = (CartesianCoordinate)PSCoordinates.Find(x => x.id.Equals(tElementWithIDref.@ref));
                    System.Windows.Point newPoint = new System.Windows.Point((((double)cartCoordinate.x)), ((double)cartCoordinate.y));
                    points.Add(newPoint);
                }
            }
            return points;
        }
        private  void calculateSignalLocation(Signalinfo signal, LinearCoordinate signalIntrensic, LinearCoordinate elementStart, LinearCoordinate elementEnd)
        {

            double? targetValue = (signalIntrensic.measure.value - elementStart.measure.value) * 1000 / signal.AttachedToElementLength;

            double? targetLengthLocation = targetValue * signal.AttachedToElementLength;

            double currentLength = 0;
            for (int i = 0; i < signal.Coordinates.Count - 1; i++)
            {
                currentLength += Math.Sqrt(Math.Pow((signal.Coordinates[i].X - signal.Coordinates[i + 1].X), 2.0) + Math.Pow((signal.Coordinates[i].Y - signal.Coordinates[i + 1].Y), 2.0));
                if (currentLength > targetLengthLocation)
                {
                    double? factor = targetLengthLocation / currentLength;
                    double xdiff = signal.Coordinates[i + 1].X - signal.Coordinates[i].X;
                    double ydiff = signal.Coordinates[i + 1].Y - signal.Coordinates[i].Y;
                    applyRotation(signal, xdiff, ydiff);
                    signal.LocationCoordinate = new System.Windows.Point((double)(signal.Coordinates[i].X + xdiff * factor), (double)(signal.Coordinates[i].Y + ydiff * factor));
                    applyHorizontalOffset(signal, signal.Coordinates[i + 1]);
                    applyDirection(signal);
                    break;
                }
            }
            if (signal.LocationCoordinate.X == 0)
            {
                signal.LocationCoordinate = new System.Windows.Point((double)(signal.Coordinates[signal.Coordinates.Count - 1].X) - DrawViewModel.GlobalDrawingPoint.X, (double)(signal.Coordinates[signal.Coordinates.Count - 1].Y) - DrawViewModel.GlobalDrawingPoint.Y);
            }
        }
        /// <summary>
        /// apply rotation on signal
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="xdiff"></param>
        /// <param name="ydiff"></param>
        private  void applyRotation(Signalinfo signal, double xdiff, double ydiff)
        {
            //negative sign for drawing
            if (ydiff >= 0 && xdiff >= 0)
            {
                signal.Orientation = -Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI);
            }
            else if (ydiff < 0 && xdiff < 0)
            {
                signal.Orientation = -Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI) - 180;
            }
            else if (ydiff > 0 && xdiff < 0)
            {
                signal.Orientation = -(180 - Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI));
            }
            else if (ydiff < 0 && xdiff > 0)
            {
                signal.Orientation = Math.Atan((double)(Math.Abs(ydiff) / Math.Abs(xdiff))) * (180 / Math.PI);
            }

        }
        /// <summary>
        /// apply the horizontal offset required for a signal.
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="backward"></param>
        private  void applyHorizontalOffset(Signalinfo signal, System.Windows.Point backward)
        {
            var x = signal.LocationCoordinate.X;
            var y = signal.LocationCoordinate.Y;
            var shift = 10;

            var shiftx = 0.0;
            var shifty = 0.0;
            if (signal.Side == "right")
            {
                if (Math.Abs(signal.Orientation) <= 90)
                {
                    shiftx = shift * Math.Sin(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                    shifty = -shift * Math.Cos(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 180 && Math.Abs(signal.Orientation) > 90)
                {
                    shiftx = shift * Math.Cos((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                    shifty = shift * Math.Sin((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 270 && Math.Abs(signal.Orientation) > 180)
                {

                    shiftx = -shift * Math.Sin((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                    shifty = shift * Math.Cos((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 360 && Math.Abs(signal.Orientation) > 270)
                {
                    shiftx = -shift * Math.Cos((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                    shifty = -shift * Math.Sin((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                }

            }
            else if (signal.Side == "left")
            {
                if (Math.Abs(signal.Orientation) <= 90)
                {
                    shiftx = -shift * Math.Sin(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                    shifty = shift * Math.Cos(Math.Abs(signal.Orientation) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 180 && Math.Abs(signal.Orientation) > 90)
                {
                    shiftx = -shift * Math.Cos((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                    shifty = -shift * Math.Sin((Math.Abs(signal.Orientation) - 90) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 270 && Math.Abs(signal.Orientation) > 180)
                {

                    shiftx = shift * Math.Sin((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                    shifty = -shift * Math.Cos((Math.Abs(signal.Orientation) - 180) * (Math.PI) / 180);
                }
                else if (Math.Abs(signal.Orientation) <= 360 && Math.Abs(signal.Orientation) > 270)
                {
                    shiftx = shift * Math.Cos((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                    shifty = shift * Math.Sin((Math.Abs(signal.Orientation) - 270) * (Math.PI) / 180);
                }

            }
            x += shiftx;
            y += shifty;

            signal.LocationCoordinate = new System.Windows.Point(x, y);
        }
        /// <summary>
        /// apply the direction to the signal
        /// </summary>
        /// <param name="signal"></param>
        private  void applyDirection(Signalinfo signal)
        {
            if (signal.Direction.Equals("2") || signal.Direction.Equals("reverse"))
            {
                signal.Orientation = signal.Orientation - 180;
            }
        }
        private  string[] extractSignalTypeAndFunction(Models.TopoModels.EULYNX.rsmSig.Signal signal, EulynxDataPrepInterface EulynxObject)
        {
            string type = "notFound";
            string function = "notFound";

            var ownsSignal = EulynxObject.hasDataContainer[0].ownsRsmEntities.ownsSignal;
            var ownsSignalType = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsSignalType;
            var ownsSignalFunction = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsSignalFunction;
            var ownsTrackAsset = EulynxObject.hasDataContainer[0].ownsDataPrepEntities.ownsTrackAsset;

            LightSignalTyped trackAsset = null;
            foreach (Models.TopoModels.EULYNX.db.SignalType signalType in ownsSignalType)
            {
                trackAsset = ownsTrackAsset.Find(x => x.id.Equals(signalType.appliesToSignal.@ref)) as LightSignalTyped;
                Models.TopoModels.EULYNX.rsmSig.Signal current_signal = ownsSignal.Find(x => x.id.Equals(trackAsset.refersToRsmSignal.@ref));
                if (current_signal.id == signal.id)
                {
                    foreach (Models.TopoModels.EULYNX.db.SignalFunction signalFunction in ownsSignalFunction)
                    {
                        if (signalFunction.appliesToSignal.@ref.Equals(signalType.appliesToSignal.@ref))
                        {
                            function = signalFunction.isOfSignalFunctionType?.ToString();
                        }
                    }
                    type = signalType.isOfSignalTypeType?.ToString();
                }
            }

            return new string[2] { type, function };
        }
        #endregion

        #region model creation
        public async Task<EulynxDataPrepInterface> createJSONproject(string Json, string Format, string ProjectName, string ProjectPath)
        {
            EulynxDataPrepInterface eulynxModel = await CreateJSONeulyxObject(Json,Format,ProjectName,ProjectPath);
            return eulynxModel;
        }
        public async Task<EulynxDataPrepInterface> createMDBproject(string MDB, string Format, string ProjectName, string ProjectPath)
        {
            EulynxDataPrepInterface eulynxModel = await CreateMDBeulyxObject(MDB, Format, ProjectName, ProjectPath);
            return eulynxModel;
        }
        /// <summary>
        /// create a Eulynx object from JSON files async.
        /// </summary>
        /// <returns></returns>
        private async Task<EulynxDataPrepInterface> CreateJSONeulyxObject(string Json,string Format,string ProjectName,string ProjectPath)
        {
            EulynxDataPrepInterface eulynxModel = null;
            await Task.Run(() =>
            {
                var files = HelperFunctions.getFileNamesFromString(Json);
                var entwurfselement_KM = files.Find(x => x.Contains("Entwurfselement_KM"));
                var entwurfselement_LA = files.Find(x => x.Contains("Entwurfselement_LA"));
                var entwurfselement_HO = files.Find(x => x.Contains("Entwurfselement_HO"));
                var entwurfselement_UH = files.Find(x => x.Contains("Entwurfselement_UH"));
                var gleiskanten = files.Find(x => x.Contains("Gleiskanten"));
                var gleisknoten = files.Find(x => x.Contains("Gleisknoten"));

                //try
                //{
                 eulynxModel = createEulynxModel(
                          "de",
                          Format,
                          entwurfselement_KM,
                          gleiskanten,
                          gleisknoten,
                          entwurfselement_LA,
                          entwurfselement_HO,
                          entwurfselement_UH,
                          null,
                          $"{ProjectPath}/{ProjectName}");
                //} catch(Exception e) { System.Windows.MessageBox.Show(e.Message); };

            });
            return eulynxModel;
        }


        /// <summary>
        /// create a Eulynx object from xls files async.
        /// </summary>
        /// <returns></returns>
        public string CreateJSONFilesFromXLS(string xml, string ProjectName, string ProjectPath)
        {
            
                var files = HelperFunctions.getFileNamesFromString(xml);
                var edge = files.Find(x => x.Contains("Edges"));
                var node = files.Find(x => x.Contains("Nodes"));
                var gradient = files.Find(x => x.Contains("Gradients"));
                var segment = files.Find(x => x.Contains("Segments"));
                    
                BuildJson buildJson = new BuildJson(edge, node, gradient, segment);

                string jsonFiles = "";
                jsonFiles += buildJson.CreateKanten(ProjectPath + "\\" + ProjectName + "\\Gleiskanten.geojson") + "+~+";
                jsonFiles += buildJson.CreateKnoten(ProjectPath + "\\" + ProjectName + "\\Gleisknoten.geojson") + "+~+";
                jsonFiles += buildJson.CreateKMLine(ProjectPath + "\\" + ProjectName + "\\Entwurfselement_KM.geojson") + "+~+";
                jsonFiles += buildJson.CreateHohe(ProjectPath + "\\" + ProjectName + "\\Entwurfselement_HO.geojson") + "+~+";
                jsonFiles += buildJson.CreaeteLage(ProjectPath + "\\" + ProjectName + "\\Entwurfselement_LA.geojson") + "+~+";
                jsonFiles += buildJson.CreaeteUH(ProjectPath + "\\" + ProjectName + "\\Entwurfselement_UH.geojson") + "+~+"; 

                return jsonFiles;
        }


        /// <summary>
        /// create a Eulynx object from MDB file async.
        /// </summary>
        /// <returns></returns>
        private async Task<EulynxDataPrepInterface> CreateMDBeulyxObject(string MDB, string Format, string ProjectName, string ProjectPath)
        {
            EulynxDataPrepInterface eulynxModel = null;
            await Task.Run(() =>
            {
                eulynxModel = createEulynxModel(
                "de",
                Format,
                null,
                null,
                null,
                null,
                null,
                null,
                MDB,
                $"{ProjectPath}/{ProjectName}");
            });
            return eulynxModel;
        }
        /// <summary>
        /// draw the created Eulynx object async.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DrawEulyxObject(ObservableCollection<CustomPolyLine> Lines,ObservableCollection<CustomCircle> Ellipses, ObservableCollection<Signalinfo> Signals)
        {
            await drawObject(ViewModels.DrawViewModel.sharedCanvasSize, BaseViewModel.eulynxModel, Lines, Ellipses, Signals);
            return true;
        }
        #endregion

        #region serialize deserialize
        /// <summary>
        /// load .euxml file representing the saved Eulynx model.
        /// </summary>
        /// <param name="f"></param>
        public async Task<EulynxDataPrepInterface> loadEuxml(string f, ObservableCollection<CustomPolyLine> PolyLines, ObservableCollection<CustomCircle> Circles, ObservableCollection<Signalinfo> Signals)
        {
            EulynxDataPrepInterface eulynxModel= null;
            EulynxValidator eulynxValidator = new();
            var ValidatorViewModel = System.Windows.Application.Current.FindResource("validatorViewModel") as ValidatorViewModel;
            string report = await eulynxValidator.validate(f,"");
            if (report.Contains("Validation is Successful"))
            {
                eulynxModel = await deserializeEuxml(f);

                await drawObject(ViewModels.DrawViewModel.sharedCanvasSize, eulynxModel, PolyLines, Circles, Signals);
            }
            else
            {
                if (System.Windows.MessageBox.Show("Euxml file is Invalid, Show details?",
                    "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ValidatorViewModel validatorViewModel = System.Windows.Application.Current.FindResource("EulynxValidatorViewModel") as ValidatorViewModel;
                    validatorViewModel.Report = report;
                    Validator validatorWindow = new();
                    validatorWindow.ShowDialog();
                }
                else
                {
                    // close the window 
                }
            }

            return eulynxModel;
        }
        /// <summary>
        /// create a Eulynx object from a .euxml file async.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<EulynxDataPrepInterface> deserializeEuxml(string file)
        {
            EulynxDataPrepInterface eulynxModel=null;
            await Task.Run(() =>
            {
                var eulynxService = EulynxService.getInstance();
                eulynxModel =  eulynxService.deserialization(file);
            });
            return eulynxModel;
        }
        #endregion
    }
}
