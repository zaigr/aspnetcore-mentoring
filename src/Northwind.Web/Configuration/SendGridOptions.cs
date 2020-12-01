namespace Northwind.Web.Configuration
{
    public class SendGridOptions
    {
        public const string SendGrid = "SendGrid";

        public string User { get; set; }

        public string ApiKey { get; set; }

        public string SenderMail { get; set; }
    }
}
