using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]

    [NavigationItem("SLAMS CRM")]
    [DefaultProperty("Description")]
    [Persistent("Quote")]
    [ImageName("BO_Quote")]


    public class Quote : BaseObject
    {
        public Quote(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Quote.Title", DefaultContexts.Save)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title ), ref title, value); }

        // one to one relationship between Quote and Account
        public Account Account { get; set; }

        public Opportunity Opportunity { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.Contact", DefaultContexts.Save)]
        public Contact Contact { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.ShippingAddress", DefaultContexts.Save)]
        public Address ShippingAddress { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.BillingAddress", DefaultContexts.Save)]
        public Address BillingAddress { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.Product", DefaultContexts.Save)]
        [VisibleInListView(false)]
        public Product Product { get; set; }



        string title;
        string approvalIssues;
        DateTime validUntil;
        ApplicationUser assignedTo;

        [RuleRequiredField("RuleRequiredField for Quote.DateCreated", DefaultContexts.Save)]
        public DateTime ValidUntil
        {
            get => validUntil;
            set => SetPropertyValue(nameof(ValidUntil), ref validUntil, value);
        }

        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }


        // the one part of the Association
        //[Association("Quote-Products")]
        //public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }

        private decimal _price;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public decimal Price { get => _price; set => SetPropertyValue(nameof(Price), ref _price, value); }

        private ApprovalStatus _status;

        public ApprovalStatus ApprovalStatus
        {
            get => _status;
            set => SetPropertyValue(nameof(ApprovalStatus), ref _status, value);
        }

        private QuoteStage quoteStage;

        //[RuleRequiredField("RuleRequiredField for Quote.QuoteStage", DefaultContexts.Save)]
        public QuoteStage QuoteStage
        {
            get => quoteStage;
            set => SetPropertyValue(nameof(QuoteStage), ref quoteStage, value);
        }

        private InvoiceStatus invoiceStatus;

        public InvoiceStatus InvoiceStatus
        {
            get => invoiceStatus;
            set => SetPropertyValue(nameof(InvoiceStatus), ref invoiceStatus, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.ApprovalIssues", DefaultContexts.Save)]
        [Size(4096)]
        public string ApprovalIssues
        {
            get => approvalIssues;
            set => SetPropertyValue(nameof(ApprovalIssues), ref approvalIssues, value);
        }
    }

    public enum ApprovalStatus
    {
        Approved,
        NotApproved
    }

    public enum QuoteStage
    {
        Draft,
        Sent,
        OnHold,
        Accepted,
        Declined
    }

    public enum InvoiceStatus
    {
        NotInvoiced,
        Invoiced
    }
}