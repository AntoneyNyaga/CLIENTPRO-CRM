using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerManagement;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AssociationAttribute = DevExpress.Xpo.AssociationAttribute;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    //[DefaultClassOptions]
    [NavigationItem("Inbox")]
    [Persistent("Communication")]
    [ImageName("Actions_EnvelopeOpen")]
    public class Communication : BaseObject
    {
        public Communication(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateTime = DateTime.Now;
        }

        string status;

        [ModelDefault("AllowEdit", "false")]
        public DateTime DateTime { get; set; }

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

        [RuleRequiredField("RuleRequiredField for Communication.Contact", DefaultContexts.Save)]
        [Association("Contact-Communications")]
        public Contact Contact
        {
            get { return _contact; }
            set { SetPropertyValue(nameof(Contact), ref _contact, value); }
        }

        [Appearance("HideSubject", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Subject { get; set; }

        [Size(4096)]
        [Appearance("HideBody", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Body { get; set; }

        public string PhoneNumber { get { return Contact?.PhoneNumbers.FirstOrDefault().Number; } }

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

        [VisibleInDetailView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [Appearance(
            "ContactedTrueStatus",
            AppearanceItemType = "ViewItem",
            TargetItems = "Status",
            Criteria = "Status == 'Sent'",
            BackColor = "Green",
            FontColor = "White")]
        [Appearance(
            "ContactedFalseStatus",
            AppearanceItemType = "ViewItem",
            TargetItems = "Status",
            Criteria = "Status == 'Not Called' OR Status == 'Not Sent'",
            BackColor = "Orange",
            FontColor = "White")]
        public string Status
        {
            get
            {
                if(IsContacted)
                {
                    return "Sent";
                } else
                {
                    if(Type == CommunicationType.Phone)
                    {
                        return "Not Called";
                    } else if(Type == CommunicationType.Email)
                    {
                        return "Not Sent";
                    } else
                    {
                        return "N/A";
                    }
                }
            }

            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [Browsable(false)]
        public bool IsContacted { get; set; }


        [Browsable(false)]
        [Association("Communication-SentEmails")]
        public XPCollection<SentEmail> SentEmails { get { return GetCollection<SentEmail>(nameof(SentEmails)); } }
    }

    public enum CommunicationType
    {
        Email,
        Phone
    }
}