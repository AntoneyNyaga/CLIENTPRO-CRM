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
using DevExpress.ExpressApp.Editors;
using System.ComponentModel.DataAnnotations;
using AssociationAttribute = DevExpress.Xpo.AssociationAttribute;
using SLAMS_CRM.Shared.BusinessObjects.CommunicationEssentials;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    [ImageName("BO_Resume")]
    [DefaultProperty(nameof(Name))]

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

       /* [VisibleInListView(false)]
        [VisibleInDetailView(true)]
        [Display(Name = "Attachments")]
        [EditorAlias(EditorAliases.FileDataPropertyEditor)]
        public IList<DataFile> Attachments { get; set; } = new List<DataFile>();*/
    }
}