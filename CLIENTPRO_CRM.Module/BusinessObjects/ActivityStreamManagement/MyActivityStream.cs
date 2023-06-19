using CLIENTPRO_CRM.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement
{
    //[DefaultClassOptions]
    //[NavigationItem("CLIENTPRO CRM")]
    public class MyActivityStream : BaseObject
    {
        /*int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }*/
        public MyActivityStream(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string createdBy;
        private DateTime date;
        private string action;
        private string accountName;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AccountName
        {
            get => accountName;
            set => SetPropertyValue(nameof(AccountName), ref accountName, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Action { get => action; set => SetPropertyValue(nameof(Action), ref action, value); }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime Date { get => date; set => SetPropertyValue(nameof(Date), ref date, value); }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CreatedBy { get => createdBy; set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value); }


        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        public string Description
        {
            get
            {
                string createdByText = string.IsNullOrEmpty(CreatedBy) ? "Someone" : CreatedBy;
                string actionText = Action == "created" ? "created a new" : "modified the";
                return $"{createdByText} {actionText} contact {AccountName}{Environment.NewLine} {((DateTime.Now - Date).Hours > 0 ? (DateTime.Now - Date).Hours + " Hours " : "") + (DateTime.Now - Date).Minutes + " Minutes ago"}";
            }
        }


        public static bool HasActivityStreamEntry(Session session, string action, string accountName)
        {
            return session.FindObject<MyActivityStream>(
                    CriteriaOperator.Parse("Action = ? And AccountName = ?", action, accountName)) !=
                null;
        }

        public static void AddActivityStreamEntry(
            Session session,
            string action,
            string accountName,
            ApplicationUser currentUser)
        {
            if (!HasActivityStreamEntry(session, action, accountName))
            {
                var activityStreamEntry = new MyActivityStream(session)
                {
                    AccountName = accountName,
                    Action = action,
                    Date = DateTime.Now,
                    CreatedBy = currentUser?.UserName
                };
                activityStreamEntry.Save();
            }
        }
    }
}
