using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class ExportDocumentation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int channelIDFromMainForm = 0;
                if (Request.QueryString["ChannelID"] != null)
                {
                    channelIDFromMainForm = Utils.StringToInt(Request.QueryString["ChannelID"]);
                }
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<string> publications = new List<string>();
                db.GetPublicationsForExport(channelIDFromMainForm, ref publications, out string errmsg);
                RadListBoxPublications.Items.Clear();
                foreach (string s in publications)
                    RadListBoxPublications.Items.Add(s);


                //RadListBoxPublications.
            }
        }
    }
}