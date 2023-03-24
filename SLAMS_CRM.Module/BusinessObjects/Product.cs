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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    [NavigationItem("Sales")]
    [Persistent("Product")]
    public class Product : BaseObject
    {
        public Product(Session session) : base(session) { }

        string description;
        private string _name;
        [RuleRequiredField("RuleRequiredField for Product.Name", DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }


        [RuleRequiredField("RuleRequiredField for Product.Description", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        private decimal _price;
        //[RuleRequiredField("RuleRequiredField for Product.Price", DefaultContexts.Save)]
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]

        public decimal Price
        {
            get => _price;
            set => SetPropertyValue(nameof(Price), ref _price, value);
        }

        [Association("Quote-Products")]
        [Browsable(false)]
        public XPCollection<Quote> Quotes => GetCollection<Quote>(nameof(Quotes));
    }
}