using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using APLan.ViewModels;
using System;
using System.Collections.Generic;
using APLan.HelperClasses;
using Models.TopoModels.EULYNX.generic;

namespace APLan.Model.CustomObjects
{
    /// <summary>
    /// object to contain all information about the polylines.
    /// </summary>
    /// 
    
    public class CustomPolyLine : CustomItem
    {
        #region attributes
        public string LineType { get; set; }
        // name as in the Eulynx Object.
        private string name;        
        private SolidColorBrush color;
        private List<CustomPoint> customPoints;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        //color is selective
        
        public SolidColorBrush Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }
        //all points used to draw this polyline
        public PointCollection Points
        {
            get;
            set;
        }

        public List<CustomPoint> CustomPoints
        {
            get => customPoints;
            set
            {
                customPoints = value;
                OnPropertyChanged();
            }
        }
        //global point represent first point found when drawing all the drawing.
        public Point GlobalPoint
        {
            get;
            set;
        }

        private double lineThicnkess = 0.5;
        public double LineThicnkess
        {
            get => lineThicnkess;
            set
            {
                lineThicnkess = value;
                OnPropertyChanged();
            }

        }
       
        #endregion

        #region constructor
        public CustomPolyLine()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
            Points = new();
            CustomPoints = new();
            
            VisualizedDataViewModel.StaticPropertyChanged += PropertiesChange;
        }

        private void PropertiesChange(object sender, PropertyChangedEventArgs e)
        {
            if (LineType != null && e.PropertyName.Equals("gleisKantenVisibility") && LineType.Equals(nameof(dataModelsTypes.GleisKanten)))
            {
                Visibility = VisualizedDataViewModel.GleisKantenVisibility;
                return;
            }
            if (LineType != null && e.PropertyName.Equals("Entwurfselement_LA_Visibility") && LineType.Equals(nameof(dataModelsTypes.Entwurfselement_LA)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_LA_Visibility;
                return;
            }
            if (LineType != null && e.PropertyName.Equals("Entwurfselement_KM_Visibility") && LineType.Equals(nameof(dataModelsTypes.Entwurfselement_KM)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_KM_Visibility;
                return;
            }
            if (LineType != null && e.PropertyName.Equals("Entwurfselement_HO_Visibility") && LineType.Equals(nameof(dataModelsTypes.Entwurfselement_HO)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_HO_Visibility;
                return;
            }
            if (LineType != null && e.PropertyName.Equals("Entwurfselement_UH_Visibility") && LineType.Equals(nameof(dataModelsTypes.Entwurfselement_UH)))
            {
                Visibility = VisualizedDataViewModel.Entwurfselement_UH_Visibility;
                return;
            }
        }

        #endregion

    }
}
