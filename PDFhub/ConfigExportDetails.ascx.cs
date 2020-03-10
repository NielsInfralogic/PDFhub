using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigExportDetails : System.Web.UI.UserControl
    {
        private object _dataItem = null;



        protected void Page_Load(object sender, EventArgs e)
        {
            Models.Channel ch = null;
            int nOutputType = 1;
            if (DataItem is Models.Channel)
            {
                ch = (DataItem as Models.Channel);
                nOutputType = ch.OutputType;
            }
        
         //   if ((bool)Session["ConfigExportDetailsFormLoaded"] == true)
          //  {
                nOutputType = DropDownListType.SelectedIndex;
                Session["ConfigExportDetailsFormLoaded"] = true;
         //   }
            PanelSMB.Visible = false;
            PanelFTP.Visible = false;
            PanelEmail.Visible = false;
            PanelGoogle.Visible = false;
            PanelS3.Visible = false;

            if (nOutputType == 1)
                PanelFTP.Visible = true;
            else if (nOutputType == 2)
                PanelEmail.Visible = true;
            else if (nOutputType == 3)
                PanelGoogle.Visible = true;
            else if (nOutputType == 4)
                PanelS3.Visible = true;
            else
                PanelSMB.Visible = true;

          /*  if (!IsPostBack)
            {
                RadDropDownListChannelGroup.Items.Clear();
                if (Session["ChannelGroupList"] != null)
                {
                    List<Models.ChannelGroup> channelGroupList = (List<Models.ChannelGroup>)Session["ChannelGroupList"];
                    if (channelGroupList != null)
                    {
                        foreach (Models.ChannelGroup cg in channelGroupList)
                            RadDropDownListChannelGroup.Items.Add(cg.Name);
                    }
                }
                RadDropDownListPublisher.Items.Clear();
                if (Session["PubliserList"] != null)
                {
                    List<Models.Publisher> publisherList = (List<Models.Publisher>)Session["PubliserList"];
                    if (publisherList != null)
                    {
                        foreach (Models.Publisher publ in publisherList)
                            RadDropDownListPublisher.Items.Add(publ.PublisherName);
                    }
                }
            }*/
        }

        public object DataItem
        {
            get
            {
                return this._dataItem;
            }
            set
            {
                if (value is Models.Channel)
                {
                    if (value != null)
                    {
                        Session["EditedChannelDataItem"] = value;
                        this._dataItem = value;
                    }
                    else if (Session["EditedChannelDataItem"] != null)
                    {
                        this._dataItem = Session["EditedChannelDataItem"];
                    }
                }
            }
        }

        protected void RadGridRegex_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.RegExpression> regList = new List<Models.RegExpression>();
            if (DataItem != null && (DataItem is Models.Channel))
                regList = (DataItem as Models.Channel).RegularExpressions;
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

        protected void RadGridRegex_UpdateCommand(object source, GridCommandEventArgs e)
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

            List<Models.RegExpression> regExpList = (DataItem as Models.Channel).RegularExpressions;
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

            List<Models.RegExpression> regExpList = (DataItem as Models.Channel).RegularExpressions;
            regExpList.Add(new Models.RegExpression() { Rank = regExpList.Count + 1, MatchExpression = me.Text, FormatExpression = fe.Text, Comment = cm.Text });
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
            List<Models.RegExpression> regExpList = (DataItem as Models.Channel).RegularExpressions;
            Models.RegExpression rx = regExpList.FirstOrDefault(p => p.Rank == rank);
            if (rx != null)
                regExpList.Remove(rx);
        }

        protected void RadGridRegex_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (!(e.Item is GridDataItem item))
                return;
            var textBox = item["Rank"];
            if (textBox == null)
                return;
            int rank = Utils.StringToInt(textBox.Text);

            List<Models.RegExpression> regExpList = (DataItem as Models.Channel).RegularExpressions;

            if (e.CommandName == "Up" || e.CommandName == "Down")
            {
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
                (DataItem as Models.Channel).RegularExpressions = regExpList.OrderBy(p => p.Rank).ToList(); ;
                RadGridRegex.Rebind();

            }
        }

    }
}