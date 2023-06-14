using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;

namespace CLIENTPRO_CRM.Module.Reports
{
    public partial class InvoiceReporting : DevExpress.XtraReports.UI.XtraReport
    {
        public InvoiceReporting()
        {
            InitializeComponent();
        }
        public void SetDataSource(Invoice invoice)
        {
            // Set the data source for the report using the provided invoice object
            // Access the report's data source and assign the invoice object
            DataSource = new Invoice[] { invoice };
        }
    }
}
