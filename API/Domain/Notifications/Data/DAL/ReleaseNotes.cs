using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public IEnumerable<ReleaseNote> GetMostRecentReleaseNotes(string systemName, string userName = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ReleaseNotesBySystem"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@ReleaseSystem", systemName);
            if (userName != null) cmd.Parameters.AddWithValue("@UserName", userName);
            Func<DataTable, List<ReleaseNote>> transform = releaseNoteTransform;
            return getRecords<ReleaseNote>(cmdText, transform);
        }

        private List<ReleaseNote> releaseNoteTransform(DataTable data)
        {
            var values = new List<ReleaseNote>();

            foreach (var row in data.AsEnumerable())
            {
                var changeType = row[0].ToString();
                var change = row[1].ToString();
                var version = row[2].ToString();
                var changeTypeExists = values.Exists(r => r.ChangeType == changeType);

                if (changeTypeExists)
                {
                    var update = values.Find(r => r.ChangeType == changeType);
                    update.Changes.Add(change);
                }
                else
                {
                    var relNote = new ReleaseNote();
                    relNote.ReleaseVersion = version;
                    relNote.ChangeType = changeType;
                    relNote.Changes = new List<string>() { change };
                    values.Add(relNote);
                }
            }

            return values;
        }

        public IEnumerable<Releases> GetAllReleaseNotes(string systemName)
        {
            string cmdText = ConfigurationManager.AppSettings["AllReleaseNotes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@ReleaseSystem", systemName);
            Func<DataTable, List<Releases>> transform = AllReleaseNoteTransform;
            return getRecords<Releases>(cmdText, transform);
        }

        public List<Releases> AllReleaseNoteTransform(DataTable data)
        {
            var versions = data.AsEnumerable().Select(row => new Releases
            {
                ReleaseVersion = row[2].ToString(),
                ReleaseDate = DateTime.Parse(row[3].ToString())
            }).GroupBy(r => r.ReleaseVersion)
              .Select(group => group.First()).ToList();

            foreach (var row in versions)
            {
                var rows = data.AsEnumerable().Where(r => r[2].ToString() == row.ReleaseVersion);
                var changeTypes = rows.Select(r => r[4].ToString()).Distinct().ToList();

                row.ReleaseNote = new List<ReleaseNote>();

                changeTypes.ForEach(changeType =>
                {
                    var changes = rows.Where(r => r[4].ToString() == changeType)
                                      .Select(r => r[5].ToString()).ToList();

                    var releases = new ReleaseNote();
                    releases.ChangeType = changeType;
                    releases.Changes = changes;
                    row.ReleaseNote.Add(releases);
                });
            }

            return versions;
        }
    }
}