using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APLan.HelperClasses
{
    public class CustomEllipse : CustomItem
    {
        private string name;
        private SolidColorBrush color;
         
        private double thickness;
        private double radiusX;
        private double radiusY;
        private Point ellipseCenter;
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

         

        public double Thickness
        {
            get => thickness;
            set
            {
                thickness = value;
                OnPropertyChanged();
            }
        }
        public Point EllipseVertexCenter
        {
            get => ellipseCenter;
            set
            {
                ellipseCenter = value;
                OnPropertyChanged();
            }
        }

        public double RadiusX
        {
            get => radiusX;
            set
            {
                radiusX = value;
                OnPropertyChanged();
            }
        }

        public double RadiusY
        {
            get => radiusY;
            set
            {
                radiusY = value;
                OnPropertyChanged();
            }
        }
        public CustomEllipse()
        {
            EllipseVertexCenter = new();
            Thickness = new();
            RadiusX = new();
            RadiusY = new();
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
        }
    }
}
