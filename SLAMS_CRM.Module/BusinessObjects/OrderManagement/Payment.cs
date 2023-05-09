using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.AccountingEssentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]
    [ImageName("Payment")]
    [NavigationItem("Orders")]

    public class Payment : BaseObject
    {
        public Payment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string PaymentNumber { get; set; }

        ApplicationUser assignedTo;
        Account account;

        [Association("Account-Payments")]
        public Account Account
        {
            get => account;
            set => SetPropertyValue(nameof(Account), ref account, value);
        }
        public PaymentType PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        [Size(400)]
        public string Notes { get; set; }

        
        [Association("ApplicationUser-Payments")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public PaymentMethodType PaymentMethod { get; set; }
        public PaymentCurrencyType PaymentCurrency { get; set; }
        public string PaymentCurrencyCode { get; set; } = string.Empty;
        public double ReceivedOrSentAmount { get; set; }

    }

    public enum PaymentType
    {
        InvoicePayment,
        BillPayment,
        ApplyCredit,
        RefundCredit
    }

    public enum PaymentMethodType
    {
        Cash,
        Check,
        CreditCard,
        BankTransfer,
        PayPal,
        Other
    }

    public enum PaymentCurrencyType
    {
        USD,
        EUR,
        GBP,
        AUD,
        CAD,
        JPY,
        INR,
        CNY,
        Other
    }
}