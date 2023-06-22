using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    [DefaultProperty("Number")]
    public class BasicPhoneNumber : XPLiteObject, BasicIPhoneNumber
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
        private BasicPhoneNumberImpl phone = new BasicPhoneNumberImpl();

        private BasicParty party;

        [Persistent]
        public string Number
        {
            get
            {
                return phone.Number;
            }
            set
            {
                string number = phone.Number;
                phone.Number = value;
                OnChanged("Number", number, phone.Number);
            }
        }

        [Association("BasicParty-BasicPhoneNumbers")]
        public BasicParty Party
        {
            get
            {
                return party;
            }
            set
            {
                SetPropertyValue("Party", ref party, value);
            }
        }

        public string PhoneType
        {
            get
            {
                return phone.PhoneType;
            }
            set
            {
                string phoneType = phone.PhoneType;
                phone.PhoneType = value;
                OnChanged("PhoneType", phoneType, phone.PhoneType);
            }
        }

        public BasicPhoneNumber(Session session)
            : base(session)
        {
        }

        public override string ToString()
        {
            return Number;
        }
    }
}