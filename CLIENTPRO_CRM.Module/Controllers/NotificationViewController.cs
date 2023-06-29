using CLIENTPRO_CRM.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;

namespace CLIENTPRO_CRM.Module.Controllers
{
    public partial class NotificationViewController : ViewController<DashboardView>
    {
        public NotificationViewController()
        {
            // Register the notification logic to be executed when the View is activated
            TargetViewNesting = Nesting.Root;
            TargetViewType = ViewType.DashboardView;

            // Create a simple action for viewing notifications
            var viewNotificationsAction = new SimpleAction(this, "ViewNotifications", PredefinedCategory.View)
            {
                Caption = "Notifications",
                ImageName = "BO_Notifications",
                //PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                PaintStyle = ActionItemPaintStyle.Image
            };
            viewNotificationsAction.Execute += ViewNotificationsAction_Execute;
        }

        private void ViewNotificationsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objectSpace = View.ObjectSpace;
            var session = ((XPObjectSpace)objectSpace).Session;

            // Retrieve notifications for the current user
            var notificationService = new Notification.NotificationService(session);
            List<Notification> notifications = notificationService.GetNotificationsForCurrentUser();

            if (notifications.Count > 0)
            {
                // Build the notification message
                var message = string.Join(Environment.NewLine, notifications.Select(n => n.Message));

                // Show the notification in a message box
                Application.ShowViewStrategy.ShowMessage(message, InformationType.Info);
            }
            else
            {
                // No notifications to display
                Application.ShowViewStrategy.ShowMessage("No new notifications.", InformationType.Info);
            }
        }

    }
}
