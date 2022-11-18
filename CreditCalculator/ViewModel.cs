﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CreditCalculator
{
    internal class ViewModel : INotifyPropertyChanged
    {

        #region Fields

        double? loanAmount = 30000;
        double? annuity;
        double? interestRatePerYear = .055;
        int? creditPeriod;
        int? paymentsPerYear = 12;

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

        /// <summary>
        /// Updates the ViewModel's Annuity and CreditPeriod fields when the model has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
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

        /// <summary>
        /// Discerns whether the payment plan is calculable and the model can be updated, given the current values in the ViewModel.
        /// </summary>
        /// <returns>True if the payment plan is calculable, else false.</returns>
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

                // Check which of (Annuity, CreditPeriod) must be calculated from the other
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

        /// <summary>
        /// Enables Binding already after hitting the ENTER key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnKeyEnterUp(object sender, KeyEventArgs e)
        {            
            TextBox tBox = (TextBox)sender;
            DependencyProperty prop = TextBox.TextProperty;

            BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
            if (binding != null) { binding.UpdateSource(); }
        }

        #endregion Private Methods
    }
}
