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
    //[NavigationItem("Opportunities")]
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


        Inbox inbox;
        Opportunity opportunity;
        Quote quote;
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


        /*[Size(50)]
        [RuleRequiredField("RuleRequiredField for Lead.Source", DefaultContexts.Save)]*/

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


        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        /*[Browsable(false)]
        public Quote Quote { get; set; }*/


        [DevExpress.Xpo.Association("Quote-Leads")]
        public Quote Quote
        {
            get => quote;
            set => SetPropertyValue(nameof(Quote), ref quote, value);
        }

        /*[Browsable(false)]
        public Communication Communication { get; set; }*/


        
        [DevExpress.Xpo.Association("Inbox-Leads")]
        public Inbox Inbox
        {
            get => inbox;
            set => SetPropertyValue(nameof(Inbox), ref inbox, value);
        }



        /*[DevExpress.Xpo.Association("Lead-Communicatons")]
        //[Browsable(false)]
        public XPCollection<Communication> Communications => GetCollection<Communication>(nameof(Communications));*/

        /*[Browsable(false)]
        public Opportunity Opportunity { get; set; }*/



        [DevExpress.Xpo.Association("Opportunity-Leads")]
        public Opportunity Opportunity
        {
            get => opportunity;
            set => SetPropertyValue(nameof(Opportunity), ref opportunity, value);
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
            if (!string.IsNullOrEmpty(FirstName)) score += 10;
            if (!string.IsNullOrEmpty(LastName)) score += 10;
            if (!string.IsNullOrEmpty(Company)) score += 10;
            if (!string.IsNullOrEmpty(EmailAddress)) score += 10;
            if (!string.IsNullOrEmpty(PhoneNumber)) score += 10;
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
        Online,
        Referral,
        Advertisement,
        Event,
        Other
    }


}