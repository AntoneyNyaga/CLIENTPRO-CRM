using CLIENTPRO_CRM.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement
{
    public class MyActivityStream : BaseObject
    {
        public MyActivityStream(Session session) : base(session)
        {
        }

        public override void AfterConstruction() { base.AfterConstruction(); }

        private string createdBy;
        private DateTime date;
        private string action;
        private string accountName;
        DateTime modifiedOn;
        DateTime createdOn;

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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClassName { get; set; }

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

        public void Save(string className)
        {
            ClassName = className;
            base.Save();
        }


        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        public string Description
        {
            get
            {
                string createdByUserText = string.IsNullOrEmpty(CreatedBy) ? "Someone" : CreatedBy;
                string actionDescription = Action == "created" ? "added" : "modified";
                string classText = string.IsNullOrEmpty(ClassName) ? "an item" : $"a {ClassName} item";
                string timeAgo = GetTimeAgo(Date);

                return $"{createdByUserText} {actionDescription} {classText} '{AccountName}'\n{timeAgo}";
            }
        }


        private string GetTimeAgo(DateTime dateTime)
        {
            TimeSpan timeDifference = DateTime.Now - dateTime;

            if(timeDifference.TotalMinutes < 1)
            {
                return "Just now";
            } else if(timeDifference.TotalHours < 1)
            {
                int minutes = (int)timeDifference.TotalMinutes;
                return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
            } else if(timeDifference.TotalDays < 1)
            {
                int hours = (int)timeDifference.TotalHours;
                return $"{hours} hour{(hours != 1 ? "s" : "")} ago";
            } else
            {
                int days = (int)timeDifference.TotalDays;
                return $"{days} day{(days != 1 ? "s" : "")} ago";
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
            ApplicationUser currentUser,
            string className)
        {
            if(!HasActivityStreamEntry(session, action, accountName))
            {
                var activityStreamEntry = new MyActivityStream(session)
                {
                    AccountName = accountName,
                    Action = action,
                    Date = DateTime.Now,
                    CreatedBy = currentUser?.UserName
                };
                activityStreamEntry.Save(className);
            }
        }

        public static MyActivityStream[] GetRecentActivityStreamEntries(Session session, int count)
        {
            return session.Query<MyActivityStream>()
                .OrderByDescending(entry => entry.ModifiedOn)
                .Take(count)
                .ToArray();
        }
    }
}
