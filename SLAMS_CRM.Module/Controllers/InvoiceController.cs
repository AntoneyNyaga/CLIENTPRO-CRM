using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SLAMS_CRM.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.Controllers
{
    public partial class InvoiceController : ObjectViewController<ListView, Invoice>
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
            View.SelectedObjects.Clear();
            View.SelectedObjects.Add(invoice);
        }
    }
}
