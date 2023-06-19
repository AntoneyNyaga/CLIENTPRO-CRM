using CLIENTPRO_CRM.Module.BusinessObjects;
using CLIENTPRO_CRM.Module.BusinessObjects.AccountingEssentials;
using CLIENTPRO_CRM.Module.BusinessObjects.PipelineManagement;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;


namespace CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]

    [NavigationItem("Orders")]
    [DefaultProperty("Description")]
    [Persistent("Quote")]
    [ImageName("BO_Quote")]


    public class Quote : BaseObject
    {
        /*int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }*/
        public Quote(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            _lastFollowUp = DateTime.Now;
            DateCreated = DateTime.Now;
        }

        [VisibleInDetailView(false)]
        public string QuoteNumber { get; set; }

        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Quote.Title", DefaultContexts.Save)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        // one to one relationship between Quote and Account
        //public Account Account { get; set; }

        [Association("Account-Quotes")]
        public Account Account
        {
            get => account;
            set => SetPropertyValue(nameof(Account), ref account, value);
        }

        public DateTime DateCreated { get; set; }

        public Opportunity Opportunity { get; set; }

        /*[RuleRequiredField("RuleRequiredField for Quote.Contact", DefaultContexts.Save)]
        public Contact Contact { get; set; }*/

        [RuleRequiredField("RuleRequiredField for Quote.ShippingAddress", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Address ShippingAddress
        {
            get => shippingAddress;
            set => SetPropertyValue(nameof(ShippingAddress), ref shippingAddress, value);
        }

        //[RuleRequiredField("RuleRequiredField for Quote.BillingAddress", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public Address BillingAddress
        {
            get => billingAddress;
            set => SetPropertyValue(nameof(BillingAddress), ref billingAddress, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.Product", DefaultContexts.Save)]
        [VisibleInListView(false)]
        public Product Product { get; set; }


        Account account;
        ApplicationUser assignedTo;
        Address billingAddress;
        Address shippingAddress;
        string title;
        string approvalIssues;
        DateTime validUntil;

        [RuleRequiredField("RuleRequiredField for Quote.DateCreated", DefaultContexts.Save)]
        public DateTime ValidUntil
        {
            get => validUntil;
            set => SetPropertyValue(nameof(ValidUntil), ref validUntil, value);
        }

        private decimal _price;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public decimal Price { get => _price; set => SetPropertyValue(nameof(Price), ref _price, value); }

        private ApprovalStatus _status;

        public ApprovalStatus ApprovalStatus
        {
            get => _status;
            set => SetPropertyValue(nameof(ApprovalStatus), ref _status, value);
        }

        private QuoteStage quoteStage;

        //[RuleRequiredField("RuleRequiredField for Quote.QuoteStage", DefaultContexts.Save)]
        public QuoteStage QuoteStage
        {
            get => quoteStage;
            set => SetPropertyValue(nameof(QuoteStage), ref quoteStage, value);
        }

        private InvoiceStatus invoiceStatus;

        public InvoiceStatus InvoiceStatus
        {
            get => invoiceStatus;
            set => SetPropertyValue(nameof(InvoiceStatus), ref invoiceStatus, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.ApprovalIssues", DefaultContexts.Save)]
        [Size(4096)]
        public string ApprovalIssues
        {
            get => approvalIssues;
            set => SetPropertyValue(nameof(ApprovalIssues), ref approvalIssues, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Quote-SalesOrders")]
        public XPCollection<SalesOrder> SalesOrders { get { return GetCollection<SalesOrder>(nameof(SalesOrders)); } }

        [Association("ApplicationUser-Quotes")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }

        private DateTime _lastFollowUp;

        [ReadOnly(true)]
        public DateTime LastFollowUp
        {
            get => _lastFollowUp;
            set => SetPropertyValue(nameof(LastFollowUp), ref _lastFollowUp, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("Quote-Invoices")]
        public XPCollection<Invoice> Invoices { get { return GetCollection<Invoice>(nameof(Invoices)); } }

        public void FollowUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var username = configuration.GetSection("Email")["UserName"];
            var password = configuration.GetSection("Email")["Password"];

            if (QuoteStage == QuoteStage.Sent && LastFollowUp.AddDays(7) < DateTime.Now)
            {
                // Create a new MailMessage object
                using MailMessage mail = new();
                // Set the email address of the recipient
                //mail.To.Add(Account.Email);
                mail.To.Add(Account.EmailAddress);

                // Set the subject of the email
                mail.Subject = "Follow-up on Quote #" + Title;

                // Set the body of the email
                mail.Body = "Dear Customer," +
                    Environment.NewLine +
                    Environment.NewLine +
                    "We hope this email finds you well. We wanted to follow up on the quote we sent you " +
                    "on " +
                    ValidUntil.ToShortDateString() +
                    ". Please let us know if you have any " +
                    "questions or concerns about the quote." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Thank you for considering our services." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Best regards," +
                    Environment.NewLine +
                    "CLIENTPRO CRM Team";

                // Create a new SmtpClient and send the email
                using (SmtpClient smtpClient = new("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(username, password);

                    smtpClient.Send(mail);
                }

                // Update the last follow-up date
                LastFollowUp = DateTime.Now;
            }
        }


        public void GenerateProposalPDF(string filePath)
        {
            // Create the document
            Document document = new();

            // Create a PDF writer
            _ = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the document
            document.Open();

            // Set up fonts and styles
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            // Add content to the document
            Paragraph title = new("Proposal for " + Title, titleFont) { Alignment = Element.ALIGN_CENTER };
            document.Add(title);

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Valid Until: " + ValidUntil.ToShortDateString(), contentFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Quote Number: " + QuoteNumber.ToString(), contentFont));
            document.Add(new Paragraph("Quote Status: " + QuoteStage.ToString(), contentFont));
            document.Add(new Paragraph("Approval Status: " + ApprovalStatus.ToString(), contentFont));
            document.Add(
                new Paragraph(
                    "Assigned To: " + (AssignedTo != null ? string.Join(", ", AssignedTo.UserName) : "Not Assigned"),
                    contentFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Billing Address:", sectionFont));
            document.Add(
                new Paragraph(
                    BillingAddress.ToString().Replace(Environment.NewLine, Environment.NewLine + " ".PadRight(45)),
                    contentFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Shipping Address:", sectionFont));
            document.Add(
                new Paragraph(
                    ShippingAddress.ToString().Replace(Environment.NewLine, Environment.NewLine + " ".PadRight(45)),
                    contentFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Products:", sectionFont));
            document.Add(
                new Paragraph(Product.Name + " " + Product.Description + " " + Price.ToString("C"), contentFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Total Price: " + Price.ToString("C"), sectionFont));

            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Thank you for your business!", contentFont));

            // Close the document
            document.Close();
        }

        public void GenerateProposal()
        {
            // Generate the proposal in PDF format
            string proposalFolder = "SentProposals";
            string proposalFileName = $"{QuoteNumber}-Proposal.pdf";
            string proposalFilePath = Path.Combine(proposalFolder, proposalFileName);

            // Create the "SentProposals" folder if it doesn't exist
            Directory.CreateDirectory(proposalFolder);

            GenerateProposalPDF(proposalFilePath);

            // Read the PDF file as bytes
            byte[] proposalBytes = File.ReadAllBytes(proposalFilePath);

            // Convert the bytes to a base64 string
            _ = Convert.ToBase64String(proposalBytes);
        }


        public void SendQuote()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var username = configuration.GetSection("Email")["UserName"];
            var password = configuration.GetSection("Email")["Password"];

            // Generate the proposal in PDF format
            GenerateProposal();

            // Load the proposal PDF file as bytes
            byte[] proposalBytes = File.ReadAllBytes(Path.Combine("SentProposals", $"{QuoteNumber}-Proposal.pdf"));
            using (MemoryStream attachmentStream = new(proposalBytes))
            {
                attachmentStream.Position = 0;
                Attachment attachment = new(attachmentStream, $"{QuoteNumber}-Proposal.pdf", "application/pdf");
                // Create the email message
                using MailMessage message = new();
                message.From = new MailAddress(username);
                //message.To.Add(Contact.Email);
                message.To.Add(Account.EmailAddress);
                message.Subject = $"Proposal for {Title}";
                message.Body = $"Dear {Account.Name},\n\nPlease find attached the proposal for {Title}.\n\nKind regards,\n\nCLIENTPRO CRM Team";

                // Attach the PDF file to the email
                message.Attachments.Add(attachment);

                // Send the email using the SmtpClient class
                using (SmtpClient smtpClient = new("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(username, password);

                    smtpClient.Send(message);

                    // Check if the email was sent successfully
                    MessageOptions options = new()
                    {
                        Duration = 2000,
                        Message = "Email sent successfully!",
                        Type = InformationType.Success
                    };
                }
            }

            // Update the QuoteStage and InvoiceStatus properties
            QuoteStage = QuoteStage.Sent;
            InvoiceStatus = InvoiceStatus.NotInvoiced;
            Save();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                GenerateQuoteNumber();
            }
        }

        private void GenerateQuoteNumber()
        {
            const string QuoteNumberFormat = "{0}-{1:0000}";
            var lastQuote = Session.Query<Quote>()?.OrderByDescending(q => q.DateCreated).FirstOrDefault();
            if (lastQuote != null)
            {
                var year = lastQuote.DateCreated.Year;
                var sequence = int.Parse(lastQuote.QuoteNumber.Split('-')[1]);
                sequence++;
                var newQuoteNumber = string.Format(QuoteNumberFormat, year, sequence);
                QuoteNumber = newQuoteNumber;
            }
            else
            {
                var currentYear = DateTime.Today.Year;
                QuoteNumber = string.Format(QuoteNumberFormat, currentYear, 1);
            }
        }
    }

    public enum ApprovalStatus
    {
        Approved,
        NotApproved
    }

    public enum QuoteStage
    {
        Draft,
        Sent,
        OnHold,
        Accepted,
        Declined
    }

    public enum InvoiceStatus
    {
        NotInvoiced,
        Invoiced
    }
}