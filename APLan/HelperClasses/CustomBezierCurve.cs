using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APLan.HelperClasses
{
    public  class CustomBezierCurve : CustomItem
    {
        private string name;
        private SolidColorBrush color;
         
        private Point startPoint;
        private Point endPoint;
        private Point anyPoint;
         
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

        public Point AnyPoint
        {
            get => anyPoint;
            set
            {
                anyPoint = value;
                OnPropertyChanged();
            }
        }  

        public CustomBezierCurve()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
            StartPoint = new();
            EndPoint = new();
            AnyPoint = new(); 

        }
    }
}
