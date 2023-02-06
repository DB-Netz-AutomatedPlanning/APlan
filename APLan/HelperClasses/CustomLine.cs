using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APLan.HelperClasses
{
    public class CustomLine : CustomItem
    {
        #region attributes
        // name as in the Eulynx Object.
        private string name;
        private SolidColorBrush color;
        private Point startPoint;
        private Point endPoint;
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
         
        
        //global point represent first point found when drawing all the drawing.
        public Point StartingPoint
        {
            get => startPoint;
            set
            {
                startPoint = value;
                OnPropertyChanged();
            }
             
        }
        public Point Endpoint
        {
            get => endPoint;
            set
            {
                endPoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region constructor
        public CustomLine()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
            StartingPoint = new();
            Endpoint = new();
             
        }
        #endregion
    }
}
