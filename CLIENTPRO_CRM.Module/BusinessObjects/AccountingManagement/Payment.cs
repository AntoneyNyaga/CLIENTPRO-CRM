using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement
{
    [DefaultClassOptions]
    [ImageName("Payment")]
    [NavigationItem("Financials")]

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

        DateTime modifiedOn;
        DateTime createdOn;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        protected override void OnSaving()
        {
            if (Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            }
            else
            {
                AddActivityStreamEntry("modified", SecuritySystem.CurrentUser as ApplicationUser);
            }
            ModifiedOn = DateTime.Now;

            if (Session.IsNewObject(this))
            {
                GeneratePaymentNumber();
            }
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = PaymentNumber,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
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