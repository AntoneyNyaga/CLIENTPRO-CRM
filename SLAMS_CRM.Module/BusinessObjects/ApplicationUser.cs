using DevExpress.ExpressApp.Security;
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
    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    [Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
    {
        ApplicationUserLoginInfo result = new(Session)
        {
            LoginProviderName = loginProviderName,
            ProviderUserKey = providerUserKey,
            User = this
        };
        return result;
    }

    //[Browsable(false)]
    //public IList<Quote> Quotes { get; } = new ObservableCollection<Quote>();

    [Association("ApplicationUser-Quote")]
    public XPCollection<Quote> AssignedProposals
    {
        get
        {
            return GetCollection<Quote>(nameof(AssignedProposals));
        }
    }

    //[Browsable(false)]
    //public IList<Opportunity> Opportunities { get; set; } = new ObservableCollection<Opportunity>();
    [Association("ApplicationUser-Opportunity")]
    public XPCollection<Opportunity> AssignedOpportunities
    {
        get
        {
            return GetCollection<Opportunity>(nameof(AssignedOpportunities));
        }
    }

    [Association("ApplicationUser-Assignment")]
    public XPCollection<Assignment> Tasks
    {
        get
        {
            return GetCollection<Assignment>(nameof(Tasks));
        }
    }

    [Association("ApplicationUser-Campaigns")]
    public XPCollection<Campaign> Campaigns
    {
        get
        {
            return GetCollection<Campaign>(nameof(Campaigns));
        }
    }

    [Association("ApplicationUser-MarketingEvents")]
    public XPCollection<MarketingEvent> MarketingEvents
    {
        get
        {
            return GetCollection<MarketingEvent>(nameof(MarketingEvents));
        }
    }

    [Association("ApplicationUser-SalesOrders")]
    public XPCollection<SalesOrder> SalesOrders
    {
        get
        {
            return GetCollection<SalesOrder>(nameof(SalesOrders));
        }
    }

    [Association("ApplicationUser-PurchaseOrders")]
    public XPCollection<PurchaseOrder> PurchaseOrders
    {
        get
        {
            return GetCollection<PurchaseOrder>(nameof(PurchaseOrders));
        }
    }

    [Association("ApplicationUser-Payments")]
    public XPCollection<Payment> Payments
    {
        get
        {
            return GetCollection<Payment>(nameof(Payments));
        }
    }

    [Association("ApplicationUser-Bills")]
    public XPCollection<Bills> Bills
    {
        get
        {
            return GetCollection<Bills>(nameof(Bills));
        }
    }
    [Association("ApplicationUser-Cases")]
    public XPCollection<Cases> Cases
    {
        get
        {
            return GetCollection<Cases>(nameof(Cases));
        }
    }
    [Association("ApplicationUser-Topics")]
    public XPCollection<Topic> Topics
    {
        get
        {
            return GetCollection<Topic>(nameof(Topics));
        }
    }
}
