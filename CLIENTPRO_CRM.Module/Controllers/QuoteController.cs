using CLIENTPRO_CRM.Module.BusinessObjects.FinancialManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace CLIENTPRO_CRM.Module.Controllers
{
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
    }
}
