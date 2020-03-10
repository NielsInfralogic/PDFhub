using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class CheckPages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int productionID = 0;
                if (Request.QueryString["ProductionID"] != null)
                {
                    productionID = Utils.StringToInt(Request.QueryString["ProductionID"]);
                    if (productionID > 0)
                        HiddenProductionID.Value = productionID.ToString();
                }

                DataProviders.DBaccess db = new DataProviders.DBaccess();

                List<Models.Page> pages = new List<Models.Page>();

                string prodName = "";
                db.GetProductionName(productionID, ref prodName, out string errmsg);

                db.GetPagesForProduction(productionID, ref pages, out  errmsg);

                lblProduct.Text = $"Check {pages.Count} pages for {prodName}";

             
                foreach (Models.Page page in pages)
                {
                    page.VirtualImagePath = "Application.png";
                    page.PageName = string.Format("{0}-{1} ({2})", page.Section, page.PageName,page.FileName);
                    if (page.Status < 10)
                    {
                        page.VirtualImagePath = "Warning.png";
                        page.PageName += " (missing)";
                    }

                }
                RadListBoxPages.DataSource = pages;
                RadListBoxPages.DataTextField = "PageName";
                RadListBoxPages.DataValueField = "VirtualImagePath";
                RadListBoxPages.DataBind();


            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            List<Models.Page> pages = new List<Models.Page>();
            int productionID = Utils.StringToInt(HiddenProductionID.Value);

            db.GetPagesForProduction(productionID, ref pages, out string errmsg);

            RadListBoxPages.DataSource = null;

            foreach (Models.Page page in pages)
            {

                string hiresPath = "";
                db.GetHiresPath(page.MasterCopySeparationSet, ref hiresPath, out errmsg);

                page.VirtualImagePath  = "Application.png";
                page.PageName = string.Format("{0}-{1} ({2})", page.Section, page.PageName, page.FileName);
                if (page.Status < 10 || hiresPath == "" || File.Exists(hiresPath) == false)
                {
                    page.VirtualImagePath = "Warning.png";
                    page.PageName += " (missing)";
                }
                else
                {
                    page.VirtualImagePath = "Danger.png";
                    if (Utils.PdfReadTest(hiresPath) == true)
                    {
                        page.VirtualImagePath = "OK.png";
                    } 
                    else
                    {
                        page.VirtualImagePath = "Danger.png";
                        page.PageName += " (read error!)";

                    }
                    

                }

                RadListBoxPages.DataSource = pages;
                RadListBoxPages.DataTextField = "PageName";
                RadListBoxPages.DataValueField = "VirtualImagePath";
                RadListBoxPages.DataBind();


            }

        }
        }
}