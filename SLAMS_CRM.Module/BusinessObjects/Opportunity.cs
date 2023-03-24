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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Opportunities")]
    
    public class Opportunity : BaseObject
    { 
        public Opportunity(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string comments;
        DateTime estimatedCloseDate;
        double probabilityOfClosing;
        string opportunityDescription;
        string opportunityName;
        string stage;

        [RuleRequiredField("RuleRequiredField for Opportunity.Opportunityname", DefaultContexts.Save)]
        [Size(50)]
        public string OpportunityName
        {
            get => opportunityName;
            set => SetPropertyValue(nameof(OpportunityName), ref opportunityName, value);
        }


        [RuleRequiredField("RuleRequiredField for Opportunity.OpportunityDescription", DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string OpportunityDescription
        {
            get => opportunityDescription;
            set => SetPropertyValue(nameof(OpportunityDescription), ref opportunityDescription, value);
        }

        public IList<Lead> Lead { get; set; } = new ObservableCollection<Lead>();

        public IList<Contact> Contact { get; set; } = new ObservableCollection<Contact>();

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

        //[RuleRequiredField("RuleRequiredField for Opportunity.Stage", DefaultContexts.Save)]
        [NotMapped]
        public StageType StageType
        {
            get => (StageType)Stage;
            set => Stage = (int)value;
        }


        //[RuleRequiredField("RuleRequiredField for Opportunity.ProbabilityOfClosing", DefaultContexts.Save)]
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

        
        [Size(SizeAttribute.Unlimited)]
        public string Comments
        {
            get => comments;
            set => SetPropertyValue(nameof(Comments), ref comments, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            CalculateProbabilityOfClosing();
        }

        public void CalculateProbabilityOfClosing()
        {
            switch (StageType)
            {
                case StageType.Prospecting:
                    ProbabilityOfClosing = 0.1;
                    break;
                case StageType.Qualification:
                    ProbabilityOfClosing = 0.2;
                    break;
                case StageType.NeedsAnalysis:
                    ProbabilityOfClosing = 0.3;
                    break;
                case StageType.ValueProposition:
                    ProbabilityOfClosing = 0.3;
                    break;
                case StageType.IdDecisionMakers:
                    ProbabilityOfClosing = 0.4;
                    break;
                case StageType.PerceptionAnalysis:
                    ProbabilityOfClosing = 0.5;
                    break;
                case StageType.ProposalPriceQuote:
                    ProbabilityOfClosing = 0.6;
                    break;
                case StageType.NegotiationReview:
                    ProbabilityOfClosing = 0.7;
                    break;
                case StageType.ClosedWon:
                    ProbabilityOfClosing = 0.8;
                    break;
                case StageType.ClosedLost:
                    ProbabilityOfClosing = 0.9;
                    break;
                default:
                    ProbabilityOfClosing = 1;
                    break;
            }
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

}