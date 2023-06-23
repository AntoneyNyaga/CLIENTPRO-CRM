using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.PipelineManagement
{
    [DefaultClassOptions]
    [ImageName("CreateLine3DChart")]
    [NavigationItem("Sales & Marketing")]
    public class MarketingEvent : BaseObject
    {
        public MarketingEvent(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }        
        public string EventName
        {
            get => eventName;
            set => SetPropertyValue(nameof(EventName), ref eventName, value?.ToUpper());
        }
        public string EventDescription { get; set; }
        public EventType Type { get; set; }
        public Product Product { get; set; }
        public string NumberOfSessions { get; set; }
        public EventFormatType? Format { get; set; }

        string eventName;
        ApplicationUser assignedTo;

        [Association("ApplicationUser-MarketingEvents")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

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
                AccountName = EventName,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
        }
    }

    public enum EventFormatType
    {
        TeleSeminar,
        Seminar,
        Program,
        LiveEvent,
        Material
    }
}