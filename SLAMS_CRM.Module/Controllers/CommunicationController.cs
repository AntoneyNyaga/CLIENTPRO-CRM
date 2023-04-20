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
            TargetObjectsCriteria = "[Type] == 'Email'"
        };

        emailAction.Execute += EmailAction_Execute;

        var callAction = new SimpleAction(this, "Call", PredefinedCategory.Edit)
        {
            Caption = "Make a Call",
            ToolTip = "Make a call to the contact or lead associated with this communication",
            ImageName = "BO_Phone",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
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
        replyAction.CustomizePopupWindowParams += ReplyAction_CustomizePopupWindowParams;
        replyAction.Execute += ReplyAction_Execute;

        var forwardAction = new PopupWindowShowAction(this, "Forward", PredefinedCategory.RecordEdit)
        {
            Caption = "Forward",
            ImageName = "Redo",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            //TargetObjectType = typeof(Communication)
            //TargetObjectsCriteria = new BinaryOperator("Type", CommunicationType.Email),
            TargetObjectsCriteria = "[Type] == 'Email'"
        };
        forwardAction.CustomizePopupWindowParams += ForwardAction_CustomizePopupWindowParams;
        forwardAction.Execute += ForwardAction_Execute;
    }


    private void EmailAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var username = configuration.GetSection("Email")["UserName"];
        var password = configuration.GetSection("Email")["Password"];

        // Get the selected communication item
        Communication communication = View.CurrentObject as Communication;

        // Create a new email message
        MailMessage mail = new()
        {
            // Set the email sender address
            From = new MailAddress(username)
        };

        // Set the email recipient address
        mail.To.Add(new MailAddress(communication.Email));

        // Set the email subject
        mail.Subject = communication.Subject;

        // Set the email body
        mail.Body = communication.Body;

        // Send the email using the SmtpClient class
        SmtpClient smtpClient = new("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(username, password)
        };
        smtpClient.Send(mail);

        var sentEmail = new SentEmail(((XPObjectSpace)View.ObjectSpace).Session)
        {
            DateTimeSent = DateTime.Now,
            Recipient = communication.Email,
            Sender = username,
            Subject = communication.Subject,
            Body = communication.Body
        };

        communication.SentEmails.Add(sentEmail);

        // Set the Status property to true
        communication.IsContacted = true;

        // Save the communication object to the database
        View.ObjectSpace.CommitChanges();
    }

    private void CallAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var accountSid = configuration.GetSection("Twilio")["AccountSid"];
        var authToken = configuration.GetSection("Twilio")["AuthToken"];
        var fromnumber = configuration.GetSection("Twilio")["FromNumber"];


        // Get the selected communication item
        Communication communication = View.CurrentObject as Communication;

        // Get the related person object
        _ = communication.Contact;

        // Get the phone numbers of the related person
        var phoneNumbers = communication.Contact.PhoneNumbers;

        // Find the phone number of the desired type (e.g., work, home, mobile)
        var phoneNumber = phoneNumbers.FirstOrDefault();

        // Make a call using the phone number using the Twilio API
        TwilioClient.Init(accountSid, authToken);
        _ = CallResource.Create(
            to: new Twilio.Types.PhoneNumber(phoneNumber.Number),
            from: new Twilio.Types.PhoneNumber(fromnumber),
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

        // Set the Status property to true
        communication.IsContacted = true;

        // Save the phone call activity to the database
        View.ObjectSpace.CommitChanges();
    }

    private void ReplyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        if(View.CurrentObject is not Email currentEmail)
            return;

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        var replyEmail = new Email(session)
        {
            From = currentEmail.Contact.Email,
            Subject = "Re: " + currentEmail.Subject,
            Body =
                $"{Environment.NewLine}{Environment.NewLine}-----------------------{Environment.NewLine}{currentEmail.Body}"
        };

        // Get the replyEmail object from the target object space
        replyEmail = ((XPObjectSpace)objectSpace).GetObject(replyEmail);

        //e.View = Application.CreateDetailView(((XPObjectSpace)objectSpace).CreateNestedObjectSpace(), replyEmail);
        e.View = Application.CreateDetailView(replyEmail, true);
    }

    private void ReplyAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        Email email = e.PopupWindow.View.CurrentObject as Email;
        if(email == null)
            return;

        var toList = new List<string>();
        toList.Add(email.Contact.Email);
        ObjectSpace.CommitChanges();
    }

    private void ForwardAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        if(View.CurrentObject is not Email currentEmail)
            return;

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        var forwardEmail = new Email(session)
        {
            Subject = "Fwd: " + currentEmail.Subject,
            Body =
                $"{Environment.NewLine}{Environment.NewLine}-----------------------{Environment.NewLine}{currentEmail.Body}"
        };

        // Get the forwardEmail object from the target object space
        forwardEmail = ((XPObjectSpace)objectSpace).GetObject(forwardEmail);

        //e.View = Application.CreateDetailView(((XPObjectSpace)objectSpace).CreateNestedObjectSpace(), forwardEmail);
        e.View = Application.CreateDetailView(forwardEmail, true);
    }

    private void ForwardAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        Email email = e.PopupWindow.View.CurrentObject as Email;
        if(email == null)
            return;

        ObjectSpace.CommitChanges();
    }
}
