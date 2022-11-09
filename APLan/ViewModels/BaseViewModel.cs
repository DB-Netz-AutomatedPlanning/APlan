using APLan.HelperClasses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace APLan.ViewModels
{
    public class BaseViewModel : Loading , INotifyPropertyChanged
    {
        private static string currentProjectNameBind = null;
        private static string currentProjectPathBind = null;
        public string CurrentProjectNameBind
        {
            get { return currentProjectNameBind; }
            set
            {
                currentProjectNameBind = value;
                OnPropertyChanged();
            }
        }
        public string CurrentProjectPathBind
        {
            get { return currentProjectPathBind; }
            set
            {
                currentProjectPathBind = value;
                OnPropertyChanged();
            }
        }
        
        public static ObservableCollection<UIElement> selected
        {
            get;
            set;
        }
        public static ObservableCollection<Signalinfo> Signals
        {
            get;
            set;
        }
        public static ObservableCollection<CanvasObjectInformation> loadedObjects
        {
            get;
            set;
        }
        
        public static ObservableCollection<CustomPolyLine> gleiskantenList
        {
            get;
            set;
        }
        public static ObservableCollection<CustomPolyLine> Entwurfselement_LA_list
        {
            get;
            set;
        }
        public static ObservableCollection<CustomPolyLine> Entwurfselement_KM_list
        {
            get;
            set;
        }
        public static ObservableCollection<CustomPolyLine> Entwurfselement_HO_list
        {
            get;
            set;
        }
        public static ObservableCollection<CustomPolyLine> Entwurfselement_UH_list
        {
            get;
            set;
        }
        public static ObservableCollection<CustomNode> gleisknotenList
        {
            get;
            set;
        }
        
        public static ObservableCollection<Point> gleiskantenPointsList
        {
            get;
            set;
        }      
        public static ObservableCollection<Point> Entwurfselement_LAPointsList
        {
            get;
            set;
        }      
        public static ObservableCollection<Point> Entwurfselement_KMPointsList
        {
            get;
            set;
        }        
        public static ObservableCollection<Point> Entwurfselement_HOPointsList
        {
            get;
            set;
        }        
        public static ObservableCollection<Point> Entwurfselement_UHPointsList
        {
            get;
            set;
        }
       
    }
}
