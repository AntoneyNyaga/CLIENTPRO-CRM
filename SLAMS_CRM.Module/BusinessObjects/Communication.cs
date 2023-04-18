using DevExpress.DashboardCommon;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.Controllers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SLAMS_CRM.Module.BusinessObjects
{
    //[DefaultClassOptions]
    [NavigationItem("Inbox")]
    [Persistent("Communication")]
    [ImageName("Actions_EnvelopeOpen")]
    //[Appearance("HideEmailFields", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("HidePhoneFields", Criteria = "Type != 'Phone'", Visibility = ViewItemVisibility.Hide)]
    public class Communication : BaseObject
    {
        public Communication(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            DateTime = DateTime.Now;
        }

        private DateTime _dateTime;
        //[ModelDefault("DisplayFormat", "{0:G}")]
        //[ModelDefault("EditMask", "G")]
        [ModelDefault("AllowEdit", "false")]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { SetPropertyValue(nameof(DateTime), ref _dateTime, value); }
        }

        private CommunicationType _type;

        public CommunicationType Type
        {
            get { return _type; }
            set
            {
                SetPropertyValue(nameof(Type), ref _type, value);
                UpdateVisibility(); // update visibility when type changes
            }
        }

        private Contact _contact;
        [Association("Contact-Communications")]
        public Contact Contact
        {
            get { return _contact; }
            set { SetPropertyValue(nameof(Contact), ref _contact, value); }
        }

        [Size(4090)]
        [Appearance("HideSubject", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Subject
        {
            get { return GetPropertyValue<string>(nameof(Subject)); }
            set { SetPropertyValue(nameof(Subject), value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [Appearance("HideBody", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Body
        {
            get { return GetPropertyValue<string>(nameof(Body)); }
            set { SetPropertyValue(nameof(Body), value); }
        }

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string PhoneNumber { get { return Contact?.PhoneNumbers?.FirstOrDefault()?.Number; } }

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        public string Email { get { return Contact?.Email; } }

        private void UpdateVisibility()
        {
            bool isEmail = Type == CommunicationType.Email;
            bool isPhone = Type == CommunicationType.Phone;

            if(Contact != null && Contact.This != null)
            {
                SetPropertyValue(nameof(Email), isEmail ? Contact.Email : null);
                SetPropertyValue(nameof(PhoneNumber), isPhone ? Contact.PhoneNumbers : null);
            } else
            {
                SetPropertyValue(nameof(Email), null);
                SetPropertyValue(nameof(PhoneNumber), null);
            }
        }
    }

    public enum CommunicationType
    {
        Email,
        Phone,
        Meeting,
        FollowUpTask
    }
}