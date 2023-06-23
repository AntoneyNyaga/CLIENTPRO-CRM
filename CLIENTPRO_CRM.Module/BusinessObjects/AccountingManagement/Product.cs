using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.CustomerService;
using CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement
{
    [DefaultClassOptions]
    [DefaultProperty("Name")]
    [NavigationItem("Accounting")]
    [ImageName("BO_Product")]
    [Persistent("Product")]
    public class Product : BaseObject
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
        public Product(Session session) : base(session)
        {
        }

        Bills bills;
        PurchaseOrder purchaseOrder;
        SalesOrder salesOrder;
        private string _name;
        [RuleRequiredField("RuleRequiredField for Product.Name", DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value?.ToUpper()); }

        private string _description;
        [RuleRequiredField("RuleRequiredField for Product.Description", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        [RuleRequiredField("RuleRequiredField for Product.ProductCode", DefaultContexts.Save)]
        [RuleUniqueValue("RuleUniqueValue for Product.ProductCode", DefaultContexts.Save)]
        public string ProductCode { get; set; }

        public decimal UnitPrice { get; set; }

        private ProductLine _productLine;
        [Association("ProductLine-Products")]
        [RuleRequiredField("RuleRequiredField for Product.ProductLine", DefaultContexts.Save)]
        [DevExpress.Xpo.DisplayName("Product Category")] // Change the display name here
        public ProductLine ProductLine
        {
            get => _productLine;
            set => SetPropertyValue(nameof(ProductLine), ref _productLine, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Products-Invoices")]
        public XPCollection<Invoice> Invoices { get { return GetCollection<Invoice>(nameof(Invoices)); } }

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
        public XPCollection<Cases> Cases { get { return GetCollection<Cases>(nameof(Cases)); } }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Bills-Products")]
        public Bills Bills { get => bills; set => SetPropertyValue(nameof(Bills), ref bills, value); }

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
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = Name,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }

    //[DefaultClassOptions]
    [DefaultProperty("Name")]
    //[NavigationItem("CLIENTPRO CRM")]
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
        public string Name { get => _name; set => SetPropertyValue(nameof(Name), ref _name, value?.ToUpper()); }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("ProductLine-Products")]
        public XPCollection<Product> Products => GetCollection<Product>(nameof(Products));

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
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = Name,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }
}
