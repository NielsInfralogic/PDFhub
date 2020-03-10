using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigPublication : System.Web.UI.Page
    {
        private List<Models.Publication> publications = new List<Models.Publication>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGridPublication.MasterTableView.EditMode = GridEditMode.EditForms;
                //RadGridInput.MasterTableView.EditMode = GridEditMode.PopUp;
            }
        }

        private void RebindPublicationGrid(bool callRebind)
        {
            //inputConfigurations = new List<Models.InputConfiguration>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetPublications(ref publications, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridPublication.DataSource = publications;
            if (callRebind)
                RadGridPublication.Rebind();
        }

        protected void RadGridPublication_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindPublicationGrid(false);
            // set primary key?
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridPublication.MasterTableView.SortExpressions.Clear();
                RadGridPublication.MasterTableView.GroupByExpressions.Clear();
                RadGridPublication.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridPublication.MasterTableView.SortExpressions.Clear();
                RadGridPublication.MasterTableView.GroupByExpressions.Clear();
                RadGridPublication.MasterTableView.CurrentPageIndex = RadGridPublication.MasterTableView.PageCount - 1;
                RadGridPublication.Rebind();
            }
        }

        protected void RadGridPublication_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            int? publicationID = (int?)deleteItem.OwnerTableView.DataKeyValues[deleteItem.ItemIndex]["PublicationID"];
            if (publicationID.HasValue == false)
            {
                RadGridPublication.Controls.Add(new LiteralControl("Unable to locate the PublicationID for deleting."));
                e.Canceled = true;
                return;
            }

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeletePublication(publicationID.Value, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                e.Canceled = true;
            }
        }

        protected void RadGridPublication_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PublicationID"], e.Item.ItemIndex);

                ImageButton docButton = (ImageButton)e.Item.FindControl("DocButton");
                if (docButton != null)
                    docButton.Attributes["onclick"] = String.Format("return ShowDocForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PublicationID"], e.Item.ItemIndex);
            }

        }
    }
}