using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class EditAddUser : System.Web.UI.Page
    {
        private Models.User _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblError.Text = "";
                Models.User user = new Models.User() { UserName = "" };

                string userNameFromMainForm = "";
                HiddenUserName.Value = "";
                if (Request.QueryString["UserName"] != null)
                {
                    userNameFromMainForm = Request.QueryString["UserName"];
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.GetUser(userNameFromMainForm, ref user, out string errmsg);

                    btnUpdate.Text = "Update";

                    HiddenUserName.Value = userNameFromMainForm;
                }
                User = user;



                List<Models.Publisher> publishers = Utils.GetPublishers();
                List<Models.NameId> list = new List<Models.NameId>();

                foreach (Models.Publisher publisher in publishers)
                {
                    bool selected = User != null ? User.publisherIDList.Contains(publisher.PublisherID) : false;

                    list.Add(new Models.NameId() { Id = publisher.PublisherID, Name = publisher.PublisherName, Selected = selected, Enabled = true });
                }

                RadCheckBoxListPublishers.DataSource = list;
                RadCheckBoxListPublishers.DataBind();

                List<Models.NameId> listPub = new List<Models.NameId>();

                List<Models.PublicationShort> publicationShortList = Utils.GetPublicationsShort();

                foreach (Models.PublicationShort pub in publicationShortList)
                {
                    bool selected = User != null ? User.publicationIDList.Contains(pub.PublicationID) : false;

                    listPub.Add(new Models.NameId() { Id = pub.PublicationID, Name = pub.Name, Selected = selected, Enabled = true });
                }

                RadCheckBoxListPublications.DataSource = listPub;
                RadCheckBoxListPublications.DataBind();

                RadDropDownUserGroup.Items.Clear();
                List<string> userGroupNames = Utils.GetUserGroupNames();
                foreach (string s in userGroupNames)
                {
                    RadDropDownUserGroup.Items.Add(new DropDownListItem(s, s));
                }
                RadDropDownUserGroup.SelectedIndex = 0;

                if (User != null)
                {
                    // Load controls..
                    txtUserName.Text = User.UserName;
                    cbAccountEnabled.Checked = User.AccountEnabled;
                    txtPassword.Text = User.Password;
                    txtFullName.Text = User.FullName;
                    txtEmail.Text = User.Email;

                    RadDropDownUserGroup.SelectedText = Utils.GetUserGroupName(User.UserGroupID);
                 

                }
            }
        }

        public Models.User User
        {
            get
            {
                if (this._user == null)
                {
                    if (Session["EditedUserDataItem"] != null)
                    {
                        this._user = (Models.User)Session["EditedUserDataItem"];
                    }
                }
                return this._user;
            }
            set
            {
                if (value is Models.User)
                {
                    if (value != null)
                    {
                        this._user = value;
                        Session["EditedUserDataItem"] = value;
                    }
                    else if (Session["EditedUserDataItem"] != null)
                    {
                        this._user = (Models.User)Session["EditedUserDataItem"];
                    }
                }
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string errmsg = "";
            lblError.Text = "";
            if (User != null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();

                User.UserName = txtUserName.Text;
                User.AccountEnabled = cbAccountEnabled.Checked.Value;
                User.Password = txtPassword.Text;
                User.FullName = txtFullName.Text;
                User.Email = txtEmail.Text;
                User.UserGroupID = Utils.GetUserGroupID(RadDropDownUserGroup.SelectedValue);


                User.publisherIDList.Clear();
                foreach (Telerik.Web.UI.ButtonListItem item in RadCheckBoxListPublishers.SelectedItems)
                {
                    User.publisherIDList.Add(Utils.StringToInt(item.Value));
                }

                User.publicationIDList.Clear();
                foreach (Telerik.Web.UI.ButtonListItem item in RadCheckBoxListPublications.SelectedItems)
                {
                    User.publicationIDList.Add(Utils.StringToInt(item.Value));
                }




                db.InsertUpdateUser(User, out errmsg);

                //    InjectScript.Text = "<script>CloseAndRebind()</" + "script>";
                if (HiddenUserName.Value != "") // update
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", true);

            }

        }
    }
}