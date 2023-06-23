using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials
{
    [DefaultClassOptions]
    [NavigationItem("Inbox")]
    [ImageName("Actions_Send")]

    public class SentEmail : BaseObject
    {
        public SentEmail(Session session) : base(session) { }

        Communication communication;
        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set { SetPropertyValue(nameof(Sender), ref _sender, value); }
        }

        private string _recipient;
        public string Recipient
        {
            get { return _recipient; }
            set { SetPropertyValue(nameof(Recipient), ref _recipient, value); }
        }

        private string _subject;
        [Size(200)]
        public string Subject
        {
            get { return _subject; }
            set { SetPropertyValue(nameof(Subject), ref _subject, value?.ToUpper()); }
        }

        private string _body;
        [Size(SizeAttribute.Unlimited)]
        public string Body
        {
            get { return _body; }
            set { SetPropertyValue(nameof(Body), ref _body, value); }
        }

        private DateTime _dateTimeSent;
        public DateTime DateTimeSent
        {
            get { return _dateTimeSent; }
            set { SetPropertyValue(nameof(DateTimeSent), ref _dateTimeSent, value); }
        }


        [Association("Communication-SentEmails")]
        public Communication Communication
        {
            get => communication;
            set => SetPropertyValue(nameof(Communication), ref communication, value);
        }

        DateTime modifiedOn;
        DateTime createdOn;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        protected override void OnSaving()
        {
            if (Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            }
            else
            {
                AddActivityStreamEntry("modified", SecuritySystem.CurrentUser as ApplicationUser);
            }
            ModifiedOn = DateTime.Now;
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = Subject,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }

}