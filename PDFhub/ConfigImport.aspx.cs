using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigImport : System.Web.UI.Page
    {
        private List<Models.ImportConfiguration> importConfigurations = new List<Models.ImportConfiguration>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGridImport.MasterTableView.EditMode = GridEditMode.EditForms;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridImport.MasterTableView.SortExpressions.Clear();
                RadGridImport.MasterTableView.GroupByExpressions.Clear();
                RadGridImport.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridImport.MasterTableView.SortExpressions.Clear();
                RadGridImport.MasterTableView.GroupByExpressions.Clear();
                RadGridImport.MasterTableView.CurrentPageIndex = RadGridImport.MasterTableView.PageCount - 1;
                RadGridImport.Rebind();
            }
        }



        private void RebindImportGrid(bool callRebind)
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetImportConfigurations(ref importConfigurations, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridImport.DataSource = importConfigurations;

            if (callRebind)
                RadGridImport.Rebind();
        }

        protected void RadGridImport_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ImportID"], e.Item.ItemIndex);
            }
        }

        protected void RadGridImport_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindImportGrid(false);
        }

        protected void RadGridImport_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            if (editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ImportID"] == null)
            {
                RadGridImport.Controls.Add(new LiteralControl("Unable to locate the ImportID for updating."));
                e.Canceled = true;
                return;
            }
            int? importID = (int?)editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ImportID"];
            if (importID.HasValue == false)
            {
                RadGridImport.Controls.Add(new LiteralControl("Unable to locate the ImportID for updating."));
                e.Canceled = true;
                return;
            }

            Models.ImportConfiguration selectedItem = new Models.ImportConfiguration() { ImportID = importID.Value };

            ReadUserControlValues(userControl, ref selectedItem);
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateImportConfiguration(selectedItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }

        }

        protected void RadGridImport_InsertCommand(object source, GridCommandEventArgs e)
        {
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            Models.ImportConfiguration newItem = new Models.ImportConfiguration() { ImportID = 0 };

            ReadUserControlValues(userControl, ref newItem);

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdateImportConfiguration(newItem, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }

        }

        protected void RadGridImport_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            int? importID = (int?)deleteItem.OwnerTableView.DataKeyValues[deleteItem.ItemIndex]["ImportID"];
            if (importID.HasValue == false)
            {
                RadGridImport.Controls.Add(new LiteralControl("Unable to locate the ImportID for deleting."));
                e.Canceled = true;
                return;
            }

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeleteImportConfiguration(importID.Value, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
        }

        private void ReadUserControlValues(UserControl userControl, ref Models.ImportConfiguration item)
        {
            item.Enabled = (bool)(userControl.FindControl("cbEnabled") as RadCheckBox).Checked;
            item.Name = (userControl.FindControl("txtImportName") as RadTextBox).Text;
            item.ImportType = ((userControl.FindControl("DropDownListType") as RadDropDownList).SelectedItem.Index);
            item.InputFolder = (userControl.FindControl("txtInputFolder") as RadTextBox).Text;
            item.DoneFolder = (userControl.FindControl("txtDoneFolder") as RadTextBox).Text;
            item.ErrorFolder = (userControl.FindControl("txtErrorFolder") as RadTextBox).Text;
            item.LogFolder = (userControl.FindControl("txtLogFolder") as RadTextBox).Text;
        }
    }
}