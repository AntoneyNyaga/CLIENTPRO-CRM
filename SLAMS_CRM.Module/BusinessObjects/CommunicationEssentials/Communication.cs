using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerManagement;
using AssociationAttribute = DevExpress.Xpo.AssociationAttribute;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    [NavigationItem("Inbox")]
    [Persistent("Communication")]
    [ImageName("Actions_EnvelopeOpen")]
    public class Communication : BaseObject
    {
        public Communication(Session session) : base(session)
        {
            isTargetContact = true; // Set the default value to true
            UseTemplate = true;
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateTime = DateTime.Now;
        }

        bool isContacted;
        Lead lead;
        bool isTargetContact;
        string status;
        private string customSubject;
        private string customBody;
        private bool useTemplate;

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

        [RuleRequiredField(
            "RuleRequiredField for Communication.Contact",
            DefaultContexts.Save,
            TargetCriteria = "IsTargetContact")]
        [Association("Contact-Communications")]
        [Appearance("HideContact", Criteria = "!IsTargetContact", Visibility = ViewItemVisibility.Hide)]
        public Contact Contact
        {
            get { return _contact; }
            set
            {
                SetPropertyValue(nameof(Contact), ref _contact, value);
                UpdateVisibility();
            }
        }

        [RuleRequiredField(
            "RuleRequiredField for Communication.Lead",
            DefaultContexts.Save,
            TargetCriteria = "!IsTargetContact")]
        [Appearance("HideLead", Criteria = "IsTargetContact", Visibility = ViewItemVisibility.Hide)]
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


        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool UseTemplate
        {
            get { return useTemplate; }
            set
            {
                SetPropertyValue(nameof(UseTemplate), ref useTemplate, value);
                UpdateVisibility();
            }
        }

        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("HideCustomBody", Criteria = "UseTemplate", Visibility = ViewItemVisibility.Hide)]
        public string CustomBody
        {
            get => customBody;
            set => SetPropertyValue(nameof(CustomBody), ref customBody, value);
        }

        [Size(200)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("HideCustomSubject", Criteria = "UseTemplate", Visibility = ViewItemVisibility.Hide)]
        public string CustomSubject
        {
            get => customSubject;
            set => SetPropertyValue(nameof(CustomSubject), ref customSubject, value);
        }

        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        [Appearance("HideBody", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Body
        {
            get
            {
                if (!string.IsNullOrEmpty(CustomBody))
                {
                    return CustomBody;
                }
                else if (EmailTemplates != null)
                {
                    EmailTemplate selectedTemplate = EmailTemplates.FirstOrDefault();
                    if (selectedTemplate != null)
                    {
                        return selectedTemplate.Body;
                    }
                }
                return null;
            }
        }

        [Size(200)]
        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        [Appearance("HideSubject", Criteria = "Type != 'Email'", Visibility = ViewItemVisibility.Hide)]
        public string Subject
        {
            get
            {
                if (!string.IsNullOrEmpty(CustomSubject))
                {
                    return CustomSubject;
                }
                else if (EmailTemplates != null)
                {
                    EmailTemplate selectedTemplate = EmailTemplates.FirstOrDefault();
                    if (selectedTemplate != null)
                    {
                        return selectedTemplate.Subject;
                    }
                }
                return null;
            }
        }

        public string PhoneNumber
        {
            get
            {
                if (IsTargetContact)
                    return Contact?.PhoneNumbers.FirstOrDefault()?.Number;
                else if (Lead != null)
                    return Lead.PhoneNumbers.FirstOrDefault()?.Number;
                else
                    return null; // Return null or a default value when neither Contact nor Lead is available
            }
        }

        public string Email
        {
            get
            {
                if (IsTargetContact)
                    return Contact?.Email;
                else if (Lead != null)
                    return Lead.Email;
                else
                    return null; // Return null or a default value when neither Contact nor Lead is available
            }
        }


        private void UpdateVisibility()
        {
            bool isEmail = Type == CommunicationType.Email;
            bool isPhone = Type == CommunicationType.Phone;
            bool hideContact = IsTargetContact; // Use the null-coalescing operator to handle null values
            bool hideTemplate = UseTemplate;

            SetPropertyValue(nameof(CustomBody), hideTemplate ? null : customBody);
            SetPropertyValue(nameof(CustomSubject), hideTemplate ? null : customSubject);
            SetPropertyValue(nameof(EmailTemplates), hideTemplate ? null : GetCollection<EmailTemplate>(nameof(EmailTemplates)));

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

        public bool IsContacted
        {
            get => isContacted;
            set => SetPropertyValue(nameof(IsContacted), ref isContacted, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Communication-SentEmails")]
        public XPCollection<SentEmail> SentEmails { get { return GetCollection<SentEmail>(nameof(SentEmails)); } }

        [Association("Communication-EmailTemplates")]
        [Appearance("HideEmailTemplates", Criteria = "!UseTemplate", Visibility = ViewItemVisibility.Hide)]
        public XPCollection<EmailTemplate> EmailTemplates
        {
            get { return GetCollection<EmailTemplate>(nameof(EmailTemplates)); }
        }

        [DevExpress.Xpo.DisplayName("Is Target Contact ?")]
        public bool IsTargetContact
        {
            get => (bool)isTargetContact;
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