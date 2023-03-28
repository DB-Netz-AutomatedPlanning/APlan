using APLan.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace APLan.Model.CustomObjects
{
    public class CustomItem : INotifyPropertyChanged
    {
        #region INotify Essentials
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        public enum dataModelsTypes
        {
            GleisKanten,
            GleisKnoten,
            Entwurfselement_LA,
            Entwurfselement_KM,
            Entwurfselement_HO,
            Entwurfselement_UH,
            GleisKantenPoints,
            Entwurfselement_LA_Points,
            Entwurfselement_KM_Points,
            Entwurfselement_HO_Points,
            Entwurfselement_UH_Points,
            DrawnPolyLine,
            DrawnCircle,

        }
        private Visibility _visibility;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<KeyValue> ExtraInfo
        {
            get;
            set;
        }
        //contains all information from the Eulynx Object.
        public ObservableCollection<KeyValue> Data
        {
            get;
            set;
        }
        public ObservableCollection<KeyValue> ShapeAttributeInfo
        {
            get;
            set;
        }
        public CustomItem()
        {
            ExtraInfo = new ObservableCollection<KeyValue>();
        }
    }
}
