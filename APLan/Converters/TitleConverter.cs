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
            var collection = ((ObservableCollection<KeyValue>)value);
            if (collection.FirstOrDefault(x=>x.Key.Equals("KN_NAME"))!=null)
            {
                return collection.FirstOrDefault(x => x.Key.Equals("KN_NAME")).Value;
            }
            else if(collection.FirstOrDefault(x => x.Key.Equals("KNOTENNAME")) != null)
            {
                return collection.FirstOrDefault(x => x.Key.Equals("KNOTENNAME")).Value;
            }
            else if (collection.FirstOrDefault(x => x.Key.Equals("ID")) != null)
            {
                return collection.FirstOrDefault(x => x.Key.Equals("ID")).Value;
            }
            return "Unkown Item";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
