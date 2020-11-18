using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Issue
    {
        public int ID { get; set; }
        public string Module { get; set; }
        public string Form { get; set; }
        public string Priority { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public List<string> Images { get; set; }
        public List<Comment> Comments { get; set; }
        public Contact CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string ReleaseVersion { get; set; }
        public string Resolution { get; set; }
        public string EnterUserStr { get; set; }
        public string EditUserStr { get; set; }
        public string StepsToRecreate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string EmailMessage { get; set; }
        public string Environment { get; set; }
        public int SystemID { get; set; }
    }

    public class Comment
    {
        public int ID { get; set; }
        public int IssueID { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public Contact CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }

    public class IssueImage
    {
        public int IssueID { get; set; }
        public string FileName { get; set; }
    }
}