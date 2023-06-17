using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials
{
    [DefaultClassOptions]
    [NavigationItem("Inbox")]
    [ImageName("Actions_Send")]

    public class SentEmail : XPLiteObject
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
            set { SetPropertyValue(nameof(Subject), ref _subject, value); }
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
    }

}