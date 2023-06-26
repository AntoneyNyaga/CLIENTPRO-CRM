using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using TaskStatus = DevExpress.Persistent.Base.General.TaskStatus;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    [DefaultProperty("Subject")]
    [ImageName("BO_Task")]
    public class BasicTask : BaseObject, BasicITask
    {
        ApplicationUser assignedTo;
        private BasicTaskImpl task = new BasicTaskImpl();

        [Persistent("DateCompleted")]
        private DateTime dateCompleted
        {
            get
            {
                return task.DateCompleted;
            }
            set
            {
                DateTime dateTime = task.DateCompleted;
                task.DateCompleted = value;
                OnChanged("dateCompleted", dateTime, task.DateCompleted);
            }
        }

        public string Subject
        {
            get
            {
                return task.Subject;
            }
            set
            {
                string subject = task.Subject;
                task.Subject = value;
                OnChanged("Subject", subject, task.Subject.ToUpper());
            }
        }

        [Size(-1)]
        [ObjectValidatorIgnoreIssue(new Type[] { typeof(ObjectValidatorLargeNonDelayedMember) })]
        public string Description
        {
            get
            {
                return task.Description;
            }
            set
            {
                string description = task.Description;
                task.Description = value;
                OnChanged("Description", description, task.Description);
            }
        }

        public DateTime DueDate
        {
            get
            {
                return task.DueDate;
            }
            set
            {
                DateTime dueDate = task.DueDate;
                task.DueDate = value;
                OnChanged("DueDate", dueDate, task.DueDate);
            }
        }

        public DateTime StartDate
        {
            get
            {
                return task.StartDate;
            }
            set
            {
                DateTime startDate = task.StartDate;
                task.StartDate = value;
                OnChanged("StartDate", startDate, task.StartDate);
            }
        }

        [Association("ApplicationUser-Assignments")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        public TaskStatus Status
        {
            get
            {
                return task.Status;
            }
            set
            {
                TaskStatus status = task.Status;
                task.Status = value;
                OnChanged("Status", status, task.Status);
            }
        }

        public int PercentCompleted
        {
            get
            {
                return task.PercentCompleted;
            }
            set
            {
                int percentCompleted = task.PercentCompleted;
                task.PercentCompleted = value;
                OnChanged("PercentCompleted", percentCompleted, task.PercentCompleted);
            }
        }

        public DateTime DateCompleted => dateCompleted;

        public BasicTask(Session session)
            : base(session)
        {
        }

        protected override void OnLoading()
        {
            task.IsLoading = true;
            base.OnLoading();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            task.DateCompleted = dateCompleted;
            task.IsLoading = false;
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted()
        {
            TaskStatus status = task.Status;
            task.MarkCompleted();
            OnChanged("Status", status, task.Status);
            OnChanged("PercentCompleted");
        }
    }
}