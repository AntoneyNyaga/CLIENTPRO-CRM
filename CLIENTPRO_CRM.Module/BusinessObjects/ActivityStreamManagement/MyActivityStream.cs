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

        public override void AfterConstruction() 
        { 
            CreatedOn = DateTime.Now;
            base.AfterConstruction(); 
        }

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
            if(Session.IsNewObject(this))
            {
                ClassName = className;
                base.Save();
            }
        }

        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        //[Persistent("Description")]
        public string Description
        {
            get
            {
                string classText = string.IsNullOrEmpty(ClassName) ? "an item" : $"a {ClassName} item";
                string timeAgo = GetTimeAgo(Date);
                string description = $"{CreatedBy} {Action} {classText} '{AccountName}'\n{timeAgo}";

                if (string.IsNullOrEmpty(description) || IsDuplicateEntry())
                {
                    if (Session.IsObjectsSaving)
                    {
                        Session.ObjectsSaved += (sender, args) =>
                        {
                            DeleteEntryIfUseless();
                            OnChanged("Description"); // Trigger a change event for the Description property
                        };
                    }
                    else
                    {
                        DeleteEntryIfUseless();
                        description = null; // Set description to null to indicate that it should not be displayed
                    }
                }

                return description;
            }
        }

        private void DeleteEntryIfUseless()
        {
            if (IsDuplicateEntry())
            {
                Session.Delete(this); // Delete the duplicate or null object
            }
        }

        private bool IsDuplicateEntry()
        {
            if (Session.IsObjectsSaving)
            {
                // Postpone the duplicate checking until the saving operation is completed
                Session.ObjectsSaved += (sender, args) =>
                {
                    var previousEntry = Session.FindObject<MyActivityStream>(
                        CriteriaOperator.And(
                            new BinaryOperator("Action", Action),
                            new BinaryOperator("AccountName", AccountName),
                            new BinaryOperator("CreatedBy", CreatedBy),
                            new BinaryOperator("ClassName", ClassName),
                            new BinaryOperator("Date", Date, BinaryOperatorType.Less)
                        )
                    );

                    if (previousEntry != null)
                    {
                        Session.Delete(previousEntry); // Delete the previous duplicate entry
                    }
                };

                return false;
            }
            else
            {
                // Perform the duplicate checking immediately
                var previousEntry = Session.FindObject<MyActivityStream>(
                    CriteriaOperator.And(
                        new BinaryOperator("Action", Action),
                        new BinaryOperator("AccountName", AccountName),
                        new BinaryOperator("CreatedBy", CreatedBy),
                        new BinaryOperator("ClassName", ClassName),
                        new BinaryOperator("Date", Date, BinaryOperatorType.Less)
                    )
                );

                return previousEntry != null;
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

        public static MyActivityStream[] GetRecentActivityStreamEntries(Session session, int count)
        { return session.Query<MyActivityStream>().OrderByDescending(entry => entry.createdOn).Take(count).ToArray(); }
    }
}
