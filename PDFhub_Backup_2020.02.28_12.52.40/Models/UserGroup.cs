using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class UserGroup
    {
        public int UserGroupID { get; set; } = 0;
        public string UserGroupName { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
    }
}