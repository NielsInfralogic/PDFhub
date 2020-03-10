using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class EditAddExport : System.Web.UI.Page
    {
        private Models.Channel _channel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Models.Channel channel = new Models.Channel();
                channel.ChannelID = 0;
                channel.PDFProcessID = 1;

                int channelIDFromMainForm = 0;
                if (Request.QueryString["ChannelID"] != null)
                {
                    channelIDFromMainForm = Utils.StringToInt(Request.QueryString["ChannelID"]);
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.GetChannel(channelIDFromMainForm, ref channel, out string errmsg);

                    btnUpdate.Text = "Update";
                }
                Channel = channel;

                RadDropDownListPDFProcessID.Items.Clear();
                List<string> pdfProcessNames = Utils.GetPDFProcessNames();
                foreach (string s in pdfProcessNames)
                {
                    RadDropDownListPDFProcessID.Items.Add(new DropDownListItem(s, s));
                }
                RadDropDownListPDFProcessID.SelectedIndex = 0;


                if (Channel != null)
                {
                    cbEnabled.Checked = Channel.Enabled;
                    txtChannelName.Text = Channel.Name;
                    txtChannelNameAlias.Text = Channel.ChannelNameAlias;
                    RadDropDownListPDFType.SelectedIndex = Channel.PDFType;
                    cbMergedPDF.Checked = Channel.MergedPDF;
                    RadDropDownListEditionsToGenerate.SelectedIndex = Channel.EditionsToGenerate;
                    cbSendCommonPages.Checked = Channel.SendCommonPages;
                    cbUseReleaseTime.Checked = Channel.UseReleaseTime;
                    int h = Channel.ReleaseTime / 100;
                    RadTimePickerReleaseTime.SelectedTime = new TimeSpan(h, Channel.ReleaseTime - h * 100, 0);
                    h = Channel.ReleaseTimeEnd / 100;
                    RadTimePickerReleaseTimeEnd.SelectedTime = new TimeSpan(h, Channel.ReleaseTimeEnd - h * 100, 0);
                    txtTransmitNameFormat.Text = Channel.TransmitNameFormat;
                    txtTransmitNameDateFormat.Text = Channel.TransmitNameDateFormat;
                    txtSubFolderNamingConvension.Text = Channel.SubFolderNamingConvension;
                    RadDropDownListTransmitNameUseAbbr.SelectedIndex = Channel.TransmitNameUseAbbr;
                    //cbSendMail.Checked = Channel.SendMail;
                    DropDownListType.SelectedIndex = Channel.OutputType;
                    txtOutputFolder.Text = Channel.OutputFolder;
                    cbSpecificUser.Checked = Channel.UseSpecificUser;
                    txtUserNameX.Text = Channel.UserName;
                    txtPasswordX.Text = Channel.Password;
                    txtFtpServer.Text = Channel.FTPServer;
                    txtFtpFolder.Text = Channel.FTPfolder;
                    txtFtpUsername.Text = Channel.FTPUserName;
                    txtFtpPassword.Text = Channel.FTPPassword;
                    cbFtpPassive.Checked = Channel.FTPPasv;
                    cbFtpXcrc.Checked = Channel.FTPXCRC;
                    RadDropDownListEncryption.SelectedIndex = Channel.FTPEncryption;
                    RadDropDownListFTPPostCheck.SelectedIndex = Channel.FTPPostCheck;

                    RadDropDownListTriggerMode.SelectedIndex = Channel.TriggerMode;
                    txtEmailReceivers.Text = Channel.TriggerEmail;

                    cbDeleteOldOutputFiles.Checked = Channel.DeleteOldOutputFilesDays> 0; // reuse...

                    cbUsePackageNames.Checked = Channel.UsePackageNames;

                    cbSelectedPagesOnly.Checked = channel.OnlySentSelectedPages;
                    RadNumericTextBoxFromPage.Value = channel.PageNumberStart;
                    RadNumericTextBoxToPage.Value = channel.PageNumberEnd;

                    if (Channel.PDFType == (int)Models.PDFtype.PDFHighRes)
                        RadDropDownListPDFProcessID.SelectedText = "None";
                    else
                    {
                        Models.PDFProcess pdfProcess = Utils.GetPDFProcessFromID(channel.PDFProcessID);
                        if (pdfProcess != null)
                            RadDropDownListPDFProcessID.SelectedText = pdfProcess.ProcessName;
                    }

                }

                DropDownListType_SelectedIndexChanged(null, null);
            }
        }

        public Models.Channel Channel
        {
            get
            {
                if (this._channel == null)
                {
                    if (Session["EditedChannelDataItem"] != null)
                    {
                        this._channel = (Models.Channel)Session["EditedChannelDataItem"];
                    }
                }
                return this._channel;
            }
            set
            {
                if (value is Models.Channel)
                {
                    if (value != null)
                    {
                        this._channel = value;
                        Session["EditedChannelDataItem"] = value;
                    }
                    else if (Session["EditedChannelDataItem"] != null)
                    {
                        this._channel = (Models.Channel)Session["EditedChannelDataItem"];
                    }
                }
            }
        }



        protected void RadGridRegex_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.RegExpression> regList = new List<Models.RegExpression>();
            if (Channel != null)
                regList = Channel.RegularExpressions;
            RadGridRegex.DataSource = regList;
        }

        protected void RadGridRegex_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                TextBox textBox = (e.Item as GridEditableItem)["MatchExpression"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 350;
                textBox = (e.Item as GridEditableItem)["FormatExpression"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 350;

                textBox = (e.Item as GridEditableItem)["Rank"].Controls[0] as TextBox;
                if (textBox != null)
                {
                    textBox.ReadOnly = true; // No edit allowed..!
                    textBox.Width = 70;
                }
            }
        }

        /*        protected void RadGridRegex_UpdateCommand(object source, GridCommandEventArgs e)
                {
                    GridEditableItem editedItem = e.Item as GridEditableItem;
                    GridTextBoxColumnEditor r = (GridTextBoxColumnEditor)editedItem.EditManager.GetColumnEditor("Rank");
                    GridTextBoxColumnEditor me = (GridTextBoxColumnEditor)editedItem.EditManager.GetColumnEditor("MatchExpression");
                    GridTextBoxColumnEditor fe = (GridTextBoxColumnEditor)editedItem.EditManager.GetColumnEditor("FormatExpression");
                    GridTextBoxColumnEditor cm = (GridTextBoxColumnEditor)editedItem.EditManager.GetColumnEditor("Comment");
                    if (r == null || me == null || fe == null || cm == null)
                    {
                        RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the Rank for updating."));
                        e.Canceled = true;
                        return;
                    }
                    int rank = Utils.StringToInt(r.Text);
                    if (rank == 0)
                    {
                        RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the Rank for updating."));
                        e.Canceled = true;
                        return;
                    }

                    List<Models.RegExpression> regExpList = Channel.RegularExpressions;
                    Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
                    if (rx != null)
                    {
                        rx.MatchExpression = me.Text;
                        rx.FormatExpression = fe.Text;
                        rx.Comment = cm.Text;
                    }
                }
    */
        /*     protected void RadGridRegex_ItemInserted(object sender, GridInsertedEventArgs e)
            {
               GridEditableItem newItem = e.Item as GridEditableItem;
                GridTextBoxColumnEditor rk = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("Rank");
                GridTextBoxColumnEditor me = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("MatchExpression");
                GridTextBoxColumnEditor fe = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("FormatExpression");

                if (me == null || fe == null || rk == null)
                {
                    RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the row for inserting."));
                    e.KeepInInsertMode = false;
                    return;
                }

                List<Models.RegExpression> regExpList = Channel.RegularExpressions;
                regExpList.Add(new Models.RegExpression() { Rank = regExpList.Count + 1, MatchExpression = me.Text, FormatExpression = fe.Text, Comment = ""});

            }
            */
        /*        protected void RadGridRegex_DeleteCommand(object source, GridCommandEventArgs e)
                {
                    string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Rank"].ToString();
                    GridEditableItem deleteItem = e.Item as GridEditableItem;
                    GridTextBoxColumnEditor r = (GridTextBoxColumnEditor)deleteItem.EditManager.GetColumnEditor("Rank");
                    if (r == null)
                    {
                        RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the item for deleting."));
                        //e.Canceled = true;
                        return;
                    }
                    int rank = Utils.StringToInt(r.Text);
                    if (rank == 0)
                    {
                        RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the item for deleting."));
                        //e.Canceled = true;
                        return;
                    }
                    List<Models.RegExpression> regExpList = Channel.RegularExpressions;
                    Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
                    if (rx != null)
                        regExpList.Remove(rx);
                }
            */
        protected void RadGridRegex_ItemCommand(object sender, GridCommandEventArgs e)
        {
          
            List<Models.RegExpression> regExpList = Channel.RegularExpressions;

            if (e.CommandName == "Up" || e.CommandName == "Down")
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item == null)
                    return;
                var textBox = item["Rank"];
                if (textBox == null)
                    return;
                int rank = Utils.StringToInt(textBox.Text);

                Models.RegExpression rx2 = null;
                Models.RegExpression rx1 = regExpList.FirstOrDefault(p => p.Rank == rank);
                if (e.CommandName == "Up")
                    rx2 = regExpList.FirstOrDefault(p => p.Rank == rank - 1);
                else
                    rx2 = regExpList.FirstOrDefault(p => p.Rank == rank + 1);
                if (rx1 != null && rx2 != null)
                {
                    int t = rx1.Rank;
                    rx1.Rank = rx2.Rank;
                    rx2.Rank = t;
                }

                Channel.RegularExpressions = regExpList.OrderBy(p => p.Rank).ToList(); ;
                RadGridRegex.Rebind();
            }

            if (e.CommandName == "Delete")
            {
                string r = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Rank"].ToString();
                int rank = r != null ? Utils.StringToInt(r) : 0;

                Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
                if (rx != null)
                {
                    regExpList.Remove(rx);
                }
            }

            if (e.CommandName == "PerformInsert" || e.CommandName == "Update" )
            {
               
                GridEditableItem item = e.Item as GridEditableItem;

                GridTextBoxColumnEditor r = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("Rank");
                              
                GridTextBoxColumnEditor me = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("MatchExpression");
                string match = me != null ? me.Text : "";
                GridTextBoxColumnEditor fm = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("FormatExpression");
                string format = fm != null ? fm.Text : "";
                GridTextBoxColumnEditor co = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("Comment");
                string comment = co != null ? co.Text : "";

                if (e.CommandName == "PerformInsert")
                    regExpList.Add(new Models.RegExpression() { Rank = regExpList.Count + 1, MatchExpression = match, FormatExpression = format, Comment = comment });
                else
                {
                    int rank = r != null ? Utils.StringToInt(r.Text) : 0;
                    Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);


                    if (rx != null)
                    { 
                        rx.MatchExpression = me.Text;
                        rx.FormatExpression = fm.Text;
                        rx.Comment = co.Text;
                    }
                    

                }

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Channel != null)
            {
                Channel.Enabled = cbEnabled.Checked.Value;
                Channel.Name = txtChannelName.Text;
                Channel.ChannelNameAlias = txtChannelNameAlias.Text;
                Channel.PDFType = RadDropDownListPDFType.SelectedIndex;
                Channel.MergedPDF = cbMergedPDF.Checked.Value;
                Channel.EditionsToGenerate = RadDropDownListEditionsToGenerate.SelectedIndex;
                Channel.SendCommonPages = cbSendCommonPages.Checked.Value;
                Channel.UseReleaseTime = cbUseReleaseTime.Checked.Value;
                TimeSpan ts = RadTimePickerReleaseTime.SelectedTime.Value;
                Channel.ReleaseTime = ts.Hours * 100 + ts.Minutes;
                ts = RadTimePickerReleaseTimeEnd.SelectedTime.Value;
                Channel.ReleaseTimeEnd = ts.Hours * 100 + ts.Minutes;
                Channel.TransmitNameFormat = txtTransmitNameFormat.Text;
                Channel.TransmitNameDateFormat = txtTransmitNameDateFormat.Text;
                Channel.SubFolderNamingConvension = txtSubFolderNamingConvension.Text;
                Channel.TransmitNameUseAbbr = RadDropDownListTransmitNameUseAbbr.SelectedIndex;

                // = Channel.SendMail = cbSendMail.Checked.Value;
                Channel.OutputType = DropDownListType.SelectedIndex;

                Channel.TriggerMode = RadDropDownListTriggerMode.SelectedIndex;
                Channel.TriggerEmail = txtEmailReceivers.Text;

                Channel.DeleteOldOutputFilesDays = cbDeleteOldOutputFiles.Checked.Value ? 1 : 0; 



                Channel.OutputFolder = txtOutputFolder.Text;
                Channel.UseSpecificUser = cbSpecificUser.Checked.Value;
                Channel.UserName = txtUserNameX.Text;
                Channel.Password = txtPasswordX.Text;
                Channel.FTPServer = txtFtpServer.Text;
                Channel.FTPfolder = txtFtpFolder.Text;
                Channel.FTPUserName = txtFtpUsername.Text;
                Channel.FTPPassword = txtFtpPassword.Text;
                Channel.FTPPasv = cbFtpPassive.Checked.Value;
                Channel.FTPXCRC = cbFtpXcrc.Checked.Value;
                Channel.FTPEncryption = RadDropDownListEncryption.SelectedIndex;
                Channel.FTPPostCheck = RadDropDownListFTPPostCheck.SelectedIndex;

                Models.PDFProcess pdfProcess = Utils.GetPDFProcessFromName(RadDropDownListPDFProcessID.SelectedValue);
                if (pdfProcess != null)
                    Channel.PDFProcessID = pdfProcess.ProcessID;


                Channel.UsePackageNames = cbUsePackageNames.Checked.Value;

                Channel.OnlySentSelectedPages = cbSelectedPagesOnly.Checked.Value;
                Channel.PageNumberStart = (int)RadNumericTextBoxFromPage.Value;
                Channel.PageNumberEnd = (int)RadNumericTextBoxToPage.Value;

                DataProviders.DBaccess db = new DataProviders.DBaccess();
                db.InsertUpdateChannel(Channel, out string errmsg);

                //    InjectScript.Text = "<script>CloseAndRebind()</" + "script>";
                if (Channel.ChannelID > 0) // update
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", true);
            }
        }

        protected void DropDownListType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int outputType = DropDownListType.SelectedIndex;
            PanelSMB.Visible = false;
            PanelFTP.Visible = false;
            PanelEmail.Visible = false;
            PanelGoogle.Visible = false;
            PanelS3.Visible = false;

            if (outputType == 1)
                PanelFTP.Visible = true;
            else if (outputType == 2)
                PanelEmail.Visible = true;
            else if (outputType == 3)
                PanelGoogle.Visible = true;
            else if (outputType == 4)
                PanelS3.Visible = true;
            else
                PanelSMB.Visible = true;
        }

        
    }
}