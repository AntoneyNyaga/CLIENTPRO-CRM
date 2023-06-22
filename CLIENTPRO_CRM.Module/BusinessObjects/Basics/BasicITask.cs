using System;
using TaskStatus = DevExpress.Persistent.Base.General.TaskStatus;


namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public interface BasicITask
    {
        string Subject { get; set; }

        string Description { get; set; }

        DateTime DueDate { get; set; }

        DateTime StartDate { get; set; }

        TaskStatus Status { get; set; }

        int PercentCompleted { get; set; }

        DateTime DateCompleted { get; }

        void MarkCompleted();
    }
}