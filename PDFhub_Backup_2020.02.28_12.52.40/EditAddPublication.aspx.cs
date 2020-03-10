using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class EditAddPublication : System.Web.UI.Page
    {
        private Models.Publication _publication = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                lblError.Text = "";
                Models.Publication publication = new Models.Publication();
                publication.PublicationID = 0;

                int publicationIDFromMainForm = 0;
                if (Request.QueryString["PublicationID"] != null)
                {
                    publicationIDFromMainForm = Utils.StringToInt(Request.QueryString["PublicationID"]);
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.GetPublication(publicationIDFromMainForm, ref publication, out string errmsg);
                    
                    btnUpdate.Text = "Update";
                }
                Publication = publication;


                RadDropDownListPublisher.Items.Clear();
                List<string> publisherNames = Utils.GetPublisherNames();
                foreach (string s in publisherNames)
                    RadDropDownListPublisher.Items.Add(new DropDownListItem(s, Utils.GetPublisherID(s).ToString()));
                RadDropDownListPublisher.SelectedIndex = 0;

                if (Publication != null)
                {
                    // Load controls..
                    txtPublicationName.Text = Publication.Name;
                    txtAnnumText.Text = Publication.AnnumText;
                    txtInputAlias.Text = Publication.InputAlias;
                    txtOutputAlias.Text = Publication.OutputAlias;
                    txtExtendedAlias.Text = Publication.ExtendedAlias;
                    txtExtendedAlias2.Text = Publication.ExtendedAlias2;
                    txtEmailRecipient.Text = Publication.EmailRecipient;
                    cbDefaultApprove.Checked = Publication.DefaultApprove;
                    cbPubdateMove.Checked = Publication.PubdateMove;
                    cbSendReport.Checked = Publication.SendReport;
                    RadTimePickerReleaseTime.SelectedTime = Publication._ReleaseTime;
                    RadNumericReleaseDays.Value = Publication.ReleaseDays;
                    RadNumericTextBoxPubdateMoveDays.Value = Publication.PubdateMoveDays;
                    RadDropDownListPublisher.SelectedValue = Publication.PublisherID.ToString();

                    cbNoReleaseTimeUsed.Checked = (Publication.NoReleaseTime & 0x01) > 0;
                    cbNoReleaseTimeUsedHighres.Checked = (Publication.NoReleaseTime & 0x02) > 0;
                    cbNoReleaseTimeUsedPrint.Checked = (Publication.NoReleaseTime & 0x04) > 0;
                    RadNumericTextBoxAutoPurgeKeepDays.Value = Publication.AutoPurgeKeepDays;
                    RadNumericTextBoxAutoPurgeKeepDaysArchive.Value = Publication.AutoPurgeKeepDaysArchive;
                    cbAllowUnplanned.Checked = Publication.AllowUnplanned;

                    RadNumericTextBoxPriority.Value = Publication.DefaultPriority;

                    BuildPublicationChannelTable();
                }
            }
        }

        public Models.Publication Publication
        {
            get
            {
                if (this._publication == null)
                {
                    if (Session["EditedPublicationDataItem"] != null)
                    {
                        this._publication = (Models.Publication)Session["EditedPublicationDataItem"];
                    }
                }
               return this._publication;
            }
            set
            {
                if (value is Models.Publication)
                {
                    if (value != null)
                    {
                        this._publication = value;
                        Session["EditedPublicationDataItem"] = value;
                    }
                    else if (Session["EditedPublicationDataItem"] != null)
                    {
                        this._publication = (Models.Publication)Session["EditedPublicationDataItem"];
                    }
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row
                DropDownList ddlTrigger = (e.Row.FindControl("ddlTrigger") as DropDownList);
                
                string trigger = (e.Row.FindControl("lblTrigger") as Label).Text;
                ddlTrigger.Items.FindByValue(trigger).Selected = true;
            }
        }

        private void BuildPublicationChannelTable()
        {
            List<Models.PublicationChannel> specificPubChannels = new List<Models.PublicationChannel>();
            if (Publication != null)
                specificPubChannels = Publication.PublicationChannels;
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
                    bool sendPlan = false;
                    Models.PublicationChannel sc = specificPubChannels.FirstOrDefault(p => p.ChannelID == channel.ChannelID);
                    if (sc != null)
                    {
                        use = true;
                        trigger = sc.Trigger;
                        pubDateMoveDays = sc.PubDateMoveDays;
                        releaseDelay = sc.ReleaseDelay;
                        sendPlan = sc.SendPlan;
                    }
                    allPubChannels.Add(new Models.PublicationChannelAll()
                    {
                        ChannelName = channel.Name,
                        Use = use,
                        Trigger = trigger,
                        PubDateMoveDays = pubDateMoveDays,
                        ReleaseDelay = releaseDelay,
                        ChannelID = channel.ChannelID,
                        SendPlan = sendPlan
                    });
                }
            }
            catch
            {
            }
            GridViewPublicationChannels.DataSource = allPubChannels;
            GridViewPublicationChannels.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string errmsg = "";
            lblError.Text = "";
            if (Publication != null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();

                Publication.Name = txtPublicationName.Text;
                Publication.AnnumText = txtAnnumText.Text;
                Publication.InputAlias = txtInputAlias.Text;
                Publication.OutputAlias = txtOutputAlias.Text;


                if (Publication.PublicationID <= 0) // Insert
                {
                    int existingID = 0;
                    db.GetPublicationFromName(Publication.Name, Publication.InputAlias, ref existingID, out errmsg);
                    if (existingID > 0)
                    {
                        lblError.Text = "Name already in use";
                        return;
                    }
                }

                Publication.ExtendedAlias = txtExtendedAlias.Text;
                Publication.ExtendedAlias2 = txtExtendedAlias2.Text;
                Publication.EmailRecipient = txtEmailRecipient.Text;
                Publication.DefaultApprove = cbDefaultApprove.Checked.Value;
                Publication.PubdateMove = cbPubdateMove.Checked.Value;
                Publication.SendReport = cbSendReport.Checked.Value;
                TimeSpan sp = RadTimePickerReleaseTime.SelectedTime.Value;
                Publication.ReleaseTime = sp.Hours * 100 + sp.Minutes;
                Publication.ReleaseDays = (int)RadNumericReleaseDays.Value.Value;

                Publication.NoReleaseTime = 0;
                if (cbNoReleaseTimeUsed.Checked.Value)
                    Publication.NoReleaseTime |= 0x01;
                if (cbNoReleaseTimeUsedHighres.Checked.Value)
                    Publication.NoReleaseTime |= 0x02;
                if (cbNoReleaseTimeUsedPrint.Checked.Value)
                    Publication.NoReleaseTime |= 0x04;


                Publication.AutoPurgeKeepDays = (int)RadNumericTextBoxAutoPurgeKeepDays.Value;
                Publication.AutoPurgeKeepDaysArchive = (int)RadNumericTextBoxAutoPurgeKeepDaysArchive.Value;

                Publication.AllowUnplanned = cbAllowUnplanned.Checked.Value;

                Publication.PubdateMoveDays = (int)RadNumericTextBoxPubdateMoveDays.Value.Value;
                Publication.PublisherID = Utils.StringToInt(RadDropDownListPublisher.SelectedValue);

                Publication.DefaultPriority = (int)RadNumericTextBoxPriority.Value;


                Publication.PublicationChannels.Clear();

                foreach (GridViewRow row in GridViewPublicationChannels.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("cbUse");
                    if (cb == null)
                        continue;

                    if (cb.Checked)
                    {
                        int channelID = Utils.StringToInt(row.Cells[6].Text);
                        DropDownList ddl = (DropDownList)row.FindControl("ddlTrigger");
                        TextBox tb1 = (TextBox)row.FindControl("txtPubDateMoveDays");
                        TextBox tb2 = (TextBox)row.FindControl("txtReleaseDelay");
                        CheckBox cb2 = (CheckBox)row.FindControl("cbSendPlan");

                        Publication.PublicationChannels.Add(new Models.PublicationChannel()
                        {
                            ChannelID = channelID,
                            PubDateMoveDays = Utils.StringToInt(tb1.Text),
                            ReleaseDelay = Utils.StringToInt(tb2.Text),
                            Trigger = ddl.SelectedIndex,
                            SendPlan = cb2.Checked
                        });
                    }
                }

                
                db.InsertUpdatePublication(Publication, out errmsg);

                //    InjectScript.Text = "<script>CloseAndRebind()</" + "script>";
                if (Publication.PublicationID > 0) // update
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", true);

            }
        }
    }
}