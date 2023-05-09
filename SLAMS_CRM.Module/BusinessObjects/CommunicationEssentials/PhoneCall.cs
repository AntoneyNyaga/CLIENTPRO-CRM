using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerManagement;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{

    [ImageName("BO_Phone")]
    public class PhoneCall : Communication
    {
        public PhoneCall(Session session)
            : base(session)
        {
            Type = CommunicationType.Phone;

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string subject;

        public string GetSubject()
        {
            return subject;
        }

        public void SetSubject(string value)
        {
            subject = value;
        }

        [Size(300)]
        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public DateTime EndOn { get; set; }

        [Browsable(false)]
        public Contact Participant { get; set; }
    }
}