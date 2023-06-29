using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    [DefaultProperty("Name")]
    public class BasicCountry : BaseObject, BasicICountry
    {
        private string name;
        private string phoneCode;

        public string Name
        {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }

        public string PhoneCode
        {
            get { return phoneCode; }
            set { SetPropertyValue(nameof(PhoneCode), ref phoneCode, value); }
        }

        public BasicCountry(Session session)
            : base(session)
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public static async Task<List<BasicICountry>> GetCountries(Session session)
        {
            const string countriesApiUrl = "https://restcountries.com/v3.1/all";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(countriesApiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<Country>>(content);

            var basicICountries = new List<BasicICountry>();
            foreach (var country in countries)
            {
                var basicICountry = new BasicCountry(session)
                {
                    Name = country.Name.Common,
                    PhoneCode = country.PhoneCode
                };
                basicICountries.Add(basicICountry);
            }

            return basicICountries;
        }

        private class Country
        {
            public CountryName Name { get; set; }

            public string PhoneCode { get; set; }
        }

        private class CountryName
        {
            public string Common { get; set; }
        }
    }
}
