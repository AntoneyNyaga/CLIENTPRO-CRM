using DevExpress.Persistent.Base;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public class BasicPersonImpl
    {
        private string firstName = "";

        private string lastName = "";

        private string middleName = "";

        private DateTime birthday;

        private string email = "";

        private static string fullNameFormat;

        public static string FullNameFormat
        {
            get
            {
                return fullNameFormat;
            }
            set
            {
                fullNameFormat = value;
                fullNameFormat = ConvertIfItIsInOldFormat(fullNameFormat);
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        public string MiddleName
        {
            get
            {
                return middleName;
            }
            set
            {
                middleName = value;
            }
        }

        public DateTime Birthday
        {
            get
            {
                return birthday;
            }
            set
            {
                birthday = value;
            }
        }

        public string FullName => ObjectFormatter.Format(fullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        private static string ReplaceIgnoreCase(string str, string oldString, string newString)
        {
            string text = "";
            int num = str.IndexOf(oldString, StringComparison.InvariantCultureIgnoreCase);
            if (num >= 0 && str.IndexOf(newString) < 0)
            {
                if (num > 0)
                {
                    text = str.Substring(0, num);
                }

                text += newString;
                int length = oldString.Length;
                if (num + length < str.Length)
                {
                    text += str.Substring(num + length, str.Length - num - length);
                }
            }
            else
            {
                text = str;
            }

            return text;
        }

        private static string ConvertIfItIsInOldFormat(string formatStr)
        {
            string str = ReplaceIgnoreCase(formatStr, "FirstName", "{FirstName}");
            str = ReplaceIgnoreCase(str, "LastName", "{LastName}");
            return ReplaceIgnoreCase(str, "MiddleName", "{MiddleName}");
        }

        public void SetFullName(string fullName)
        {
            string text2 = LastName = "";
            string text5 = FirstName = MiddleName = text2;
            int num = fullName.IndexOf(',');
            if (num > 0)
            {
                fullName = fullName.Remove(0, num + 1).Trim() + " " + fullName.Substring(0, num);
            }

            string[] array = fullName.Split(new char[1] { ' ' });
            FirstName = array[0];
            if (array.Length == 2)
            {
                LastName = array[1];
                return;
            }

            if (array.Length == 3)
            {
                MiddleName = array[1];
                LastName = array[2];
                return;
            }

            for (int i = 2; i < array.Length; i++)
            {
                LastName = LastName + " " + array[i];
            }
        }
    }
}