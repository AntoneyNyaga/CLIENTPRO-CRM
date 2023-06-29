using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.Controllers
{
    public partial class ClearActivityStreamController : ObjectViewController<ListView, MyActivityStream>
    {
        private SimpleAction clearActivityStreamAction;
        private SimpleAction refreshActivityStreamAction;

        public ClearActivityStreamController()
        {
            clearActivityStreamAction = new SimpleAction(this, "ClearActivityStreamAction", PredefinedCategory.Edit);
            clearActivityStreamAction.Caption = "Clear Activity Feed";
            clearActivityStreamAction.ConfirmationMessage = "Are you sure you want to clear the activity stream?";
            clearActivityStreamAction.ImageName = "Delete";
            clearActivityStreamAction.Execute += ClearActivityStreamAction_Execute;

            refreshActivityStreamAction = new SimpleAction(this, "RefreshActivityStreamAction", PredefinedCategory.View);
            refreshActivityStreamAction.Caption = "Refresh Activity Feed";
            refreshActivityStreamAction.ImageName = "Actions_Refresh";
            refreshActivityStreamAction.Execute += RefreshActivityStreamAction_Execute;
        }

        private void ClearActivityStreamAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objectSpace = View.ObjectSpace;
            var session = ((XPObjectSpace)objectSpace).Session;

            using(var uow = new UnitOfWork(session.DataLayer))
            {
                ClearAllActivityStreamEntries(objectSpace);
                objectSpace.CommitChanges();
            }

            // Refresh the current ListView
            View?.Refresh();
        }

        private void RefreshActivityStreamAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // Refresh the current ListView
            View?.Refresh();
        }

        public static void ClearAllActivityStreamEntries(IObjectSpace objectSpace)
        {
            // Delete all activity stream entries
            var activityStreamEntries = objectSpace.GetObjects<MyActivityStream>(null);
            var entriesToDelete = new List<MyActivityStream>(activityStreamEntries);

            foreach(var entry in entriesToDelete)
            {
                objectSpace.Delete(entry);
            }
        }
    }
}
