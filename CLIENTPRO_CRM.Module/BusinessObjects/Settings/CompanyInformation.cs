using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Settings
{
    //[DefaultClassOptions]
    [ImageName("EditNames")]
    public class CompanyInformation : BaseObject
    {
        public CompanyInformation(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("CompanyInformation-Invoices")]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhonenumber { get; set; }
        public string CompanyWebsite { get; set; }
        public BusinessModel BusinessModel { get; set; }

        public DateTime FiscalYearStart { get; set; }

        public int HoursInWorkDay { get; set; }

        public TimesheetPeriod TimesheetPeriod { get; set; }

        public StandardPdfPaperSize StandardPdfPaperSize { get; set; }

        public ForecastPeriod ForecastPeriod { get; set; }

        public StandardPdfFont StandardPdfFont { get; set; }

        public bool RetainForecastPeriods { get; set; }

        public string FooterTextForPdfFilesAndHtmlReports { get; set; }

        public bool RetainHistoryPeriods { get; set; }

        public string FooterLinkUrlForPdfFilesAndHtmlReports { get; set; }

        public TimeSpan WorkDayBegins { get; set; }

        public DayOfWeek DefaultFirstDayOfWeek { get; set; }

        public TimeSpan WorkDayEnds { get; set; }

        public bool ShowVacationsInCalendar { get; set; }

        [Size(-1)]
        [Delayed(true)]
        [ImageEditor]
        public byte[] CompanyLogo
        {
            get { return GetDelayedPropertyValue<byte[]>("CompanyLogo"); }
            set { SetDelayedPropertyValue("CompanyLogo", value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Address CompanyAddress { get; set; }
    }

    public enum BusinessModel
    {
        BusinessToBusiness,
        BusinessToCustomer,
        // Add more business models as needed
    }

    // Enum for Timesheet Period
    public enum TimesheetPeriod
    {
        Weekly,
        BiWeekly,
        Monthly,
    }

    // Enum for Standard PDF Paper Size
    public enum StandardPdfPaperSize
    {
        A4,
        Letter,
        // Add more paper sizes as needed
    }

    // Enum for Standard PDF Font
    public enum StandardPdfFont
    {
        Arial,
        TimesNewRoman,
    }
    public enum ForecastPeriod
    {
        Monthly,
        Quarterly,

    }
}