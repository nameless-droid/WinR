using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WinR
{
    public class AutoSuggestBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate AlternateTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //var dataItem = (CountryInfo)item;
            //if (dataItem.Name.Equals("Bulgaria"))
            //{
            //    return this.AlternateTemplate;
            //}
            return this.DefaultTemplate;
        }
    }

    public class CountryInfo
    {
        public string Name { get; set; }
        public string tooltip { get; set; }

        public string ToLower()
        {
            return Name.ToLower();
        }
    }
    
    public class ItemsWithTooltip
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }

        public string ToLower()
        {
            return Name.ToLower();
        }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
