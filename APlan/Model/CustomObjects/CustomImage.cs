using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using APLan.HelperClasses;

namespace APLan.Model.CustomObjects
{
    public class CustomImage : CustomItem
    {
        private BitmapImage name;
        private double height;
        private double width;
        private double setLeft;
        private double setTop;

        public BitmapImage Name
        {
            get => name;
            set
            {
                name = value;
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
        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }
        public double SetLeft
        {
            get => setLeft;
            set
            {
                setLeft = value;
                OnPropertyChanged();
            }
        }
        public double SetTop
        {
            get => setTop;
            set
            {
                setTop = value;
                OnPropertyChanged();
            }
        }
        public CustomImage()
        {
            Name = new();
            Height = new();
            Width = new();
            SetLeft = new();
            SetTop = new();
            Data = new ObservableCollection<KeyValue>();

        }
    }
}
