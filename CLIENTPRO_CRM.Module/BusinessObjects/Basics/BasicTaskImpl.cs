
using System;
using TaskStatus = DevExpress.Persistent.Base.General.TaskStatus;


namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public class BasicTaskImpl
    {
        private bool isLoading;

        private string subject;

        private string description;

        private DateTime dueDate;

        private DateTime startDate;

        private TaskStatus status;

        private int percentCompleted;

        private DateTime dateCompleted;

        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        public TaskStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == value)
                {
                    return;
                }

                status = value;
                if (IsLoading)
                {
                    return;
                }

                switch (status)
                {
                    case TaskStatus.NotStarted:
                        percentCompleted = 0;
                        break;
                    case TaskStatus.Completed:
                        percentCompleted = 100;
                        break;
                    case TaskStatus.InProgress:
                        if (percentCompleted == 100)
                        {
                            percentCompleted = 75;
                        }

                        break;
                    case TaskStatus.WaitingForSomeoneElse:
                    case TaskStatus.Deferred:
                        if (percentCompleted == 100)
                        {
                            percentCompleted = 0;
                        }

                        break;
                }

                CheckDateCompleted();
            }
        }

        public int PercentCompleted
        {
            get
            {
                return percentCompleted;
            }
            set
            {
                if (percentCompleted == value)
                {
                    return;
                }

                percentCompleted = value;
                if (!IsLoading)
                {
                    if (percentCompleted == 100)
                    {
                        status = TaskStatus.Completed;
                    }

                    if (percentCompleted == 0)
                    {
                        status = TaskStatus.NotStarted;
                    }

                    if (percentCompleted > 0 && percentCompleted < 100)
                    {
                        status = TaskStatus.InProgress;
                    }

                    CheckDateCompleted();
                }
            }
        }

        public DateTime DateCompleted
        {
            get
            {
                return dateCompleted;
            }
            set
            {
                if (isLoading)
                {
                    dateCompleted = value;
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
            }
        }

        private void CheckDateCompleted()
        {
            if (Status == TaskStatus.Completed)
            {
                dateCompleted = DateTime.Now;
            }
            else
            {
                dateCompleted = DateTime.MinValue;
            }
        }

        public void MarkCompleted()
        {
            Status = TaskStatus.Completed;
        }
    }
}