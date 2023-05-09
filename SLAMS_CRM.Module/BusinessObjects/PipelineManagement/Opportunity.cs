﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.AccountingEssentials;
using SLAMS_CRM.Module.BusinessObjects.OrderManagement;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAMS_CRM.Module.BusinessObjects.PipelineManagement
{
    [DefaultClassOptions]
    [NavigationItem("Opportunities")]
    [Persistent("Opportunity")]
    [ImageName("ProductQuickShippments")]

    public class Opportunity : BaseObject
    {
        public Opportunity(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        decimal opportunityAmount;
        DateTime estimatedCloseDate;
        double probabilityOfClosing;
        string opportunityDescription;
        string opportunityName;
        string stage;
        string leadSource;

        [RuleRequiredField("RuleRequiredField for Opportunity.Opportunityname", DefaultContexts.Save)]
        [Size(50)]
        public string OpportunityName
        {
            get => opportunityName;
            set => SetPropertyValue(nameof(OpportunityName), ref opportunityName, value);
        }


        [RuleRequiredField("RuleRequiredField for Opportunity.OpportunityDescription", DefaultContexts.Save)]
        [Size(4096)]
        public string OpportunityDescription
        {
            get => opportunityDescription;
            set => SetPropertyValue(nameof(OpportunityDescription), ref opportunityDescription, value);
        }

        [Association("ApplicationUser-Opportunity")]
        public XPCollection<ApplicationUser> AssignedUsers
        {
            get
            {
                return GetCollection<ApplicationUser>(nameof(AssignedUsers));
            }
        }

        public Account AccountName { get; set; }

        [Browsable(false)]
        public int Stage
        {
            get => stage == null ? 0 : (int)Enum.Parse(typeof(StageType), stage);
            set
            {
                SetPropertyValue(nameof(Stage), ref stage, Enum.GetName(typeof(StageType), value));
                CalculateProbabilityOfClosing();
            }
        }

        [NotMapped]
        public StageType StageType { get => (StageType)Stage; set => Stage = (int)value; }


        [ReadOnly(false)]
        public double ProbabilityOfClosing
        {
            get => probabilityOfClosing;
            set => SetPropertyValue(nameof(ProbabilityOfClosing), ref probabilityOfClosing, value);
        }

        [RuleRequiredField("RuleRequiredField for Opportunity.EstimatedCloseDate", DefaultContexts.Save)]

        public DateTime EstimatedCloseDate
        {
            get => estimatedCloseDate;
            set => SetPropertyValue(nameof(EstimatedCloseDate), ref estimatedCloseDate, value);
        }

        [Browsable(false)]
        public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();


        [Browsable(false)]
        public int LeadSource
        {
            get => leadSource == null ? 0 : (int)Enum.Parse(typeof(LeadSource), leadSource);
            set { SetPropertyValue(nameof(LeadSource), ref leadSource, Enum.GetName(typeof(LeadSource), value)); }
        }

        [NotMapped]
        public LeadSource LeadSourceType { get => (LeadSource)LeadSource; set => LeadSource = (int)value; }

        [Browsable(false)]
        [Association("Opportunity-SalesOrders")]
        public XPCollection<SalesOrder> SalesOrders
        {
            get
            {
                return GetCollection<SalesOrder>(nameof(SalesOrders));
            }
        }

        public decimal OpportunityAmount
        {
            get => opportunityAmount;
            set => SetPropertyValue(nameof(OpportunityAmount), ref opportunityAmount, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            CalculateProbabilityOfClosing();
        }


        public void CalculateProbabilityOfClosing()
        {
            ProbabilityOfClosing = StageType switch
            {
                StageType.Prospecting => 0.1,
                StageType.Qualification => 0.2,
                StageType.NeedsAnalysis => 0.3,
                StageType.ValueProposition => 0.3,
                StageType.IdDecisionMakers => 0.4,
                StageType.PerceptionAnalysis => 0.5,
                StageType.ProposalPriceQuote => 0.6,
                StageType.NegotiationReview => 0.7,
                StageType.ClosedWon => 0.8,
                StageType.ClosedLost => 0.9,
                _ => 1,
            };
        }
    }

    public enum StageType
    {
        Prospecting,
        Qualification,
        NeedsAnalysis,
        ValueProposition,
        IdDecisionMakers,
        PerceptionAnalysis,
        ProposalPriceQuote,
        NegotiationReview,
        ClosedWon,
        ClosedLost
    }

    public enum LeadSource
    {
        ColdCall,
        ExistingCustomer,
        SelfGenerated,
        Employee,
        Partner,
        PublicRelations,
        DirectMail,
        Conference,
        TradeShow,
        Website,
        WordOfMouth,
        Email,
        Campaign,
        Other
    }
}