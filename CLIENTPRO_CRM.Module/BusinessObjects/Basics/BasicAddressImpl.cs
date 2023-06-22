using DevExpress.Persistent.Base;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public class BasicAddressImpl
    {
        private static string fullAddressFormat;

        private string street;

        private string city;

        private string stateProvince;

        private string zipPostal;

        private BasicICountry country;

        public static string FullAddressFormat
        {
            get
            {
                return fullAddressFormat;
            }
            set
            {
                fullAddressFormat = value;
            }
        }

        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }

        public string StateProvince
        {
            get
            {
                return stateProvince;
            }
            set
            {
                stateProvince = value;
            }
        }

        public string ZipPostal
        {
            get
            {
                return zipPostal;
            }
            set
            {
                zipPostal = value;
            }
        }

        public BasicICountry Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
            }
        }

        public string FullAddress => ObjectFormatter.Format(fullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
    }
}