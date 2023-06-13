using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace SLAMS_CRM.Module.BusinessObjects.PipelineManagement
{
    public class SalesPipeline : BaseObject
    {
        public SalesPipeline(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

    }
}