using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement
{
    [DefaultClassOptions]
    [ImageName("Business_Money")]
    [NavigationItem("Financials")]


    public class Bills : BaseObject
    {
        /* int id;
         [Key(true)]

         [VisibleInDetailView(false)]
         [VisibleInListView(false)]
         [VisibleInLookupListView(false)]
         public int Id
         {
             get { return id; }
             set { SetPropertyValue(nameof(Id), ref id, value); }
         }*/
        public Bills(Session session) : base(session)
        {
        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [VisibleInDetailView(false)]
        public string BillNumber { get; set; }

        //public string BillSubject { get; set; }
        
        public string BillSubject
        {
            get => billSubject;
            set => SetPropertyValue(nameof(BillSubject), ref billSubject, value?.ToUpper());
        }

        string billSubject;
        PurchaseOrder relatedPurchaseOrder;
        ApplicationUser assignedTo;
        Account supplier;

        [Association("Account-Bills")]
        public Account Supplier { get => supplier; set => SetPropertyValue(nameof(Supplier), ref supplier, value); }

        [Size(300)]
        public string Notes { get; set; }


        [Association("ApplicationUser-Bills")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public TermsType Terms { get; set; }

        public DateTime SupplierBillDate { get; set; }

        public DateTime SupplierDueDate { get; set; }

        public decimal AmountDue { get; set; }

        [Association("PurchaseOrder-Bills")]
        public PurchaseOrder RelatedPurchaseOrder
        {
            get => relatedPurchaseOrder;
            set => SetPropertyValue(nameof(RelatedPurchaseOrder), ref relatedPurchaseOrder, value);
        }

        public PaymentCurrencyType CurrencyType { get; set; }

        public string TaxInformation { get; set; }

        public ShippingProviderType ShippingProvider { get; set; }

        [Association("Bills-Products")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }


        private void GenerateBillNumber()
        {
            const string BillNumberFormat = "BILL{0}{1}{2:0000}";
            var lastBill = Session.Query<Bills>()?.OrderByDescending(b => b.SupplierBillDate).FirstOrDefault();
            if (lastBill != null)
            {
                var year = lastBill.SupplierBillDate.Year;
                var month = lastBill.SupplierBillDate.Month;
                var sequence = int.Parse(lastBill.BillNumber[7..]);
                sequence++;
                var newBillNumber = string.Format(BillNumberFormat, year, month, sequence);
                BillNumber = newBillNumber;
            }
            else
            {
                BillNumber = string.Format(BillNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
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
                GenerateBillNumber();
            }
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = BillSubject,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }

    public enum ShippingProviderType
    {
        FedEx,
        UPS,
        USPS,
        DHL,
        Other
    }
}