using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Accounting")]
    [ImageName("AccountingNumberFormat")]

    public class Account : BaseObject
    {
        public Account(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            createdOn = DateTime.Now;
        }


        bool isAccountCreated;
        //Invoice invoice;
        string associatedWith;
        Address shippingAddress;
        DateTime modifiedOn;
        DateTime createdOn;
        double annualRevenue;
        string industryType;
        string accountType;
        string description;
        //PhoneNumber officePhone;
        string emailAddress;
        string website;
        string name;

        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Account.Name", DefaultContexts.Save)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }

        [RuleRequiredField("RuleRequiredField for Account.EmailAddress", DefaultContexts.Save)]
        [Size(50)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }

        /*public XPCollection<PhoneNumber> GetPhoneNumbers()
        { return GetCollection<PhoneNumber>("PhoneNumbers"); }*/

        [RuleRequiredField("RuleRequiredField for Account.OfficePhone", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public PhoneNumber OfficePhone
        {
            get { return GetPropertyValue<PhoneNumber>(nameof(OfficePhone)); }
            set { SetPropertyValue(nameof(OfficePhone), value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        [RuleRequiredField("RuleRequiredField for Account.ShippingAddress", DefaultContexts.Save)]
        public Address ShippingAddress
        {
            get => shippingAddress;
            set => SetPropertyValue(nameof(ShippingAddress), ref shippingAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }


        [Browsable(false)]
        public int AccountType
        {
            get => accountType == null ? 0 : (int)Enum.Parse(typeof(AccountType), accountType);
            set { SetPropertyValue(nameof(AccountType), ref accountType, Enum.GetName(typeof(AccountType), value)); }
        }

        [RuleRequiredField("RuleRequiredField for Account.AccountType", DefaultContexts.Save)]
        [NotMapped]
        //public AccountType? Type { get => (AccountType)AccountType; set => AccountType = (int)value; }
        public AccountType? Type { get; set; }


        public double AnnualRevenue
        {
            get => annualRevenue;
            set => SetPropertyValue(nameof(AnnualRevenue), ref annualRevenue, value);
        }


        [Browsable(false)]
        public int IndustryType
        {
            get => industryType == null ? 0 : (int)Enum.Parse(typeof(IndustryType), industryType);
            set { SetPropertyValue(nameof(IndustryType), ref industryType, Enum.GetName(typeof(IndustryType), value)); }
        }

        [RuleRequiredField("RuleRequiredField for Account.Industry", DefaultContexts.Save)]
        [NotMapped]
        //public IndustryType? Industry { get => (IndustryType)IndustryType; set => IndustryType = (int)value; }
        public IndustryType? Industry { get; set; }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [ReadOnly(true)]
        [Editable(false)]
        public string AssociatedWith
        {
            get
            {
                if(IsAccountCreated)
                {
                    return "Lead";
                }
                else
                {
                    return "Contact";
                }
            }

            set => SetPropertyValue(nameof(AssociatedWith), ref associatedWith, value);
        }

        [Browsable(false)]
        public bool IsAccountCreated
        {
            get => isAccountCreated;
            set => SetPropertyValue(nameof(IsAccountCreated), ref isAccountCreated, value);
        }


        [Editable(false)]
        [ReadOnly(false)]
        [Browsable(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }

        [Editable(false)]
        [ReadOnly(true)]
        [Browsable(false)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        [Browsable(false)]
        public IList<Opportunity> Opportunities { get; set; } = new ObservableCollection<Opportunity>();

        protected override void OnSaving()
        {
            if(Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            } else
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
            activityStreamEntry.Save();
        }

        [Browsable(false)]
        public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();


        [Browsable(false)]
        [DevExpress.Xpo.Association("Account-Invoices")]
        public XPCollection<Invoice> Invoices { get { return GetCollection<Invoice>(nameof(Invoices)); } }
    }

    public enum AccountType
    {
        Analyst,
        Competitor,
        Customer,
        Integrator,
        Investor,
        partner,
        Press,
        Prospect,
        Reseller,
        Other
    }

    public enum IndustryType
    {
        Agriculture,
        Automotive,
        BankingAndFinance,
        Biotechnology,
        Chemicals,
        Construction,
        ConsumerGoods,
        Education,
        EnergyAndUtilities,
        EntertainmentAndMedia,
        HealthCare,
        HospitalityAndTourism,
        InformationTechnology,
        Insurance,
        Manufacturing,
        Mining,
        Pharmaceuticals,
        RealEstate,
        Retail,
        Telecommunications,
        TransportationAndLogistics
    }
}