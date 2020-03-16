using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LayUp.Common
{

    class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           int? a = (value as int?);

           if (a==1)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
           // throw new NotImplementedException();
        }
    }
}
