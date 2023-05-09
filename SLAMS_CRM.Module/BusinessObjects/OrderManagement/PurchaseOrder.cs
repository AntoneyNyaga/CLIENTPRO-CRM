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
    [ImageName("NewOrder")]
    [NavigationItem("Orders")]


    public class PurchaseOrder : BaseObject
    {
        public PurchaseOrder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseOrderSubject { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        
        [Association("Account-PurchaseOrders")]
        public Account Supplier
        {
            get => supplier;
            set => SetPropertyValue(nameof(Supplier), ref supplier, value);
        }

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
        [DevExpress.Xpo.Aggregated]
        public Address BillingAddress { get; set; }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]
        public Address ShippingAddress { get; set; }

        [Association("PurchaseOrder-Products")]
        public XPCollection<Product> Products
        {
            get
            {
                return GetCollection<Product>(nameof(Products));
            }
        }

        [Browsable(false)]
        [Association("PurchaseOrder-Bills")]
        public XPCollection<Bills> Bills
        {
            get
            {
                return GetCollection<Bills>(nameof(Bills));
            }
        }
    }

    public enum PurchaseOrderStatus
    {
        Draft,
        Ordered,
        Partialshippment,
        Received
    }
}