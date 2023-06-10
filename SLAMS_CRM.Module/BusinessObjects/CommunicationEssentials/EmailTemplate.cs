using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    [ImageName("BO_Resume")]

    public class EmailTemplate : BaseObject
    { 
        public EmailTemplate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        Lead lead;
        Communication email;
        Contact contact;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(nameof(Name), ref _name, value); }
        }

        private string _subject;
        [Size(200)]
        public string Subject
        {
            get { return _subject; }
            set { SetPropertyValue(nameof(Subject), ref _subject, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [Delayed(true)]
        public string Body
        {
            get { return GetDelayedPropertyValue<string>(nameof(Body)); }
            set { SetDelayedPropertyValue(nameof(Body), value); }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Contact-EmailTemplates")]
        public Contact Contact
        {
            get => contact;
            set => SetPropertyValue(nameof(Contact), ref contact, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Lead-EmailTemplates")]
        public Lead Lead
        {
            get => lead;
            set => SetPropertyValue(nameof(Lead), ref lead, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Communication-EmailTemplates")]
        public Communication Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        public static void CreateHardcodedTemplates(Session session)
        {
            // Create hardcoded email templates here

            // Template 1
            var template1 = new EmailTemplate(session)
            {
                Name = "Template 1",
                Subject = "Subject 1",
                Body = "Body 1"
            };
            template1.Save();

            // Template 2
            var template2 = new EmailTemplate(session)
            {
                Name = "Template 2",
                Subject = "Subject 2",
                Body = "Body 2"
            };
            template2.Save();

            // Template 3
            var template3 = new EmailTemplate(session)
            {
                Name = "Template 3",
                Subject = "Subject 3",
                Body = "Body 3"
            };
            template3.Save();

            // Add more templates as needed
        }
    }
}