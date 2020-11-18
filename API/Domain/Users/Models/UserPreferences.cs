
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class UserPreferences
    {
        public IList<PreferenceGroup> PreferenceGroups { get; set; }
        public IList<Preference> Preferences { get; set; }
    }

    public class PreferenceGroup
    {
        public string Name { get; set; }
        public IList<int> PreferenceIDs { get; set; }
    }

    public class Preference
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public byte[] Image { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
    }
    public class PreferenceMobile
    {
        public int PreferenceID { get; set; }
        public int ContactID { get; set; }
        public string PreferenceModuleName { get; set; }
        public string PreferenceName { get; set; }
        public dynamic JsonString { get; set; }
        public Boolean Active { get; set; }
        public Boolean Default { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class EmailGroupList
    {
        public string EmailGroupName { get; set; }
        public string GroupOwnerName { get; set; }
        public string Email { get; set; }

    }
}