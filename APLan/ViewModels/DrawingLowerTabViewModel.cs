using System;
using System.ComponentModel;

namespace APLan.ViewModels
{
    public class DrawingLowerTabViewModel : BaseViewModel
    {
        #region staticPropertyChanged
        public static new event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static new void RaiseStaticPropertyChanged(string PropertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
        #region fields
        private static string _xlocation = String.Empty;
        private static string _ylocation = String.Empty;
        #endregion
        #region properties
        public static string Xlocation
        {
            get => _xlocation;
            set
            {
                if (_xlocation != value)
                {
                    _xlocation = value;
                    RaiseStaticPropertyChanged(nameof(Xlocation));// reports this property
                }
            }
        }
        public static string Ylocation
        {
            get => _ylocation;
            set
            {
                if (_ylocation != value)
                {
                    _ylocation = value;
                    RaiseStaticPropertyChanged(nameof(Ylocation));// reports this property
                }
            }
        }
        #endregion
    }
}
