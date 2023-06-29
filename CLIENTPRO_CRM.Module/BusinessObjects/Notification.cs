using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CLIENTPRO_CRM.Module.BusinessObjects
{
    [ImageName("BO_Notifications")]
    public class Notification : BaseObject
    {
        public Notification(Session session) : base(session)
        {
        }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public class NotificationService
        {
            private Session session;

            public NotificationService(Session session) { this.session = session; }

            // This method generates notifications based on some criteria--Tasks for this case
            public List<Notification> GetNotificationsForCurrentUser()
            {
                List<Notification> notifications = new List<Notification>();
                List<BasicTask> tasks = GetAssignedTasksForCurrentUser();
                DateTime notificationThreshold = DateTime.Now.AddDays(2); // Define the threshold for approaching deadlines

                foreach(BasicTask task in tasks)
                {
                    if(task.DueDate <= notificationThreshold)
                    {
                        // Create a notification object
                        Notification notification = new Notification(session)
                        {
                            Message = $"Task '{task.Subject}' is due on {task.DueDate.ToShortDateString()}.",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                return notifications;
            }

            private List<BasicTask> GetAssignedTasksForCurrentUser()
            {
                // Retrieve the current user's ID or any other identifier
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                // Check if the current user is valid
                if(currentUser != null)
                {
                    // Implement the logic to query the DB and retrieve assigned tasks for the current user
                    List<BasicTask> assignedTasks = session.Query<BasicTask>()
                        .Where(task => task.AssignedTo == currentUser)
                        .ToList();

                    return assignedTasks;
                }

                // Return an empty list if the current user is not found or invalid
                return new List<BasicTask>();
            }
        }
    }
}
