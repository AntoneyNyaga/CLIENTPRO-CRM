using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]
    [ImageName("Business_Money")]
    [NavigationItem("Orders")]


    public class Bills : BaseObject
    {
        public Bills(Session session) : base(session)
        {
        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [VisibleInDetailView(false)]
        public string BillNumber { get; set; }

        public string BillSubject { get; set; }

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

        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {
                GenerateBillNumber();
            }
        }

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