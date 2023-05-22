using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.OrderManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.PipelineManagement
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
        
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public EventType Type { get; set; }
        public Product Product { get; set; }
        public string NumberOfSessions { get; set; }
        public EventFormatType? Format { get; set; }

        ApplicationUser assignedTo;

        [Association("ApplicationUser-MarketingEvents")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
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