using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.PipelineManagement
{
    [DefaultClassOptions]
    [NavigationItem("Opportunities")]
    [ImageName("ChartPoints")]
    public class Campaign : BaseObject
    { 
        public Campaign(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [RuleRequiredField]
        public string Name { get; set; }
        public string Description { get; set; }

        [RuleRequiredField]
        public CampaignStatus? Status { get; set; }

        [RuleRequiredField]
        public CampaignType? Type { get; set; }

        //assignedTo
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
        public double Budget { get; set; }
        public int Impressions { get; set; }
        public double ExpectedCost { get; set; }
        public double ActualCost { get; set; }
        public string OpportunitiesWon { get; set; }
        public double ExpectedRevenue { get; set; }
        public double CostPerimpression { get; set; }
        public double CostPerClickThrough { get; set; }

        [Size(4096)]
        public string Objective { get; set; }

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