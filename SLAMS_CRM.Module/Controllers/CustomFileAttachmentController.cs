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
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraRichEdit;
using SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomFileAttachmentController : ViewController
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
