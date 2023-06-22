using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultProperty("Name")]
    public class BasicCountry : XPLiteObject, BasicICountry
    {
        int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }
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