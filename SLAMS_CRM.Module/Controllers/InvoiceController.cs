using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SLAMS_CRM.Module.BusinessObjects;

namespace SLAMS_CRM.Module.Controllers
{
    public partial class InvoiceController : ObjectViewController<ListView, Quote>
    {
        public InvoiceController()
        {
            var simpleAction = new SimpleAction(this, "GenerateInvoiceAction", PredefinedCategory.Edit)
            {
                Caption = "Generate Invoice",
                ImageName = "BO_Invoice",
                ConfirmationMessage = "Are you sure you want to generate an invoice for the selected account?",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
            simpleAction.Execute += SimpleAction_Execute;
        }

        private void SimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var invoice = View.ObjectSpace.CreateObject<Invoice>();
            // Add any necessary properties to the new invoice
            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
            View.SelectedObjects.Clear();
            View.SelectedObjects.Add(invoice);
        }
    }
}
