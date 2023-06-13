using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials;
using SLAMS_CRM.Module.BusinessObjects.CustomerService;
using SLAMS_CRM.Module.BusinessObjects.OrderManagement;
using SLAMS_CRM.Module.BusinessObjects.PipelineManagement;
using System.ComponentModel;

namespace SLAMS_CRM.Module.BusinessObjects;

[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(UserName))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo
{
    public ApplicationUser(Session session) : base(session)
    {
    }

    [VisibleInDetailView(false)]
    [VisibleInListView(false)]
    [VisibleInLookupListView(false)]
    [Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(
        string loginProviderName,
        string providerUserKey)
    {
        ApplicationUserLoginInfo result = new(Session)
        {
            LoginProviderName = loginProviderName,
            ProviderUserKey = providerUserKey,
            User = this
        };
        return result;
    }



    [Association("ApplicationUser-Quotes")]
    public XPCollection<Quote> AssignedProposals { get { return GetCollection<Quote>(nameof(AssignedProposals)); } }

    [Association("ApplicationUser-Opportunities")]
    public XPCollection<Opportunity> AssignedOpportunities
    {
        get { return GetCollection<Opportunity>(nameof(AssignedOpportunities)); }
    }

    [Association("ApplicationUser-Assignment")]
    public XPCollection<Assignment> AssignedTasks { get { return GetCollection<Assignment>(nameof(AssignedTasks)); } }

    [Association("ApplicationUser-Campaigns")]
    public XPCollection<Campaign> AssignedCampaigns
    {
        get { return GetCollection<Campaign>(nameof(AssignedCampaigns)); }
    }

    [Association("ApplicationUser-MarketingEvents")]
    public XPCollection<MarketingEvent> AssignedMarketingEvents
    {
        get { return GetCollection<MarketingEvent>(nameof(AssignedMarketingEvents)); }
    }

    [Association("ApplicationUser-SalesOrders")]
    public XPCollection<SalesOrder> AssignedSalesOrders
    {
        get { return GetCollection<SalesOrder>(nameof(AssignedSalesOrders)); }
    }

    [Association("ApplicationUser-PurchaseOrders")]
    public XPCollection<PurchaseOrder> AssignedPurchaseOrders
    {
        get { return GetCollection<PurchaseOrder>(nameof(AssignedPurchaseOrders)); }
    }

    [Association("ApplicationUser-Payments")]
    public XPCollection<Payment> AssignedPayments { get { return GetCollection<Payment>(nameof(AssignedPayments)); } }

    [Association("ApplicationUser-Bills")]
    public XPCollection<Bills> AssignedBills { get { return GetCollection<Bills>(nameof(AssignedBills)); } }

    [Association("ApplicationUser-Cases")]
    public XPCollection<Cases> AssignedCases { get { return GetCollection<Cases>(nameof(AssignedCases)); } }

    [Association("ApplicationUser-Topics")]
    public XPCollection<Topic> AssignedTopics { get { return GetCollection<Topic>(nameof(AssignedTopics)); } }
}
