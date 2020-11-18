using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EmailSystem.BusinessLayer.Interfaces;
using Mandrill;
using Mandrill.Models;
using Mandrill.Utilities;
using Mandrill.Requests.Messages;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Utilities.EmailSystem.BusinessLayer.Classes
{
   
    public class MandrillEmailSender : ISendEmail
    {
        private readonly MandrillApi _mandrillApi;

        public bool EmailWasSent { get; private set; }

        public MandrillEmailSender()
        {
            _mandrillApi = new MandrillApi("FBC78eKstG6JrmK37WGq5g");
        }

        public async Task SendEmail(IEnumerable<string> emailAddresses, string subject, string message, string fromEmailAddress, IEnumerable<string> attachmentFilePathList = null, string bccAddress = "")
        {
            if (emailAddresses == null)
                throw new NullReferenceException("You must provide a list of email addresses to send an email");
            var mandrillEmailAddresses =  emailAddresses.Select(emailAddress => new EmailAddress  { Email = emailAddress });

            var emailAttachments = new List<EmailAttachment>();
            if (attachmentFilePathList != null)
            {
                foreach (var attachmentFilePath in attachmentFilePathList)
                {
                    var webClient = new WebClient();
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                    var imageByteArray = webClient.DownloadData(attachmentFilePath);
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    EmailAttachment attachment = new EmailAttachment();
                    attachment.Content = Convert.ToBase64String(imageByteArray);
                    attachment.Name = attachmentFilePath.Split('/').Last();
                    attachment.Type = "image/jpeg";
                    emailAttachments.Add(attachment);
                }
            }
            var emailMessage = new EmailMessage
            {
                FromEmail = fromEmailAddress,
                Subject = subject,
                To = mandrillEmailAddresses,
                Html = message,
                InlineCss = true,
                BccAddress = bccAddress
            };
            string Env = System.Configuration.ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
            if (!Env.Contains("L3SQ"))
            {
                emailMessage.To = new List<EmailAddress>() { new EmailAddress("dev@wwmach.com") };
            }
            var newemailmessageRequest = new SendMessageRequest(emailMessage);
            newemailmessageRequest.Key = "FBC78eKstG6JrmK37WGq5g";
            newemailmessageRequest.Async = true;
           
            emailMessage.PreserveRecipients = false;

            if(emailAttachments.Any())
            {
                emailMessage.Attachments = emailAttachments;
            }

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                var results = await _mandrillApi.SendMessage(newemailmessageRequest);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                foreach (var r in results)
                {
                    if (r.Status.ToString().ToLower() == "error")
                    {
                        throw new Exception();
                    }
                }
            }
            catch (MandrillException exception)
            {
                throw; //new MandrillException(exception.Message);
            }
            catch (Exception exception)
            {
                throw; //new Exception(exception.Message);
            }

        }
    }
}
