using CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials;
using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials;
using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Collections.ObjectModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [Persistent("Contact")]
    [ImageName("BO_Person")]


    public class Contact : BasicPerson
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

        SourceType? sourceType;
        Company company;
        string jobTitle;
        //Company company;
        bool isCustomer;
        string type;

        [Size(50)]
        public string JobTitle { get => jobTitle; set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value); }


        [Association("Company-Contacts")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Account Account { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Type
        {
            get => type == null ? 0 : (int)Enum.Parse(typeof(SourceType), type);
            set => SetPropertyValue(nameof(Type), ref type, Enum.GetName(typeof(SourceType), value));
        }

        [ImmediatePostData(true)]
        public SourceType? SourceType
        {
            get => sourceType;
            set => SetPropertyValue(nameof(SourceType), ref sourceType, value);
        }

        public void UpdateAccount()
        {
            if (Account == null)
            {
                Account = new Account(Session); // Create a new Account object if it is null
            }

            Account.Name = FullName;
            Account.EmailAddress = Email;
            Account.ShippingAddress = Address;
            Account.Industry = Company?.Industry;
            Account.IsAccountCreated = 2;

        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Account.Save();
        }

        [Association("Contact-Communications")]
        public XPCollection<Communication> Communications => GetCollection<Communication>(nameof(Communications));

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Contact-EmailTemplates")]
        public XPCollection<EmailTemplate> EmailTemplates
        {
            get { return GetCollection<EmailTemplate>(nameof(EmailTemplates)); }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsCustomer
        {
            get => isCustomer;
            set => SetPropertyValue(nameof(IsCustomer), ref isCustomer, value);
        }

        public string ConvertedFrom { get; set; }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(FullName) || propertyName == nameof(Email) || propertyName == nameof(Address))
            {
                UpdateAccount();
            }
        }
    }
}
