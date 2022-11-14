using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CreditCalculator
{
    internal class PaymentPlan
    {
        double annuity;
        double loanAmount;
        double interestRatePerYear;
        int creditPeriod;
        int paymentsPerYear;

        public double LoanAmount 
        {
            get => loanAmount;
            set
            {
                loanAmount = value;
                CalculateAnnuity();
                CalculateCreditPeriod();
            }
        }
        public double Annuity 
        { 
            get => annuity;
            set
            {
                annuity = value;
                CalculateCreditPeriod();
            }
        }

        public double InterestRatePerYear 
        {
            get => interestRatePerYear;
            set
            {
                interestRatePerYear = value;
                Calculate();
            }
        } 

        public int CreditPeriod 
        {
            get => creditPeriod;
            set
            {
                creditPeriod = value;
                Calculate();
            }
        }

        public int PaymentsPerYear 
        {
            get => paymentsPerYear;
            set
            {
                paymentsPerYear = value;
                Calculate();
            }
        }

        private double interestRate => InterestRatePerYear / PaymentsPerYear;
        private double qFactor => 1 + interestRate;
        private double trueCreditPeriod;

        void Calculate()
        {
            CalculateAnnuity();
            CalculateCreditPeriod();
        }

        void CalculateAnnuity()
        {
            if (CreditPeriod > 0 && LoanAmount > 0 && Annuity == 0)
            {
                double qn = Math.Pow(qFactor, (double)CreditPeriod);
                Annuity = LoanAmount * qn * (1 - qFactor) / (1 - qn);
            }                
        }

        void CalculateCreditPeriod()
        {
            if (Annuity > 0 && LoanAmount > 0 && CreditPeriod == 0)
            {
                try
                {
                    double annuityLimit = interestRate * LoanAmount;
                    if (Annuity <= annuityLimit)
                    {
                        throw new Exception($"The annuity must be greater than {annuityLimit}.");
                    }

                    trueCreditPeriod = -Math.Log(1 - interestRate * LoanAmount / Annuity) / Math.Log(1 + interestRate);
                    CreditPeriod = Convert.ToInt32(Math.Ceiling(trueCreditPeriod));
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
