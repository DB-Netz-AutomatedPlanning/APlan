using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APLan.HelperClasses
{
    public class CustomItem : INotifyPropertyChanged
    {
        #region INotify Essentials
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
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
            ExtraInfo= new ObservableCollection<KeyValue>();
        }
    }
}
