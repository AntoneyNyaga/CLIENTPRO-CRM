using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Blazor;
using DevExpress.ExpressApp.Scheduler.Blazor.Editors;
using DevExpress.Persistent.Base.General;

namespace CLIENTPRO_CRM.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SchedulerController : ObjectViewController<ListView, IEvent>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is SchedulerListEditor schedulerListEditor)
            {
                IDxSchedulerAdapter schedulerAdapter = schedulerListEditor.GetSchedulerAdapter();
                // Obtain an instance of the DxSchedulerModel type.
                schedulerAdapter.SchedulerModel.ActiveViewType = DevExpress.Blazor.SchedulerViewType.WorkWeek;
            }
        }
    }
}
