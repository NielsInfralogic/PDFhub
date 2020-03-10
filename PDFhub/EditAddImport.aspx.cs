using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class EditAddImport : System.Web.UI.Page
    {
        private Models.ImportConfiguration _import = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Models.ImportConfiguration import = new Models.ImportConfiguration();
                import.ImportID = 0;

                int importIDFromMainForm = 0;
                if (Request.QueryString["ImportID"] != null)
                {
                    importIDFromMainForm = Utils.StringToInt(Request.QueryString["ImportID"]);
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.GetImportConfiguration(importIDFromMainForm, ref import, out string errmsg);

                    btnUpdate.Text = "Update";
                }

                Import = import;

                if (Import != null)
                {
                    cbEnabled.Checked = Import.Enabled;
                    txtImportName.Text = Import.Name;
                    DropDownListType.SelectedIndex = Import.ImportType-1;
                    txtInputFolder.Text = Import.InputFolder;
                    txtDoneFolder.Text = Import.DoneFolder;
                    txtErrorFolder.Text = Import.ErrorFolder;
                    txtLogFolder.Text = Import.LogFolder;
                    txtCopyFolder.Text = Import.CopyFolder;
                    cbSendMail.Checked = Import.SendErrorEmail;
                    txtEmailReceivers.Text = Import.EmailReceiver;
                }
            }
        }


        public Models.ImportConfiguration Import
        {
            get
            {
                if (this._import == null)
                {
                    if (Session["EditedImportDataItem"] != null)
                    {
                        this._import = (Models.ImportConfiguration)Session["EditedImportDataItem"];
                    }
                }
                return this._import;
            }
            set
            {
                if (value is Models.ImportConfiguration)
                {
                    if (value != null)
                    {
                        this._import = value;
                        Session["EditedImportDataItem"] = value;
                    }
                    else if (Session["EditedImportDataItem"] != null)
                    {
                        this._import = (Models.ImportConfiguration)Session["EditedImportDataItem"];
                    }
                }
            }
        }

        protected void RadGridPPI_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Models.PPITranslations> ppiTranslations = new List<Models.PPITranslations>();
            if (Import != null)
                ppiTranslations = Import.PPITranslations;
            RadGridPPI.DataSource = ppiTranslations;
        }



        protected void RadGridPPI_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
 
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                TextBox textBox = (e.Item as GridEditableItem)["PPIProduct"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 250;
                textBox = (e.Item as GridEditableItem)["PPIEdition"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 250;
                textBox = (e.Item as GridEditableItem)["Publication"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 250;

/*                textBox = (e.Item as GridEditableItem)["RuleID"].Controls[0] as TextBox;
                if (textBox != null)
                {
                    textBox.ReadOnly = true; // No edit allowed..!
                    textBox.Width = 1;
                }*/
            }
        }

        protected void RadGridPPI_ItemCommand(object sender, GridCommandEventArgs e)
        {

            List<Models.PPITranslations> ppiTranslations = Import.PPITranslations;

            if (e.CommandName == "Delete")
            {
                string id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RuleID"].ToString();
                int ruleID = id != null ? Utils.StringToInt(id) : 0;

                Models.PPITranslations tx = ppiTranslations.FirstOrDefault(p => p.RuleID == ruleID);
                if (tx != null)
                {
                    ppiTranslations.Remove(tx);
                }
            }

            if (e.CommandName == "PerformInsert" || e.CommandName == "Update")
            {

                GridEditableItem item = e.Item as GridEditableItem;

                GridTextBoxColumnEditor r = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("RuleID");

                GridTextBoxColumnEditor pr = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("PPIProduct");
                string ppiProduct = pr != null ? pr.Text : "";
                GridTextBoxColumnEditor ed = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("PPIEdition");
                string ppiEdition = ed != null ? ed.Text : "";
                GridTextBoxColumnEditor pu = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("Publication");
                string publication = pu != null ? pu.Text : "";

                if (e.CommandName == "PerformInsert")
                    ppiTranslations.Add(new Models.PPITranslations() { RuleID = ppiTranslations.Count + 1, PPIProduct = ppiProduct, PPIEdition = ppiEdition, Publication = publication });
                else
                {
                    int id = r != null ? Utils.StringToInt(r.Text) : 0;
                    Models.PPITranslations tx = ppiTranslations.FirstOrDefault(p => p.RuleID == id);


                    if (tx != null)
                    {
                        tx.PPIProduct = ppiProduct;
                        tx.PPIEdition = ppiEdition;
                        tx.Publication = publication;
                    }


                }

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Import != null)
            {
                Import.Enabled = cbEnabled.Checked.Value;
                Import.Name = txtImportName.Text;
                Import.ImportType = DropDownListType.SelectedIndex +1;
                Import.InputFolder = txtInputFolder.Text;
                Import.DoneFolder = txtDoneFolder.Text;
                Import.ErrorFolder = txtErrorFolder.Text;
                Import.LogFolder = txtLogFolder.Text;
                Import.CopyFolder = txtCopyFolder.Text;
                Import.SendErrorEmail = cbSendMail.Checked.Value ;
                Import.EmailReceiver = txtEmailReceivers.Text;

                DataProviders.DBaccess db = new DataProviders.DBaccess();
                db.InsertUpdateImportConfiguration(Import, out string errmsg);

                if (Import.ImportID > 0) // update
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind('navigateToInserted');", true);
            }


        }

    }
}