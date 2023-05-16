using APLan.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace APLan.ViewModels
{
    public class VisualizedDataViewModel : BaseViewModel
    {

        #region staticPropertyChanged
        public static new event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static new void RaiseStaticPropertyChanged(string PropertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));
        }

        public static  event EventHandler<PropertyChangedEventArgs> StaticPropertyPointVisibilityChanged;
        public static void RaiseStaticPropertyPointVisibilityChanged(string PropertyName)
        {
            StaticPropertyPointVisibilityChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        #region attributes


        //lines and Nodes visibility
        private static Visibility _gleisKantenVisibility;
        private static Visibility _Entwurfselement_LA_Visibility;
        private static Visibility _Entwurfselement_KM_Visibility;
        private static Visibility _Entwurfselement_HO_Visibility;
        private static Visibility _Entwurfselement_UH_Visibility;
        private static Visibility _gleisknotenVisibility;


        //points visibility
        private static Visibility _gleisKantenPointsVisibility;
        private static Visibility _Entwurfselement_LA_PointsVisibility;
        private static Visibility _Entwurfselement_KM_PointsVisibility;
        private static Visibility _Entwurfselement_HO_PointsVisibility;
        private static Visibility _Entwurfselement_UH_PointsVisibility;
        #endregion
        
        #region properties
        public static Visibility GleisKantenVisibility
        {
            get => _gleisKantenVisibility;
            set
            {
                _gleisKantenVisibility = value;
                RaiseStaticPropertyChanged("gleisKantenVisibility");
            }

        }
        //horizontal
        public static Visibility Entwurfselement_LA_Visibility
        {
            get => _Entwurfselement_LA_Visibility;
            set
            {
                _Entwurfselement_LA_Visibility = value;
                RaiseStaticPropertyChanged("Entwurfselement_LA_Visibility");
            }
        }
        //meilage
        public static Visibility Entwurfselement_KM_Visibility
        {
            get => _Entwurfselement_KM_Visibility;
            set
            {
                _Entwurfselement_KM_Visibility = value;
                RaiseStaticPropertyChanged("Entwurfselement_KM_Visibility");
            }
        }
        //vertical
        public static Visibility Entwurfselement_HO_Visibility
        {
            get => _Entwurfselement_HO_Visibility;
            set
            {
                _Entwurfselement_HO_Visibility = value;
                RaiseStaticPropertyChanged("Entwurfselement_HO_Visibility");
            }
        }
        //cant
        public static Visibility Entwurfselement_UH_Visibility
        {
            get => _Entwurfselement_UH_Visibility;
            set
            {
                _Entwurfselement_UH_Visibility = value;
                RaiseStaticPropertyChanged("Entwurfselement_UH_Visibility");
            }
        }
        //nodes
        public static Visibility GleisknotenVisibility
        {
            get => _gleisknotenVisibility;
            set
            {
                _gleisknotenVisibility = value;
                RaiseStaticPropertyChanged("GleisknotenVisibility");
            }
        }


        public static Visibility GleisKantenPointsVisibility
        {
            get => _gleisKantenPointsVisibility;
            set
            {
                _gleisKantenPointsVisibility = value;
                RaiseStaticPropertyPointVisibilityChanged("GleisKantenPointsVisibility");
            }

        }
        //horizontal
        public static Visibility Entwurfselement_LA_PointsVisibility
        {
            get => _Entwurfselement_LA_PointsVisibility;
            set
            {
                _Entwurfselement_LA_PointsVisibility = value;
                RaiseStaticPropertyPointVisibilityChanged("Entwurfselement_LA_PointsVisibility");
            }
        }
        //meilage
        public static Visibility Entwurfselement_KM_PointsVisibility
        {
            get => _Entwurfselement_KM_PointsVisibility;
            set
            {
                _Entwurfselement_KM_PointsVisibility = value;
                RaiseStaticPropertyPointVisibilityChanged("Entwurfselement_KM_PointsVisibility");
            }
        }
        //vertical
        public static Visibility Entwurfselement_HO_PointsVisibility
        {
            get => _Entwurfselement_HO_PointsVisibility;
            set
            {
                _Entwurfselement_HO_PointsVisibility = value;
                RaiseStaticPropertyPointVisibilityChanged("Entwurfselement_HO_PointsVisibility");
            }
        }
        //cant
        public static Visibility Entwurfselement_UH_PointsVisibility
        {
            get => _Entwurfselement_UH_PointsVisibility;
            set
            {
                _Entwurfselement_UH_PointsVisibility = value;
                RaiseStaticPropertyPointVisibilityChanged("Entwurfselement_UH_PointsVisibility");
            }
        }

        #endregion

        #region Commands
        //lines and nodes.
        public ICommand Kanten { get; set; }
        public ICommand Knoten { get; set; }
        public ICommand Horizontal { get; set; }
        public ICommand Vertical { get; set; }
        public ICommand Meilage { get; set; }
        public ICommand Cant { get; set; }

        //points 
        public ICommand KantenPoints { get; set; }
        public ICommand HorizontalPoints { get; set; }
        public ICommand VerticalPoints { get; set; }
        public ICommand MeilagePoints { get; set; }
        public ICommand CantPoints { get; set; }
        #endregion

        #region constructor
        public VisualizedDataViewModel()
        {

            Kanten = new RelayCommand(ExecuteKanten);
            Knoten = new RelayCommand(ExecuteKnoten);
            Horizontal = new RelayCommand(ExecuteHorizontal);
            Vertical = new RelayCommand(ExecuteVertical);
            Meilage = new RelayCommand(ExecuteMeilage);
            Cant = new RelayCommand(ExecuteCant);


            KantenPoints = new RelayCommand(ExecuteKantenPoints);
            HorizontalPoints = new RelayCommand(ExecuteHorizontalPoints);
            VerticalPoints = new RelayCommand(ExecuteVerticalPoints);
            MeilagePoints = new RelayCommand(ExecuteMeilagePoints);
            CantPoints = new RelayCommand(ExecuteCantPoints);


            GleisKantenVisibility = Visibility.Visible;
            Entwurfselement_LA_Visibility = Visibility.Visible;
            Entwurfselement_KM_Visibility = Visibility.Visible;
            Entwurfselement_HO_Visibility = Visibility.Visible;
            Entwurfselement_UH_Visibility = Visibility.Visible;
            GleisknotenVisibility = Visibility.Visible;


            GleisKantenPointsVisibility = Visibility.Collapsed;
            Entwurfselement_LA_PointsVisibility = Visibility.Collapsed;
            Entwurfselement_KM_PointsVisibility = Visibility.Collapsed;
            Entwurfselement_HO_PointsVisibility = Visibility.Collapsed;
            Entwurfselement_UH_PointsVisibility = Visibility.Collapsed;
        }
        #endregion

        #region logic
        private void ExecuteKanten(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (GleisKantenVisibility == Visibility.Visible)
            {
                GleisKantenVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                GleisKantenVisibility = Visibility.Visible;
                box.IsChecked = true;

            }
        }
        private void ExecuteKnoten(object parameter)
        {

            CheckBox box = ((CheckBox)parameter);
            if (GleisknotenVisibility == Visibility.Visible)
            {
                GleisknotenVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                GleisknotenVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteHorizontal(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_LA_Visibility == Visibility.Visible)
            {
                Entwurfselement_LA_Visibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_LA_Visibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteVertical(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_HO_Visibility == Visibility.Visible)
            {
                Entwurfselement_HO_Visibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_HO_Visibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteMeilage(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_KM_Visibility == Visibility.Visible)
            {
                Entwurfselement_KM_Visibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_KM_Visibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteCant(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_UH_Visibility == Visibility.Visible)
            {
                Entwurfselement_UH_Visibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_UH_Visibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteKantenPoints(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (GleisKantenPointsVisibility == Visibility.Visible)
            {
                GleisKantenPointsVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                GleisKantenPointsVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteHorizontalPoints(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_LA_PointsVisibility == Visibility.Visible)
            {
                Entwurfselement_LA_PointsVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_LA_PointsVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteVerticalPoints(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_HO_PointsVisibility == Visibility.Visible)
            {
                Entwurfselement_HO_PointsVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_HO_PointsVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteMeilagePoints(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_KM_PointsVisibility == Visibility.Visible)
            {
                Entwurfselement_KM_PointsVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_KM_PointsVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        private void ExecuteCantPoints(object parameter)
        {
            CheckBox box = ((CheckBox)parameter);
            if (Entwurfselement_UH_PointsVisibility == Visibility.Visible)
            {
                Entwurfselement_UH_PointsVisibility = Visibility.Collapsed;
                box.IsChecked = false;
            }
            else
            {
                Entwurfselement_UH_PointsVisibility = Visibility.Visible;
                box.IsChecked = true;
            }
        }
        #endregion
    }
}
