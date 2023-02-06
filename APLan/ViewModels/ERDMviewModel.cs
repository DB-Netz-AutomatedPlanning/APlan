using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using APLan.Commands;
using APLan.HelperClasses;
using ERDM_Implementation;
using java.lang.reflect;
using RCA_Model.Tier_0;
using RCA_Model.Tier_1;
using RCA_Model.Tier_2;
using RCA_Model.Tier_3;
using Point = System.Windows.Point;

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

            BaseViewModel.ERDMmodel = ERDMcreator.createModel(SegmentsFilePath,GradientsFilePath,NodesFilePath,EdgesFilePath);


            drawERDM(ERDMmodel);

            var baseViewModel = System.Windows.Application.Current.FindResource("baseViewModel") as BaseViewModel;
            baseViewModel.WelcomeVisibility = Visibility.Collapsed;
            ((Window)parameter).Close();
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
        private void drawERDM(MapData mapData)
        {
            drawSegments(mapData); //Segments
            drawNodes(mapData); //Nodes
        }
        //add the segments to the LA list and the corresponding points.
        private void drawSegments(MapData mapData)
        {
            var segments = mapData.consistsOfTier3Objects.FindAll(x => (x is CurveSegmentArc || x is CurveSegmentLine || x is CurveSegmentTransition));
            foreach (CurveSegment segment in segments)
            {
                CustomPolyLine polyLine = new();
                TrackEdgeSection Section = mapData.consistsOfTier2Objects.Find(x => (x is TrackEdgeSection) && segment.appliesToTrackEdgeSection.Contains(x.id)) as TrackEdgeSection;
                var trackEdgePoints = mapData.consistsOfTier2Objects.FindAll(x => (x is TrackEdgePoint) && Section.hasStartTrackEdgePoint.Equals(x.id) || Section.hasEndTrackEdgePoint.Equals(x.id));
                var geoCoordinates = mapData.consistsOfTier0Objects.FindAll(x => (x is GeoCoordinates) && (x.id.Equals((trackEdgePoints[0] as TrackEdgePoint).isLocatedAtGeoCoordinates) || x.id.Equals((trackEdgePoints[1] as TrackEdgePoint).isLocatedAtGeoCoordinates)));

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
        //draw nodes of the ERDM model.
        private void drawNodes(MapData mapData)
        {
            var nodes = mapData.consistsOfTier1Objects.FindAll(x=>(x is TrackNode));
            var geoID= nodes.Select(x=>((TrackNode)x).isLocatedAtGeoCoordinates);
            var geoCoordinates = mapData.consistsOfTier0Objects.FindAll(x => geoID.Contains(x.id));
            geoCoordinates.ForEach(x => {
                gleisknotenList.Add(
                    new(){
                        NodePoint = new System.Windows.Point((double)((x as GeoCoordinates).xCoordinate),(double)((x as GeoCoordinates).yCoordinate)),
                        Color = System.Windows.Media.Brushes.Red
                     });
            });
        }
        #endregion
    }
}
