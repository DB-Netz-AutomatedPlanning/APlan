using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using APLan.Commands;
using APLan.HelperClasses;
using ERDM_Implementation;
using java.lang.reflect;
using ERDM.Tier_0;
using ERDM.Tier_1;
using ERDM.Tier_2;
using ERDM.Tier_3;
using Point = System.Windows.Point;
using System.Collections.Immutable;
using java.util;

namespace APLan.ViewModels
{
    public class ERDMviewModel : BaseViewModel
    {
        #region attributes
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;
        private string projectName;
        private string projectDirectoryPath;
        private string segmentsFilePath;
        private string gradientsFilePath;
        private string nodesFilePath;
        private string edgesFilePath;

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged();
            }
        }
        public  string ProjectDirectoryPath
        {
            get { return projectDirectoryPath; }
            set
            {
                projectDirectoryPath = value;
                OnPropertyChanged();
            }
        }
        public  string SegmentsFilePath
        {
            get { return segmentsFilePath; }
            set
            {
                segmentsFilePath = value;
                OnPropertyChanged();
            }
        }
        public  string GradientsFilePath
        {
            get { return gradientsFilePath; }
            set
            {
                gradientsFilePath = value;
                OnPropertyChanged();
            }
        }
        public  string NodesFilePath
        {
            get { return nodesFilePath; }
            set
            {
                nodesFilePath = value;
                OnPropertyChanged();
            }
        }
        public  string EdgesFilePath
        {
            get { return edgesFilePath; }
            set
            {
                edgesFilePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand AddProjectDirectoryPath { get; set; }
        public ICommand AddSegmentsFile { get; set; }
        public ICommand AddGradientsFile { get; set; }
        public ICommand AddNodesFile { get; set; }
        public ICommand AddEdgesFile { get; set; }
        public ICommand Create { get; set; }
        public ICommand Cancel { get; set; }
        #endregion

        #region constructor
        public ERDMviewModel()
        {
            //comands functions
            AddProjectDirectoryPath = new RelayCommand(ExecuteAddProjectDirectoryPath);
            AddSegmentsFile = new RelayCommand(ExecuteAddSegmentsFile);
            AddGradientsFile = new RelayCommand(ExecuteAddGradientsFile);
            AddNodesFile = new RelayCommand(ExecuteAddNodesFile);
            AddEdgesFile = new RelayCommand(ExecuteAddEdgesFile);
            Create = new RelayCommand(ExecuteCreate);
            Cancel = new RelayCommand(ExecuteCancel);

            //folder and file browsers
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = true;
            openFileDialog1 = new OpenFileDialog();
        }
        #endregion

        #region logic
        private void ExecuteAddProjectDirectoryPath(object parameter)
        {
            folderBrowserDialog1.ShowDialog();
            ProjectDirectoryPath = folderBrowserDialog1.SelectedPath;
        }
        private void ExecuteAddSegmentsFile(object parameter)
        {
            openFileDialog1.Filter = "Types (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            SegmentsFilePath = openFileDialog1.FileName;
        }
        private void ExecuteAddGradientsFile(object parameter)
        {
            openFileDialog1.Filter = "Types (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            GradientsFilePath = openFileDialog1.FileName;
        }
        private void ExecuteAddNodesFile(object parameter)
        {
            openFileDialog1.Filter = "Types (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            NodesFilePath = openFileDialog1.FileName;
        }
        private void ExecuteAddEdgesFile(object parameter)
        {
            openFileDialog1.Filter = "Types (*.xls)|*.xls";
            openFileDialog1.ShowDialog();
            EdgesFilePath = openFileDialog1.FileName;
        }
        private void ExecuteCreate(object parameter)
        {
            //clear previous project drawings.
            clearDrawings();
            //put any other models to null.
            APLan.ViewModels.DrawViewModel.model = null;

            ERDMobjectsCreator ERDMcreator = new();

            BaseViewModel.erdmModel = ERDMcreator.createModel(SegmentsFilePath,GradientsFilePath,NodesFilePath,EdgesFilePath);

            if (erdmModel != null) { 
                drawERDM(erdmModel);
                var baseViewModel = System.Windows.Application.Current.FindResource("baseViewModel") as BaseViewModel;
                baseViewModel.WelcomeVisibility = Visibility.Collapsed;
                ((Window)parameter).Close();
            }
        }
        private void ExecuteCancel(object parameter)
        {
            ((Window)parameter).Close();
        }

        /// <summary>
        /// clear all data drawn previously.
        /// </summary>
        private void clearDrawings()
        {
            //clear old data.
            gleiskantenList.Clear();
            gleiskantenPointsList.Clear();
            Entwurfselement_LA_list.Clear();
            Entwurfselement_LAPointsList.Clear();
            Entwurfselement_KM_list.Clear();
            Entwurfselement_KMPointsList.Clear();
            Entwurfselement_HO_list.Clear();
            Entwurfselement_HOPointsList.Clear();
            Entwurfselement_UH_list.Clear();
            Entwurfselement_UHPointsList.Clear();
            gleisknotenList.Clear();
            Signals.Clear();
            ViewModels.DrawViewModel.GlobalDrawingPoint = new(0,0);
        }

        /// <summary>
        /// draw the ERDM informations.
        /// </summary>
        /// <param name="mapData"></param>
        private void drawERDM(ERDM.ERDMmodel erdmModel)
        {
            var allMapData=erdmModel.Tier0.MapData.FindAll(t => t is MapData).ToList(); // get all MapData.
            allMapData.ForEach(mapData => drawMapData((MapData)mapData, erdmModel)); // draw each one.
        }
        /// <summary>
        /// draw a MapData informations.
        /// </summary>
        private void drawMapData(MapData mapData, ERDM.ERDMmodel erdmModel)
        {
           drawSegments(mapData,erdmModel); //Segments
           drawNodes(mapData,erdmModel); //Nodes
        }
        /// <summary>
        /// add the segments to the LA list and the corresponding points.
        /// </summary>
        /// <param name="erdmModel"></param>
        private void drawSegments(MapData mapData,ERDM.ERDMmodel erdmModel)
        {
            var segments = getAllSegmentsOfMapData(mapData, erdmModel);
            foreach (CurveSegment segment in segments)
            {
                CustomPolyLine polyLine = new();
                TrackEdgeSection Section = erdmModel.Tier2.TrackEdgeSection.Find(x => (x is TrackEdgeSection) && segment.appliesToTrackEdgeSection.Contains(x.id)) as TrackEdgeSection;
                var trackEdgePoints = erdmModel.Tier2.TrackEdgePoint.FindAll(x => (x is TrackEdgePoint) && Section.hasStartTrackEdgePoint.Equals(x.id) || Section.hasEndTrackEdgePoint.Equals(x.id));
                var geoCoordinates = erdmModel.Tier0.GeoCoordinates.FindAll(x => (x is GeoCoordinates) && (x.id.Equals((trackEdgePoints[0] as TrackEdgePoint).isLocatedAtGeoCoordinates) || x.id.Equals((trackEdgePoints[1] as TrackEdgePoint).isLocatedAtGeoCoordinates)));

                var point1 = new Point((double)((GeoCoordinates)geoCoordinates[0]).xCoordinate,(double)((GeoCoordinates)geoCoordinates[0]).yCoordinate);
                var point2 = new Point((double)((GeoCoordinates)geoCoordinates[1]).xCoordinate, (double)((GeoCoordinates)geoCoordinates[1]).yCoordinate);

                if (ViewModels.DrawViewModel.GlobalDrawingPoint.X == 0)
                    ViewModels.DrawViewModel.GlobalDrawingPoint = point1;


                polyLine.Name = Section.name;
                polyLine.Points.Add(point1);
                polyLine.Points.Add(point2);
                polyLine.Color = System.Windows.Media.Brushes.Red;

                Entwurfselement_LAPointsList.Add(point1);
                Entwurfselement_LAPointsList.Add(point2);
                Entwurfselement_LA_list.Add(polyLine);
            }
        }
        /// <summary>
        /// draw nodes of the ERDM model.
        /// </summary>
        /// <param name="erdmModel"></param>
        private void drawNodes(MapData mapData,ERDM.ERDMmodel erdmModel)
        {
            var nodes = getAllNodesOfMapData(mapData,erdmModel);
            var geoID= nodes.Select(x=>((TrackNode)x).isLocatedAtGeoCoordinates);
            var geoCoordinates = erdmModel.Tier0.GeoCoordinates.FindAll(x => geoID.Contains(x.id));
            geoCoordinates.ForEach(x => {
                gleisknotenList.Add(
                    new(){
                        NodePoint = new System.Windows.Point((double)((x as GeoCoordinates).xCoordinate),(double)((x as GeoCoordinates).yCoordinate)),
                        Color = System.Windows.Media.Brushes.Red
                     });
            });
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

            erdmModel.Tier3.CurveSegmentArc.FindAll(x => mapData.consistsOfTier3Objects.Contains(x.id)).ForEach(x=>curveSegments.Add(x));
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
        #endregion
    }
}
