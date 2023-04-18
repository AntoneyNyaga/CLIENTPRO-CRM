using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using SLAMS_CRM.Module.BusinessObjects;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Twilio.TwiML.Voice;

public partial class CommunicationController : ObjectViewController<ListView, Communication>
{
    public CommunicationController()
    {
        // Define action items for email, call, reply and forward
        var emailAction = new SimpleAction(this, "Email", PredefinedCategory.Edit)
        {
            Caption = "Send Email",
            ToolTip = "Send an email to the contact or lead associated with this communication",
            ImageName = "Actions_Send",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            //TargetObjectType = typeof(Communication)
            TargetObjectsCriteria = "[Type] == 'Email'"
        };

        // Enable the action when a lead is selected with LeadStatus = Qualified
        //convertAction.TargetObjectsCriteria = "[LeadStatus] == 'Qualified'";

        emailAction.Execute += EmailAction_Execute;

        var callAction = new SimpleAction(this, "Call", PredefinedCategory.Edit)
        {
            Caption = "Make a Call",
            ToolTip = "Make a call to the contact or lead associated with this communication",
            ImageName = "BO_Phone",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            //TargetObjectType = typeof(Communication)
            TargetObjectsCriteria = "[Type] == 'Phone'"
        };
        callAction.Execute += CallAction_Execute;

        var replyAction = new PopupWindowShowAction(this, "Reply", PredefinedCategory.RecordEdit)
        {
            Caption = "Reply",
            ImageName = "PreviousComment",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            //TargetObjectType = typeof(Communication)
            //TargetObjectsCriteria = new BinaryOperator("Type", CommunicationType.Email),
            TargetObjectsCriteria = "[Type] == 'Email'"
        };
        replyAction.CustomizePopupWindowParams += replyAction_CustomizePopupWindowParams;
        replyAction.Execute += replyAction_Execute;

        var forwardAction = new PopupWindowShowAction(this, "Forward", PredefinedCategory.RecordEdit)
        {
            Caption = "Forward",
            ImageName = "Redo",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            //TargetObjectType = typeof(Communication)
            //TargetObjectsCriteria = new BinaryOperator("Type", CommunicationType.Email),
            TargetObjectsCriteria = "[Type] == 'Email'"
        };
        forwardAction.CustomizePopupWindowParams += forwardAction_CustomizePopupWindowParams;
        forwardAction.Execute += forwardAction_Execute;

        // Add action items to the controller
        //Actions.AddRange(new[] { emailAction, callAction, replyAction, forwardAction });
    }


    private void EmailAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        // Get the selected communication item
        Communication communication = View.CurrentObject as Communication;

        // Create a new email message
        MailMessage mail = new MailMessage();

        // Set the email sender address
        mail.From = new MailAddress("flaughters@gmail.com");

        // Set the email recipient address
        mail.To.Add(new MailAddress(communication.Email));

        // Set the email subject
        mail.Subject = communication.Subject;

        // Set the email body
        mail.Body = communication.Body;

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var username = configuration.GetSection("Email")["UserName"];
        var password = configuration.GetSection("Email")["Password"];

        // Send the email using the SmtpClient class
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
        smtpClient.Send(mail);

        // Save the communication object to the database
        View.ObjectSpace.CommitChanges();
    }

    private void CallAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        // Get the selected communication item
        Communication communication = View.CurrentObject as Communication;

        // Get the related person object
        Person person = communication.Contact;

        // Get the phone numbers of the related person
        var phoneNumbers = communication.Contact.PhoneNumbers;

        // Find the phone number of the desired type (e.g., work, home, mobile)
        var phoneNumber = phoneNumbers.FirstOrDefault();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var accountSid = configuration.GetSection("Twilio")["AccountSid"];
        var authToken = configuration.GetSection("Twilio")["AuthToken"];

        // Make a call using the phone number
        // For example, using the Twilio API
        TwilioClient.Init(accountSid, authToken);
        var call = CallResource.Create(
            to: new Twilio.Types.PhoneNumber(phoneNumber.Number),
            from: new Twilio.Types.PhoneNumber("+254796456117"),
            url: new Uri("http://demo.twilio.com/docs/voice.xml"));

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        // Create a new phone call activity for the contact
        var phoneCall = new PhoneCall(session);
        phoneCall.SetSubject(communication.Subject);
        phoneCall.Description = communication.Body;
        phoneCall.StartOn = communication.DateTime;
        phoneCall.EndOn = communication.DateTime.AddMinutes(30);
        phoneCall.Participant = communication.Contact;
        //phoneCall.Direction = communication.Direction;

        // Save the phone call activity to the database
        View.ObjectSpace.CommitChanges();
    }

    private void replyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        Communication currentCommunication = (Communication)this.View.CurrentObject;

        if(currentCommunication == null)
            return;

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        if(currentCommunication.Type == CommunicationType.Email)
        {
            //Email replyEmail = new Email(this.ObjectSpace.Session);
            var replyEmail = new Email(session);
            replyEmail.From = currentCommunication.Contact.Email;
            replyEmail.Subject = "Re: " + currentCommunication.Subject;
            replyEmail.Body = Environment.NewLine +
                Environment.NewLine +
                "-----------------------" +
                Environment.NewLine +
                currentCommunication.Body;
            e.View = this.Application.CreateDetailView(this.ObjectSpace, replyEmail);
        }
    }

    private void replyAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        Email email = (Email)e.PopupWindow.View.CurrentObject;
        var toList = new List<string>();
        toList.Add(((Communication)this.View.CurrentObject).Contact.Email);
        this.ObjectSpace.CommitChanges();
    }

    private void forwardAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        Communication currentCommunication = (Communication)this.View.CurrentObject;

        if(currentCommunication == null)
            return;

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        if(currentCommunication.Type == CommunicationType.Email)
        {
            //Email forwardEmail = new Email(this.ObjectSpace.Session);
            var forwardEmail = new Email(session);
            forwardEmail.Subject = "Fwd: " + currentCommunication.Subject;
            forwardEmail.Body = Environment.NewLine +
                Environment.NewLine +
                "-----------------------" +
                Environment.NewLine +
                currentCommunication.Body;
            e.View = this.Application.CreateDetailView(this.ObjectSpace, forwardEmail);
        }
    }

    private void forwardAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        Email email = (Email)e.PopupWindow.View.CurrentObject;
        this.ObjectSpace.CommitChanges();
    }
}
