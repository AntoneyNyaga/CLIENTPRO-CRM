using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [Persistent("Lead")]
    [ImageName("BO_Lead")]

    public class Lead : Person
    {
        public Lead(Session session) : base(session)
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

            //UpdateScore();
        }

        bool isConvertedToContact;
        string jobTitle;
        int score;
        string source;
        Company company;
        string status;
        string converted;
        Account account;


        [Size(50)]
        [ImmediatePostData(true)]
        public string JobTitle { get => jobTitle; set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value); }


        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        [RuleRequiredField("RuleRequiredField for Lead.Company", DefaultContexts.Save)]
        public Company Company { get => company; set => SetPropertyValue(nameof(Company), ref company, value); }


        [Browsable(false)]
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

        [Browsable(false)]
        public int Status
        {
            get => status == null ? 0 : (int)Enum.Parse(typeof(LeadStatus), status);
            set => SetPropertyValue(nameof(Status), ref status, Enum.GetName(typeof(LeadStatus), value));
        }

        [RuleRequiredField("RuleRequiredField for Lead.LeadStatus", DefaultContexts.Save)]
        [NotMapped]
        public LeadStatus? LeadStatus { get; set; }

        [Browsable(false)]
        [ImmediatePostData]
        public Contact Contact { get; set; }


        //[EditorBrowsable(EditorBrowsableState.Never)]
        [ReadOnly(false)]
        public int Score { get => score; set => SetPropertyValue(nameof(Score), ref score, value); }


        [Browsable(false)]
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
        public String Converted
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
        }

        [RuleRequiredField("RuleRequiredField for Lead.Account", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (Account != null)
            {
                Account.Name = FullName;
                Account.EmailAddress = Email;
                Account.ShippingAddress = Address1;
                if (PhoneNumbers != null)
                {
                    // save phone numbers to the office phone of the account
                    foreach (var phoneNumber in PhoneNumbers)
                    {
                        if (phoneNumber.PhoneType == "Office" || phoneNumber.PhoneType == "Work")
                        {
                            Account.OfficePhone = new PhoneNumber(Session)
                            {
                                Number = phoneNumber.Number,
                                PhoneType = phoneNumber.PhoneType,
                            };
                        }
                    }
                }
                Account.Save();
                Account.IsAccountCreated = true;
            }
        }

       


        private void UpdateScore()
        {
            int score = 0;

            // Increase score based on the lead status
            switch(LeadStatus)
            {
                case BusinessObjects.LeadStatus.New:
                    score += 10;
                    break;
                case BusinessObjects.LeadStatus.Contacted:
                    score += 20;
                    break;
                case BusinessObjects.LeadStatus.Qualified:
                    score += 30;
                    break;
                default:
                    break;
            }

            // Increase score based on the lead source
            switch(SourceType)
            {
                case BusinessObjects.SourceType.ColdCall:
                    score += 10;
                    break;
                case BusinessObjects.SourceType.ExistingCustomer:
                    score += 20;
                    break;
                case BusinessObjects.SourceType.SelfGenerated:
                    score += 30;
                    break;
                // Add more cases for other source types as needed
                default:
                    break;
            }

            // Increase score based on the lead's job title
            if(!string.IsNullOrEmpty(JobTitle))
            {
                // Add score based on job title keyword matches
                if(JobTitle.Contains("CEO") || JobTitle.Contains("Chief Executive Officer"))
                {
                    score += 50;
                } else if(JobTitle.Contains("Manager") || JobTitle.Contains("Software Developer"))
                {
                    score += 30;
                } else if(JobTitle.Contains("Sales") || JobTitle.Contains("Marketing"))
                {
                    score += 20;
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