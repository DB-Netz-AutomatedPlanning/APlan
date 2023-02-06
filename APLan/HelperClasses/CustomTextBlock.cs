using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace APLan.HelperClasses
{
    public class CustomTextBlock : CustomItem
    {
        #region attributes
        private SolidColorBrush color;
        /// <summary>
        /// Node location after removing the global point (first point to be evaluated in the station)
        /// </summary>
        public Point NodePoint
        {
            get;
            set;
        }
        public double Height
        {
            get;
            set;
        }
        public double Width
        {
            get;
            set;
        }

        public HorizontalAlignment TxtHoriAlignment
        {
            get;
            set;
        }

        public double RotationAngle
        {
            get;
            set;
        }
        /// <summary>
        /// name of the node when hovering over.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        public SolidColorBrush Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region constructor
        public CustomTextBlock()
        {
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();
        }
        #endregion
    }
}
