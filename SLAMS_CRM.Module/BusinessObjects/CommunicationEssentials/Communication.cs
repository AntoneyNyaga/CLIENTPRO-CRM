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

        Lead lead;
        string targetContactOrLead;
        bool? isTargetContact;
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
        [Appearance("HideContact", Criteria = "IsTargetContact == 'true'", Visibility = ViewItemVisibility.Hide)]
        public Contact Contact
        {
            get { return _contact; }
            set
            {
                SetPropertyValue(nameof(Contact), ref _contact, value);
                UpdateVisibility();
            }
        }

        [Appearance("HideLead", Criteria = "IsTargetContact == 'false'", Visibility = ViewItemVisibility.Hide)]
        [Association("Lead-Communications")]
        public Lead Lead
        {
            get => lead;
            set
            {
                SetPropertyValue(nameof(Lead), ref lead, value);
                UpdateVisibility();
            }
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
            bool hideContact = IsTargetContact ?? false; // Use the null-coalescing operator to handle null values

            SetPropertyValue(nameof(Contact), hideContact ? null : _contact);
            SetPropertyValue(nameof(Lead), hideContact ? null : lead); // Update the condition to hide/show the Lead property

            if (Contact != null && Contact.This != null)
            {
                SetPropertyValue(nameof(Email), isEmail ? Contact.Email : null);
                SetPropertyValue(nameof(PhoneNumber), isPhone ? Contact.PhoneNumbers : null);
            }
            else
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
                if (IsContacted)
                {
                    return "Sent";
                }
                else
                {
                    if (Type == CommunicationType.Phone)
                    {
                        return "Not Called";
                    }
                    else if (Type == CommunicationType.Email)
                    {
                        return "Not Sent";
                    }
                    else
                    {
                        return "N/A";
                    }
                }
            }

            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsContacted { get; set; }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Communication-SentEmails")]
        public XPCollection<SentEmail> SentEmails { get { return GetCollection<SentEmail>(nameof(SentEmails)); } }

        [Association("Communication-EmailTemplates")]
        [Appearance("HideEmailTemplates", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public XPCollection<EmailTemplate> EmailTemplates
        {
            get
            {
                return GetCollection<EmailTemplate>(nameof(EmailTemplates));
            }
        }

        [RuleRequiredField("RuleRequiredField for Communication.IsTargetContact", DefaultContexts.Save, CustomMessageTemplate = "IsTargetContact is required.")]
        [DevExpress.Xpo.DisplayName("Is Target Contact")]
        public bool? IsTargetContact
        {
            get => isTargetContact;
            set
            {
                SetPropertyValue(nameof(IsTargetContact), ref isTargetContact, value);
                UpdateVisibility();
            }
        }
       
    }

    public enum CommunicationType
    {
        Email,
        Phone
    }
}