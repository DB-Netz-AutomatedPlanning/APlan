using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace APLan.HelperClasses
{
    public class CustomArc : CustomItem
    {
        private string name;
        private SolidColorBrush color;
        private double radius;
        private Point startPoint;
        private Point endPoint;
        private bool isLargeArc;
        private SweepDirection sweepDirection;
        private Point center;
        private double thickness;
        private Size size;
        private netDxf.Vector3 vector3;
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

        public Point StartPoint
        {
            get => startPoint;
            set
            {
                startPoint = value;
                OnPropertyChanged();
            }
        }

        public Point EndPoint
        {
            get => endPoint;
            set
            {
                endPoint = value;
                OnPropertyChanged();
            }
        }

        public bool IsLargeArc
        {
            get => isLargeArc;
            set
            {
                isLargeArc = value;
                OnPropertyChanged();
            }
        }

        public SweepDirection SweepDirection
        {
            get => sweepDirection;
            set
            {
                sweepDirection = value;
                OnPropertyChanged();
            }
        }
        public Point Center
        {
            get => center;
            set
            {
                center = value;
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

        public Size Size
        {
            get => size;
            set
            {
                size = value;
                OnPropertyChanged();
            }
        }
        public netDxf.Vector3 Normal
        {
            get => vector3;
            set
            {
                vector3 = value;
                OnPropertyChanged();
            }
        }

        public CustomArc()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
            StartPoint = new();
            EndPoint = new();
            Radius = new();
            SweepDirection = new();
            IsLargeArc = new();
            center = new();
            Size = new();
            Thickness = new();
            Normal = new();

        }

}
}
