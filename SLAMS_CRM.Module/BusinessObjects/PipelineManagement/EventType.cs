using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace SLAMS_CRM.Module.BusinessObjects.PipelineManagement
{

    public class EventType : BaseObject
    {
        public EventType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        public string EventTypeName { get; set; }

    }
}