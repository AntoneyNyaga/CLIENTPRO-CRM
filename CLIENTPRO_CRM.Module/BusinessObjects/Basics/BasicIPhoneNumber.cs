using DevExpress.Persistent.Validation;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public interface BasicIPhoneNumber
    {
        [RuleRequiredField("BasicIPhoneNumber_Number_Required", DefaultContexts.Save, CustomMessageTemplate = "Phone number is required.")]
        [RuleRegularExpression("BasicIPhoneNumber_Number_Regex", DefaultContexts.Save, @"^\+?[0-9]{1,4}-?[0-9]{3,4}-?[0-9]{4,}$", CustomMessageTemplate = "Invalid phone number. Example: +254 5678 9012")]
        //[RuleRange("BasicIPhoneNumber_Number_Length", DefaultContexts.Save, "10", "15", CustomMessageTemplate = "Phone number must be between 10 and 15 characters.")]
        string Number { get; set; }

        PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Home,
        Mobile,
        Work,
        Other
    }
}
