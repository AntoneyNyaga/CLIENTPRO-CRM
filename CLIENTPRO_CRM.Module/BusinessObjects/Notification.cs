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
                List<BasicTask> tasks = GetAssignedTasksForCurrentUser();
                List<Quote> proposals = GetAssignedProposalsForCurrentUser();
                List<Opportunity> opportunities = GetAssignedOpportunitiesForCurrentUser();
                List<Campaign> campaigns = GetAssignedCampaignsForCurrentUser();
                List<MarketingEvent> marketingevents = GetAssignedMarketingEventsForCurrentUser();
                List<PurchaseOrder> purchaseorders = GetAssignedPurchaseOrdersForCurrentUser();
                List<SalesOrder> salesorders = GetAssignedSalesOrdersForCurrentUser();
                List<Payment> payments = GetAssignedPaymentsForCurrentUser();
                List<Bills> bills = GetAssignedBillsForCurrentUser();
                List<Cases> cases = GetAssignedCasesForCurrentUser();
                List<Topic> topics = GetAssignedTopicsForCurrentUser();
                List<Assignment> assignments = GetAssignedAssignmentsForCurrentUser();
                DateTime notificationThreshold = DateTime.Now.AddDays(2); // Define the threshold for approaching deadlines

                foreach(BasicTask task in tasks)
                {
                    if(task.DueDate <= notificationThreshold)
                    {
                        // Create a notification object
                        Notification notification = new Notification(session)
                        {
                            Message = $"Task '{task.Subject}' is due on {task.DueDate.ToShortDateString()}.\n",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                foreach(Quote proposal in proposals)
                {
                    if(proposal.ValidUntil <= notificationThreshold)
                    {
                        Notification notification = new Notification(session)
                        {
                            Message =
                                $"Proposal '{proposal.Title}' is due on {proposal.ValidUntil.ToShortTimeString()}.\n",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                foreach(Opportunity opportunity in opportunities)
                {
                    if(opportunity.EstimatedCloseDate <= notificationThreshold)
                    {
                        Notification notification = new Notification(session)
                        {
                            Message =
                                $"Opportunity '{opportunity.OpportunityName}' is due on {opportunity.EstimatedCloseDate.ToShortTimeString()}.\n",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                foreach(Campaign campaign in campaigns)
                {
                    if(campaign.EndDate <= notificationThreshold)
                    {
                        Notification notification = new Notification(session)
                        {
                            Message = $"Campaign '{campaign.Name}' is due on {campaign.EndDate.ToShortTimeString()}.\n",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                foreach(MarketingEvent marketingevent in marketingevents)
                {
                    Notification notification = new Notification(session)
                    {
                        Message = $"Marketing Event '{marketingevent.EventName}' is assigned to you.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(PurchaseOrder purchaseOrder in purchaseorders)
                {
                    Notification notification = new Notification(session)
                    {
                        Message =
                            $"Purchase Order '{purchaseOrder.PurchaseOrderSubject}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(SalesOrder salesOrder in salesorders)
                {
                    Notification notification = new Notification(session)
                    {
                        Message = $"Sales Order '{salesOrder.SalesOrderSubject}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(Payment payment in payments)
                {
                    Notification notification = new Notification(session)
                    {
                        Message = $"Payment '{payment.PaymentNumber}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(Bills bill in bills)
                {
                    if(bill.SupplierDueDate <= notificationThreshold)
                    {
                        Notification notification = new Notification(session)
                        {
                            Message = $"Bill '{bill.BillSubject}' is due on {bill.SupplierDueDate.ToShortTimeString()}.\n",
                            Timestamp = DateTime.Now
                        };

                        notifications.Add(notification);
                    }
                }

                foreach(Cases acase in cases)
                {
                    Notification notification = new Notification(session)
                    {
                        Message = $"Case No. '{acase.CaseNumber}' assigned to you, is still pending.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(Topic topic in topics)
                {
                    Notification notification = new Notification(session)
                    {
                        Message = $"Please provide more information on the topic '{topic.Name}' assigned to you.\n",
                        Timestamp = DateTime.Now
                    };

                    notifications.Add(notification);
                }

                foreach(Assignment assignment in assignments)
                {
                    if(assignment.DueDate <= notificationThreshold)
                    {
                        Notification notification = new Notification(session)
                        {
                            Message =
                                $"Task '{assignment.Description}' is due on {assignment.DueDate.ToShortTimeString()}.\n",
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

            private List<Quote> GetAssignedProposalsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Quote> assignedTasks = session.Query<Quote>()
                        .Where(proposal => proposal.AssignedTo == currentUser)
                        .ToList();

                    return assignedTasks;
                }

                return new List<Quote>();
            }

            private List<Opportunity> GetAssignedOpportunitiesForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Opportunity> assignedOpportunities = session.Query<Opportunity>()
                        .Where(opportunity => opportunity.AssignedTo == currentUser)
                        .ToList();

                    return assignedOpportunities;
                }

                return new List<Opportunity>();
            }

            private List<Campaign> GetAssignedCampaignsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Campaign> assignedCampaigns = session.Query<Campaign>()
                        .Where(campaign => campaign.AssignedTo == currentUser)
                        .ToList();

                    return assignedCampaigns;
                }

                return new List<Campaign>();
            }

            private List<MarketingEvent> GetAssignedMarketingEventsForCurrentUser()
            {
                // Retrieve the current user's ID or any other identifier
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    // Implement the logic to query the DB and retrieve assigned marketing events for the current user
                    List<MarketingEvent> assignedMarketingEvents = session.Query<MarketingEvent>()
                        .Where(marketingEvent => marketingEvent.AssignedTo == currentUser)
                        .ToList();

                    return assignedMarketingEvents;
                }

                // Return an empty list if the current user is not found or invalid
                return new List<MarketingEvent>();
            }

            private List<PurchaseOrder> GetAssignedPurchaseOrdersForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<PurchaseOrder> assignedPurchaseOrders = session.Query<PurchaseOrder>()
                        .Where(purchaseOrder => purchaseOrder.AssignedTo == currentUser)
                        .ToList();

                    return assignedPurchaseOrders;
                }

                return new List<PurchaseOrder>();
            }

            private List<SalesOrder> GetAssignedSalesOrdersForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<SalesOrder> assignedSalesOrders = session.Query<SalesOrder>()
                        .Where(salesOrder => salesOrder.AssignedTo == currentUser)
                        .ToList();

                    return assignedSalesOrders;
                }

                return new List<SalesOrder>();
            }

            private List<Payment> GetAssignedPaymentsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Payment> assignedPayments = session.Query<Payment>()
                        .Where(payment => payment.AssignedTo == currentUser)
                        .ToList();

                    return assignedPayments;
                }

                return new List<Payment>();
            }

            private List<Bills> GetAssignedBillsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Bills> assignedBills = session.Query<Bills>()
                        .Where(bill => bill.AssignedTo == currentUser)
                        .ToList();

                    return assignedBills;
                }

                return new List<Bills>();
            }

            private List<Cases> GetAssignedCasesForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Cases> assignedCases = session.Query<Cases>()
                        .Where(caseEntity => caseEntity.AssignedTo == currentUser)
                        .ToList();

                    return assignedCases;
                }

                return new List<Cases>();
            }

            private List<Topic> GetAssignedTopicsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Topic> assignedTopics = session.Query<Topic>()
                        .Where(topic => topic.AssignedTo == currentUser)
                        .ToList();

                    return assignedTopics;
                }

                return new List<Topic>();
            }

            private List<Assignment> GetAssignedAssignmentsForCurrentUser()
            {
                ApplicationUser currentUser = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if(currentUser != null)
                {
                    List<Assignment> assignedAssignments = session.Query<Assignment>()
                        .Where(assignment => assignment.AssignedTo == currentUser)
                        .ToList();

                    return assignedAssignments;
                }

                return new List<Assignment>();
            }
        }
    }
}
