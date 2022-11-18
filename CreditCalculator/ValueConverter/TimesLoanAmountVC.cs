using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CreditCalculator.ValueConverter
{
    internal class TimesLoanAmountVC : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null)
            {
                double val = (double)value;
                return $"{val.ToString("## ##0.00 ").Trim()} x loan amount";
            }

            return string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value;

            if (val != string.Empty)
            {
                // " x loan amount" => 14

                val = val.Substring(0, val.Length - 14);
                val = val.Replace(" ", "").Trim();

                return double.Parse(val);
            }

            return null;
        }
    }
}
