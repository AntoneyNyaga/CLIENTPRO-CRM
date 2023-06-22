namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement
{
    public class BasicPhoneNumberImpl
    {
        private string number;

        private string phoneType;

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

        public string PhoneType
        {
            get
            {
                return phoneType;
            }
            set
            {
                phoneType = value;
            }
        }
    }
}