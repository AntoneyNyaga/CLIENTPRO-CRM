using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    [DefaultProperty("FullName")]
    [ImageName("BO_Person")]
    [CalculatedPersistentAlias("FullName", "FullNamePersistentAlias")]
    public class BasicPerson : BasicParty, BasicIPerson
    {
        private const string defaultFullNameFormat = "{FirstName} {MiddleName} {LastName}";

        private const string defaultFullNamePersistentAlias = "concat(FirstName,' ', MiddleName,' ', LastName)";

        private BasicPersonImpl person = new BasicPersonImpl();

        private static string fullNamePersistentAlias;

        public static string FullNamePersistentAlias => fullNamePersistentAlias;

        public string FirstName
        {
            get
            {
                return person.FirstName;
            }
            set
            {
                string firstName = person.FirstName;
                person.FirstName = value;
                OnChanged("FirstName", firstName, person.FirstName);
            }
        }

        public string LastName
        {
            get
            {
                return person.LastName;
            }
            set
            {
                string lastName = person.LastName;
                person.LastName = value;
                OnChanged("LastName", lastName, person.LastName);
            }
        }

        public string MiddleName
        {
            get
            {
                return person.MiddleName;
            }
            set
            {
                string middleName = person.MiddleName;
                person.MiddleName = value;
                OnChanged("MiddleName", middleName, person.MiddleName);
            }
        }

        public DateTime Birthday
        {
            get
            {
                return person.Birthday;
            }
            set
            {
                DateTime birthday = person.Birthday;
                person.Birthday = value;
                OnChanged("Birthday", birthday, person.Birthday);
            }
        }

        [SearchMemberOptions(SearchMemberMode.Include)]
        public string FullName => GetFullName();

        [Size(255)]
        public string Email
        {
            get
            {
                return person.Email;
            }
            set
            {
                string email = person.Email;
                person.Email = value;
                OnChanged("Email", email, person.Email);
            }
        }

        static BasicPerson()
        {
            fullNamePersistentAlias = "concat(FirstName,' ', MiddleName,' ', LastName)";
            PersonImpl.FullNameFormat = "{FirstName} {MiddleName} {LastName}";
        }

        public static void SetFullNameFormat(string format, string persistentAlias)
        {
            PersonImpl.FullNameFormat = format;
            fullNamePersistentAlias = persistentAlias;
        }

        public BasicPerson(Session session)
            : base(session)
        {
        }

        public void SetFullName(string fullName)
        {
            person.SetFullName(fullName);
        }

        protected virtual string GetFullName()
        {
            return ObjectFormatter.Format(PersonImpl.FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }

        protected override string GetDisplayName()
        {
            return GetFullName();
        }
    }
}