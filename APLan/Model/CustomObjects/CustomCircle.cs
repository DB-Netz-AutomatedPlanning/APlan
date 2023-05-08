using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using APLan.ViewModels;
using APLan.HelperClasses;
using System.Collections.Generic;

namespace APLan.Model.CustomObjects
{
    public class CustomCircle : CustomItem
    {

        private string name;
        private SolidColorBrush color;
        private double _radiusX;
        private double _radiusY;
        private double thickness;
       

        public string CircleType { get; set; }

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

        public double RadiusX
        {
            get => _radiusX;
            set
            {
                _radiusX = value;
                OnPropertyChanged();
            }
        }

        public double RadiusY
        {
            get => _radiusY;
            set
            {
                _radiusY = value;
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
        public CustomPoint Center
        {
            get;
            set;
        }

        public CustomCircle()
        {
            Center = new();
            Thickness = new();
            RadiusX = new();
            RadiusY = new();
            Data = new ObservableCollection<KeyValue>();
            ShapeAttributeInfo = new ObservableCollection<KeyValue>();

            VisualizedDataViewModel.StaticPropertyChanged += PropertiesChange;
        }

        private void PropertiesChange(object sender, PropertyChangedEventArgs e)
        {
            if (CircleType != null && e.PropertyName.Equals("GleisknotenVisibility") && CircleType.Equals("GleisKnoten"))
            {
                Visibility = VisualizedDataViewModel.GleisknotenVisibility;
            }
        }
    }
}
