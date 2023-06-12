using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerService;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAMS_CRM.Module.BusinessObjects.OrderManagement
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

        int quantity;
        Bills bills;
        PurchaseOrder purchaseOrder;
        SalesOrder salesOrder;
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
        public int Quantity
        {
            get => quantity;
            private set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        private decimal pricePerQuantity;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public decimal PricePerQuantity { get => pricePerQuantity; set => SetPropertyValue(nameof(PricePerQuantity), ref pricePerQuantity, value); }

        private ProductLine _productLine;
        [Association("ProductLine-Products")]
        [RuleRequiredField("RuleRequiredField for Product.ProductLine", DefaultContexts.Save)]
        public ProductLine ProductLine
        {
            get => _productLine;
            set => SetPropertyValue(nameof(ProductLine), ref _productLine, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Products-Invoices")]
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
        [Association("SalesOrder-Products")]
        public SalesOrder SalesOrder
        {
            get => salesOrder;
            set => SetPropertyValue(nameof(SalesOrder), ref salesOrder, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("PurchaseOrder-Products")]
        public PurchaseOrder PurchaseOrder
        {
            get => purchaseOrder;
            set => SetPropertyValue(nameof(PurchaseOrder), ref purchaseOrder, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Product-Cases")]
        public XPCollection<Cases> Cases
        {
            get
            {
                return GetCollection<Cases>(nameof(Cases));
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Bills-Products")]
        public Bills Bills
        {
            get => bills;
            set => SetPropertyValue(nameof(Bills), ref bills, value);
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("ProductLine-Products")]
        public XPCollection<Product> Products => GetCollection<Product>(nameof(Products));
    }
}
