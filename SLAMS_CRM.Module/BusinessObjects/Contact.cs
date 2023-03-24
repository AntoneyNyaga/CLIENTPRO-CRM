using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;

namespace SLAMS_CRM.Module.BusinessObjects
{
    //[DefaultClassOptions]
    [NavigationItem("Contacts")]


    [ObjectCaptionFormat("{0:FullName}")]
    [DefaultProperty(nameof(FullName))]
    public class Contact : BaseObject
    { 
        public Contact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string notes;
        string company;
        private const string V = "{FirstName} {LastName}";
        string phoneNumber;
        string emailAddress;
        string lastName;
        string firstName;

        [RuleRequiredField("RuleRequiredField for Contact.FirstName", DefaultContexts.Save)]
        public string FirstName
        {
            get => firstName;
            set => SetPropertyValue(nameof(FirstName), ref firstName, value);
        }

        [RuleRequiredField("RuleRequiredField for Contact.LastName", DefaultContexts.Save)]
        public string LastName
        {
            get => lastName;
            set => SetPropertyValue(nameof(LastName), ref lastName, value);
        }


        [Size(100)]
        [RuleRequiredField("RuleRequiredField for Contact.Company", DefaultContexts.Save)]
        public string Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }



        [RuleRegularExpression("RuleRegularExpression for Contact.PhoneNumber", DefaultContexts.Save, @"^\(\d{3}\) \d{3}-\d{4}$")]
        [RuleRequiredField("RuleRequiredField for Contact.PhoneNumber", DefaultContexts.Save)]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value); }
        }
        [RuleRequiredField("RuleRequiredField for Contact.Address", DefaultContexts.Save)]
        //[DevExpress.Xpo.Association("Contact-Address")]
        public Address Address { get; set; }


        [RuleRegularExpression("RuleRegularExpression for Contact.EmailAddress", DefaultContexts.Save, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        [RuleRequiredField("RuleRequiredField for Contact.EmailAddress", DefaultContexts.Save)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }



        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        [Browsable(false)]
        public Communication Communication { get; set; }

        [Browsable(false)]
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

        public static String FullNameFormat = V;


    }
}