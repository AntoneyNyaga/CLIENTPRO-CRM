namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    public class BasicPhoneNumberImpl
    {
        private string number;

        PhoneType phonetype;

        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        public PhoneType PhoneType
        {
            get
            {
                return phonetype;
            }
            set
            {
                phonetype = value;
            }
        }
    }
}