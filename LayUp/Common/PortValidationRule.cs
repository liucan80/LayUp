using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LayUp.Common
{
    public class PortValidationRule : ValidationRule
    {
        public int Min { get; set; } = 0;
        public int Max { get; set; } = 65535;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int port = 0;

            try
            {
                if (((string)value).Length > 0)
                    port = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((port < Min) || (port > Max))
            {
                return new ValidationResult(false,
                  $"Please enter an Number in the range: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
