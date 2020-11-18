using API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace API.Utilities
{
    public static class XML
    {

        public static string Serialize<T>(T value)
        {
            if (value == null) return null;

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, settings))
                    serializer.Serialize(xw, value);

                return sw.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                    return (T)serializer.Deserialize(xmlReader);
            }
        }

        public static Preference GetPreferenceName(string name, IList<Preference> pre)
        {
            if (pre == null) return null;

            var prefs = pre.Where(p => p.Name == name);

            return prefs.Count() == 0 ? null : prefs.First();
        }
    }
}