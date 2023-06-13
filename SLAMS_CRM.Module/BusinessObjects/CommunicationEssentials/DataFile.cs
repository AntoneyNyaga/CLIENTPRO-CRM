using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SLAMS_CRM.Shared.BusinessObjects.CommunicationEssentials
{
    [ImageName("BO_Attachment")]
    [DefaultClassOptions]
    [DefaultProperty(nameof(FileName))]
    public class DataFile : FileAttachmentBase
    {
        public DataFile(Session session) : base(session)
        {
        }

        private string fileName;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FileName
        {
            get { return fileName; }
            set { SetPropertyValue(nameof(FileName), ref fileName, value); }
        }

        [Delayed(true)]
        [Size(SizeAttribute.Unlimited)]
        public byte[] Content
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(Content)); }
            set { SetDelayedPropertyValue(nameof(Content), value); }
        }
    }
}
