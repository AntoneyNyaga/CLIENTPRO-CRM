using DevExpress.DashboardCommon;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("SLAMS CRM")]
    [Persistent("Communication")]
    [ImageName("Actions_EnvelopeOpen")]
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

        [Browsable(false)]
        public int Type
        {
            get => type == null ? 0 : (int)Enum.Parse(typeof(CommunicationType), type);
            set => SetPropertyValue(nameof(Type), ref type, Enum.GetName(typeof(CommunicationType), value));
        }

        [RuleRequiredField("RuleRequiredField for Communication.Type", DefaultContexts.Save)]

        [NotMapped]
        public CommunicationType? CommunicationType { get; set;  }


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

        [Association("Communication-Leads")]
        public XPCollection<Lead> Leads
        {
            get
            {
                return GetCollection<Lead>(nameof(Leads));
            }
        }

        //public IList<Contact> Contact { get; set; } = new ObservableCollection<Contact>();


        [Association("Communication-Contacts")]
        public XPCollection<Contact> Contacts
        {
            get
            {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }



        [Browsable(false)]
        public int Outcome
        {
            get => outcome == null ? 0 : (int)Enum.Parse(typeof(CommunicationOutcome), outcome);
            set => SetPropertyValue(nameof(Outcome), ref outcome, Enum.GetName(typeof(CommunicationOutcome), value));
        }

        [RuleRequiredField("RuleRequiredField for Communication.Outcome", DefaultContexts.Save)]

        [NotMapped]
        public CommunicationOutcome? CommunicationOutcome { get; set;  }



        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
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