using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement
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

        [VisibleInDetailView(false)]
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
        public decimal ReceivedOrSentAmount { get; set; }

        private void GeneratePaymentNumber()
        {
            const string PaymentNumberFormat = "PAY{0:yyyyMMdd}{1:0000}";
            var lastPayment = Session.Query<Payment>()?.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
            var sequence = lastPayment != null ? int.Parse(lastPayment.PaymentNumber[11..]) + 1 : 1;
            var newPaymentNumber = string.Format(PaymentNumberFormat, DateTime.Today, sequence);
            PaymentNumber = newPaymentNumber;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                GeneratePaymentNumber();
            }
        }

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