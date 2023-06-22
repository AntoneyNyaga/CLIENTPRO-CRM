using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials;
using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]
    [ImageName("NewOrder")]
    [NavigationItem("Orders")]


    public class PurchaseOrder : BaseObject
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
        public PurchaseOrder(Session session) : base(session)
        {
        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [VisibleInDetailView(false)]
        public string PurchaseOrderNumber { get; set; }

        public string PurchaseOrderSubject { get; set; }

        public DateTime PurchaseOrderDate { get; set; }

        public PurchaseOrderStatus Status { get; set; }


        [Association("Account-PurchaseOrders")]
        public Account Supplier { get => supplier; set => SetPropertyValue(nameof(Supplier), ref supplier, value); }

        [Size(4096)]
        public string Notes { get; set; }


        Invoice relatedInvoice;
        Account supplier;
        SalesOrder relatedSalesOrder;
        ApplicationUser assignedTo;

        [Association("ApplicationUser-PurchaseOrders")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public TermsType Terms { get; set; }

        [Association("SalesOrder-PurchaseOrders")]
        public SalesOrder RelatedSalesOrder
        {
            get => relatedSalesOrder;
            set => SetPropertyValue(nameof(RelatedSalesOrder), ref relatedSalesOrder, value);
        }

        [Association("Invoice-PurchaseOrders")]
        public Invoice RelatedInvoice
        {
            get => relatedInvoice;
            set => SetPropertyValue(nameof(RelatedInvoice), ref relatedInvoice, value);
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public BasicAddress BillingAddress { get; set; }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public BasicAddress ShippingAddress { get; set; }

        [Association("PurchaseOrder-Products")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("PurchaseOrder-Bills")]
        public XPCollection<Bills> Bills { get { return GetCollection<Bills>(nameof(Bills)); } }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("PurchaseOrder-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("PurchaseOrder-SalesOrders")]
        public XPCollection<SalesOrder> SalesOrders
        {
            get
            {
                return GetCollection<SalesOrder>(nameof(SalesOrders));
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {
                GeneratePurchaseOrderNumber();
            }
        }

        private void GeneratePurchaseOrderNumber()
        {
            const string PurchaseOrderNumberFormat = "PO{0}{1:0000}";
            var lastPurchaseOrder = Session.Query<PurchaseOrder>()?.OrderByDescending(po => po.PurchaseOrderDate)
                .FirstOrDefault();
            if (lastPurchaseOrder != null)
            {
                var year = lastPurchaseOrder.PurchaseOrderDate.Year;
                var month = lastPurchaseOrder.PurchaseOrderDate.Month;
                var sequence = int.Parse(lastPurchaseOrder.PurchaseOrderNumber[7..]);
                sequence++;
                var newPurchaseOrderNumber = string.Format(PurchaseOrderNumberFormat, year, month, sequence);
                PurchaseOrderNumber = newPurchaseOrderNumber;
            }
            else
            {
                PurchaseOrderNumber = string.Format(
                    PurchaseOrderNumberFormat,
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);
            }
        }
    }

    public enum PurchaseOrderStatus
    {
        Draft,
        Ordered,
        PartialShippment,
        Received
    }
}