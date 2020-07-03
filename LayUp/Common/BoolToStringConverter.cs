using System;
using System.Globalization;
using System.Windows.Data;

namespace LayUp.Common
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? a = (value as bool?);
            if (a==true)
            {
                return "已连接";
            }
            else
            {
                return "未连接";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}