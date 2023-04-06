using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using netDxf.Collections;
using NPOI.POIFS.Storage;

namespace APLan.HelperClasses
{
    public class ValueTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate ListTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is HelperClasses.KeyValue && (item as HelperClasses.KeyValue).Value.GetType().Equals(typeof(List<string>)))
                return ListTemplate;
            return DefaultTemplate;
        }
    }

}
