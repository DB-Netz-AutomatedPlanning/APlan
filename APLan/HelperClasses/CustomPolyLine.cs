using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APLan.HelperClasses
{
    /// <summary>
    /// object to contain all information about the polylines.
    /// </summary>
    public class CustomPolyLine  : CustomItem
    {
        #region attributes
        // name as in the Eulynx Object.
        private string name;
        private SolidColorBrush color;
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
        //global point represent first point found when drawing all the drawing.
        public Point GlobalPoint
        {
            get;
            set;
        }
        #endregion
        
        #region constructor
        public CustomPolyLine()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
            Points = new();
        }
        #endregion

    }
}
