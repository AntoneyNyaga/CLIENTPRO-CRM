using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.PipelineManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]
    [ImageName("CustomerQuickSales")]
    [NavigationItem("Orders")]

    public class SalesOrder : BaseObject
    {
        public SalesOrder(Session session) : base(session)
        {
        }
        public override void AfterConstruction() { base.AfterConstruction(); }

        [VisibleInDetailView(false)]
        public string SalesOrderNumber { get; set; }

        public string SalesOrderSubject { get; set; }

        public DateTime SalesOrderDate { get; set; }

        public SalesOrderStatus Status { get; set; }

        [Size(4096)]
        public string Notes { get; set; }

        PurchaseOrder relatedPurchaseOrder;
        Quote relatedQuote;
        Opportunity opportunity;
        ApplicationUser assignedTo;

        [Association("ApplicationUser-SalesOrders")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public DateTime DueDate { get; set; }

        public TermsType Terms { get; set; }

        public DateTime DeliveryDate { get; set; }

        [Association("Opportunity-SalesOrders")]
        public Opportunity Opportunity
        {
            get => opportunity;
            set => SetPropertyValue(nameof(Opportunity), ref opportunity, value);
        }


        [Association("Quote-SalesOrders")]
        public Quote RelatedQuote
        {
            get => relatedQuote;
            set => SetPropertyValue(nameof(RelatedQuote), ref relatedQuote, value);
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]
        public Address BillingAddress { get; set; }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]
        public Address ShippingAddress { get; set; }

        [Association("SalesOrder-Products")]
        //[Browsable(false)]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }

        [Browsable(false)]
        [Association("SalesOrder-PurchaseOrders")]
        public XPCollection<PurchaseOrder> PurchaseOrders
        {
            get { return GetCollection<PurchaseOrder>(nameof(PurchaseOrders)); }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (Session.IsNewObject(this))
            {
                GenerateSalesOrderNumber();
            }
        }
        
        [Association("PurchaseOrder-SalesOrders")]
        public PurchaseOrder RelatedPurchaseOrder
        {
            get => relatedPurchaseOrder;
            set => SetPropertyValue(nameof(RelatedPurchaseOrder), ref relatedPurchaseOrder, value);
        }

        private void GenerateSalesOrderNumber()
        {
            const string SalesOrderNumberFormat = "SO{0}{1:0000}";
            var lastSalesOrder = Session.Query<SalesOrder>()?.OrderByDescending(po => po.SalesOrderDate)
                .FirstOrDefault();
            if(lastSalesOrder != null)
            {
                var year = lastSalesOrder.SalesOrderDate.Year;
                var month = lastSalesOrder.SalesOrderDate.Month;
                var sequence = int.Parse(lastSalesOrder.SalesOrderNumber[7..]);
                sequence++;
                var newPurchaseOrderNumber = string.Format(SalesOrderNumberFormat, year, month, sequence);
                SalesOrderNumber = newPurchaseOrderNumber;
            } else
            {
                SalesOrderNumber = string.Format(SalesOrderNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
        }
    }

    public enum SalesOrderStatus
    {
        Ordered,
        InManufacturing,
        PartiallyShippedAndInvoiced,
        PartiallyShippedAndNotInvoiced,
        ShippedAndNotInvoiced,
        Closed
    }

    public enum TermsType
    {
        COD,                    // Cash on Delivery
        Net30,                  // Payment due within 30 days from the invoice date
        Net60,                  // Payment due within 60 days from the invoice date
        Net90,                  // Payment due within 90 days from the invoice date
        Net10Net30,             // 1/10 Net 30 - 1% discount if paid within 10 days, otherwise full payment due within 30 days
        DueOnReceipt,           // Payment due immediately upon receipt of the invoice or goods
        EOM,                    // End of Month - Payment due at the end of the calendar month
        CWO,                    // Cash with Order - Payment required upfront before processing the order or delivering goods
        LetterOfCredit          // Letter of Credit - Payment guaranteed by the buyer's bank upon presentation of specified documents
    }
}