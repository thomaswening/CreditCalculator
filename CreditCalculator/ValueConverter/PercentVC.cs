using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CreditCalculator.ValueConverter
{
    class PercentVC : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null) 
            {
                double val = (double)value;
                return val.ToString("0.00 %");
            }

            return string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value;
            val = val.Replace("%", "").Trim();

            return val == string.Empty ? null : double.Parse(val) / 100;
        }
    }
}
