using CLIENTPRO_CRM.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIENTPRO_CRM.Module.Controllers
{
    public partial class NotificationViewController : ViewController<DashboardView>
    {
        private int notificationCount;

        public NotificationViewController()
        {
            // Register the notification logic to be executed when the View is activated
            TargetViewNesting = Nesting.Root;
            TargetViewType = ViewType.DashboardView;

            var viewNotificationsAction = new SimpleAction(this, "ViewNotifications", PredefinedCategory.View)
            {
                Caption = "Notifications",
                ImageName = "BO_Notifications",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage
            };
            viewNotificationsAction.Execute += ViewNotificationsAction_Execute;
            viewNotificationsAction.Execute += UpdateNotificationCountAction_Execute;
        }

        private void ViewNotificationsAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ShowNotifications();
        }

        private void UpdateNotificationCountAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CalculateNotificationCount();
        }

        private void ShowNotifications()
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

        private void CalculateNotificationCount()
        {
            var objectSpace = View.ObjectSpace;
            var session = ((XPObjectSpace)objectSpace).Session;

            // Retrieve notifications count for the current user
            var notificationService = new Notification.NotificationService(session);
            List<Notification> notifications = notificationService.GetNotificationsForCurrentUser();

            notificationCount = notifications.Count;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            CalculateNotificationCount();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            UpdateNotificationCountCaption();
        }

        private void UpdateNotificationCountCaption()
        {
            var viewNotificationsAction = Actions["ViewNotifications"] as SimpleAction;
            if (viewNotificationsAction != null)
            {
                viewNotificationsAction.Caption = $"Notifications ({notificationCount})";
            }
        }
    }
}
