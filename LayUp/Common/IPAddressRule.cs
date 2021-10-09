using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LayUp.Common
{
    public class IPAddressRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string IPAddress = value as string;

            if (!string.IsNullOrWhiteSpace(IPAddress))
            {
                string IPAddressFormartRegex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

                // 检查输入的字符串是否符合IP地址格式
                if (!Regex.IsMatch(IPAddress, IPAddressFormartRegex))
                {
                    return new ValidationResult(false, "IP地址格式不正确");
                }
            }
            return new ValidationResult(true, null);
        }
    }
}
