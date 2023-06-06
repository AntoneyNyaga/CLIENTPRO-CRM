using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MySqlX.XDevAPI;
using SLAMS_CRM.Module.BusinessObjects.AccountingEssentials;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAMS_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [Persistent("Lead")]
    [ImageName("BO_Lead")]

    public class Lead : Person
    {
        public Lead(DevExpress.Xpo.Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Set ConvertedFrom based on the value of SourceType

            if(SourceType.HasValue)
            {
                ConvertedFrom = SourceType.Value.ToString();
            }
            UpdateAccount();

            UpdateScore();
        }


        Company company;
        bool isConvertedToContact;
        string jobTitle;
        int score;
        string source;
        string status;
        string converted;
        Account account;

        [Size(50)]
        [ImmediatePostData(true)]
        public string JobTitle { get => jobTitle; set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value); }

        [DevExpress.Xpo.Association("Company-Leads")]
        public Company Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }
        //public Company Company { get => company; set => SetPropertyValue(nameof(Company), ref company, value); }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Source
        {
            get => source == null ? 0 : (int)Enum.Parse(typeof(SourceType), source);
            set => SetPropertyValue(nameof(Source), ref source, Enum.GetName(typeof(SourceType), value));
        }

        [VisibleInDetailView(false)]
        [ReadOnly(true)]
        [ImmediatePostData(true)]
        public string ConvertedFrom { get; set; }


        [RuleRequiredField("RuleRequiredField for Lead.SourceType", DefaultContexts.Save)]
        [NotMapped]
        [ImmediatePostData(true)]
        public SourceType? SourceType
        {
            get => source == null ? null : (SourceType?)Enum.Parse(typeof(SourceType), source);
            set
            {
                SetPropertyValue(nameof(SourceType), ref source, value?.ToString());

                // Update ConvertedFrom whenever SourceType is set
                if (value.HasValue)
                {
                    ConvertedFrom = value.Value.ToString();
                }
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Status
        {
            get => status == null ? 0 : (int)Enum.Parse(typeof(LeadStatus), status);
            set => SetPropertyValue(nameof(Status), ref status, Enum.GetName(typeof(LeadStatus), value));
        }

        [RuleRequiredField("RuleRequiredField for Lead.LeadStatus", DefaultContexts.Save)]
        [NotMapped]
        public LeadStatus? LeadStatus { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ImmediatePostData]
        public Contact Contact { get; set; }

        [ModelDefault("AllowEdit", "false")]
        //[Editable(false)]
        public int Score { get => score; set => SetPropertyValue(nameof(Score), ref score, value); }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsConvertedToContact
        {
            get => isConvertedToContact;
            set => SetPropertyValue(nameof(IsConvertedToContact), ref isConvertedToContact, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [Appearance(
            "ConvertedBackground",
            AppearanceItemType = "ViewItem",
            TargetItems = "Converted",
            Criteria = "Converted == 'Converted To Contact'",
            BackColor = "Green",
            FontColor = "White")]
        [Appearance(
            "ConvertedFontWeight",
            AppearanceItemType = "ViewItem",
            TargetItems = "Converted",
            Criteria = "Converted == 'Not Yet Converted'",
            BackColor = "Orange",
            FontColor = "White")]
        public string Converted
        {
            get
            {
                if (IsConvertedToContact)
                {
                    return "Converted To Contact";
                }
                else
                {
                    return "Not Yet Converted";
                    //return ConvertedFrom;
                }
            }
            set => SetPropertyValue(nameof(Converted), ref converted, value);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            // Update the score whenever a property changes

            if (propertyName == nameof(LeadStatus) ||
                propertyName == nameof(SourceType) ||
                propertyName == nameof(JobTitle))
            {
                UpdateScore();
            }
            if (propertyName == nameof(FullName) || propertyName == nameof(Email) || propertyName == nameof(Address1))
            {
                UpdateAccount();
            }
        }

        //[RuleRequiredField("RuleRequiredField for Lead.Account", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        public void UpdateAccount()
        {
            if (Account == null)
            {
                Account = new Account(Session); // Create a new Account object if it is null
            }

            Account.Name = FullName;
            Account.EmailAddress = Email;
            Account.ShippingAddress = Address1;

            Account.IsAccountCreated = 1;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Account.Save();
        }

        private void UpdateScore()
        {
            int score = 0;

            // Map lead statuses to scores
            Dictionary<LeadStatus, int> statusScoreMap = new Dictionary<LeadStatus, int>()
            {
                { CustomerManagement.LeadStatus.New, 10 },
                { CustomerManagement.LeadStatus.Contacted, 20 },
                { CustomerManagement.LeadStatus.Qualified, 30 }
            };

            // Map lead sources to scores
            Dictionary<SourceType, int> sourceScoreMap = new Dictionary<SourceType, int>()
            {
                { CustomerManagement.SourceType.ColdCall, 10 },
                { CustomerManagement.SourceType.ExistingCustomer, 20 },
                { CustomerManagement.SourceType.SelfGenerated, 30 },

            };

            // Map job titles to scores
            Dictionary<string, int> jobTitleScoreMap = new Dictionary<string, int>()
            {
                { "CEO", 50 },
                { "Chief Executive Officer", 50 },
                { "Manager", 30 },
                { "Software Developer", 30 },
                { "Sales", 20 },
                { "Marketing", 20 }
            };

            // Increase score based on the lead status
            if (LeadStatus.HasValue && statusScoreMap.ContainsKey(LeadStatus.Value))
            {
                score += statusScoreMap[LeadStatus.Value];
            }

            // Increase score based on the lead source
            if (SourceType.HasValue && sourceScoreMap.ContainsKey(SourceType.Value))
            {
                score += sourceScoreMap[SourceType.Value];
            }

            // Increase score based on the lead's job title
            if (!string.IsNullOrEmpty(JobTitle))
            {
                foreach (var jobTitle in jobTitleScoreMap.Keys)
                {
                    if (JobTitle.Contains(jobTitle))
                    {
                        score += jobTitleScoreMap[jobTitle];
                        break;
                    }
                }
            }

            // Update the lead's score property
            Score = score;
        }
       
    }


    public enum LeadStatus
    {
        None,
        Unknown,
        New,
        Contacted,
        Qualified
    }

    public enum SourceType
    {
        ColdCall,
        ExistingCustomer,
        SelfGenerated,
        Employee,
        Partner,
        PublicRelations,
        DirectMail,
        Conference,
        TradeShow,
        Website,
        WordOfMouth,
        Email,
        Campaign,
        Other
    }
}