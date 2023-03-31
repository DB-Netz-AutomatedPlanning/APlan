using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using APLan.HelperClasses;

namespace APLan.Model.CustomObjects
{
    public class CustomRectangle : CustomItem
    {
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

        public PointCollection Points
        {
            get;
            set;
        }

        public Point GlobalPoint
        {
            get;
            set;
        }

        public CustomRectangle()
        {
            Data = new ObservableCollection<KeyValue>();
            Points = new();
        }

    }
}
