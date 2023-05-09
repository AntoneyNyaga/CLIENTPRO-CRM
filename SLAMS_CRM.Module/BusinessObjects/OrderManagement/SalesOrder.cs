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
        public SalesOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [VisibleInDetailView(false)]
        public string SalesOrderNumber { get; set; }
        public string SalesOrderSubject { get; set; }
        public SalesOrderStatus Status { get; set; }

        [Size(4096)]
        public string Notes { get; set; }

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
        public XPCollection<Product> Products
        {
            get
            {
                return GetCollection<Product>(nameof(Products));
            }
        }
        [Browsable(false)]
        [Association("SalesOrder-PurchaseOrders")]
        public XPCollection<PurchaseOrder> PurchaseOrders
        {
            get
            {
                return GetCollection<PurchaseOrder>(nameof(PurchaseOrders));
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
        COD,
        Net30,
        Net60,
        Net90,
        Net120,
        Net150,
        Net180,
        Net210,
        Net240,
        Net270,
        Net300,
        Net330,
        Net360
    }
}