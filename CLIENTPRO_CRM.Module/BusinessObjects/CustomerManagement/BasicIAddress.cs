namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    public interface BasicIAddress
    {
        string Street { get; set; }

        string City { get; set; }

        string StateProvince { get; set; }

        string ZipPostal { get; set; }

        BasicICountry Country { get; set; }

        string FullAddress { get; }
    }
}