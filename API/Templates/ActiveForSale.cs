using API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using API.Data;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using HtmlAgilityPack;
using NReco.PdfGenerator;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using SelectPdf;

//using iTextSharp.text.pdf;
//using System.IO;

namespace API.Templates
{
    public class ActiveForSale
    {
        string _groupName = "Sales List";
        string _templatePath = "~/Templates/HTML/ActiveForSale.html";
        string _subject = "New Active for Sale List Available!";
        string _body = "Attached is the new Active for Sale list as of " + DateTime.Now.ToShortDateString() + ".";
        string _pdfTitle = "Active For Sale.pdf";
        decimal _CategoryRenderHeight = 150.19M;

        public bool[] isDealerPrice { get; set; }
        public List<bool> isListPrice { get; set; }

        public bool IsPreview { get; set; }
        public bool SalesList { get; set; }
        public bool DealerPrice { get; set; }
        public bool ListPrice { get; set; }
        public bool SpecialPrice { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Recipients { get; set; }
        public IEnumerable<Equipment> Data { get; set; }
        public IEnumerable<ForSaleList> Data1 { get; set; }

        #region Preview List Methods
        
        public byte[] GetPreview()
        {
            var doc = GenerateHtmlDocument(null);
            BuildReport(doc.DocumentNode, null);

            if (!DealerPrice)
                HideColumn(doc.DocumentNode, "col7");

            if (!ListPrice)
                HideColumn(doc.DocumentNode, "col6");
            if (!SpecialPrice)
                HideColumn(doc.DocumentNode, "col8");

            byte[] data = ConvertToPDF(doc.DocumentNode, "test", "test");

            

            return data;
        }

        #endregion
        
        #region Distribute List Methods

        public async Task DistributeList()
        {
            var recipients = GetRecipients();

            if (SalesList)
            {
                recipients.AddRange(GetRecipients(_groupName));
            }
                recipients.ForEach(recipient => 
            {
                // update user with info that doesn't come back from AD
                // this should probably be a Contact whenever we get that model up and running.
                recipient.UpdateInfo();

                // generate a new document for every user so that the equipment links use their info
                Distribute(recipient);
            });
        }
        
        void Distribute(Recipient recipient)
        {
            var attachments = new Dictionary<string, byte[]>();

            var doc = GenerateHtmlDocument(recipient);
            BuildReport(doc.DocumentNode, recipient);

            if (!DealerPrice)
                HideColumn(doc.DocumentNode, "col7");

            if (!ListPrice)
                HideColumn(doc.DocumentNode, "col6");

            if (!SpecialPrice)
                HideColumn(doc.DocumentNode, "col8");
            
            attachments.Add(_pdfTitle, ConvertToPDF(doc.DocumentNode, recipient.firstName, recipient.lastName));

            var senderName = "Notifications";
            var senderEmail = "notifications@wwmach.com";

            SendEmail(senderName, senderEmail, recipient, attachments);
        }

        public byte[] DistributeListPublic()
        {
            this._templatePath = "~/Templates/HTML/ActiveForSalePublic.html";
            var doc = GenerateHtmlDocument(null);
            BuildReport(doc.DocumentNode, null);
            HideColumn(doc.DocumentNode, "col7");
            HideColumn(doc.DocumentNode, "col8");
            return ConvertToPDF(doc.DocumentNode, "", "");
        }

        
        #endregion

        #region Active Directory Methods

        List<Recipient> GetRecipients()
        {
            var list = new List<Recipient>();

            if (Recipients != null && Recipients.Count > 0)
            {
                Recipients.ForEach(emailAddress =>
                {
                    var principal = ActiveDirectory.GetPrincipalByEmail(emailAddress);

                    if (principal == null) list.Add(new Recipient(emailAddress));
                    else list.Add(new Recipient(principal.GivenName, principal.Surname, principal.EmailAddress));
                });
            }

            return list;
        }

        List<Recipient> GetRecipients(string groupName)
        {
            var list = new List<Recipient>();

            ActiveDirectory.GetMembersInGroup(_groupName).ForEach(member =>
                list.Add(new Recipient(member.GivenName, member.Surname, member.EmailAddress))
            );

            return list;
        }

        #endregion

        #region HTML Methods
        
        HtmlDocument GenerateHtmlDocument(Recipient recipient)
        {
            var path = HostingEnvironment.MapPath(_templatePath);
            var doc = new HtmlDocument();
            doc.Load(path);

            if (recipient == null) return doc;

            //var root = doc.DocumentNode;

            foreach (var prop in recipient.GetType().GetProperties())
            {
                try
                {
                    var value = prop.GetValue(recipient, null);

                    if (value != null && !prop.Name.Equals("ContactID"))
                        doc.GetElementbyId(prop.Name).InnerHtml = value.ToString();
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }

            // don't forget to update the link
            if (recipient.email != null)
                doc.GetElementbyId("email").SetAttributeValue("href", "mailto:" + recipient.email);

            return doc;
        }

        void HideColumn(HtmlNode root, string className)
        {
            var nodes = HTML.GetClass(root, className);
            HTML.SetClassName(nodes, "hidden");
        }

        void BuildReport(HtmlNode root, Recipient recipient)
        {
            // remove existing template
            var content = HTML.GetId(root, "content");
            content.Element("section").Remove();
            //content.SelectSingleNode("//section").Remove();

            // get the contact id to pass to each record for detail link
            var contactId = recipient == null || recipient.ContactID == 0 ? 
                null : recipient.ContactID.ToString();

            // enumerate over records and append to content
            Categories.ForEach(cat => 
            {
                //Adding 25.19 pixels to the render height.  Calculating length of the page.  Will do drawing to render multi line and return pixel count.
                //We're going to increment a pixel counter as we iterate through the data list.
                //If the pixel count is going to go over with either the header OR a row of the data, we need to instead insert a continuation header.
                SetCategoryRows(cat, contactId, ref content);
                //If no machines found, hide section for this category.
                //Will need to break the category rows into 2 pieces and re-issue a header if the page end is reached.
            });
        }

        void SetCategoryRows(string category, string contactId, ref HtmlNode content)
        {
            //Logic.  If we're dealing with attachments the "category" is the "product catalog name"
           // var list = new List<HtmlNode>();
            var equipDetailUrl = "https://worldwidemachinery.com/equipment/details/";
            string Env = System.Configuration.ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
            if (Env.Contains("galsql01") || Env.Contains("localhost"))
            {
                equipDetailUrl = "https://staging.worldwidemachinery.com/equipment/details/";
            }

            var trim = new char[] { ' ', ',' };

            var section = HtmlNode.CreateNode("<section></section>");

            var header = HtmlNode.CreateNode("<header class=\"normalHeader\"></header>");
            var headerContinued = HtmlNode.CreateNode("<header class=\"normalHeader\"></header>");
            var continuationColumnHeader = HtmlNode.CreateNode("<header class=\"variableHeader\"></header>");

            header.InnerHtml = category.ToUpper();
            headerContinued.InnerHtml = category.ToUpper() + " - Continued";
            continuationColumnHeader.InnerHtml = @"<div class=""col1"">Year / Make / Model</div>
                <div class=""col2"">S/N</div>
                <div class=""col3"">Hours</div>
                <div class=""col4"">Description</div>
                <div class=""col5"">Location</div>
                <div class=""col6"">List Price</div>
                <div class=""col7"">Dealer Price</div>";


            //var recordsMachines = Data.Where(rec => rec.Category == category && rec.EquipmentType.ToLower().Equals("machine")).OrderBy(rec => rec.ModelNum).ThenBy(rec => rec.SerialNum).ToList();

            Func<string, object> convert = str =>
            {
                Regex digitsOnly = new Regex(@"[^\d]");
                try 
                {
                    if (digitsOnly.Replace(str, "") == string.Empty) return str;
                    else return int.Parse(str); 
                }
                catch { return str; }
            };

            var recordsMachines = Data.Where(rec => rec.Category == category && rec.EquipmentType.ToLower().Equals("machine"))
                .OrderByDescending(//Next line is Alphanumeric sort
                    rec => Regex.Split(rec.ModelNum.Replace(" ", ""), "([0-9]+)").Select(convert), new EnumerableComparer.EnumerableComparer<object>()
                ).ThenByDescending(rec => rec.YearManufactured).ThenBy(rec => rec.SerialNum).ToList();

            var recordsAttachments = Data.Where(rec => rec.AttachmentCategory == category && rec.EquipmentType.ToLower().Equals("attachment"))
                .OrderByDescending(
                     rec => Regex.Split(rec.ModelNum.Replace(" ", ""), "([0-9]+)").Select(convert), new EnumerableComparer.EnumerableComparer<object>()
                ).ThenByDescending(rec => rec.YearManufactured).ThenBy(rec => rec.SerialNum).ToList();

            decimal preRenderHeight = _CategoryRenderHeight;

            if (recordsMachines.Count > 0 || recordsAttachments.Count > 0)
            {
                //If records of machines found > attachments we're dealing with a machine category not an attachment category.
                var records = new List<Equipment>();
                if (recordsMachines.Count >= recordsAttachments.Count)
                    records = recordsMachines;
                else
                    records = recordsAttachments;
                if (SpecialPrice && !DealerPrice && !ListPrice)
                {
                    records = records.Where(r => r.BrokerPrice.HasValue && r.BrokerPrice > 1).ToList();
                }
                if(records.Count>0)
                    section.AppendChild(header);


                for (int index = 0; index < records.Count; index++)
                {
                    var detailUrl = equipDetailUrl + records[index].SerialNum;
                    //detailUrl += contactId == null ? "" : "/cid?=" + contactId;

                    var row = HtmlNode.CreateNode("<a><img src= https://api.wwmach.com/Templates/Images/camera-icon.png /></a>");
                    row.SetAttributeValue("href", detailUrl);
                    var spec = new string[] { records[index].YearManufactured.ToString(), records[index].ManufacturerName, records[index].ModelNum };
                    if (records[index].YearManufactured.ToString() == "0")
                        spec = spec.Skip(1).ToArray();
                    AppendRow(row, String.Join(" ", spec).TrimEnd(trim), "col1");

                    //wrapping long serial number with multiple dots
                    if (records[index].SerialNum.Length > 12)
                    {
                        var sn = records[index].SerialNum.Remove(records[index].SerialNum.Length - 5);
                        AppendRow(row, sn + "....", "col2");
                    }
                    else
                    {
                        var sn = records[index].SerialNum;
                        AppendRow(row, sn, "col2");
                    }

                    var hours = records[index].Hours.HasValue ? String.Format("{0:N0}", records[index].Hours) : null;
                    AppendRow(row, hours, "col3");

                    var desc = records[index].MarketingDescription == null ? null : records[index].MarketingDescription;
                    AppendRow(row, desc, "col4");

                    var loc = new string[] { records[index].City, records[index].State };
                    AppendRow(row, String.Join(", ", loc).TrimEnd(trim), "col5");

                    var listPrice = records[index].Price.HasValue && records[index].Price >1 ? String.Format("{0:C0}", records[index].Price) : "";
                    AppendRow(row, listPrice, "col6");

                    var dealerPrice = records[index].MinPrice.HasValue && records[index].MinPrice >1? String.Format("{0:C0}", records[index].MinPrice) : "";
                    AppendRow(row, dealerPrice, "col7");

                    var brokerPrice = records[index].BrokerPrice.HasValue && records[index].BrokerPrice >1 ? String.Format("{0:C0}", records[index].BrokerPrice) : "";
                    AppendRow(row, brokerPrice, "col8");

                    section.AppendChild(row);

                }
                content.AppendChild(section);
            }
            else
                _CategoryRenderHeight = preRenderHeight;

        }

        int GetNumberOfEmptyRows(decimal renderHeight, decimal maxPageRenderPixels)
        {
            //We already know we've reached the end of the page.
            //Calculate how many empty rows.
            decimal pageDifference = maxPageRenderPixels - _CategoryRenderHeight;

            int EmptyRows = (int)(pageDifference / 25.19M);

            return EmptyRows;
        }

        void AppendRow(HtmlNode row, string data, string className)
        {
            var node = HtmlNode.CreateNode("<div></div>");
            node.InnerHtml = data;
            node.SetAttributeValue("class", className);

            row.AppendChild(node);
        }

        HtmlNode AddCategory(string category, HtmlNode section, string contactId)
        {

            section.Descendants("h1").First().InnerHtml = category;

            var records = Data.Where(rec => rec.Category == category).OrderBy(rec => rec.ModelNum).ToList();

            if (records.Count == 0)
            {
                var msg = HtmlNode.CreateNode("<p>No machines for sale!</p>");

                section.Element("table").Remove();
                section.AppendChild(msg);

                return section;
            }

            var equipDetailUrl = "https://worldwidemachinery.com/equipment/details/";
            var recordTemplate = section.Descendants("tr").First();
                        
            section.Descendants("tr").First().Remove();

            records.ForEach(rec =>
            {
                var record = recordTemplate.CloneNode(true);
                var details = equipDetailUrl + rec.SerialNum;

                //details += contactId == null ? "" : "/cid?=" + contactId;

                var sn = rec.SerialNum == null ? null : "S/N " + rec.SerialNum;
                var hours = rec.Hours.HasValue ? rec.Hours.ToString() + " hrs." : null;
                var marketingDesc = rec.MarketingDescription == null ? null : rec.MarketingDescription;

                var spec = new string[] { rec.YearManufactured.ToString(), rec.ManufacturerName, rec.ModelNum };
                var desc = new string[] { sn, hours, marketingDesc };
                var loc = new string[] { rec.City, rec.State };

                var trim = new char[] { ' ', ',' };

                GetDataField(record, "details").SetAttributeValue("href", details);
                GetDataField(record, "spec").InnerHtml = String.Join(" ", spec).TrimEnd(trim);
                GetDataField(record, "desc").InnerHtml = String.Join(", ", desc).TrimEnd(trim);
                GetDataField(record, "loc").InnerHtml = String.Join(", ", loc).TrimEnd(trim);

                GetDataField(record, "dealer").InnerHtml = rec.MinPrice.HasValue ?
                    String.Format("{0:C0}", rec.MinPrice) : "N/A";
                GetDataField(record, "list").InnerHtml = rec.Price.HasValue ?
                    String.Format("{0:C0}", rec.Price) : "N/A";

                section.Descendants("tbody").Last().AppendChild(record);
            });

            return section;
        }

        HtmlNode GetDataField(HtmlNode node, string value)
        {
            var nodes = node.Descendants().Where(n => n.GetAttributeValue("data-field", "").Equals(value));

            if (nodes.Count() == 0) return null;

            return nodes.First();
        }

        decimal MeasureTextHeight(string descriptionText, string locationText, string yearMakeModelText, string serialNum)
        {
            System.Drawing.Font font = new System.Drawing.Font("Arial", 14.0F);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(2000, 2000);
            System.Drawing.Bitmap bmpLocation = new System.Drawing.Bitmap(2000, 2000);
            System.Drawing.Bitmap bmpYearMakeModel = new System.Drawing.Bitmap(2000, 2000);
            System.Drawing.Graphics graphYearMakeModel = System.Drawing.Graphics.FromImage(bmpYearMakeModel);
            System.Drawing.Graphics graphLocation = System.Drawing.Graphics.FromImage(bmpLocation);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp);
            graph.TextRenderingHint = graphLocation.TextRenderingHint = graphYearMakeModel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            System.Drawing.SizeF sizeDescription = graph.MeasureString(descriptionText, font, new System.Drawing.SizeF(725.135F,500.0F), System.Drawing.StringFormat.GenericTypographic);
            System.Drawing.SizeF sizeLocation = graphLocation.MeasureString(locationText, font, 155, System.Drawing.StringFormat.GenericTypographic);
            System.Drawing.SizeF sizeYearMakeModel = graphYearMakeModel.MeasureString(yearMakeModelText, font, 265, System.Drawing.StringFormat.GenericTypographic);
            //System.Drawing.Region[] regionDescription = graph.MeasureCharacterRanges(descriptionText.Trim(), font, new System.Drawing.RectangleF(0, 0, 728, 1000), System.Drawing.StringFormat.GenericTypographic);


            ////measuring the text height
            //System.Drawing.SizeF sizeSerialNumber = graph.MeasureString(serialNum, font, 155, System.Drawing.StringFormat.GenericTypographic);
            
            //size = System.Windows.Forms.TextRenderer.MeasureText(text, font, size, System.Windows.Forms.TextFormatFlags.NoPadding);
            int multiplierDesc = (int)(sizeDescription.Height / 20.85F);
            int multiplierLocation = (int)(sizeLocation.Height / 20.85F);
            int multiplierYearMakeModel = (int)(sizeYearMakeModel.Height / 20.85F);

            if (multiplierDesc >= multiplierLocation && multiplierDesc >= multiplierYearMakeModel)
                return (multiplierDesc * 14.00M) + 11.19M;
            else if (multiplierLocation >= multiplierDesc && multiplierLocation >= multiplierYearMakeModel)
                return (multiplierLocation * 14.00M) + 11.19M;
            else if (multiplierYearMakeModel >= multiplierDesc && multiplierYearMakeModel >= multiplierLocation)
                return (multiplierYearMakeModel * 14.00M) + 11.19M;

            //    //encapsulating the text width
            //else if (multiplierSerial >= multiplierDesc && multiplierSerial >= multiplierLocation && multiplierSerial >= multiplierYearMakeModel)
            //    return (multiplierSerial * 14.00M) + 11.19M;

            else
                return 25.19M;
        }

        #endregion

        #region PDF Methods
        //KROMERO: Leaving this for now in case we go down this rabbit hole again.
//        byte[] ConvertToPDF(HtmlNode root, string firstname, string lastname)
//        {
//            //var pdf = new HtmlToPdfConverter();
//            HtmlToPdf pdf = new HtmlToPdf();
//            pdf.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
//            pdf.Options.MarginLeft = 36;
//            pdf.Options.MarginRight = 36;
//            pdf.Options.MarginTop = 10;
//            pdf.Options.MarginBottom = 10;

//            //FOOTER
//            string dateTime = string.Format("<div style=\"text-align:left;\"><b>{0}</b></div>", DateTime.Now.ToShortDateString());
//            pdf.Options.DisplayFooter = true;
//            pdf.Footer.DisplayOnFirstPage = true;
//            pdf.Footer.DisplayOnOddPages = true;
//            pdf.Footer.DisplayOnEvenPages = true;
//            pdf.Footer.Height = 25;

//            PdfTextSection page = new PdfTextSection(0, 10, "Page: {page_number}", new System.Drawing.Font("Arial", 12));
//            page.HorizontalAlign = PdfTextHorizontalAlign.Right;
//            pdf.Footer.Add(page);
           
//            PdfHtmlSection footer = new PdfHtmlSection(dateTime, "");
//            footer.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
//            pdf.Footer.Add(footer);

//            //HEADER
//            var header = string.Empty;          

//            if (!DealerPrice && ListPrice) {
//                header = @"<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: lightgray; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 203px; display: inline-block;"">&nbsp; &nbsp; Year / Make / Model</div>
//                <div style=""width: 127px; display: inline-block;"">&nbsp; S/N</div>
//                <div style=""width: 60px; display: inline-block;"">&nbsp; Hours</div>
//                <div style=""width: 372px; display: inline-block;"">&nbsp; Description</div>
//                <div style=""width: 130px; display: inline-block;"">Location</div>
//                <div style=""width: 105px; display: inline-block; font-weight: 600;"">List Price</div>
//            </header>\n<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: orange; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 205px; display: inline-block; color: white;""></div>
//                <div style=""width: 127px; display: inline-block;""></div>
//                <div style=""width: 60px; display: inline-block;""></div>
//                <div style=""width: 480px; display: inline-block;""></div>
//                <div style=""width: 130px; display: inline-block;""></div>
//            </header>";
//            }


//            else if (!ListPrice && DealerPrice) {
//                header = @"<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: lightgray; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 203px; display: inline-block;"">&nbsp; &nbsp; Year / Make / Model</div>
//                <div style=""width: 132px; display: inline-block;"">&nbsp; S/N</div>
//                <div style=""width: 60px; display: inline-block;"">&nbsp; Hours</div>
//                <div style=""width: 372px; display: inline-block;"">&nbsp; Description</div>
//                <div style=""width: 130px; display: inline-block;"">Location</div>
//                <div style=""width: 100px; display: inline-block; font-weight: 600;"">Dealer Price</div>
//            </header>\n<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: orange; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 205px; display: inline-block; color: white;""></div>
//                <div style=""width: 127px; display: inline-block;""></div>
//                <div style=""width: 60px; display: inline-block;""></div>
//                <div style=""width: 480px; display: inline-block;""></div>
//                <div style=""width: 130px; display: inline-block;""></div>
//            </header>";
//            }

//            else if (ListPrice && DealerPrice) {
//                header = @"<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: lightgray; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 203px; display: inline-block;"">&nbsp; &nbsp; Year / Make / Model</div>
//                <div style=""width: 130px; display: inline-block;"">&nbsp; S/N</div>
//                <div style=""width: 60px; display: inline-block;"">&nbsp; Hours</div>
//                <div style=""width: 265px; display: inline-block;"">&nbsp; Description</div>
//                <div style=""width: 130px; display: inline-block;"">Location</div>
//                <div style=""width: 105px; display: inline-block; font-weight: 600;"">List Price</div>
//                <div style=""width: 100px; display: inline-block; font-weight: 600;"">Dealer Price</div>
//            </header>\n<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: orange; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 205px; display: inline-block; color: white;""></div>
//                <div style=""width: 127px; display: inline-block;""></div>
//                <div style=""width: 60px; display: inline-block;""></div>
//                <div style=""width: 480px; display: inline-block;""></div>
//                <div style=""width: 130px; display: inline-block;""></div>
//            </header>";
//            }

//            else if (!ListPrice && !DealerPrice) {
//                header = @"<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: lightgray; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 205px; display: inline-block;"">&nbsp; &nbsp; Year / Make / Model</div>
//                <div style=""width: 127px; display: inline-block;"">&nbsp; &nbsp;S/N</div>
//                <div style=""width: 60px; display: inline-block;"">&nbsp; &nbsp;Hours</div>
//                <div style=""width: 480px; display: inline-block;"">&nbsp; &nbsp;Description</div>
//                <div style=""width: 130px; display: inline-block;"">&nbsp;Location</div>
//            </header>\n<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: orange; position: absolute; right: 0px; margin: 0; padding: 2px;"">
//                <div style=""width: 205px; display: inline-block; color: white;""></div>
//                <div style=""width: 127px; display: inline-block;""></div>
//                <div style=""width: 60px; display: inline-block;""></div>
//                <div style=""width: 480px; display: inline-block;""></div>
//                <div style=""width: 130px; display: inline-block;""></div>
//            </header>";
//            }


//            pdf.Options.DisplayHeader = true;
//            pdf.Header.DisplayOnFirstPage = false;
//            pdf.Header.DisplayOnOddPages = true;
//            pdf.Header.DisplayOnEvenPages = true;
//            pdf.Header.Height = 35;
//            PdfHtmlSection pdfheader = new PdfHtmlSection(header, "");
//            pdfheader.DrawBackground = false;
//            pdfheader.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
//            pdfheader.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

//            pdf.Options.PdfBookmarkOptions.CssSelectors = new string[]{".normalHeader"};
//            pdf.Options.ViewerPreferences.PageMode = PdfViewerPageMode.UseOutlines;
//            pdf.Header.Add(pdfheader);
//            //Convert to PDF from HTML
//            SelectPdf.PdfDocument doc = pdf.ConvertHtmlString(root.OuterHtml);
//            //----------------------------------------------------------------------------------------------------------
//            //Find the Page Numbers for Categories
//            Dictionary<int, string> Categories = new Dictionary<int, string>();
//            foreach (PdfBookmark bookmark in doc.Bookmarks)
//            {
//                string text = bookmark.Text;
//                int pageno = bookmark.Destination.Page.PageIndex;
//                if (!Categories.ContainsKey(pageno))
//                    Categories.Add(pageno, text);
//                else
//                    Categories[pageno] = text;

//            }
//            // create a new pdf font
//            SelectPdf.PdfFont font = doc.AddFont(PdfStandardFont.Courier);
//            font.Size = 5;

//            //----------------------------------------------------------------------------------------------------------
//            //Save the PDF to a temporary location without the continued headers
//            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
//            int secondsSinceEpoch = (int)t.TotalSeconds;
//            string tempFileLoc = "C:\\PDFs\\"+secondsSinceEpoch.ToString() + ".pdf";
//            doc.Save(tempFileLoc);
//            //Read the PDF back in
//            PdfReader pdfReader = new PdfReader(tempFileLoc);
//            using (FileStream fs = new FileStream(tempFileLoc.Replace(".pdf","")+"_2.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
//            {
//                //Use PDFStamper to write out a modified version of the PDF
//                PdfStamper stamper = new PdfStamper(pdfReader, fs);
//                    //Find the Category related to the last page, and stamp text into the correct continued header location.
//                    int PageCount = pdfReader.NumberOfPages;
//                    for (int x = 2; x <= PageCount; x++)
//                    {
//                        PdfContentByte cb = stamper.GetOverContent(x);
//                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1257, BaseFont.NOT_EMBEDDED);
//                        cb.SetColorFill(iTextSharp.text.Color.WHITE);
//                        cb.SetFontAndSize(bf, 9);

//                        cb.BeginText();

//                        int ix = x-2;
//                        bool foundKey = false;
//                        bool noKey = false;
//                        //Search for the last category in case it spans more than one page or doesnt exist.
//                        while(!foundKey && !noKey)
//                        {
//                            if (!Categories.ContainsKey(ix))
//                            {
//                                 ix--;
//                            }
//                            else foundKey = true;

//                            if (ix < 0)
//                            {
//                                noKey = true;
//                            }
//                        }
//                       //Stamp the header
//                        if(!noKey)
//                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Categories[ix] + " - CONTINUED", 40, 554, 0);
//                        cb.EndText();
//                    }
//                    stamper.Close();
//            }

//            //-------------------------------------------------------------------------------------------------------
//            //Read the modified PDF back into memory and output as byte[] in response
//            PdfReader pdfreader =  new PdfReader(tempFileLoc.Replace(".pdf", "") + "_2.pdf");
//            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
//            {
//                PdfStamper stamper = new PdfStamper(pdfreader, ms, '\0', true);
//                stamper.Close();
//                byte[] result = ms.ToArray();
//                //Delete temp files
//                if (System.IO.File.Exists(tempFileLoc))
//                    System.IO.File.Delete(tempFileLoc);
//                if (System.IO.File.Exists(tempFileLoc.Replace(".pdf", "") + "_2.pdf"))
//                    System.IO.File.Delete(tempFileLoc.Replace(".pdf", "") + "_2.pdf");
//                return result;
//            }

//        }

        byte[] ConvertToPDF(HtmlNode root, string firstname, string lastname)
        {
            //var pdf = new HtmlToPdfConverter();
            HtmlToPdf pdf = new HtmlToPdf();
            pdf.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            pdf.Options.MarginLeft = 36;
            pdf.Options.MarginRight = 36;
            pdf.Options.MarginTop = 10;
            pdf.Options.MarginBottom = 10;

            //FOOTER
            string dateTime = string.Format("<div style=\"text-align:left;\"><b>{0}</b></div>", DateTime.Now.ToShortDateString());
            pdf.Options.DisplayFooter = true;
            pdf.Footer.DisplayOnFirstPage = true;
            pdf.Footer.DisplayOnOddPages = true;
            pdf.Footer.DisplayOnEvenPages = true;
            pdf.Footer.Height = 25;

            PdfTextSection page = new PdfTextSection(0, 10, "Page: {page_number}", new System.Drawing.Font("Arial", 12));
            page.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf.Footer.Add(page);

            PdfHtmlSection footer = new PdfHtmlSection(dateTime, "");
            footer.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            pdf.Footer.Add(footer);

            //HEADER
            var header = string.Empty;

            header = @"<header style=""font-weight: 600; font-family: Arial; font-size: 12px; background-color: lightgray; position: absolute;  padding: 2px;"">
                <div style=""width: 215px; display: inline-block;""> Year / Make / Model</div>
                <div style=""width: 127px; display: inline-block;"">S/N</div>
                <div style=""width: 60px; display: inline-block;"">Hours</div>
                <div style=""width: @Desciptionpx; display: inline-block;"">&nbsp;&nbsp;Description</div>
                <div style=""width: 130px; display: inline-block;"">Location</div>
                <div style=""width: @listpxpx; display: @listPriceDisplay; font-weight: 600;"">List Price</div>
                <div style=""width: @dealerpxpx; display: @dealerPriceDisplay; font-weight: 600;"">Dealer Price</div>
                <div style=""width: @specialpxpx; display: @specialPriceDisplay; font-weight: 600;"">Special Price</div>
            </header>";

            if (!DealerPrice && ListPrice && !SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "inline-block");
                header = header.Replace("@dealerPriceDisplay", "none");
                header = header.Replace("@specialPriceDisplay", "none");
                header = header.Replace("@Desciption", "357");
                header = header.Replace("@listpx", "100");
                header = header.Replace("@dealerpx", "0");
                header = header.Replace("@specialpx", "0");
            }
            else if (DealerPrice && !ListPrice && !SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "none");
                header = header.Replace("@dealerPriceDisplay", "inline-block");
                header = header.Replace("@specialPriceDisplay", "none");
                header = header.Replace("@Desciption", "357");
                header = header.Replace("@listpx", "0");
                header = header.Replace("@dealerpx", "100");
                header = header.Replace("@specialpx", "0");
            }

