using CLIENTPRO_CRM.Module.BusinessObjects.OrderManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;

namespace CLIENTPRO_CRM.Module.Controllers
{
    // Placeholder for the InvoiceData class
    public class InvoiceData
    {
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceDueDate { get; set; }

        public string CompanyName { get; set; }

        public string City { get; set; }

        public string CountryName { get; set; }

        public string CompanyWebsite { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyPhoneNumber { get; set; }

        public string AccountName { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }


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
            // Retrieve the selected invoice object
            Invoice invoice = e.CurrentObject as Invoice;

            // Execute the SQL query and retrieve the necessary invoice data
            InvoiceData invoiceData = ExecuteSqlQuery(invoice);

            // Generate the PDF invoice
            byte[] pdfBytes = GeneratePdfInvoice(invoiceData);

            string filePath = Path.Combine(Path.GetTempPath(), "invoice.pdf");
            File.WriteAllBytes(filePath, pdfBytes);

            string recipientEmail = "fredrick_ochieng@outlook.com";
            string subject = "Invoice";
            string body = "Please find the attached invoice.";
            SendEmailWithAttachment(recipientEmail, subject, body, filePath);
        }

        private InvoiceData ExecuteSqlQuery(Invoice invoice)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetSection("ConnectionStrings")["MySqlConnection"];

            InvoiceData invoiceData = new InvoiceData();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "SELECT `invoice`.`InvoiceNumber`, `invoice`.`InvoiceDate`, `invoice`.`InvoiceDueDate`, " +
                        "`companyinformation`.`CompanyName`, `address`.`City`, `country`.`Name`, `companyinformation`.`CompanyWebsite`, " +
                        "`companyinformation`.`CompanyEmail`, `companyinformation`.`CompanyPhonenumber`, `account`.`Name` AS `account_Name`, " +
                        "`product`.`Name` AS `product_Name`, `product`.`Quantity`, `product`.`PricePerQuantity`, " +
                        "(`product`.`Quantity` * `product`.`PricePerQuantity`) AS `UnitPrice` " +
                        "FROM (((((`account` `account` " +
                        "INNER JOIN `invoice` `invoice` ON (`invoice`.`Account` = `account`.`Oid`)) " +
                        "INNER JOIN `product` `product` ON (`product`.`Invoices` = `invoice`.`Oid`)) " +
                        "INNER JOIN `companyinformation` `companyinformation` ON (`companyinformation`.`Oid` = `invoice`.`CompanyInformation`)) " +
                        "INNER JOIN `address` `address` ON (`address`.`Oid` = `companyinformation`.`CompanyAddress`)) " +
                        "INNER JOIN `country` `country` ON (`country`.`Oid` = `address`.`Country`))",
                    connection))
                {
                    DataTable dataTable = new DataTable();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        invoiceData.InvoiceNumber = row["InvoiceNumber"].ToString();
                        invoiceData.InvoiceDate = Convert.ToDateTime(row["InvoiceDate"]);
                        invoiceData.InvoiceDueDate = Convert.ToDateTime(row["InvoiceDueDate"]);
                        invoiceData.CompanyName = row["CompanyName"].ToString();
                        invoiceData.City = row["City"].ToString();
                        invoiceData.CountryName = row["Name"].ToString();
                        invoiceData.CompanyWebsite = row["CompanyWebsite"].ToString();
                        invoiceData.CompanyEmail = row["CompanyEmail"].ToString();
                        invoiceData.CompanyPhoneNumber = row["CompanyPhoneNumber"].ToString();
                        invoiceData.AccountName = row["account_Name"].ToString();
                        invoiceData.ProductName = row["product_Name"].ToString();
                        invoiceData.Quantity = Convert.ToInt32(row["Quantity"]);
                        invoiceData.PricePerQuantity = Convert.ToDecimal(row["PricePerQuantity"]);
                        invoiceData.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
                    }
                }

                connection.Close();
            }

            return invoiceData;
        }


        private byte[] GeneratePdfInvoice(InvoiceData invoiceData)
        {
            // Create a new PDF document
            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            // Set up fonts and styles
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            // Open the document
            document.Open();

            // Add the invoice details
            document.Add(new Paragraph("Invoice For Account Name: " + invoiceData.AccountName, titleFont));
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("Invoice Number: " + invoiceData.InvoiceNumber, contentFont));
            document.Add(new Paragraph("Invoice Date: " + invoiceData.InvoiceDate.ToShortDateString(), contentFont));
            document.Add(
                new Paragraph("Invoice Due Date: " + invoiceData.InvoiceDueDate.ToShortDateString(), contentFont));
            document.Add(Chunk.NEWLINE);

            // Add the company information
            document.Add(new Paragraph("Company Name: " + invoiceData.CompanyName, contentFont));
            document.Add(new Paragraph("City: " + invoiceData.City, contentFont));
            document.Add(new Paragraph("Country: " + invoiceData.CountryName, contentFont));
            document.Add(new Paragraph("Website: " + invoiceData.CompanyWebsite, contentFont));
            document.Add(new Paragraph("Email: " + invoiceData.CompanyEmail, contentFont));
            document.Add(new Paragraph("Phone Number: " + invoiceData.CompanyPhoneNumber, contentFont));
            document.Add(Chunk.NEWLINE);

            // Add the product information
            document.Add(new Paragraph("Product Name: " + invoiceData.ProductName, contentFont));
            document.Add(new Paragraph("Quantity: " + invoiceData.Quantity, contentFont));
            document.Add(new Paragraph("Price per Quantity: " + invoiceData.PricePerQuantity, contentFont));
            document.Add(new Paragraph("Unit Price: " + invoiceData.UnitPrice, contentFont));

            // Close the document
            document.Close();

            // Get the generated PDF data as a byte array
            byte[] pdfBytes = stream.ToArray();

            return pdfBytes;
        }


        private void SendEmailWithAttachment(
            string recipientEmail,
            string subject,
            string body,
            string attachmentFilePath)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var username = configuration.GetSection("Email")["UserName"];
            var password = configuration.GetSection("Email")["Password"];

            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(username);
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                // Attach the invoice PDF file
                Attachment attachment = new Attachment(attachmentFilePath);
                mailMessage.Attachments.Add(attachment);

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    // Configure your SMTP server settings
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
                    smtpClient.EnableSsl = true;

                    // Send the email
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}
