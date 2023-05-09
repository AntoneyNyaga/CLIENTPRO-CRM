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
    [ImageName("Business_Money")]
    [NavigationItem("Orders")]


    public class Bills : BaseObject
    {
        public Bills(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string BillNumber { get; set; }
        public string BillSubject { get; set; }

        PurchaseOrder relatedPurchaseOrder;
        ApplicationUser assignedTo;
        Account supplier;

        [Association("Account-Bills")]
        public Account Supplier
        {
            get => supplier;
            set => SetPropertyValue(nameof(Supplier), ref supplier, value);
        }
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
        public double AmountDue { get; set; }
        
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
        public XPCollection<Product> Products
        {
            get
            {
                return GetCollection<Product>(nameof(Products));
            }
        }
    }

    public enum ShippingProviderType
    {
        [ImageName("Business_Money")]
        [XafDisplayName("FedEx")]
        FedEx,
        [ImageName("Business_Money")]
        [XafDisplayName("UPS")]
        UPS,
        [ImageName("Business_Money")]
        [XafDisplayName("USPS")]
        USPS,
        [ImageName("Business_Money")]
        [XafDisplayName("DHL")]
        DHL,
        [ImageName("Business_Money")]
        [XafDisplayName("Other")]
        Other
    }   
}