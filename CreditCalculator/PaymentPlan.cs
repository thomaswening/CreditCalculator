using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using CreditCalculator.ValueConverter;

namespace CreditCalculator
{
    internal class PaymentPlan : INotifyPropertyChanged
    {

        #region Fields

        double trueCreditPeriod;
        double annuity;
        double lastPayment;
        double totalPaidInterest;

        int creditPeriod;

        #endregion Fields

        #region Properties

        public double LoanAmount { get; set; }
        public int PaymentsPerYear { get; set; }        
        public double InterestRatePerYear { get; set; }

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

        public double LastPayment
        {
            get => lastPayment;
            set
            {
                if (lastPayment != value) 
                {
                    lastPayment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastPayment)));
                }
            }
        }

        public double TotalPaidInterest
        {
            get => totalPaidInterest;
            set
            {
                if (totalPaidInterest != value)
                {
                    totalPaidInterest = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPaidInterest)));
                }
            }
        }


        double interestRate => InterestRatePerYear / PaymentsPerYear;
        double qFactor => 1 + interestRate;
        private double qn => Math.Pow(qFactor, (double)creditPeriod);
        private double qnMinus1 => Math.Pow(qFactor, (double)creditPeriod - 1);


        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

        #region Private Methods

        public void CalculateAnnuity()
        {
            if (CreditPeriod > 0 && LoanAmount > 0)
            {
                Annuity = LoanAmount * qn * (1 - qFactor) / (1 - qn);
            }                
        }

        public void CalculateCreditPeriod()
        {
            if (Annuity > 0 && LoanAmount > 0)
            {
                double annuityLimit = interestRate * LoanAmount;
                if (Annuity <= annuityLimit)
                {
                    string annuityLimitFormatted = (string)new CurrencyVC().Convert(annuityLimit, typeof(string), "€", CultureInfo.GetCultureInfo("en-us"));
                    throw new Exception($"The annuity must be greater than {annuityLimitFormatted.Trim()}.");
                }

                // trueCreditPeriod: t = -ln(1 - i*K/A) / ln(1 + i)

                trueCreditPeriod = -Math.Log(1 - interestRate * LoanAmount / Annuity) / Math.Log(1 + interestRate);
                CreditPeriod = Convert.ToInt32(Math.Ceiling(trueCreditPeriod));
            }
        }

        public void CalculateLastPayment()
        {
            // LastPayment: An = q^n * K0 + q * (1-q^(n-1)) / i * A

            LastPayment = qn * LoanAmount + qFactor * (1 - qnMinus1) / interestRate * Annuity;
        }

        public void CalculateTotalPaidInterest()
        {
            // TotalPaidInterest: Z = (n - q^(n-1)) * A + i * q^(n-1) * K0 - (i * K0 - A) * (1 - q^(n-1)) / i

            double temp = (CreditPeriod - qnMinus1) * Annuity;
            temp += interestRate * qnMinus1 * LoanAmount;
            temp += (interestRate * LoanAmount - Annuity) * (1 - qnMinus1) / interestRate;

            TotalPaidInterest = temp;
        }

        #endregion Private Methods

    }
}
