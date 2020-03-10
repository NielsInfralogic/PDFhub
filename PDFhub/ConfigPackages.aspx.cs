using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ConfigPackages : System.Web.UI.Page
    {
        private List<Models.Package> packages = new List<Models.Package>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void RebindPackageGrid(bool callRebind)
        {
            //inputConfigurations = new List<Models.InputConfiguration>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetPackages(ref packages, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
            }
            RadGridPackage.DataSource = packages;
            if (callRebind)
                RadGridPackage.Rebind();
        }
        protected void RadGridPackage_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            RebindPackageGrid(false);
            // set primary key?
        }

        protected void RadGridPackage_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                TextBox textBox = (e.Item as GridEditableItem)["Name"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 350;
                textBox = (e.Item as GridEditableItem)["ProductAlias"].Controls[0] as TextBox;
                if (textBox != null)
                    textBox.Width = 350;

                textBox = (e.Item as GridEditableItem)["PackageID"].Controls[0] as TextBox;
                if (textBox != null)
                {
                    textBox.ReadOnly = true; // No edit allowed..!
                    textBox.Width = 70;
                }
            }
        }

        protected void RadGridPackage_ItemCommand(object sender, GridCommandEventArgs e)
        {

            if (e.CommandName == "Delete")
            {
                string r = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageID"].ToString();
                int id = r != null ? Utils.StringToInt(r) : 0;

              //  Models.Package pg = packages.FirstOrDefault(p => p.PackageID == id);
              //  if (pg != null)
              //  {
              //      packages.Remove(pg);

                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.DeletePackage(id, out string errmsg);
             //   }
                
            }

            if (e.CommandName == "PerformInsert" || e.CommandName == "Update")
            {

                GridEditableItem item = e.Item as GridEditableItem;

                GridTextBoxColumnEditor idx = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("PackageID");

                GridTextBoxColumnEditor nm = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("Name");
                string name = nm != null ? nm.Text : "";

                GridTextBoxColumnEditor pa = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("ProductAlias");
                string prod = pa != null ? pa.Text : "";
                GridTextBoxColumnEditor si = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("SectionIndex");
                int sectionIndex = si != null ? Utils.StringToInt(si.Text) : 1;

                GridTextBoxColumnEditor co = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("Comment");
                string comment = co != null ? co.Text : "";

                Models.Package package = null;

                if (e.CommandName == "PerformInsert")
                {

                    package = new Models.Package() { PackageID=0, Name = name, ProductAlias = prod, SectionIndex = sectionIndex, Condition = 0, Comment = comment };
                    packages.Add(package);
                }
                else
                {
                    int id = idx != null ? Utils.StringToInt(idx.Text) : 0;
                    /*     package = packages.FirstOrDefault(p => p.PackageID == id);

                    if (package != null)
                    { 
                        package.Name = name;
                        package.ProductAlias = prod;
                        package.Comment = comment;
                        package.SectionIndex = sectionIndex;
                    }*/
                    package = new Models.Package() { PackageID = id, Name = name, ProductAlias = prod, SectionIndex = sectionIndex, Condition = 0, Comment = comment };

                }

                if (package != null)
                {
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.InsertUpdatePackage(package, out string errmsg);
                }
            }
        }
    }
}