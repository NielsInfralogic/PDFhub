using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigInput : System.Web.UI.Page
    {
        private List<Models.InputConfiguration> inputConfigurations = new List<Models.InputConfiguration>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGridInput.MasterTableView.EditMode = GridEditMode.EditForms;
                //RadGridInput.MasterTableView.EditMode = GridEditMode.PopUp;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridInput.MasterTableView.SortExpressions.Clear();
                RadGridInput.MasterTableView.GroupByExpressions.Clear();
                RadGridInput.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridInput.MasterTableView.SortExpressions.Clear();
                RadGridInput.MasterTableView.GroupByExpressions.Clear();
                RadGridInput.MasterTableView.CurrentPageIndex = RadGridInput.MasterTableView.PageCount - 1;
                RadGridInput.Rebind();
            }
        }

        private void RebindInputGrid(bool callRebind)
        {
            //inputConfigurations = new List<Models.InputConfiguration>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetInputConfigurations(ref inputConfigurations, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridInput.DataSource = inputConfigurations;
            if (callRebind)
                RadGridInput.Rebind();
        }


        protected void RadGridInput_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["InputID"], e.Item.ItemIndex);

            }
        }
        protected void RadGridInput_PreRender(object sender, System.EventArgs e)
        {
        /*    if (!this.IsPostBack && this.RadGridInput.MasterTableView.Items.Count > 1)
            {
                this.RadGridInput.MasterTableView.Items[1].Edit = true;
                this.RadGridInput.MasterTableView.Rebind();
            }*/
        }

        protected void RadGridInput_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindInputGrid(false);
            // set primary key?
        }      

        protected void RadGridInput_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            if (editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["InputID"] == null)
            {
                RadGridInput.Controls.Add(new LiteralControl("Unable to locate the InputID for updating."));
                e.Canceled = true;
                return;
            }
            int? inputID = (int?)editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["InputID"];
            if (inputID.HasValue == false)
            {
                RadGridInput.Controls.Add(new LiteralControl("Unable to locate the InputID for updating."));
                e.Canceled = true;
                return;
            }

            Models.InputConfiguration selectedItem = new Models.InputConfiguration() { InputID = inputID.Value };
            
            ReadUserControlValues(userControl, ref selectedItem);

            selectedItem.RegularExpressions = ((Models.InputConfiguration)Session["EditedInputDataItem"]).RegularExpressions;
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateInputConfiguration(selectedItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }

            Session["EditedInputDataItem"] = null;
        }

        protected void RadGridInput_InsertCommand(object source, GridCommandEventArgs e)
        {
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            Models.InputConfiguration newItem = new Models.InputConfiguration() { InputID = 0 };

            ReadUserControlValues(userControl, ref newItem);

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateInputConfiguration(newItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
            Session["EditedInputDataItem"] = null;

        }

        protected void RadGridInput_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            int? inputID = (int?)deleteItem.OwnerTableView.DataKeyValues[deleteItem.ItemIndex]["InputID"];
            if (inputID.HasValue == false)
            {
                RadGridInput.Controls.Add(new LiteralControl("Unable to locate the InputID for deleting."));
                e.Canceled = true;
                Session["EditedInputDataItem"] = null;
                return;
            }

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeleteInputConfiguration(inputID.Value, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
            Session["EditedInputDataItem"] = null;
        }

        private void ReadUserControlValues(UserControl userControl, ref Models.InputConfiguration item)
        {
            item.Enabled = (bool)(userControl.FindControl("cbEnabled") as RadCheckBox).Checked;
            item.InputName = (userControl.FindControl("txtInputName") as RadTextBox).Text;
            item.InputType = (Models.InputType)((userControl.FindControl("DropDownListType") as RadDropDownList).SelectedItem.Index);
            //item.InputType = MapInputType(item.InputTypeStr);
            item.NamingMask = (userControl.FindControl("txtNamingMask") as RadTextBox).Text;
            item.Separators = (userControl.FindControl("txtSeparators") as RadTextBox).Text;

            item.SearchMask = (userControl.FindControl("txtSearchMask") as RadTextBox).Text;
            item.StableTime = (int)(userControl.FindControl("RadNumericStableTime") as RadNumericTextBox).Value;
            item.PollTime = (int)(userControl.FindControl("RadNumericPollTime") as RadNumericTextBox).Value;

            item.InputPath = (userControl.FindControl("txtInputFolder") as RadTextBox).Text;
            item.UseSpecificUser = (bool)(userControl.FindControl("cbSpecificUser") as RadCheckBox).Checked;
            item.UserName = (userControl.FindControl("txtUserName") as RadTextBox).Text;
            item.Password = (userControl.FindControl("txtPassword") as RadTextBox).Text;
            item.UseRegex = (bool)(userControl.FindControl("cbRegex") as RadCheckBox).Checked;

            item.FTPserver = (userControl.FindControl("txtFtpServer") as RadTextBox).Text;
            item.FTPusername = (userControl.FindControl("txtFtpUsername") as RadTextBox).Text;
            item.FTPpassword = (userControl.FindControl("txtFtpPassword") as RadTextBox).Text;
            item.FTPfolder = (userControl.FindControl("txtFtpFolder") as RadTextBox).Text;
            item.FTPpasw = (bool)(userControl.FindControl("cbFtpPassive") as RadCheckBox).Checked;
            item.FTPxcrc = (bool)(userControl.FindControl("cbFtpXcrc") as RadCheckBox).Checked;
            item.FTPxcrc = (bool)(userControl.FindControl("cbFtpXcrc") as RadCheckBox).Checked;
            item.FTPtls = (Models.EncryptionType)((userControl.FindControl("RadDropDownListEncryption") as RadDropDownList).SelectedItem.Index);

           

        }
    }
}