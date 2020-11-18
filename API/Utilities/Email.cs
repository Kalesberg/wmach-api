using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mandrill;
using System.Net;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using System.Threading.Tasks;
using Mandrill.Utilities;

namespace API.Utilities
{
    public class Email : EmailMessage
    {
        readonly string _apiKey = "FBC78eKstG6JrmK37WGq5g";
       
        
         public Email(string senderName, string senderEmail, string recipient, string subjects, string body)
        {
            FromName = "";
            FromEmail = "";
            Subject = subjects;
            Html = body;
            Recipients(recipient);
            if(senderName!=null)
             { FromName = senderName; }
            if (senderEmail != null)
             { FromEmail = senderEmail; }
           

        }
       
         public Email(string senderName, string senderEmail, string[] recipient, string subjects, string body)
         {
             FromName = "";
             FromEmail = "";
             Subject = subjects;
             Html = body;
             Recipients(recipient);
             if (senderName != null)
             { FromName = senderName; }
             if (senderEmail != null)
             { FromEmail = senderEmail; }

         }

        public Email()
        {

        } 

        public void Recipients(string emailAddress)
        {
            Recipients(new string[] { emailAddress });
        }

        public void Recipients(IEnumerable<string> emailAddresses)
        {
            To = emailAddresses.Select(value => new EmailAddress(value));
        }

        public void Recipients(string[] emailAddress)
        {
            List<EmailAddress> tos = new List<EmailAddress>();

            foreach(string recipient in emailAddress)
            {
                EmailAddress re = new EmailAddress(recipient);
                tos.Add(re);
            }
            To = tos;
        }

        public void addAttachments(IEnumerable<string> urlPaths, string type)
        {
            Attachments = urlPaths.Select(value =>
            {
                var webClient = new WebClient();
                var byteArray = webClient.DownloadData(value);
                
                return new EmailAttachment() 
                { 
                    Content = Convert.ToBase64String(byteArray),
                    Name = value.Split('/').Last(),
                    Type = type
                };
            });
        }

        public void addAttachments(IDictionary<string, byte[]> content, string type)
        {
            Attachments = content.Select(value =>
            {
                return new EmailAttachment() 
                {
                    Content = Convert.ToBase64String(value.Value),
                    Name = value.Key,
                    Type = type
                };
            });
        }

        public void addAttachments(IDictionary<string, string> content, string type)
        {
            Attachments = content.Select(value =>
            {
                return new EmailAttachment()
                {
                    Content = value.Value,
                    Name = value.Key,
                    Type = type
                };
            });
        }
       

        public async Task Send()
        {
            string Env = System.Configuration.ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
            if (!Env.Contains("L3SQ"))
            {
                this.To = new List<EmailAddress>() { new EmailAddress("dev@wwmach.com") };
            }

            System.Runtime.ExceptionServices.ExceptionDispatchInfo capturedException = null;
            try
            {
                MandrillApi _mandrillApi = new MandrillApi(_apiKey);
                SendMessageRequest newemailmessageRequest = new SendMessageRequest(this);
                newemailmessageRequest.Async = true;
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                var results = await _mandrillApi.SendMessage(newemailmessageRequest);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                foreach (var r in results)
                {
                    if (r.Status.ToString().ToLower() == "error")
                        throw new Exception();
                }
            }
            catch (Exception exception)
            {
                //throw;
                capturedException = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception);
            }
            // send to dev if error happen during sending
            if (capturedException != null)
            {
                var email = new Utilities.Email();

                string environment = Env.Contains("L3SQ") ? "Production: " : "Testing Env: ";

                email.FromName = this.FromName;
                email.FromEmail = this.FromEmail;
                email.Recipients("dev@wwmach.com");
                email.Subject = environment + "Email Failed to send!";
                string emailsFailedToSent = string.Join(",", this.To.Select(e => e.Email));
                email.Html = environment + "The email sent to the following recipient failed: " + emailsFailedToSent;
                MandrillApi api = new MandrillApi(_apiKey);
                SendMessageRequest errorEmail = new SendMessageRequest(email);
                errorEmail.Async = true;
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                await api.SendMessage(errorEmail);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                capturedException.Throw();
            }
        }
       
    }
}