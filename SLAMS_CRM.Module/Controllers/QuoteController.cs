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
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QuoteController : ObjectViewController<ListView, Quote>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public QuoteController()
        {
            var followUpAction = new SimpleAction(this, "FollowUpAction", PredefinedCategory.Edit)
            {
                Caption = "Follow Up",
                ConfirmationMessage = "Are you sure you want to follow up on this quote?",
                ImageName = "FollowUp",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects

            };
            followUpAction.Execute += FollowUpAction_Execute;

            var sendQuoteAction = new SimpleAction(this, "SendQuoteAction", PredefinedCategory.Edit)
            {
                Caption = "Send Quote",
                ConfirmationMessage = "Are you sure you want to send this quote to the customer?",
                ImageName = "Actions_Send",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            };
            sendQuoteAction.Execute += SendQuoteAction_Execute;
        }

        private void FollowUpAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is not Quote quote)
            {
                return;
            }

            quote.FollowUp();
            View.ObjectSpace.CommitChanges();
        }

        private void SendQuoteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is not Quote quote)
            {
                return;
            }

            quote.SendQuote();
            View.ObjectSpace.CommitChanges();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
