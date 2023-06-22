using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultProperty("Name")]
    public class BasicCountry : BaseObject, BasicICountry
    {
        private string name;

        private string phoneCode;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetPropertyValue("Name", ref name, value);
            }
        }

        public string PhoneCode
        {
            get
            {
                return phoneCode;
            }
            set
            {
                SetPropertyValue("PhoneCode", ref phoneCode, value);
            }
        }

        public BasicCountry(Session session)
            : base(session)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}