namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public interface BasicIPhoneNumber
    {
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
