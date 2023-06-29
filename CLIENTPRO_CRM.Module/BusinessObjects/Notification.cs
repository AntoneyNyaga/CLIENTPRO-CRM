using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.Basics;
using CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials;
using CLIENTPRO_CRM.Module.BusinessObjects.CustomerService;
using CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.PipelineManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

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

            public List<Notification> GetNotificationsForCurrentUser()
            {
                List<Notification> notifications = new List<Notification>();
                DateTime notificationThreshold = DateTime.Now.AddDays(2); // Define the threshold for approaching deadlines

                // Retrieve the current user's ID
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    notifications.AddRange(GetNotificationsForTasks(currentUser, notificationThreshold));
                    notifications.AddRange(GetNotificationsForProposals(currentUser, notificationThreshold));
                    notifications.AddRange(GetNotificationsForOpportunities(currentUser, notificationThreshold));
                    notifications.AddRange(GetNotificationsForCampaigns(currentUser, notificationThreshold));
                    notifications.AddRange(GetNotificationsForMarketingEvents(currentUser));
                    notifications.AddRange(GetNotificationsForPurchaseOrders(currentUser));
                    notifications.AddRange(GetNotificationsForSalesOrders(currentUser));
                    notifications.AddRange(GetNotificationsForPayments(currentUser));
                    notifications.AddRange(GetNotificationsForBills(currentUser, notificationThreshold));
                    notifications.AddRange(GetNotificationsForCases(currentUser));
                    notifications.AddRange(GetNotificationsForTopics(currentUser));
                    notifications.AddRange(GetNotificationsForAssignments(currentUser, notificationThreshold));
                }

                return notifications;
            }

            private List<Notification> GetNotificationsForTasks(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<BasicTask> tasks = session.Query<BasicTask>()
                    .Where(task => task.AssignedTo == currentUser && task.DueDate <= notificationThreshold)
                    .ToList();

                return tasks.Select(
                    task => new Notification(session)
                    {
                        Message = $"Task '{task.Subject}' is due on {task.DueDate.ToShortDateString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForProposals(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<Quote> proposals = session.Query<Quote>()
                    .Where(
                        proposal => proposal.AssignedTo == currentUser && proposal.ValidUntil <= notificationThreshold)
                    .ToList();

                return proposals.Select(
                    proposal => new Notification(session)
                    {
                        Message = $"Proposal '{proposal.Title}' is due on {proposal.ValidUntil.ToShortTimeString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForOpportunities(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<Opportunity> opportunities = session.Query<Opportunity>()
                    .Where(
                        opportunity => opportunity.AssignedTo == currentUser &&
                            opportunity.EstimatedCloseDate <= notificationThreshold)
                    .ToList();

                return opportunities.Select(
                    opportunity => new Notification(session)
                    {
                        Message =
                            $"Opportunity '{opportunity.OpportunityName}' is due on {opportunity.EstimatedCloseDate.ToShortTimeString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForCampaigns(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<Campaign> campaigns = session.Query<Campaign>()
                    .Where(campaign => campaign.AssignedTo == currentUser && campaign.EndDate <= notificationThreshold)
                    .ToList();

                return campaigns.Select(
                    campaign => new Notification(session)
                    {
                        Message = $"Campaign '{campaign.Name}' is due on {campaign.EndDate.ToShortTimeString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForMarketingEvents(ApplicationUser currentUser)
            {
                List<MarketingEvent> marketingevents = session.Query<MarketingEvent>()
                    .Where(marketingevent => marketingevent.AssignedTo == currentUser)
                    .ToList();

                return marketingevents.Select(
                    marketingevent => new Notification(session)
                    {
                        Message = $"Marketing Event '{marketingevent.EventName}' is assigned to you.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForPurchaseOrders(ApplicationUser currentUser)
            {
                List<PurchaseOrder> purchaseorders = session.Query<PurchaseOrder>()
                    .Where(purchaseOrder => purchaseOrder.AssignedTo == currentUser)
                    .ToList();

                return purchaseorders.Select(
                    purchaseOrder => new Notification(session)
                    {
                        Message =
                            $"Purchase Order '{purchaseOrder.PurchaseOrderSubject}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForSalesOrders(ApplicationUser currentUser)
            {
                List<SalesOrder> salesorders = session.Query<SalesOrder>()
                    .Where(salesOrder => salesOrder.AssignedTo == currentUser)
                    .ToList();

                return salesorders.Select(
                    salesOrder => new Notification(session)
                    {
                        Message = $"Sales Order '{salesOrder.SalesOrderSubject}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForPayments(ApplicationUser currentUser)
            {
                List<Payment> payments = session.Query<Payment>()
                    .Where(payment => payment.AssignedTo == currentUser)
                    .ToList();

                return payments.Select(
                    payment => new Notification(session)
                    {
                        Message = $"Payment '{payment.PaymentNumber}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForBills(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<Bills> bills = session.Query<Bills>()
                    .Where(bill => bill.AssignedTo == currentUser && bill.SupplierDueDate <= notificationThreshold)
                    .ToList();

                return bills.Select(
                    bill => new Notification(session)
                    {
                        Message = $"Bill '{bill.BillSubject}' is due on {bill.SupplierDueDate.ToShortTimeString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForCases(ApplicationUser currentUser)
            {
                List<Cases> cases = session.Query<Cases>()
                    .Where(caseEntity => caseEntity.AssignedTo == currentUser)
                    .ToList();

                return cases.Select(
                    acase => new Notification(session)
                    {
                        Message = $"Case No. '{acase.CaseNumber}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForTopics(ApplicationUser currentUser)
            {
                List<Topic> topics = session.Query<Topic>().Where(topic => topic.AssignedTo == currentUser).ToList();

                return topics.Select(
                    topic => new Notification(session)
                    {
                        Message = $"Please provide more information on the topic '{topic.Name}' assigned to you.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }

            private List<Notification> GetNotificationsForAssignments(
                ApplicationUser currentUser,
                DateTime notificationThreshold)
            {
                List<Assignment> assignments = session.Query<Assignment>()
                    .Where(
                        assignment => assignment.AssignedTo == currentUser &&
                            assignment.DueDate <= notificationThreshold)
                    .ToList();

                return assignments.Select(
                    assignment => new Notification(session)
                    {
                        Message =
                            $"Task '{assignment.Description}' is due on {assignment.DueDate.ToShortTimeString()}.\n",
                        Timestamp = DateTime.Now
                    })
                    .ToList();
            }
        }
    }
}
