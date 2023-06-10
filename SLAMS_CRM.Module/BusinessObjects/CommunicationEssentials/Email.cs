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
    }
}