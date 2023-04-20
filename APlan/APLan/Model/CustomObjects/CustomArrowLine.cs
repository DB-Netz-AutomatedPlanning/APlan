using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using APLan.HelperClasses;
using APLan.Views;

namespace APLan.Model.CustomObjects
{
    public  class CustomArrowLine : CustomItem
    {
        private string name;
        private SolidColorBrush color;        
        private double width;
        private double height;
        private Point setCanvasPoint;
        private double distanceValue;
        
        private Thickness thickness;
        private Geometry geometry;
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

       public Geometry GeometryData
        {
            get => geometry;
            set
            {
                geometry = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }
        public double Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }
        public double DistanceValue
        {
            get => distanceValue;
            set
            {
                distanceValue = value;
                OnPropertyChanged();
            }
        }
        public Point SetCanvasPoint
        {
            get => setCanvasPoint;
            set
            {
                setCanvasPoint = value;
                OnPropertyChanged();
            }
        }
         
        public Thickness ThicknessAttrib
        {
            get => thickness;
            set
            {
                thickness = value;
                OnPropertyChanged();
            }
        }
        public CustomArrowLine()
        {            
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
        }
    }
}
