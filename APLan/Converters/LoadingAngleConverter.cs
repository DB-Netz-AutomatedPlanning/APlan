using APLan.ViewModels;
using jdk.@internal.org.objectweb.asm.tree.analysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace APLan.Converters
{
    public class LoadingAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int angle = 0;
            NewProjectViewModel newProjectViewModel = (NewProjectViewModel)((System.Windows.ResourceDictionary)value)["NewProjectViewModel"];
            EulynxValidatorViewModel eulynxValidatorViewModel = (EulynxValidatorViewModel)((System.Windows.ResourceDictionary)value)["EulynxValidatorViewModel"];

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
