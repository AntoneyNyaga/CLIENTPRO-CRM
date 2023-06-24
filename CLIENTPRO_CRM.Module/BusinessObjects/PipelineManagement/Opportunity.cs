using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.ActivityStreamManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement;
using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CLIENTPRO_CRM.Module.BusinessObjects.PipelineManagement
{
    [DefaultClassOptions]
    [NavigationItem("Sales & Marketing")]
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


        Account accountName;
        ApplicationUser assignedTo;
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
            set => SetPropertyValue(nameof(OpportunityName), ref opportunityName, value?.ToUpper());
        }


        [RuleRequiredField("RuleRequiredField for Opportunity.OpportunityDescription", DefaultContexts.Save)]
        [Size(4096)]
        public string OpportunityDescription
        {
            get => opportunityDescription;
            set => SetPropertyValue(nameof(OpportunityDescription), ref opportunityDescription, value);
        }


        [Association("ApplicationUser-Opportunities")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        //public Account AccountName { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Stage
        {
            get => stage == null ? 0 : (int)Enum.Parse(typeof(StageType), stage);
            set
            {
                SetPropertyValue(nameof(Stage), ref stage, Enum.GetName(typeof(StageType), value));
                CalculateProbabilityOfClosing();
            }
        }


        public StageType StageType { get => (StageType)Stage; set => Stage = (int)value; }

        [ModelDefault("AllowEdit", "false")]
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Opportunity-Quotes")]
        public XPCollection<Quote> Quotes
        {
            get
            {
                return GetCollection<Quote>(nameof(Quotes));
            }
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int LeadSource
        {
            get => leadSource == null ? 0 : (int)Enum.Parse(typeof(LeadSource), leadSource);
            set { SetPropertyValue(nameof(LeadSource), ref leadSource, Enum.GetName(typeof(LeadSource), value)); }
        }


        public LeadSource LeadSourceType { get => (LeadSource)LeadSource; set => LeadSource = (int)value; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Opportunity-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        [Association("Account-Opportunities")]
        public Account AccountName
        {
            get => accountName;
            set => SetPropertyValue(nameof(AccountName), ref accountName, value);
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

        DateTime modifiedOn;
        DateTime createdOn;

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

        protected override void OnSaving()
        {
            if (Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            }
            else
            {
                AddActivityStreamEntry("modified", SecuritySystem.CurrentUser as ApplicationUser);
            }
            ModifiedOn = DateTime.Now;

            CalculateProbabilityOfClosing();

            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = OpportunityName,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save(GetType().Name); // Pass the class name as a parameter
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