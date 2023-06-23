using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.CustomerManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CommunicationEssentials
{

    [ImageName("BO_Phone")]
    public class PhoneCall : Communication
    {
        public PhoneCall(Session session)
            : base(session)
        {
            Type = CommunicationType.Phone;

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string subject;

        public string GetSubject()
        {
            return subject;
        }

        public void SetSubject(string value)
        {
            subject = value;
        }

        [Size(300)]
        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        [ReadOnly(true)]
        [Editable(false)]
        public DateTime EndOn { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Contact Participant { get; set; }

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
}