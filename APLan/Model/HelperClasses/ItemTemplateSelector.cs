using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace APLan.HelperClasses
{
    public class ItemTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate dataTemplate { get; set; }
        public DataTemplate ListDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is HelperClasses.KeyValue && (item as HelperClasses.KeyValue).Value.GetType().Equals(typeof(List<string>)))
                return ListDataTemplate;
            return dataTemplate;
        }
    }

}
