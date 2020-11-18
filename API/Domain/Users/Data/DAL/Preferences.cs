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
        public UserPreferences Preferences_GET(string username)
        {
            var cmdText = ConfigurationManager.AppSettings["Preferences_GET"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@Username", username);
            Func<DataTable, UserPreferences> transform = PreferenceTransform;
            return getRecords<UserPreferences>(cmdText, transform);
        }

        public object Preferences_POST(string username, JObject json)
        {
            var cmdText = ConfigurationManager.AppSettings["Preferences_POST"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@Username", username);
            var id = InsertRecord(cmdText, json);
            return new { ID = id };
        }

        public void Preferences_PUT(string username, string id, JObject json)
        {
            var cmdText = ConfigurationManager.AppSettings["Preferences_PUT"];
            if (String.IsNullOrWhiteSpace(cmdText)) return;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@ID", id);
            getRecords<string>(cmdText, json);
        }

        public void Preferences_DELETE(string username, string id)
        {
            var cmdText = ConfigurationManager.AppSettings["Preferences_DELETE"];
            if (String.IsNullOrWhiteSpace(cmdText)) return;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@ID", id);
            getRecords<string>(cmdText);
        }

        UserPreferences PreferenceTransform(DataTable data)
        {
            var userPreference = new UserPreferences();
            userPreference.Preferences = new List<Preference>();
            userPreference.PreferenceGroups = new List<PreferenceGroup>();

            var preferenceGroup = new PreferenceGroup();
            preferenceGroup.PreferenceIDs = new List<int>();

            data.AsEnumerable()
                .OrderBy(row => (string)row["Group"])
                .ToList()
                .ForEach(row =>
                {
                    var group = (string)row["Group"];
                    var id = (int)row["ID"];

                    if (preferenceGroup.Name != null && preferenceGroup.Name != group)
                    {
                        userPreference.PreferenceGroups.Add(preferenceGroup);
                        preferenceGroup = new PreferenceGroup();
                        preferenceGroup.PreferenceIDs = new List<int>();
                    }

                    preferenceGroup.Name = group;
                    preferenceGroup.PreferenceIDs.Add(id);

                    userPreference.Preferences.Add(
                        new Preference()
                        {
                            ID = id,
                            Name = (string)row["Name"],
                            Data = row.IsNull("Data") ? null : (string)row["Data"],
                            Image = row.IsNull("Image") ? null : (byte[])row["Image"],
                            IsActive = (bool)row["IsActive"],
                            CreatedBy = (string)row["CreatedBy"],
                            CreatedDate = (DateTime)row["CreatedDate"],
                            EditedBy = (string)row["EditedBy"],
                            EditedDate = (DateTime)row["EditedDate"]
                        }
                    );
                });

            userPreference.PreferenceGroups.Add(preferenceGroup);

            return userPreference;
        }

        public int CreateNewPreference(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreatePreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public IEnumerable<PreferenceMobile> GetPreferenceInfo(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetPreferenceInfo"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<PreferenceMobile>(cmdText, sqlParams);
        }
        public Boolean DeactivePreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactivePreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean EditPreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["EditPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean MakeDefaultPreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["MakeDefaultPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public List<EmailGroupList> GetEmailGroupList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEmailGroupList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<EmailGroupList>(cmdText, sqlParams);
        }
        
    }
}