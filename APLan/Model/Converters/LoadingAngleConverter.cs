using APLan.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace APLan.Converters
{
    public class LoadingAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int angle = 0;
            NewProjectViewModel newProjectViewModel = (NewProjectViewModel)((System.Windows.ResourceDictionary)value)["NewProjectViewModel"];
            ValidatorViewModel eulynxValidatorViewModel = (ValidatorViewModel)((System.Windows.ResourceDictionary)value)["EulynxValidatorViewModel"];

            angle += newProjectViewModel.LoadingIconAngle;
            angle += eulynxValidatorViewModel.LoadingIconAngle;
            //foreach (object v in values)
            //{

            //    angle += (int)v;
            //}
            return angle;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
