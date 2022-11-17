using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CreditCalculator
{
    internal class PaymentPlan : INotifyPropertyChanged
    {

        #region Fields

        double trueCreditPeriod;
        double annuity;
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


        private double interestRate => InterestRatePerYear / PaymentsPerYear;
        private double qFactor => 1 + interestRate;

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

        #region Private Methods

        public void CalculateAnnuity()
        {
            if (CreditPeriod > 0 && LoanAmount > 0)
            {
                double qn = Math.Pow(qFactor, (double)CreditPeriod);
                Annuity = LoanAmount * qn * (1 - qFactor) / (1 - qn);
            }                
        }

        public void CalculateCreditPeriod()
        {
            if (Annuity > 0 && LoanAmount > 0)
            {
                try
                {
                    double annuityLimit = interestRate * LoanAmount;
                    if (Annuity <= annuityLimit)
                    {
                        throw new Exception($"The annuity must be greater than {annuityLimit}.");
                    }

                    // trueCreditPeriod: t = -ln(1 - i*K/A) / ln(1 + i)
                    trueCreditPeriod = -Math.Log(1 - interestRate * LoanAmount / Annuity) / Math.Log(1 + interestRate);
                    CreditPeriod = Convert.ToInt32(Math.Ceiling(trueCreditPeriod));
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        #endregion Private Methods

    }
}
