using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Communication : BaseObject
    {
        public Communication(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string notes;
        string outcome;
        string description;
        string subject;
        DateTime dateTime;
        string type;

        [RuleRequiredField("RuleRequiredField for Communication.Type", DefaultContexts.Save)]
        [Size(50)]
        public string Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }

        [RuleRequiredField("RuleRequiredField for Communication.DateTime", DefaultContexts.Save)]
        public DateTime DateTime
        {
            get => dateTime;
            set => SetPropertyValue(nameof(DateTime), ref dateTime, value);
        }

        [RuleRequiredField("RuleRequiredField for Communication.Subject", DefaultContexts.Save)]
        [Size(100)]
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref subject, value);
        }


        [RuleRequiredField("RuleRequiredField for Communication.Description", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }



        public IList<Lead> Lead { get; set; } = new ObservableCollection<Lead>();

        public IList<Contact> Contact { get; set; } = new ObservableCollection<Contact>();


        [RuleRequiredField("RuleRequiredField for Communication.Outcome", DefaultContexts.Save)]
        [Size(50)]
        public string Outcome
        {
            get => outcome;
            set => SetPropertyValue(nameof(Outcome), ref outcome, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
    }

    public class TypeConverter : EnumConverterBase<CommunicationType>
    {
        public override Enum GetEnumValue(object value)
        {
            switch (value?.ToString()?.ToLower())
            {
                case "email": return CommunicationType.Email;
                case "phone call": return CommunicationType.PhoneCall;
                case "meeting": return CommunicationType.Meeting;
                default: return CommunicationType.Unknown;
            }
        }
    }

    public class OutcomeConverter : EnumConverterBase<CommunicationOutcome>
    {
        public override Enum GetEnumValue(object value)
        {
            switch (value?.ToString()?.ToLower())
            {
                case "successful": return CommunicationOutcome.Successful;
                case "unsuccessful": return CommunicationOutcome.Unsuccessful;
                case "follow-up required": return CommunicationOutcome.FollowUpRequired;
                default: return CommunicationOutcome.Unknown;
            }
        }
    }

    public enum CommunicationType
    {
        Unknown,
        Email,
        PhoneCall,
        Meeting
    }

    public enum CommunicationOutcome
    {
        Unknown,
        Successful,
        Unsuccessful,
        FollowUpRequired
    }
}