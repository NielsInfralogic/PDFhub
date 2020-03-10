using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class ChangeChannels : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int productionID = 0;
                if (Request.QueryString["ProductionID"] != null)
                {
                    productionID = Utils.StringToInt(Request.QueryString["ProductionID"]);
                    if (productionID > 0)
                        HiddenProductionID.Value = productionID.ToString();
                }

                DataProviders.DBaccess db = new DataProviders.DBaccess();

                List<int> channelIDForProduct = new List<int>();

                string prodName = "";

                db.GetChannelsForProduction(productionID, ref prodName, ref channelIDForProduct, out string errmsg);

                lblProduct.Text = "Change export for " +prodName;

                List<Models.Channel> channels = Utils.GetChannels();
                List<Models.ChannelShort> cs = new List<Models.ChannelShort>();
                foreach (Models.Channel ch in channels)
                {
                    bool selected = channelIDForProduct.Contains(ch.ChannelID);

                    cs.Add(new Models.ChannelShort() { ChannelID = ch.ChannelID, Name = ch.Name, Selected = selected, Enabled = true });
                }
                
                RadCheckBoxListChannels.DataSource = cs;
                RadCheckBoxListChannels.DataBind();
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            int productionID = Utils.StringToInt(HiddenProductionID.Value);
            List<int> selectedChannels = new List<int>();
            foreach(Telerik.Web.UI.ButtonListItem item in RadCheckBoxListChannels.SelectedItems)
            {
                selectedChannels.Add(Utils.StringToInt(item.Value));
            }

            if (productionID > 0 && selectedChannels.Count > 0)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.SetChannelsForProduction(productionID, selectedChannels, out string errmsg) == false)
                    Utils.WriteLog(true, "ERROR db.SetChannelsForProduction() - " + errmsg);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
        }

    }
}