using API.Data;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace API.Managers
{
    public static class IssueManager
    {
        public async static System.Threading.Tasks.Task<Issue> CreateIssue(Issue issue, string token)
        {
            //GRAB USER DETAILS
            var db = DAL.GetInstance(DB.IssueTrack);
            var contact = Authentication.GetContactDetails(token);
            var userExists = db.IsIssueTrackUser(contact.UserName);

            if (!userExists && contact.UserName != null && contact.FirstName != null && contact.LastName != null)
            {
                db.CreateIssueTrackUser(new JObject { { "UserName", contact.UserName }, { "FirstName", contact.FirstName }, { "LastName", contact.LastName } });
            }

            JObject sqlParams = new JObject{{"User", contact.UserName}, {"Form", issue.Form}, {"IssueType", issue.IssueType},
            {"Priority", issue.Priority}, {"Description", issue.Description}, {"Module", issue.Module}};

            var images = ConvertImagesToByteArrays(issue.Images);
            var PhotoIDs = CreateIssuePhotos(images);

            var array = new JArray();
            PhotoIDs.ForEach(photoID =>
            {
                array.Add(photoID);
            });
                
            sqlParams.Add("PhotoIDs", array);
            var issueID = db.CreateIssue(sqlParams);

            if (issueID != 0)
            {
                issue.ID = issueID;
                issue.Created = DateTime.Now;
                issue.Comments = new List<Comment>();
                /////////////send email on creation//////////////
                string body = string.Empty;
                //using streamreader for reading my htmltemplate   
                using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("../Templates/HTML/IssueTrackTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
                string subject = "New Issue: " + issue.Description;
                body = body.Replace("{IssueTitle}", subject);
                string URL = "http://wb02l3.wwm.com:8082/IssueDetail.aspx?Function=View&ID=" + issue.ID.ToString();
                body = body.Replace("{IssueURL}", URL); 
                body = body.Replace("{IssueModule}", issue.Module);  
                body = body.Replace("{IssueID}", issue.ID.ToString());
                body = body.Replace("{IssueAssignedTo}", "Developers");
                body = body.Replace("{IssueRelease}", issue.ReleaseVersion);
                body = body.Replace("{IssueResolution}", issue.Resolution);
                body = body.Replace("{IssueEnteredby}", issue.EnterUserStr);
                body = body.Replace("{IssueLastEdited}", issue.EditUserStr);
                body = body.Replace("{IssueSummary}", issue.StepsToRecreate);
             
                string EmailDistributionList = ConfigurationManager.AppSettings["MobileIssueEmailDistributionList"];
                await new Email(issue.CreatedBy.FirstName +' '+ issue.CreatedBy.LastName, issue.CreatedBy.Email, EmailDistributionList, subject, body).Send();
                return issue;
            }
            else return null;
        }

        public async static System.Threading.Tasks.Task<Issue> UpdateIssue(Issue issue, string token)
        {
            //GRAB USER DETAILS
            var db = DAL.GetInstance(DB.IssueTrack);
            var contact = Authentication.GetContactDetails(token);
            var userExists = db.IsIssueTrackUser(contact.UserName);
            var resolution = issue.Resolution == "" || issue.Resolution == null ? "This Issue has been resolved" : issue.Resolution;

            string NewUrl = "https://staging.m2.wwmach.com/issueTrack/issue/" + issue.ID.ToString();
            if (issue.Environment == "Production")
            {
                NewUrl = "https://m2.wwmach.com/issueTrack/issue/" + issue.ID.ToString();
            }
            if (!userExists && contact.UserName != null && contact.FirstName != null && contact.LastName != null)
            {
                db.CreateIssueTrackUser(new JObject { { "UserName", contact.UserName }, { "FirstName", contact.FirstName }, { "LastName", contact.LastName } });
            }

            DateTime dt = DateTime.Today;
            if (issue.Status == "Completed" || issue.Status == "Canceled")
            {
                issue.CompletedDate = dt;
            }
            else
            {
                issue.CompletedDate = issue.CompletedDate;
            }

            try
            {
                /////////////send email on creation//////////////
                string body = string.Empty;
                //using streamreader for reading my htmltemplate   
                using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("../../../Templates/HTML/EmailTriggerTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Url}", NewUrl);
                body = body.Replace("{FirstName}", contact.FirstName);
                body = body.Replace("{LastName}", contact.LastName);
                body = body.Replace("{Date}", issue.Created.ToString());
                body = body.Replace("{IssueTrackAction}", "has resolved your issue");
                body = body.Replace("{IssueID}", issue.ID.ToString());
                body = body.Replace("{LinkMessage}", issue.Form);
                body = body.Replace("{MessageBody}", resolution);
                if (issue.Status == "Canceled" || issue.Status == "Completed")
                {
                    await new Email(contact.FirstName + " " + contact.LastName, contact.UserName + "@wwmach.com", issue.CreatedBy.UserName + "@wwmach.com",
                        "[M1 Mobile] Status change for Issue #" + " " + issue.ID.ToString(), body).Send();
                }
            }
            catch ( Exception e)
            {
                
                throw;
            }
         
            JObject sqlParams = new JObject { { "IssueID", issue.ID }, { "Form", issue.Form }, { "Description", issue.Description }, { "IssueType", issue.IssueType },
            {"DateCompleted", issue.CompletedDate}, {"Priority", issue.Priority}, {"Module", issue.Module}, {"Status", issue.Status}, {"Resolution", issue.Resolution } };

            var images = ConvertImagesToByteArrays(issue.Images);
            var photoIDS = CreateIssuePhotos(images);

            var array = new JArray();
            photoIDS.ForEach(photoID =>
            {
                array.Add(photoID);
            });
            sqlParams.Add("PhotoIDs", array);

            if (issue.ID != null)
            {
                db.UpdateIssue(sqlParams);
            }

            return issue;
        }

        public async static System.Threading.Tasks.Task<Issue> SendEmail(Issue issue, string token)
        {
            //GRAB USER DETAILS
            var db = DAL.GetInstance(DB.IssueTrack);
            var contact = Authentication.GetContactDetails(token);
            var userExists = db.IsIssueTrackUser(contact.UserName);
            string NewUrl = "https://staging.m2.wwmach.com/issueTrack/issue/" + issue.ID.ToString();
            if (issue.Environment == "Production")
            {
                NewUrl = "https://m2.wwmach.com/issueTrack/issue/" + issue.ID.ToString();
            }
            
            if (!userExists && contact.UserName != null && contact.FirstName != null && contact.LastName != null)
            {
                db.CreateIssueTrackUser(new JObject { { "UserName", contact.UserName }, { "FirstName", contact.FirstName }, { "LastName", contact.LastName } });
            }

            JObject sqlParams = new JObject { { "IssueID", issue.ID }, { "EmailMessage", issue.EmailMessage } };

            if (issue.ID != null)
            {
                /////////////send email on creation//////////////
                string body = string.Empty;
                //using streamreader for reading my htmltemplate   
                using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("../../../Templates/HTML/EmailTriggerTemplate.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Url}", NewUrl);
                body = body.Replace("{FirstName}", contact.FirstName);
                body = body.Replace("{LastName}", contact.LastName);
                body = body.Replace("{Date}", issue.Created.ToString());
                body = body.Replace("{IssueTrackAction}", "sent you an email on your reported issue");
                body = body.Replace("{IssueID}", issue.ID.ToString());
                body = body.Replace("{MessageBody}", issue.EmailMessage);
                body = body.Replace("{LinkMessage}", issue.Form);
                
               await new Email(contact.FirstName + " " + contact.LastName, contact.UserName + "@wwmach.com", issue.CreatedBy.UserName + "@wwmach.com", "[M1 Mobile] Issue #" + " "
                            + issue.ID.ToString() + " " + issue.Form, body).Send();
               
               db.SendIssueTrackMessage(sqlParams);
               return issue;
            }
            else return null;
        }

        public async static System.Threading.Tasks.Task<Comment> CreateIssueComments(Comment comment, string token)
        {
            var db = DAL.GetInstance(DB.IssueTrack);
            var userExists = db.IsIssueTrackUser(comment.CreatedBy.UserName);
            var userDeveloper = db.IsUserDeveloper(comment.CreatedBy.UserName);
            var contact = Authentication.GetContactDetails(token);

            string NewUrl = "https://staging.m2.wwmach.com/issueTrack/issue/" + comment.IssueID.ToString();
            if (!userExists)
            {
                db.CreateIssueTrackUser(new JObject { { "UserName", comment.CreatedBy.UserName }, { "FirstName", comment.CreatedBy.FirstName }, { "LastName", comment.CreatedBy.LastName } });
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["issuetrack"].ConnectionString);
                con.Open();
                string query =  ("SELECT * from Issue WHERE IssueID = '" + comment.IssueID.ToString() + "'");
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rdr = com.ExecuteReader();
                try
                {
                    if (rdr.Read())
                    {
                        //get the Issue Summary from DB
                        var formName = rdr[6].ToString();
                        var environment = rdr[1].ToString();

                        if (environment == "Production")
                        {
                            NewUrl = "https://m2.wwmach.com/issueTrack/issue/" + comment.IssueID.ToString();
                        }

                        /////////////send email on creation//////////////
                        string body = string.Empty;
                        //using streamreader for reading my htmltemplate   
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("../../../Templates/HTML/EmailTriggerTemplate.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        if (userDeveloper)//send email to user if developer adds comment
                        {
                            body = body.Replace("{Url}", NewUrl);
                            body = body.Replace("{FirstName}", comment.CreatedBy.FirstName);
                            body = body.Replace("{LastName}", comment.CreatedBy.LastName);
                            body = body.Replace("{Date}", rdr[22].ToString());
                            body = body.Replace("{MessageBody}", comment.Text);
                            body = body.Replace("{IssueID}", comment.IssueID.ToString());
                            body = body.Replace("{IssueTrackAction}", "added a comment to your issue");
                            body = body.Replace("{LinkMessage}", formName);
                            await new Email(comment.CreatedBy.FirstName + " " + comment.CreatedBy.LastName, comment.CreatedBy.UserName + "@wwmach.com", comment.CreatedBy.UserName + "@wwmach.com",
                                "[M1 Mobile] New Comment Added for Issue #" + " " + comment.IssueID, body).Send();
                        }
                        else//send email to assigned developer if user adds comment
                        {
                            body = body.Replace("{Url}", NewUrl);
                            body = body.Replace("{FirstName}", comment.CreatedBy.FirstName);
                            body = body.Replace("{LastName}", comment.CreatedBy.LastName);
                            body = body.Replace("{Date}", rdr[22].ToString());
                            body = body.Replace("{MessageBody}", comment.Text);
                            body = body.Replace("{IssueID}", comment.IssueID.ToString());
                            body = body.Replace("{IssueTrackAction}", "added a comment to an issue");
                            body = body.Replace("{LinkMessage}", formName);
                            await new Email(comment.CreatedBy.FirstName + " " + comment.CreatedBy.LastName, comment.CreatedBy.UserName + "@wwmach.com", contact.UserName + "@wwmach.com",
                                "[M1 Mobile] New Comment Added for Issue #" + " " + comment.IssueID, body).Send();
                        }
                        rdr.Close();
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    
                    throw;
                }
               
            }
            catch (Exception e)
            {
                
                throw;
            }
        
            var sqlParams = new JObject { { "IssueID", comment.IssueID }, { "User", comment.CreatedBy.UserName }, { "Comment", comment.Text } };
            var commentID = db.CreateIssueTrackComment(sqlParams);
            if (commentID != 0)
            {
                comment.ID = commentID;
                comment.Created = DateTime.Now;
                comment.CreatedBy.IsDeveloper = db.IsUserDeveloper(comment.CreatedBy.UserName);
                return comment;
            }
            else return null;
        }

        public static List<string> CreateIssuePhotos(List<byte[]> images)
        {
            List<string> PhotoIDs = new List<string>();
            try
            {
                images.ForEach(img =>
                {
                    var photoID = Guid.NewGuid().ToString();
                    Photos.Save(img, photoID, "IssuePhotoDirectory");
                    PhotoIDs.Add(photoID);
                });

                return PhotoIDs;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static List<byte[]> ConvertImagesToByteArrays(List<string> images)
        {
            var byteArrays = new List<Byte[]>();
            images.ForEach(img =>
            {
                var byteArr = Convert.FromBase64String(img);
                byteArrays.Add(byteArr);
            });

            return byteArrays;
        }

        public static List<byte[]> ConvertImagesToByteArrays(string image)
        {
            var byteArrays = new List<Byte[]>();
            var byteArr = Convert.FromBase64String(image);
            byteArrays.Add(byteArr);
            return byteArrays;
        }
    }
}