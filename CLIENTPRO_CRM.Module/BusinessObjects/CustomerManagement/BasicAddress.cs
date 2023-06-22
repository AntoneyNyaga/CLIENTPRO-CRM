using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultProperty("FullAddress")]
    [CalculatedPersistentAlias("FullAddress", "FullAddressPersistentAlias")]
    public class BasicAddress : BaseObject, BasicIAddress
    {
        /*int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }*/
        private const string defaultFullAddressFormat = "{Country.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}";

        private const string defaultfullAddressPersistentAlias = "concat(Country.Name,' ', StateProvince, ' ', City, ' ', Street, ' ', ZipPostal)";

        private static string fullAddressPersistentAlias;

        private BasicAddressImpl address = new BasicAddressImpl();

        public static string FullAddressPersistentAlias => fullAddressPersistentAlias;

        public string Street
        {
            get
            {
                return address.Street;
            }
            set
            {
                string street = address.Street;
                address.Street = value;
                OnChanged("Street", street, address.Street);
            }
        }

        public string City
        {
            get
            {
                return address.City;
            }
            set
            {
                string city = address.City;
                address.City = value;
                OnChanged("City", city, address.City);
            }
        }

        public string StateProvince
        {
            get
            {
                return address.StateProvince;
            }
            set
            {
                string stateProvince = address.StateProvince;
                address.StateProvince = value;
                OnChanged("StateProvince", stateProvince, address.StateProvince);
            }
        }

        public string ZipPostal
        {
            get
            {
                return address.ZipPostal;
            }
            set
            {
                string zipPostal = address.ZipPostal;
                address.ZipPostal = value;
                OnChanged("ZipPostal", zipPostal, address.ZipPostal);
            }
        }

        BasicICountry BasicIAddress.Country
        {
            get
            {
                return address.Country;
            }
            set
            {
                BasicICountry country = address.Country;
                address.Country = value;
                OnChanged("Country", country, address.Country);
            }
        }

        public BasicCountry Country
        {
            get
            {
                return address.Country as BasicCountry;
            }
            set
            {
                BasicICountry country = address.Country;
                address.Country = (BasicICountry)value;
                OnChanged("Country", country, address.Country);
            }
        }

        public string FullAddress => GetFullAddress();

        static BasicAddress()
        {
            fullAddressPersistentAlias = "concat(Country.Name,' ', StateProvince, ' ', City, ' ', Street, ' ', ZipPostal)";
            AddressImpl.FullAddressFormat = "{Country.Name}; {StateProvince}; {City}; {Street}; {ZipPostal}";
        }

        public BasicAddress(Session session)
            : base(session)
        {
        }

        public static void SetFullAddressFormat(string format, string persistentAlias)
        {
            AddressImpl.FullAddressFormat = format;
            fullAddressPersistentAlias = persistentAlias;
        }

        protected virtual string GetFullAddress()
        {
            return ObjectFormatter.Format(AddressImpl.FullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }
    }
}