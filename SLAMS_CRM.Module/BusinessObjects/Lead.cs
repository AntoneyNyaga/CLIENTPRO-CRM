using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
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
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
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


        string notes;
        int score;
        string source;
        string phoneNumber;
        string emailAddress;
        string company;
        string lastName;
        string firstName;

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


        [Size(20)]
        [RegularExpression(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{4}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
        }

        [RuleRequiredField("RuleRequiredField for Lead.Address", DefaultContexts.Save)]
        public Address Address { get; set; }


        [Size(50)]
        public string Source
        {
            get => source;
            set => SetPropertyValue(nameof(Source), ref source, value);
        }
        public LeadStatus Status { get; set; }


        [Range(0, 100)]
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
    }

    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified
    }
}