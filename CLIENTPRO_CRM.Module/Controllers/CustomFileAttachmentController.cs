using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace CLIENTPRO_CRM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomFileAttachmentController : ViewController<DetailView>
    {
        private SimpleAction saveToAction;
        private SimpleAction openAction;
        private SimpleAction clearContentAction;

        public CustomFileAttachmentController()
        {
            InitializeComponent();

            // Target required Views (via the TargetXXX properties) and create their Actions.
            saveToAction = new SimpleAction(this, "SaveTo", PredefinedCategory.View);
            saveToAction.Caption = "Save To";
            saveToAction.Execute += SaveToAction_Execute;

            openAction = new SimpleAction(this, "Open", PredefinedCategory.View);
            openAction.Caption = "Open";
            openAction.Execute += OpenAction_Execute;

            clearContentAction = new SimpleAction(this, "ClearContent", PredefinedCategory.Edit);
            clearContentAction.Caption = "Clear Content";
            clearContentAction.Execute += ClearContentAction_Execute;
        }

        private void SaveToAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void OpenAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ClearContentAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }
    }
}
