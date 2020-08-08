using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LayUp.API;

namespace LayUp.Common
{
    class StringToPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object a = (value as string);
            if (a=="ModbusTCP")
            {
                return PageManager.PageModbusTCP;
            }
            else
            {
                return PageManager.PageMXComponent;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           // throw new NotImplementedException();
           return null;
        }
    }
}
