using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    [DefaultProperty("Number")]
    [ImageName("BO_Phone")]
    public class BasicPhoneNumber : BaseObject, BasicIPhoneNumber
    {
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

        [DevExpress.Xpo.Association("BasicParty-BasicPhoneNumbers")]
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

        public PhoneType PhoneType
        {
            get
            {
                return phone.PhoneType;
            }
            set
            {
                PhoneType phoneType = phone.PhoneType;
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