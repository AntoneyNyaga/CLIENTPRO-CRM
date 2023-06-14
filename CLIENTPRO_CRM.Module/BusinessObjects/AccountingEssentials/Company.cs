using CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials
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
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        string emailAddress;
        string website;
        string phoneNumber;
        string companyName;
        string industryType;
        Address address;

        [Size(50)]
        public string CompanyName
        {
            get => companyName;
            set => SetPropertyValue(nameof(CompanyName), ref companyName, value);
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
        public Address Address { get => address; set => SetPropertyValue(nameof(Address), ref address, value); }

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
    }
}
