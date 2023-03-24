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

    [NavigationItem("Sales")]
    [DefaultProperty("Description")]
    [Persistent("Quote")]

    
    public class Quote : BaseObject
    { 
        public Quote(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [RuleRequiredField("RuleRequiredField for Quote.Lead", DefaultContexts.Save)]
        public IList<Lead> Lead { get; set; } = new ObservableCollection<Lead>();


        string description;
        DateTime dateCreated;

        [RuleRequiredField("RuleRequiredField for Quote.DateCreated", DefaultContexts.Save)]
        public DateTime DateCreated
        {
            get => dateCreated;
            set => SetPropertyValue(nameof(DateCreated), ref dateCreated, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.Description", DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        
        private XPCollection<Product> _products;
        [Association("Quote-Products")]
        [RuleRequiredField("RuleRequiredField for Quote.Products", DefaultContexts.Save)]
        public XPCollection<Product> Products
        {
            get => _products ?? (_products = new  XPCollection<Product>(Session));
        }

        private decimal _price;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        //[RuleRequiredField("RuleRequiredField for Quote.Price", DefaultContexts.Save)]
        public decimal Price
        {
            get => _price;
            set => SetPropertyValue(nameof(Price), ref _price, value);
        }

        private QuoteStatus _status;
        //[RuleRequiredField("RuleRequiredField for Quote.Status", DefaultContexts.Save)]

        public QuoteStatus Status
        {
            get => _status;
            set => SetPropertyValue(nameof(Status), ref _status, value);
        }

        private string _notes;
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => _notes;
            set => SetPropertyValue(nameof(Notes), ref _notes, value);
        }
    }

    public enum QuoteStatus
    {
        Sent,
        Accepted,
        Declined
    }
}