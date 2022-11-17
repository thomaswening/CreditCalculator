using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CreditCalculator
{
    internal class ViewModel : INotifyPropertyChanged
    {

        #region Fields

        double? loanAmount;
        double? annuity;
        double? interestRatePerYear;
        int? creditPeriod;
        int? paymentsPerYear;

        readonly PaymentPlan paymentPlan = new();

        #endregion Fields

        #region Properties

        public double? LoanAmount
        {
            get => loanAmount;
            set
            {
                if (loanAmount != value)
                {
                    loanAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoanAmount)));

                    UpdateModel();
                }
            }
        }

        public double? Annuity
        {
            get => annuity;
            set
            {
                if (annuity != value)
                {
                    annuity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Annuity)));

                    UpdateModel();
                }
            }
        }

        public double? InterestRatePerYear
        {
            get => interestRatePerYear;
            set
            {
                if (interestRatePerYear != value)
                {
                    interestRatePerYear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(interestRatePerYear)));

                    UpdateModel();
                }
            }
        }

        public int? CreditPeriod
        {
            get => creditPeriod;
            set
            {
                if (creditPeriod != value)
                {
                    creditPeriod = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreditPeriod)));

                    UpdateModel();
                }
            }
        }

        public int? PaymentsPerYear
        {
            get => paymentsPerYear;
            set
            {
                if (paymentsPerYear != value)
                {
                    paymentsPerYear = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaymentsPerYear)));

                    UpdateModel();
                }
            }
        }

        #endregion Properties

        #region Public Constructors

        public ViewModel()
        {
            paymentPlan.PropertyChanged += OnModelChanged;
        }

        #endregion Public Constructors

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion Events

        #region Private Methods

        public void ClearAnnuity() => Annuity = null;
        public void ClearCreditPeriod() => CreditPeriod = null;

        void OnModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(paymentPlan.Annuity):
                    Annuity = paymentPlan.Annuity;
                    break;

                case nameof(paymentPlan.CreditPeriod):
                    CreditPeriod = paymentPlan.CreditPeriod;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        bool IsPaymentPlanCalculable()
        {
            List<object?> variables = new()
            {
                LoanAmount,
                InterestRatePerYear,
                PaymentsPerYear
            };

            return variables.TrueForAll(x => x is not null) && (Annuity is not null ^ CreditPeriod is not null);
        }

        void UpdateModel()
        {
            if (IsPaymentPlanCalculable()) 
            {
                paymentPlan.LoanAmount = (double)LoanAmount!;
                paymentPlan.InterestRatePerYear = (double)InterestRatePerYear!;
                paymentPlan.PaymentsPerYear = (int)PaymentsPerYear!;

                if (Annuity is null)
                {
                    paymentPlan.CreditPeriod = (int)CreditPeriod!;
                    paymentPlan.CalculateAnnuity();
                }
                else
                {
                    paymentPlan.Annuity = (double)Annuity!;
                    paymentPlan.CalculateCreditPeriod();
                }
            }
        }

        #endregion Private Methods
    }
}
