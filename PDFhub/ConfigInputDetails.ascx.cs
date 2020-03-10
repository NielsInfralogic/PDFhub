using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


namespace PDFhub
{
    public partial class ConfigInputDetails : System.Web.UI.UserControl
    {
        private object _dataItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            PanelSMB.Visible = false;
            PanelFTP.Visible = false;
            PanelEmail.Visible = false;
            PanelGoogle.Visible = false;
            PanelS3.Visible = false;

            if (DropDownListType.SelectedIndex == 1)
                PanelFTP.Visible = true;
            else if (DropDownListType.SelectedIndex == 2)
                PanelEmail.Visible = true;
            else if (DropDownListType.SelectedIndex == 3)
                PanelGoogle.Visible = true;
            else if (DropDownListType.SelectedIndex == 4)
                PanelS3.Visible = true;
            else
                PanelSMB.Visible = true;
        }

        public object DataItem
        {
            get
            {
                if (this._dataItem == null)
                {
                    if (Session["EditedInputDataItem"] != null)
                    {
                        this._dataItem = (Models.InputConfiguration)Session["EditedInputDataItem"];
                    }
                }

                return this._dataItem;
            }
            set
            {
                if (value is Models.InputConfiguration)
                {
                    if (value != null)
                    {
                        Session["EditedInputDataItem"] = value;
                        this._dataItem = value;
                    }
                    else if (Session["EditedInputDataItem"] != null)
                    {
                        this._dataItem = Session["EditedInputDataItem"];
                    }
                }
            }
        }

        protected void RadGridRegex_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.RegExpression> regList = new List<Models.RegExpression>();
            if (DataItem != null && (DataItem is Models.InputConfiguration))
                regList = (DataItem as Models.InputConfiguration).RegularExpressions;
            RadGridRegex.DataSource = regList;
        }


        protected void RadGridRegex_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                if ((e.Item as GridEditableItem)["MatchExpression"].Controls[0] is TextBox textBox)
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


    /*    protected void RadGridRegex_UpdateCommand(object source, GridCommandEventArgs e)
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

            List<Models.RegExpression> regExpList = (DataItem as Models.InputConfiguration).RegularExpressions;
            Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
            if (rx != null)
            {
                rx.MatchExpression = me.Text;
                rx.FormatExpression = fe.Text;
                rx.Comment = cm.Text;
            }
        }

        protected void RadGridRegex_InsertCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem newItem = e.Item as GridEditableItem;
            GridTextBoxColumnEditor me = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("MatchExpression");
            GridTextBoxColumnEditor fe = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("FormatExpression");
            GridTextBoxColumnEditor cm = (GridTextBoxColumnEditor)newItem.EditManager.GetColumnEditor("Comment");

            if (me == null || fe == null || cm == null)
            {
                RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the row for inserting."));
                e.Canceled = true;
                return;
            }

            List<Models.RegExpression> regExpList = (DataItem as Models.InputConfiguration).RegularExpressions;
            regExpList.Add(new Models.RegExpression() { Rank = regExpList.Count+1, MatchExpression = me.Text, FormatExpression = fe.Text, Comment = cm.Text });
        }
        protected void RadGridRegex_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem deleteItem = e.Item as GridEditableItem;
            GridTextBoxColumnEditor r = (GridTextBoxColumnEditor)deleteItem.EditManager.GetColumnEditor("Rank");
            if (r == null)
            {
                RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the item for deleting."));
                e.Canceled = true;
                return;
            }
            int rank = Utils.StringToInt(r.Text);
            if (rank == 0)
            {
                RadGridRegex.Controls.Add(new LiteralControl("Unable to locate the item for deleting."));
                e.Canceled = true;
                return;
            }
            List<Models.RegExpression> regExpList = (DataItem as Models.InputConfiguration).RegularExpressions;
            Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
            if (rx != null)
                regExpList.Remove(rx);
        }*/

        protected void RadGridRegex_ItemCommand(object sender, GridCommandEventArgs e)
        {

            List<Models.RegExpression> regExpList = new List<Models.RegExpression>();
            if (DataItem != null && (DataItem is Models.InputConfiguration))
                regExpList = (DataItem as Models.InputConfiguration).RegularExpressions;

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
                (DataItem as Models.InputConfiguration).RegularExpressions = regExpList.OrderBy(p => p.Rank).ToList(); ;
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
    }
}