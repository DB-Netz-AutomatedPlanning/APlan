using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APLan.HelperClasses
{
    public class CustomCircle : CustomItem
    {

        private string name;
        private SolidColorBrush color;
        private double radius;
        private double thickness;
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

        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                OnPropertyChanged();
            }
        }

        public double Thickness
        {
            get => radius;
            set
            {
                thickness = value;
                OnPropertyChanged();
            }
        }
        public Point EllipseVertexCenter
        {
            get;
            set;
        }
        public CustomCircle()
        {
            EllipseVertexCenter = new();
            Thickness = new();
            Radius = new();
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
        }
    }
}
