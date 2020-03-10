using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class UserManagement : System.Web.UI.Page
    {
        private List<Models.User> users = new List<Models.User>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGridUsers.MasterTableView.EditMode = GridEditMode.EditForms;
                //RadGridInput.MasterTableView.EditMode = GridEditMode.PopUp;
            }

        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridUsers.MasterTableView.SortExpressions.Clear();
                RadGridUsers.MasterTableView.GroupByExpressions.Clear();
                RadGridUsers.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridUsers.MasterTableView.SortExpressions.Clear();
                RadGridUsers.MasterTableView.GroupByExpressions.Clear();
                RadGridUsers.MasterTableView.CurrentPageIndex = RadGridUsers.MasterTableView.PageCount - 1;
                RadGridUsers.Rebind();
            }
        }

        private void RebindUsersGrid(bool callRebind)
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetUsers(ref users, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridUsers.DataSource = users;

            if (callRebind)
                RadGridUsers.Rebind();
        }

        protected void RadGridUsers_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserName"], e.Item.ItemIndex);

            }
        }

     

        protected void RadGridUsers_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindUsersGrid(false);
        }

        protected void RadGridUsers_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            string userName = (string)deleteItem.OwnerTableView.DataKeyValues[deleteItem.ItemIndex]["UserName"];
            if (userName == null)
            {
                RadGridUsers.Controls.Add(new LiteralControl("Unable to locate the UserName for deleting."));
                e.Canceled = true;
                Session["EditedUserDataItem"] = null;
                return;
            }

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeleteUser(userName, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
            Session["EditedUserDataItem"] = null;
        }

      
    }
}