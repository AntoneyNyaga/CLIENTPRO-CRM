using DevExpress.Xpo;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    //[DefaultClassOptions]
    //[NavigationItem("Inbox")]
    public class Email : Communication
    {
        public Email(Session session) : base(session)
        {
            Type = CommunicationType.Email;
        }

        private string _to;
        [Size(4090)]
        //[Appearance("HideToField", Visibility = ViewItemVisibility.Hide)]
        public string To
        {
            get { return _to; }
            set { SetPropertyValue(nameof(To), ref _to, value); }
        }

        private string _from;
        [Size(4090)]
        //[Appearance("HideToField", Visibility = ViewItemVisibility.Hide)]
        public string From
        {
            get { return _from; }
            set { SetPropertyValue(nameof(From), ref _from, value); }
        }

        private string _cc;
        [Size(4090)]
        //[Appearance("HideCCField", Visibility = ViewItemVisibility.Hide)]
        public string CC
        {
            get { return _cc; }
            set { SetPropertyValue(nameof(CC), ref _cc, value); }
        }

        private string _bcc;
        [Size(4090)]
        //[Appearance("HideBCCField", Visibility = ViewItemVisibility.Hide)]
        public string BCC
        {
            get { return _bcc; }
            set { SetPropertyValue(nameof(BCC), ref _bcc, value); }
        }
    }
}