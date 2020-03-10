using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigPublicationDetails : System.Web.UI.UserControl
    {
        private object _dataItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public object DataItem
        {
            get
            {
                return this._dataItem;
            }
            set
            {
                if (value is Models.Publication)
                {
                    if (value != null)
                    {
                        Session["EditedPublicationDataItem"] = value;
                        this._dataItem = value;
                    }
                    else if (Session["EditedPublicationDataItem"] != null)
                    {
                        this._dataItem = Session["EditedPublicationDataItem"];
                    }
                }
            }
        }

       protected void RadGridPublicationChannels_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.PublicationChannel> specificPubChannels = new List<Models.PublicationChannel>();
            if (DataItem != null && (DataItem is Models.Publication))
                specificPubChannels = (DataItem as Models.Publication).PublicationChannels;
            List<Models.PublicationChannelAll> allPubChannels = new List<Models.PublicationChannelAll>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            string errmsg = "";

            if (Session["ChannelList"] == null)
            {
                List<Models.Channel> channels = new List<Models.Channel>();
                db.GetChannelList(ref channels, out errmsg);
                Session["ChannelList"] = channels;
            }
            try
            {
                List<Models.Channel> channelList = (List<Models.Channel>)HttpContext.Current.Session["ChannelList"];
                foreach (Models.Channel channel in channelList)
                {
                    bool use = false;
                    int trigger = (int)Models.PushTrigger.None;
                    int pubDateMoveDays = 0;
                    int releaseDelay = 0;
                    Models.PublicationChannel sc = specificPubChannels.FirstOrDefault(p => p.ChannelID == channel.ChannelID);
                    if (sc != null)
                    {
                        use = true;
                        trigger = sc.Trigger;
                        pubDateMoveDays = sc.PubDateMoveDays;
                        releaseDelay = sc.ReleaseDelay;
                    }
                    allPubChannels.Add(new Models.PublicationChannelAll()
                    {
                        ChannelName = channel.Name,
                        Use = use,
                        Trigger = trigger,
                        PubDateMoveDays = pubDateMoveDays,
                        ReleaseDelay = releaseDelay
                    });
                }
            }
            catch
            {
            }

            RadGridPublicationChannels.DataSource = allPubChannels;
        }

        protected void RadGridPublicationChannels_BatchEditCommand(object sender, Telerik.Web.UI.GridBatchEditingEventArgs e)
        {
            GridTableView masterTable = (sender as RadGrid).MasterTableView;
            GridDataItem dataItems = masterTable.Items[2];


            TableCell cell = dataItems["Use"];


            //RadDropDownList triggerEditor = masterTable.GetBatchColumnEditor("TriggerIDDropDown") as RadDropDownList;
            // int n = triggerEditor.SelectedIndex;
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                GridDataItem itm = command.Item;
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;
              /*  foreach (string key in command.NewValues.Keys)
                {
                    if (newValues[key] != oldValues[key]) //You may want to implement stronger difference checks here, or a check for the command name (e.g., when inserting there is little point in looking up old values
                    {
                        string output = String.Format("column: {0} with new value {1}<br />", key, command.NewValues[key]);
                        Response.Write(output);
                    }
                }*/
                //a simple way of getting the value of a column whose name you know
                string channelName = newValues["ChannelName"].ToString();
                string newUse= newValues["Use"].ToString();
                string newTrigger = newValues["_Trigger"].ToString();
                string pubDateMoveDays = newValues["PubDateMoveDays"].ToString();
                string releaseDelay = newValues["ReleaseDelay"].ToString();
            }


        }

        protected void RadGridPublicationChannels_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            String id = item.GetDataKeyValue("ChannelName").ToString();
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                //NotifyUser("Product with ID " + id + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                //NotifyUser("Product with ID " + id + " is updated!");
            }
        }

        

        protected void RadGridPublicationChannels_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            GridDropDownListColumnEditor trigger = (GridDropDownListColumnEditor)(editedItem.EditManager.GetColumnEditor("_Trigger"));
            GridTextBoxColumnEditor channelName = (GridTextBoxColumnEditor)editedItem.EditManager.GetColumnEditor("ChannelName");
            GridNumericColumnEditor pubDateMove = (GridNumericColumnEditor)editedItem.EditManager.GetColumnEditor("PubDateMoveDays");
            GridNumericColumnEditor releaseDelay = (GridNumericColumnEditor)editedItem.EditManager.GetColumnEditor("ReleaseDelay");
            GridCheckBoxColumnEditor use = (GridCheckBoxColumnEditor)(editedItem.EditManager.GetColumnEditor("Use"));
            if (trigger == null || use == null  || channelName == null || pubDateMove == null)
            {
                RadGridPublicationChannels.Controls.Add(new LiteralControl("Unable to locate the ChannelName for updating."));
                e.Canceled = true;
                return;
            }

            int selectedChannelID = Utils.GetChannelID(channelName.Text);
            List<Models.PublicationChannel> pcList = (DataItem as Models.Publication).PublicationChannels;
            Models.PublicationChannel pc = pcList.FirstOrDefault(p => p.ChannelID == selectedChannelID);
            if (pc != null)
            {
                if (use.CheckBoxControl.Checked == false)
                {
                    pcList.Remove(pc);
                }
                pc.Trigger = trigger.SelectedIndex;
                pc.PubDateMoveDays = Utils.StringToInt(pubDateMove.Text);
                pc.ReleaseDelay = Utils.StringToInt(releaseDelay.Text); 
            }
            else
            {
                Models.PublicationChannel pcNew = new Models.PublicationChannel()
                {
                    ChannelID = selectedChannelID,
                    Trigger = trigger.SelectedIndex,
                    PubDateMoveDays = Utils.StringToInt(pubDateMove.Text),
                    ReleaseDelay = Utils.StringToInt(releaseDelay.Text)
                };
            }
        }

        protected void RadGridPublicationChannels_PreRender(object sender, EventArgs e)
        {
            RadNumericTextBox numericTextBox = (RadGridPublicationChannels.MasterTableView.GetBatchColumnEditor("PubDateMoveDays") as GridNumericColumnEditor).NumericTextBox;
            numericTextBox.Width = Unit.Pixel(40);
            numericTextBox = (RadGridPublicationChannels.MasterTableView.GetBatchColumnEditor("ReleaseDelay") as GridNumericColumnEditor).NumericTextBox;
            numericTextBox.Width = Unit.Pixel(40);
            TextBox textBox = (RadGridPublicationChannels.MasterTableView.GetBatchColumnEditor("ChannelName") as GridTextBoxColumnEditor).TextBoxControl;
            textBox.ReadOnly = true; // No edit allowed..!
            textBox.BackColor = System.Drawing.Color.LightGray;
        }
    }
}