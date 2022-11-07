using APLan.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace APLan.Converters
{
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ExtraInfo = ((ObservableCollection<KeyValue>)((CustomItem)value).ExtraInfo);
            var Data = ((ObservableCollection<KeyValue>)((CustomItem)value).Data);

            if (ExtraInfo.FirstOrDefault(x => x.Key.Equals("KN_NAME")) != null)
            {
                return ExtraInfo.FirstOrDefault(x => x.Key.Equals("KN_NAME")).Value;
            }
            else if (ExtraInfo.FirstOrDefault(x => x.Key.Equals("KNOTENNAME")) != null)
            {
                return ExtraInfo.FirstOrDefault(x => x.Key.Equals("KNOTENNAME")).Value;
            }
            else if (ExtraInfo.FirstOrDefault(x => x.Key.Equals("ID")) != null)
            {
                return ExtraInfo.FirstOrDefault(x => x.Key.Equals("ID")).Value;
            } else if (Data.FirstOrDefault(x => x.Key.Equals("Name"))!=null)
            {
                return Data.FirstOrDefault(x => x.Key.Equals("Name")).Value;
            }
            return "Item";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
