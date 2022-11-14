using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CreditCalculator
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        double loanAmount;
        double annuity;
        double interestRatePerYearInPercent;
        int creditPeriod;
        int paymentsPerYear;

        PaymentPlan paymentPlan = new();

        public double LoanAmount
        {
            get => loanAmount;
            set
            {
                if (loanAmount != value)
                {
                    loanAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoanAmount)));
                }
            }
        }

        public double Annuity
        {
            get => annuity;
            set
            {
                if (annuity != value)
                {
                    annuity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Annuity)));
                }
            }
        }

        public double InterestRatePerYearInPercent
        {
            get => interestRatePerYearInPercent;
            set
            {
                if (interestRatePerYearInPercent != value)
                {
                    interestRatePerYearInPercent = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(interestRatePerYearInPercent)));
                }
            }   
        }

        public int CreditPeriod
        {
            get => creditPeriod;
            set
            {
                if (creditPeriod != value)
                {
                    creditPeriod = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreditPeriod)));
                }
            }
        }

        public int PaymentsPerYear
        {
            get => paymentsPerYear;
            set
            {
                if (paymentsPerYear != value)
                {
                    paymentsPerYear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaymentsPerYear)));
                }
            }
        }
    }
}
