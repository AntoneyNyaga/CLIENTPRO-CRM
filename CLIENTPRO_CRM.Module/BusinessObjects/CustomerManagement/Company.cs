using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [ImageName("BO_Department")]
    public class Company : BaseObject
    {
        public Company(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            UpdateAccount();
        }

        string emailAddress;
        string website;
        string phoneNumber;
        string companyName;
        string industryType;
        BasicAddress address;
        Account account;

        [Size(50)]
        public string CompanyName
        {
            get => companyName;
            set => SetPropertyValue(nameof(CompanyName), ref companyName, value?.ToUpper());
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int IndustryType
        {
            get => industryType == null ? 0 : (int)Enum.Parse(typeof(IndustryType), industryType);
            set { SetPropertyValue(nameof(IndustryType), ref industryType, Enum.GetName(typeof(IndustryType), value)); }
        }


        public IndustryType Industry { get => (IndustryType)IndustryType; set => IndustryType = (int)value; }

        [RuleRequiredField("RuleRequiredField for Company.Address", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public BasicAddress Address { get => address; set => SetPropertyValue(nameof(Address), ref address, value); }

        [Size(50)]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
        }

        [Size(100)]
        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }

        [Size(50)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }

        [Association("Company-Leads")]
        public XPCollection<Lead> Leads { get { return GetCollection<Lead>(nameof(Leads)); } }

        [Association("Company-Contacts")]
        public XPCollection<Contact> Contacts { get { return GetCollection<Contact>(nameof(Contacts)); } }

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
            else if (Session.IsObjectToSave(this) && !Session.IsNewObject(this))
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
                AccountName = CompanyName,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (propertyName == nameof(CompanyName) || propertyName == nameof(EmailAddress) || propertyName == nameof(Address))
            {
                UpdateAccount();
            }
        }
        public void UpdateAccount()
        {
            if (Account == null)
            {
                Account = new Account(Session); // Create a new Account object if it is null
            }

            Account.Name = CompanyName;
            Account.EmailAddress = EmailAddress;
            Account.ShippingAddress = Address;
            Account.Industry = Industry;
            Account.IsAccountCreated = 3;
        }
    }
}
