using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class ResendToChannel : System.Web.UI.Page
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

                if (db.GetChannelsForProduction(productionID, ref prodName, ref channelIDForProduct, out string errmsg) == false)
                    Utils.WriteLog(false, "db.GetChannelsForProduction() - " + errmsg);

                lblProduct.Text = "Do re-export for " + prodName;

                List<Models.Channel> channels = Utils.GetChannels();
                List<Models.ChannelShort> cs = new List<Models.ChannelShort>();
                foreach (Models.Channel ch in channels)
                {
                    bool selected = channelIDForProduct.Contains(ch.ChannelID);
                    if (selected)
                        cs.Add(new Models.ChannelShort() { ChannelID = ch.ChannelID, Name = ch.Name, Selected = false, Enabled = true });
                }

                RadCheckBoxListChannels.DataSource = cs;
                RadCheckBoxListChannels.DataBind();
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            int productionID = Utils.StringToInt(HiddenProductionID.Value);
            List<int> selectedChannels = new List<int>();
            foreach (Telerik.Web.UI.ButtonListItem item in RadCheckBoxListChannels.SelectedItems)
            {
                selectedChannels.Add(Utils.StringToInt(item.Value));
            }

           


            if (productionID > 0 && selectedChannels.Count > 0)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.ResendChannelsForProduction(productionID, selectedChannels, RadRadioButtonListResent.SelectedIndex == 1,
                    RadRadioButtonReprocess.SelectedIndex, out string errmsg) == false)
                        Utils.WriteLog(true, "ERROR db.ResendChannelsForProduction() - " + errmsg);

                
                if (cbResendTrigger.Checked.Value)
                {
                    foreach (int channelID in selectedChannels)
                    {
                        db.IssueTriggerForProduction(productionID, channelID, out errmsg);
                    }
                   
                }

                if (RadCheckBoxRelease.Checked.Value)
                {
                    foreach (int channelID in selectedChannels)
                    {
                        db.ReleaseProduction(productionID, channelID, out errmsg);
                    }
                }
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
        }
    }
}