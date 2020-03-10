using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ViewPages : System.Web.UI.Page
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
                string prodName = "";

                db.GetProductionName(productionID, ref prodName,  out string errmsg);

             
                this.Title = prodName;
            }

        }

        protected void RadImageGallery1_NeedDataSource(object sender, ImageGalleryNeedDataSourceEventArgs e)
        {
            RadImageGallery1.DataSource = GetDataTable();
        }

        private DataTable GetDataTable()
        {
            int productionID = Utils.StringToInt(HiddenProductionID.Value);
            DataTable dtImageGallery = new DataTable();
            DataColumn newColumn;
            newColumn = dtImageGallery.Columns.Add("ID", Type.GetType("System.Int32"));
            newColumn = dtImageGallery.Columns.Add("Picture", Type.GetType("System.String"));
            newColumn = dtImageGallery.Columns.Add("Description", Type.GetType("System.String"));

         
            string errmsg = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            List<int> masterSetList = new List<int>();
            if (db.GetMasterCopySeparationSetsForProduction(productionID, ref masterSetList, out errmsg) == false)
                return dtImageGallery;

            string virtualImageFolder = "/CCPreviews";
            string realImageFolder = HttpContext.Current.Server.MapPath(virtualImageFolder);

            
            foreach (int masterSet in masterSetList)
            {
                Models.Page page = new Models.Page() { MasterCopySeparationSet = masterSet };

                db.MasterCopySeparationSetPage(ref page, out errmsg);
                if (page.ProofStatus >= 10 && page.Status >= 10)
                {
                    string fileTitle = $"{page.MasterCopySeparationSet}-{page.Version}.jpg";
                    if (System.IO.File.Exists(realImageFolder + "\\" + fileTitle))
                        page.VirtualImagePath = virtualImageFolder + "/" + fileTitle;
                    else
                    {
                        fileTitle = $"{page.MasterCopySeparationSet}.jpg";
                        if (System.IO.File.Exists(realImageFolder + "\\" + fileTitle))
                            page.VirtualImagePath = virtualImageFolder + "/" + fileTitle;
                    }
                }

                if (page.VirtualImagePath == "")
                    page.VirtualImagePath = "/Images/NoPage.png";

                DataRow newRow = dtImageGallery.NewRow();
                newRow["ID"] = page.MasterCopySeparationSet;
                newRow["Picture"] = page.VirtualImagePath;
                newRow["Description"] = page.PageName + " of " + masterSetList.Count.ToString();
                dtImageGallery.Rows.Add(newRow);


            }   
            return dtImageGallery;

        }

        
    }
}