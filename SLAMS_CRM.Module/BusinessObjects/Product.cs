﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Name")]
    [NavigationItem("Accounting")]
    [ImageName("BO_Product")]
    [Persistent("Product")]
    public class Product : BaseObject
    {
        public Product(Session session) : base(session)
        {
        }

        Invoice invoices;
        private string _name;
        [RuleRequiredField("RuleRequiredField for Product.Name", DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value); }

        private string _description;
        [RuleRequiredField("RuleRequiredField for Product.Description", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private double _price;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public double Price { get => _price; set => SetPropertyValue(nameof(Price), ref _price, value); }

        private ProductLine _productLine;
        [Association("ProductLine-Products")]
        [RuleRequiredField("RuleRequiredField for Product.ProductLine", DefaultContexts.Save)]
        public ProductLine ProductLine
        {
            get => _productLine;
            set => SetPropertyValue(nameof(ProductLine), ref _productLine, value);
        }

        [Browsable(false)]
        [Association("Invoice-Products")]
        public Invoice Invoices
        {
            get => invoices;
            set => SetPropertyValue(nameof(Invoices), ref invoices, value);
        }
    }

    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    //[NavigationItem("SLAMS CRM")]
    [ImageName("BO_ProductLine")]
    [Persistent("ProductLine")]
    public class ProductLine : BaseObject
    {
        public ProductLine(Session session) : base(session)
        {
        }

        private string _name;
        [RuleRequiredField("RuleRequiredField for ProductLine.Name", DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value); }

        [Browsable(false)]
        [Association("ProductLine-Products")]
        public XPCollection<Product> Products => GetCollection<Product>(nameof(Products));
    }
}
