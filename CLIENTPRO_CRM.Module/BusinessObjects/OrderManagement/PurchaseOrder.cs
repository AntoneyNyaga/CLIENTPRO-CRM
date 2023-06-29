using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Text.RegularExpressions;

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

        //public string PurchaseOrderSubject { get; set; }

        public string PurchaseOrderSubject
        {
            get => purchaseOrderSubject;
            set => SetPropertyValue(nameof(PurchaseOrderSubject), ref purchaseOrderSubject, value?.ToUpper());
        }

        public DateTime PurchaseOrderDate { get; set; }

        public PurchaseOrderStatus Status { get; set; }


        [Association("Account-PurchaseOrders")]
        public Account Supplier { get => supplier; set => SetPropertyValue(nameof(Supplier), ref supplier, value); }

        [Size(4096)]
        public string Notes { get; set; }


        string purchaseOrderSubject;
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

        private void GeneratePurchaseOrderNumber()
        {
            const string PurchaseOrderNumberFormat = "PO{0}{1:00}{2:0000}";
            var lastPurchaseOrder = Session.Query<PurchaseOrder>()
                .OrderByDescending(po => po.PurchaseOrderDate)
                .FirstOrDefault();

            if (lastPurchaseOrder != null)
            {
                var year = lastPurchaseOrder.PurchaseOrderDate.Year;
                var month = lastPurchaseOrder.PurchaseOrderDate.Month;
                var purchaseOrderNumber = lastPurchaseOrder.PurchaseOrderNumber;
                var regex = new Regex(@"\d+$"); // Matches the last sequence of digits in the purchase order number
                var match = regex.Match(purchaseOrderNumber);

                if (match.Success && int.TryParse(match.Value, out var sequence))
                {
                    sequence++;
                    var newPurchaseOrderNumber = string.Format(PurchaseOrderNumberFormat, year, month, sequence);
                    PurchaseOrderNumber = newPurchaseOrderNumber;
                }
                else
                {
                    // Handle parsing error
                }
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
                GeneratePurchaseOrderNumber();
            }
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = PurchaseOrderSubject,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
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