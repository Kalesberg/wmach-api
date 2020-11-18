using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSystem.BusinessLayer.Interfaces
{
    public interface ISendEmail
    {
        bool EmailWasSent { get;}
        Task SendEmail(IEnumerable<string> emailAdresses, string subject, string message, string fromEmailAddress, IEnumerable<string> attachmentFilePathList = null, string bccAddress = "");
    }
}
