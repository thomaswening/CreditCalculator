using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCalculator
{
    internal static class TimeString
    {
        public static string Convert(int? paymentsPerYear, int? creditPeriod)
        {
            if (paymentsPerYear is null || creditPeriod is null) return string.Empty;

            PaymentMode paymentMode = GetPaymentMode((int)paymentsPerYear);
            int creditPeriodInMonths;

                switch (paymentMode)
                {
                    case PaymentMode.MONTHLY:
                        creditPeriodInMonths = (int)creditPeriod;
                        break;

                    case PaymentMode.QUARTERLY:
                        creditPeriodInMonths = (int)creditPeriod * 3;
                        break;

                    case PaymentMode.HALF_YEARLY:
                        creditPeriodInMonths = (int)creditPeriod * 6;
                        break;

                    case PaymentMode.YEARLY:
                        creditPeriodInMonths = (int)creditPeriod * 12;
                        break;

                    case PaymentMode.UNDEFINED:
                        return string.Empty;

                    default:
                        throw new NotImplementedException();
                }

                return ConvertMonthsToTimeString(creditPeriodInMonths);
        }

        static string ConvertMonthsToTimeString(int months)
        {
            int years = months / 12;
            int remainingMonths = System.Convert.ToInt32(Math.Ceiling(months % 12.0));

            StringBuilder sb = new();


            if (years > 1) sb.Append($"{years} years ");
            else if (years == 1) sb.Append($"{years} year ");

            if (remainingMonths > 1) sb.Append($"{remainingMonths} months ");
            else if (remainingMonths == 1) sb.Append($"{remainingMonths} month ");

            return sb.ToString();
        }

        enum PaymentMode
        {
            MONTHLY,
            QUARTERLY,
            HALF_YEARLY,
            YEARLY,
            UNDEFINED
        }

        static PaymentMode GetPaymentMode(int paymentsPerYear)
        {
            return paymentsPerYear switch
            {
                1 => PaymentMode.YEARLY,
                2 => PaymentMode.HALF_YEARLY,
                4 => PaymentMode.QUARTERLY,
                12 => PaymentMode.MONTHLY,
                _ => PaymentMode.UNDEFINED,
            };
        }
    }
}
