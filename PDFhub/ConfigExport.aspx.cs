using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigExport : System.Web.UI.Page
    {
        private List<Models.Channel> channels = new List<Models.Channel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGridExport.MasterTableView.EditMode = GridEditMode.EditForms;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridExport.MasterTableView.SortExpressions.Clear();
                RadGridExport.MasterTableView.GroupByExpressions.Clear();
                RadGridExport.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridExport.MasterTableView.SortExpressions.Clear();
                RadGridExport.MasterTableView.GroupByExpressions.Clear();
                RadGridExport.MasterTableView.CurrentPageIndex = RadGridExport.MasterTableView.PageCount - 1;
                RadGridExport.Rebind();
            }
        }

        private void RebindExportGrid(bool callRebind)
        {
            //inputConfigurations = new List<Models.InputConfiguration>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetChannelList(ref channels, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridExport.DataSource = channels;
            if (callRebind)
                RadGridExport.Rebind();
        }

        protected void RadGridExport_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindExportGrid(false);
        }

        protected void RadGridExport_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            if (editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ChannelID"] == null)
            {
                RadGridExport.Controls.Add(new LiteralControl("Unable to locate the ChannelID for updating."));
                e.Canceled = true;
                return;
            }
            int? channelID = (int?)editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ChannelID"];
            if (channelID.HasValue == false)
            {
                RadGridExport.Controls.Add(new LiteralControl("Unable to locate the ChannelID for updating."));
                e.Canceled = true;
                return;
            }

            Models.Channel selectedItem = new Models.Channel() { ChannelID = channelID.Value };

            ReadUserControlValues(userControl, ref selectedItem);
            selectedItem.RegularExpressions = ((Models.Channel)Session["EditedChannelDataItem"]).RegularExpressions;
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateChannel(selectedItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }

            Session["EditedChannelDataItem"] = null;
        }

        protected void RadGridExport_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ChannelID"], e.Item.ItemIndex);

                ImageButton docButton = (ImageButton)e.Item.FindControl("DocButton");
                if (docButton != null)
                    docButton.Attributes["onclick"] = String.Format("return ShowDocForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ChannelID"], e.Item.ItemIndex);

            }
        }


        protected void RadGridExport_InsertCommand(object source, GridCommandEventArgs e)
        {
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            Models.Channel newItem = new Models.Channel() { ChannelID = 0 };

            ReadUserControlValues(userControl, ref newItem);

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateChannel(newItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
            Session["EditedChannelDataItem"] = null;
        }

        protected void RadGridExport_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            int? channelID = (int?)deleteItem.OwnerTableView.DataKeyValues[deleteItem.ItemIndex]["ChannelID"];
            if (channelID.HasValue == false)
            {
                RadGridExport.Controls.Add(new LiteralControl("Unable to locate the ChannelID for deleting."));
                e.Canceled = true;
                Session["EditedChannelDataItem"] = null;
                return;
            }

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeleteChannel(channelID.Value, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
            Session["EditedChannelDataItem"] = null;
        }

        private void ReadUserControlValues(UserControl userControl, ref Models.Channel item)
        {
            item.Enabled = (bool)(userControl.FindControl("cbEnabled") as RadCheckBox).Checked;
            item.Name = (userControl.FindControl("txtChannelName") as RadTextBox).Text;
            item.ChannelNameAlias = (userControl.FindControl("txtChannelNameAlias") as RadTextBox).Text;
            item.OutputType = /*(Models.ChannelType) */((userControl.FindControl("DropDownListType") as RadDropDownList).SelectedItem.Index);

            item.PDFType = /*(Models.PDFType) */ ((userControl.FindControl("RadDropDownListPDFType") as RadDropDownList).SelectedItem.Index);
            item.MergedPDF = (bool)(userControl.FindControl("cbMergedPDF") as RadCheckBox).Checked;
            item.EditionsToGenerate = ((userControl.FindControl("RadDropDownListEditionsToGenerate") as RadDropDownList).SelectedItem.Index);
            item.SendCommonPages = (bool)(userControl.FindControl("cbSendCommonPages") as RadCheckBox).Checked;

            //            item.PublisherID = Utils.GetPublisherID((userControl.FindControl("RadDropDownListPublisher") as RadDropDownList).SelectedItem.Value);
            //            item.ChannelGroupID = Utils.GetChannelGroupID((userControl.FindControl("RadDropDownListChannelGroup") as RadDropDownList).SelectedItem.Value);

            item.UseReleaseTime = (bool)(userControl.FindControl("cbUseReleaseTime") as RadCheckBox).Checked;
            TimeSpan? dtStart = (userControl.FindControl("RadTimePickerReleaseTime") as RadTimePicker).SelectedTime;
            TimeSpan? dtEnd = (userControl.FindControl("RadTimePickerReleaseTimeEnd") as RadTimePicker).SelectedTime;
            if (dtStart.HasValue)
                item.ReleaseTime = dtStart.Value.Hours * 100 + dtStart.Value.Minutes;
            if (dtEnd.HasValue)
                item.ReleaseTimeEnd = dtEnd.Value.Hours * 100 + dtEnd.Value.Minutes;

            item.TransmitNameFormat = (userControl.FindControl("txtTransmitNameFormat") as RadTextBox).Text;
            item.TransmitNameDateFormat = (userControl.FindControl("txtTransmitNameDateFormat") as RadTextBox).Text;
            item.SubFolderNamingConvension = (userControl.FindControl("txtSubFolderNamingConvension") as RadTextBox).Text;
            item.TransmitNameUseAbbr = ((userControl.FindControl("RadDropDownListTransmitNameUseAbbr") as RadDropDownList).SelectedItem.Index);
            item.OutputFolder = (userControl.FindControl("txtOutputFolder") as RadTextBox).Text;
            item.UseSpecificUser = (bool)(userControl.FindControl("cbSpecificUser") as RadCheckBox).Checked;
            item.UserName = (userControl.FindControl("txtUserName") as RadTextBox).Text;
            item.Password = (userControl.FindControl("txtPassword") as RadTextBox).Text;

            item.FTPServer = (userControl.FindControl("txtFtpServer") as RadTextBox).Text;
            item.FTPUserName = (userControl.FindControl("txtFtpUsername") as RadTextBox).Text;
            item.FTPPassword = (userControl.FindControl("txtFtpPassword") as RadTextBox).Text;
            item.FTPfolder = (userControl.FindControl("txtFtpFolder") as RadTextBox).Text;
            item.FTPPasv = (bool)(userControl.FindControl("cbFtpPassive") as RadCheckBox).Checked;
            item.FTPXCRC = (bool)(userControl.FindControl("cbFtpXcrc") as RadCheckBox).Checked;
            item.FTPEncryption = /*(Models.EncryptionType)*/((userControl.FindControl("RadDropDownListEncryption") as RadDropDownList).SelectedItem.Index);
            item.FTPPostCheck = ((userControl.FindControl("RadDropDownListFTPPostCheck") as RadDropDownList).SelectedItem.Index);

        }

        protected void RadGridExport_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                UserControl MyUserControl = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl;
                GridDataItem parentItem = (e.Item as GridEditFormItem).ParentItem;

                RadDropDownList ddlType = (RadDropDownList)MyUserControl.FindControl("DropDownListType");
                if (ddlType == null)
                    return;
                
                int nOutputType = ddlType.SelectedIndex;
                Panel panelSMB = (Panel)MyUserControl.FindControl("PanelSMB");
                Panel panelFTP = (Panel)MyUserControl.FindControl("PanelFTP");
                Panel panelEmail = (Panel)MyUserControl.FindControl("PanelEmail");
                Panel panelGoogle = (Panel)MyUserControl.FindControl("PanelGoogle");
                Panel panelS3 = (Panel)MyUserControl.FindControl("PanelS3");

                panelSMB.Visible = false;
                panelFTP.Visible = false;
                panelEmail.Visible = false;
                panelGoogle.Visible = false;
                panelS3.Visible = false;

                if (nOutputType == 1)
                    panelFTP.Visible = true;
                else if (nOutputType == 2)
                    panelEmail.Visible = true;
                else if (nOutputType == 3)
                    panelGoogle.Visible = true;
                else if (nOutputType == 4)
                    panelS3.Visible = true;
                else
                    panelSMB.Visible = true;

                Session["ConfigExportDetailsFormLoaded"] = false;
            }
        }
    }
}