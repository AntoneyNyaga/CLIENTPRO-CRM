using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials
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