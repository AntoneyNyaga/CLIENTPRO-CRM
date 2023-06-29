using DevExpress.Persistent.Validation;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public interface BasicIPerson
    {
        [RuleRequiredField("BasicIPerson_Firstname_Required", DefaultContexts.Save, CustomMessageTemplate = "First name is required.")]
        string FirstName { get; set; }

        [RuleRequiredField("BasicIPerson_Lastname_Required", DefaultContexts.Save, CustomMessageTemplate = "Last name is required.")]
        string LastName { get; set; }

        string MiddleName { get; set; }

        //[Required(ErrorMessage = "Birthday is required.")]
        DateTime Birthday { get; set; }

        string FullName { get; }

        [RuleRequiredField("BasicIPerson_email_Required", DefaultContexts.Save, CustomMessageTemplate = "Email is required.")]
        [RuleRegularExpression("BasicIPerson_email_Regex", DefaultContexts.Save, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", CustomMessageTemplate = "Invalid email address. Example: example@mail.com")]
        string Email { get; set; }

        void SetFullName(string fullName);
    }
}
