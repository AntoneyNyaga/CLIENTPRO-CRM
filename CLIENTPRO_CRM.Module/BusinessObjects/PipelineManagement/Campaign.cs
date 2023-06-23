using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.PipelineManagement
{
    [DefaultClassOptions]
    [NavigationItem("Sales & Marketing")]
    [ImageName("ChartPoints")]
    public class Campaign : BaseObject
    {
      /* int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }*/
        public Campaign(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [RuleRequiredField]
        //public string Name { get; set; }
        
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value?.ToUpper());
        }
        public string Description { get; set; }

        [RuleRequiredField]
        public CampaignStatus? Status { get; set; }

        [RuleRequiredField]
        public CampaignType? Type { get; set; }

        //assignedTo
        string name;
        ApplicationUser assignedTo;

        [RuleRequiredField]
        [Association("ApplicationUser-Campaigns")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public int Impressions { get; set; }
        public decimal ExpectedCost { get; set; }
        public decimal ActualCost { get; set; }
        public string OpportunitiesWon { get; set; }
        public decimal ExpectedRevenue { get; set; }
        public int CostPerimpression { get; set; }
        public decimal CostPerClickThrough { get; set; }

        [Size(4096)]
        public string Objective { get; set; }

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
                AccountName = Name,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }
    public enum CampaignStatus
    {
        [ImageName("State_Validation_Invalid")]
        Planning,
        [ImageName("State_Validation_Valid")]
        Active,
        [ImageName("State_Validation_Invalid")]
        Inactive,
        [ImageName("State_Validation_Invalid")]
        Completed,
        [ImageName("State_Validation_Invalid")]
        InQueue,
        [ImageName("State_Validation_Invalid")]
        Sending

    }
    public enum CampaignType
    {
        [ImageName("State_Validation_Valid")]
        Email,
        [ImageName("State_Validation_Invalid")]
        Phone,
        [ImageName("State_Validation_Invalid")]
        Mail,
        [ImageName("State_Validation_Invalid")]
        Web,
        [ImageName("State_Validation_Invalid")]
        Television,
        [ImageName("State_Validation_Invalid")]
        Newsletter,
    }
}