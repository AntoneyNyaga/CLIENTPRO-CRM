using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.AccountingEssentials;
using SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials;
using SLAMS_CRM.Module.BusinessObjects.OrderManagement;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SLAMS_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [Persistent("Contact")]
    [ImageName("BO_Person")]


    public class Contact : Person
    {
        public Contact(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            UpdateAccount();
        }

        string jobTitle;
        Company company;
        Account account;
        bool isCustomer;

        [Size(50)]
        public string JobTitle { get => jobTitle; set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value); }

        [RuleRequiredField("RuleRequiredField for Contact.Company", DefaultContexts.Save)]
        public Company Company { get => company; set => SetPropertyValue(nameof(Company), ref company, value); }

        //[RuleRequiredField("RuleRequiredField for Contact.Account", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        public void UpdateAccount()
        {
            if(Account == null)
            {
                Account = new Account(Session); // Create a new Account object if it is null
            }

            Account.Name = FullName;
            Account.EmailAddress = Email;
            Account.ShippingAddress = Address1;
            Account.IsAccountCreated = 2;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Account.Save();
        }

        [Browsable(false)]
        public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();

        [Browsable(false)]
        [Association("Contact-Communications")]
        public XPCollection<Communication> Communications => GetCollection<Communication>(nameof(Communications));

        [Browsable(false)]
        public bool IsCustomer
        {
            get => isCustomer;
            set => SetPropertyValue(nameof(IsCustomer), ref isCustomer, value);
        }

        //[Browsable(false)]
        public string ConvertedFrom
        {
            get
            {
                var lead = Session.FindObject<Lead>(CriteriaOperator.Parse("Contact.Oid == ?", Oid));
                return lead?.Converted;
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if(propertyName == nameof(FullName) || propertyName == nameof(Email) || propertyName == nameof(Address1))
            {
                UpdateAccount();
            }
        }
    }
}
