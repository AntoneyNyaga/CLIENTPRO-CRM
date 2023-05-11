using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{

    [ImageName("Glyph_Message")]
    public class Email : Communication
    {
        public Email(Session session) : base(session)
        {
            Type = CommunicationType.Email;
        }

        private string _to;
        [Size(100)]
        [Browsable(false)]
        public string To
        {
            get { return _to; }
            set { SetPropertyValue(nameof(To), ref _to, value); }
        }

        
        private string _from;
        [Size(100)]
        [Browsable(false)]
        public string From
        {
            get { return _from; }
            set { SetPropertyValue(nameof(From), ref _from, value); }
        }

        private string _cc;
        [Size(100)]
        public string CC
        {
            get { return _cc; }
            set { SetPropertyValue(nameof(CC), ref _cc, value); }
        }
    }
}