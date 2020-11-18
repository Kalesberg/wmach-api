using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NReco.PdfGenerator;
using HtmlAgilityPack;

namespace API.Utilities
{
    public static class HTML
    {
        public static HtmlToPdfConverter Converter()
        {
            return new HtmlToPdfConverter();
        }

        public static byte[] ConvertToByteArray(string html)
        {
            var htmlToPdf = new HtmlToPdfConverter();

            return htmlToPdf.GeneratePdf(html);
        }

        public static HtmlNode GetId(HtmlNode node, string id)
        {
            return node.Descendants().Where(n => n.Id == id).FirstOrDefault();
        }

        public static IEnumerable<HtmlNode> GetElements(HtmlNode node, string name)
        {
            return node.Descendants().Where(n => n.Name == name);
        }

        public static IEnumerable<HtmlNode> GetAttributes(HtmlNode node, string name, string value)
        {
            return node.Descendants().Where(n =>
                n.GetAttributeValue(name, "").Equals(value));
        }

        public static IEnumerable<HtmlNode> GetClass(HtmlNode node, string className)
        {
            return GetAttributes(node, "class", className);
        }

        public static void SetClassName(IEnumerable<HtmlNode> nodes, string value)
        {
            foreach (var n in nodes) SetClassName(n, value);
        }

        public static void SetClassName(HtmlNode node, string value)
        {
            node.SetAttributeValue("class", value);
        }
    }
}