using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using SLAMS_CRM.Module.BusinessObjects;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("SLAMS CRM")]
    [Persistent("Lead")]
    [ImageName("BO_Lead")]

    [ObjectCaptionFormat("{0:FullName}")]
    [DefaultProperty(nameof(FullName))]
    public class Lead : BaseObject
    {
        public Lead(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string jobTitle;
        Communication inbox;
        //Opportunity opportunity;
        private const string V = "{FirstName} {LastName}";
        string notes;
        int score;
        string source;
        string phoneNumber;
        string emailAddress;
        string company;
        string lastName;
        string firstName;
        string status;

        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Lead.FirstName", DefaultContexts.Save)]
        public string FirstName
        {
            get => firstName;
            set => SetPropertyValue(nameof(FirstName), ref firstName, value);
        }

        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Lead.LastName", DefaultContexts.Save)]
        public string LastName
        {
            get => lastName;
            set => SetPropertyValue(nameof(LastName), ref lastName, value);
        }

        
        [Size(50)]
        public string JobTitle
        {
            get => jobTitle;
            set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value);
        }


        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Lead.Company", DefaultContexts.Save)]
        public string Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [Size(100)]
        [RuleRequiredField("RuleRequiredField for Lead.EmailAddress", DefaultContexts.Save)]
        [RuleRegularExpression(DefaultContexts.Save, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", CustomMessageTemplate = "Invalid email address format")]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }


        [RuleRegularExpression("RuleRegularExpression for Lead.PhoneNumber", DefaultContexts.Save, @"^(\+)?\d+(\s*\-\s*\d+)*$")]
        [RuleRequiredField("RuleRequiredField for Lead.PhoneNumber", DefaultContexts.Save)]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
        }

        [RuleRequiredField("RuleRequiredField for Lead.Address", DefaultContexts.Save)]
        
        public Address Address { get; set; }


        [Browsable(false)]
        public int Source
        {
            get => source == null ? 0 : (int)Enum.Parse(typeof(SourceType), source);
            set => SetPropertyValue(nameof(Source), ref source, Enum.GetName(typeof(SourceType), value));
        }

        [RuleRequiredField("RuleRequiredField for Lead.Source", DefaultContexts.Save)]

        [NotMapped]
      
        public SourceType? SourceType { get; set; }

        [Browsable(false)]
        public int Status
        {
            get => status == null ? 0 : (int)Enum.Parse(typeof(LeadStatus), status);
            set => SetPropertyValue(nameof(Status), ref status, Enum.GetName(typeof(LeadStatus), value));
        }

        [RuleRequiredField("RuleRequiredField for Lead.Status", DefaultContexts.Save)]

        [NotMapped]
        public LeadStatus? LeadStatus { get; set; }



        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Score
        {
            get => score;
            set => SetPropertyValue(nameof(Score), ref score, value);
        }


        [Size(4096)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
        
        [DevExpress.Xpo.Association("Communication-Leads")]
        public Communication Inbox
        {
            get => inbox;
            set => SetPropertyValue(nameof(Inbox), ref inbox, value);
        }



        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == nameof(FirstName) || propertyName == nameof(LastName) || propertyName == nameof(Company) ||
                propertyName == nameof(EmailAddress) || propertyName == nameof(PhoneNumber) || propertyName == nameof(Source))
            {
                CalculateScore();
            }
        }

        private void CalculateScore()
        {
            int score = 0;
            if (!string.IsNullOrEmpty(FirstName)) score += 20;
            if (!string.IsNullOrEmpty(LastName)) score += 20;
            if (!string.IsNullOrEmpty(Company)) score += 20;
            if (!string.IsNullOrEmpty(EmailAddress)) score += 20;
            if (!string.IsNullOrEmpty(PhoneNumber)) score += 20;
            //if (!string.IsNullOrEmpty(Source)) score += 10;

            Score = score;
        }

        //[Browsable (false)]
        [ReadOnly(true)]
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName
        {
            get
            {
                return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public String DisplayName
        {
            get
            {
                return FullName;
            }
        }

        private static String FullNameFormat = V;

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