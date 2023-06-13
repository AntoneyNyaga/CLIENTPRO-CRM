using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel.DataAnnotations;

namespace SLAMS_CRM.Shared.BusinessObjects.CommunicationEssentials
{
    public class DataFile : FileAttachmentBase
    {
        public DataFile(Session session) : base(session)
        {
        }

        [StringLength(260)]
        public string FileName { get; set; }

        public byte[] Content { get; set; }
    }
}