            else if (!DealerPrice && !ListPrice && SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "none");
                header = header.Replace("@dealerPriceDisplay", "none");
                header = header.Replace("@specialPriceDisplay", "inline-block");
                header = header.Replace("@Desciption", "357");
                header = header.Replace("@listpx", "0");
                header = header.Replace("@dealerpx", "0");
                header = header.Replace("@specialpx", "100");
            }

            else if (DealerPrice && ListPrice && !SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "inline-block");
                header = header.Replace("@dealerPriceDisplay", "inline-block");
                header = header.Replace("@specialPriceDisplay", "none");
                header = header.Replace("@Desciption", "250");
                header = header.Replace("@listpx", "100");
                header = header.Replace("@dealerpx", "100");
                header = header.Replace("@specialpx", "0");
            }

            else if (DealerPrice && !ListPrice && SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "none");
                header = header.Replace("@dealerPriceDisplay", "inline-block");
                header = header.Replace("@specialPriceDisplay", "inline-block");
                header = header.Replace("@Desciption", "250");
                header = header.Replace("@listpx", "0");
                header = header.Replace("@dealerpx", "100");
                header = header.Replace("@specialpx", "100");
            }

            else if (!DealerPrice && ListPrice && SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "inline-block");
                header = header.Replace("@dealerPriceDisplay", "none");
                header = header.Replace("@specialPriceDisplay", "inline-block");
                header = header.Replace("@Desciption", "250");
                header = header.Replace("@listpx", "100");
                header = header.Replace("@dealerpx", "0");
                header = header.Replace("@specialpx", "100");
            }

            else if (DealerPrice && ListPrice && SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "inline-block");
                header = header.Replace("@dealerPriceDisplay", "inline-block");
                header = header.Replace("@specialPriceDisplay", "inline-block");
                header = header.Replace("@Desciption", "142");
                header = header.Replace("@listpx", "100");
                header = header.Replace("@dealerpx", "100");
                header = header.Replace("@specialpx", "100");
            }

            else if (!DealerPrice && !ListPrice && !SpecialPrice)
            {
                header = header.Replace("@listPriceDisplay", "none");
                header = header.Replace("@dealerPriceDisplay", "none");
                header = header.Replace("@specialPriceDisplay", "none");
                header = header.Replace("@Desciption", "464");
                header = header.Replace("@listpx", "100");
                header = header.Replace("@dealerpx", "100");
                header = header.Replace("@specialpx", "100");
            }





            pdf.Options.DisplayHeader = true;
            pdf.Header.DisplayOnFirstPage = false;
            pdf.Header.DisplayOnOddPages = true;
            pdf.Header.DisplayOnEvenPages = true;
            pdf.Header.Height = 25;
            PdfHtmlSection pdfheader = new PdfHtmlSection(header, "");
            pdfheader.DrawBackground = false;
            pdfheader.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            pdfheader.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

            pdf.Options.PdfBookmarkOptions.CssSelectors = new string[] { ".normalHeader" };


            pdf.Header.Add(pdfheader);

            PdfDocument doc = pdf.ConvertHtmlString(root.OuterHtml);

            // create a new pdf font
            PdfFont font = doc.AddFont(PdfStandardFont.Courier);
            font.Size = 5;

            //doc.Save("C:\\PDFs\\test.pdf");

            byte[] bytes = doc.Save();

            return bytes;
        }

        #endregion

        #region Email Delivery

        async Task SendEmail(string senderName, string senderEmail, Recipient recipient, IDictionary<string, byte[]> pdfs)
        {
  
            var email = new Utilities.Email();
            try
            {
                email.FromName = senderName;
                email.FromEmail = senderEmail;
                email.Recipients(recipient.email);
                email.Subject = _subject;
                email.Html = _body;
                email.addAttachments(pdfs, "pdf");
               await email.Send();
               
            }
            catch(Exception EX)
            {
                string test = EX.Message;
            }
        }

        void SendFailureEmail(string senderName, string senderEmail, Recipient recipient)
        {
            var email = new Utilities.Email();
            email.FromName = senderName;
            email.FromEmail = senderEmail;
            email.Recipients("dev@wwmach.com");
            email.Subject = "Active For Sale Email Failure";
            email.Html = "The email sent to the following recipient failed: " + recipient.email;
        }

        #endregion
    }
    
    /* This should probably be a contact whenever we get that model up and running. */
    public class Recipient
    {
        // use camelCase so that the props will match up with the element ids
        public int ContactID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string divisionImageURL { get; set; }

        public Recipient() { }

        public Recipient(string emailAddress)
        {
            email = emailAddress;
        }

        public Recipient(string first, string last, string emailAddress)
        {
            firstName = first;
            lastName = last;
            email = emailAddress;
        }

        public void UpdateInfo()
        {
            var db = DAL.GetInstance();
            var sqlParams = new JObject();

            if (email != null && email.Trim().ToLower().Equals("rfellet@wwmach.com"))
                email = "rfellet@wrsrents.com";

            sqlParams.Add("Email", email);

            var contactInfo = db.getContactsFromEmail(sqlParams);
            var info = (contactInfo.Count() > 0) ? contactInfo.First() : null;

            sqlParams = new JObject();
            sqlParams.Add("FirstName", firstName);
            sqlParams.Add("LastName", lastName);
            if (info == null)
            {
                contactInfo = db.getContactByName(sqlParams);
                info = (contactInfo.Count() > 0) ? contactInfo.First() : null;
            }

            if (info != null && info.ContactAddress != null)
            {
                if (info.ContactAddress.street != null && info.ContactAddress.street.Length > 0)
                    street = info.ContactAddress.street;
                if (info.ContactAddress.city != null && info.ContactAddress.city.Length > 0)
                    city = info.ContactAddress.city;
                if (info.ContactAddress.state != null && info.ContactAddress.state.Length > 0)
                    state = info.ContactAddress.state;
                if (info.ContactAddress.zip != null && info.ContactAddress.zip.Length > 0)
                    zip = info.ContactAddress.zip;
            }

            if (info != null)
            {
                if (info.BusinessPhone != null && info.BusinessPhone.Length > 0)
                {
                    phone = Regex.Replace(info.BusinessPhone, @"[^\d]", "");

                    // only match those phone numbers with area code (US)
                    phone = phone.Length == 10 ? FormatPhoneNumber(phone) : null;
                }
                if(info.ContactId != null)
                    ContactID = info.ContactId.Value;
            }

            if (info != null && info.DivisionImageURI != null && info.DivisionImageURI.Length > 0)
            {
                divisionImageURL = info.DivisionImageURI;
            }
        }

        string FormatPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Substring(0, 3) + "." +
                phoneNumber.Substring(3, 3) + "." +
                phoneNumber.Substring(6);
        }
    }
}