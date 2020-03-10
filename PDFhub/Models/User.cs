using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class User
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";

        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public int UserGroupID { get; set; } = 0;

        public List<int> publisherIDList;

        public List<int> publicationIDList;
        public bool AccountEnabled { get; set; } = true;
        public User()
        {
            publicationIDList = new List<int>();
            publisherIDList = new List<int>();
        }

        public string _UserGroup
        {
            get
            {
                return Utils.GetUserGroupName(UserGroupID);
            }
        }

        public string _EnabledImageUrl
        {
            get
            {
                return AccountEnabled ? "Ok" : "Stop";
            }
        }

        

        public string _Publishers
        {
            get
            {
                string s = "";
                foreach (int publisherID in publisherIDList)
                {
                    if (s != "")
                        s += ",";
                    s += Utils.GetPublisherName(publisherID);
                }

                return s;
            }
        }

        public string _Publications
        {
            get
            {
                string s = "";
                foreach (int publicationID in publicationIDList)
                {
                    if (s != "")
                        s += ",";
                    s += Utils.GetPublicationName(publicationID);
                }

                return s;
            }
        }
    }
}