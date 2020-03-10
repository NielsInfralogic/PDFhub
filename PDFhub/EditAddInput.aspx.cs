using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class EditAddInput : System.Web.UI.Page
    {
        private Models.InputConfiguration _input = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Models.InputConfiguration input = new Models.InputConfiguration
                {
                    InputID = 0
                };

                RadTextBoxTestResult.Text = "";

               int inputIDFromMainForm;
                if (Request.QueryString["InputID"] != null)
                {
                    inputIDFromMainForm = Utils.StringToInt(Request.QueryString["InputID"]);
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    if (db.GetInputConfiguration(ref input, inputIDFromMainForm, out string errmsg) == false)
                        Utils.WriteLog(false, "ERROR: db.GetInputConfiguration() - " + errmsg);

                    btnUpdate.Text = "Update";
                }

                Input = input;

                if (Input != null)
                {
                    cbEnabled.Checked = Input.Enabled;
                    txtInputName.Text = Input.InputName;
                    DropDownListType.SelectedIndex = (int)Input.InputType;
                    //Input.InputType = MapInputType(item.InputTypeStr);
                    txtNamingMask.Text  = Input.NamingMask;
                    txtSeparators.Text = Input.Separators;

                    txtSearchMask.Text = Input.SearchMask;
                    RadNumericStableTime.Value = Input.StableTime;
                    RadNumericPollTime.Value = Input.PollTime;

                    txtInputFolder.Text = Input.InputPath;
                    cbSpecificUser.Checked = Input.UseSpecificUser;
                    txtUserNameX.Text = Input.UserName;
                    txtPasswordX.Text  = Input.Password;
                    cbRegex.Checked = Input.UseRegex;

                    txtFtpServer.Text = Input.FTPserver;
                    txtFtpUsername.Text  =Input.FTPusername;
                    txtFtpPassword.Text = Input.FTPpassword;
                    txtFtpFolder.Text = Input.FTPfolder;
                    cbFtpPassive.Checked = Input.FTPpasw;
                    cbFtpXcrc.Checked = Input.FTPxcrc;
                    RadDropDownListEncryption.SelectedIndex = (int)Input.FTPtls;

                    cbArchive.Checked = Input.MakeCopy;
                    txtCopyFolder.Text = Input.CopyFolder;

                    RadCheckBoxAckFile.Checked = Input.SendAckFile;
                    RadNumericAckCode.Value = Input.AckFlagValue;
                    RadTextBoxAckFileFolder.Text = Input.AckFileFolder;

                }
                DropDownListType_SelectedIndexChanged(null, null);

            }
        }

        public Models.InputConfiguration Input
        {
            get
            {
                if (this._input == null)
                {
                    if (Session["EditedInputDataItem"] != null)
                    {
                        this._input = (Models.InputConfiguration)Session["EditedInputDataItem"];
                    }
                }

                return this._input;
            }
            set
            {
                if (value is Models.InputConfiguration)
                {
                    if (value != null)
                    {
                        Session["EditedInputDataItem"] = value;
                        this._input = (Models.InputConfiguration)value;
                    }
                    else if (Session["EditedInputDataItem"] != null)
                    {
                        this._input = (Models.InputConfiguration)Session["EditedInputDataItem"];
                    }
                }
            }
        }

        protected void RadGridRegex_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.RegExpression> regList = new List<Models.RegExpression>();
            if (Input != null)
                regList = Input.RegularExpressions;
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

        protected void RadGridRegex_ItemCommand(object sender, GridCommandEventArgs e)
        {
            List<Models.RegExpression> regExpList = Input.RegularExpressions;

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

                Input.RegularExpressions = regExpList.OrderBy(p => p.Rank).ToList(); ;
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

            if (e.CommandName == "PerformInsert" || e.CommandName == "Update")
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
            if (Input != null)
            {

                Input.Enabled = cbEnabled.Checked.Value;
                Input.InputName = txtInputName.Text;
                Input.InputType = (Models.InputType)(DropDownListType.SelectedItem.Index);
                //Input.InputType = MapInputType(item.InputTypeStr);
                Input.NamingMask = txtNamingMask.Text;
                Input.Separators = txtSeparators.Text;

                Input.SearchMask = txtSearchMask.Text;
                Input.StableTime = (int)RadNumericStableTime.Value;
                Input.PollTime = (int)RadNumericPollTime.Value;

                Input.InputPath = txtInputFolder.Text;
                Input.UseSpecificUser = cbSpecificUser.Checked.Value;
                Input.UserName = txtUserNameX.Text;
                Input.Password = txtPasswordX.Text;
                Input.UseRegex = cbRegex.Checked.Value;

                Input.FTPserver = txtFtpServer.Text;
                Input.FTPusername = txtFtpUsername.Text;
                Input.FTPpassword = txtFtpPassword.Text;
                Input.FTPfolder = txtFtpFolder.Text;
                Input.FTPpasw = cbFtpPassive.Checked.Value;
                Input.FTPxcrc = cbFtpXcrc.Checked.Value;
                Input.FTPxcrc = cbFtpXcrc.Checked.Value;
                Input.FTPtls = (Models.EncryptionType)(RadDropDownListEncryption.SelectedItem.Index);

                Input.MakeCopy = cbArchive.Checked.Value;
                Input.CopyFolder = txtCopyFolder.Text;

                Input.SendAckFile = RadCheckBoxAckFile.Checked.Value;
                Input.AckFlagValue = (int)RadNumericAckCode.Value;
                Input.AckFileFolder = RadTextBoxAckFileFolder.Text;


                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.InsertUpdateInputConfiguration(Input, out string errmsg) == false)
                    Utils.WriteLog(false, "ERROR: db.InsertUpdateInputConfiguration() - " + errmsg)
;
                //    InjectScript.Text = "<script>CloseAndRebind()</" + "script>";
                if (Input.InputID > 0) // update
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

        protected void btnTestRegex_Click(object sender, EventArgs e)
        {
            if (Input == null)
                return;
            if (Input.RegularExpressions == null)
                return;

            RadTextBoxTestResult.Text = "No matches!";

            //business logic goes here
            System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace;
            foreach (Models.RegExpression regex in Input.RegularExpressions)
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(RadTextBoxTestInput.Text, regex.MatchExpression, options);
                if (match.Success)
                {
                    RadTextBoxTestResult.Text = System.Text.RegularExpressions.Regex.Replace(RadTextBoxTestInput.Text, regex.MatchExpression, regex.FormatExpression, options);                  
                    break;
                }
                
            }
        }
    }
}
 