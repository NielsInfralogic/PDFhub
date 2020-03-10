using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PDFhub
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["ChannelList"] = null;
            Session["PubliserList"] = null;
            Session["UserGroupList"] = null;
            Session["ConfigExportDetailsFormLoaded"] = false;

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            string errmsg = "";

            List<Models.Channel> channelList = new List<Models.Channel>();
            if (db.GetChannelList(ref channelList, out errmsg))
                Session["ChannelList"] = channelList;
                
            List<Models.Publisher> publisterList = new List<Models.Publisher>();
            if (db.GetPublisherList(ref publisterList, out errmsg))
                Session["PubliserList"] = publisterList;

            List<Models.UserGroup> userGroupList = new List<Models.UserGroup>();
            if (db.GetUserGroups(ref userGroupList, out errmsg))
                Session["UserGroupList"] = userGroupList;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}