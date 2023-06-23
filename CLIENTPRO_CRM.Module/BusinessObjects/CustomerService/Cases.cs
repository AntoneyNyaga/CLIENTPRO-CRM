using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerService
{
    [DefaultClassOptions]
    [ImageName("Warning")]
    [NavigationItem("Customer Service & Settings")]


    public class Cases : BaseObject
    {
        public Cases(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string CaseNumber { get; set; }
        public CaseType Type { get; set; }

        [Size(400)]
        public string Subject { get; set; }

        Product asset;
        Account primaryAccount;
        ApplicationUser assignedTo;

        [Association("ApplicationUser-Cases")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }
        public CaseStatus Status { get; set; }
        public CasePriority Priority { get; set; }
        public CaseCategory Category { get; set; }
        public DefaultBookingCategoryType DefaultBookingCategory { get; set; }

        [Association("Account-Cases")]
        public Account PrimaryAccount
        {
            get => primaryAccount;
            set => SetPropertyValue(nameof(PrimaryAccount), ref primaryAccount, value);
        }

        [Association("Product-Cases")]
        public Product Asset
        {
            get => asset;
            set => SetPropertyValue(nameof(Asset), ref asset, value);
        }
        public DateTime DateClosed { get; set; }
        public DateTime DateBilled { get; set; }
        [Size(4096)]
        public string Description { get; set; }
        [Size(4096)]
        public string Resolution { get; set; }

        DateTime modifiedOn;
        DateTime createdOn;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        protected override void OnSaving()
        {
            if (Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            }
            else
            {
                AddActivityStreamEntry("modified", SecuritySystem.CurrentUser as ApplicationUser);
            }
            ModifiedOn = DateTime.Now;
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = Subject,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }

    public enum CaseType
    {
        InternalDevelopment,
        InternalMaintenance,
        InternalSupport,
        InternalTraining,
        ContractDevelopment,
        ContractMaintenance,
        ContractSupport,
        ContractTraining
    }

    public enum CaseStatus
    {
        Assigned,
        Pending,
        Completed,
        NoFault,
        Expired
    }

    public enum CasePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum CaseCategory
    {
        Documentation,
        OfficeSupplies,
        ComputerHardware,
        ServerHardware,
        Networking,
        ClientSoftware,
        ServerSoftware,
        UserTraining,
        Website
    }

    public enum DefaultBookingCategoryType
    {
        BidDevelopment,
        Maintenance,
        Support,
        Training
    }
}