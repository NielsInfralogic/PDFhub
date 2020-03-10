using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Device.Detection;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class Default : System.Web.UI.Page
    {
        private const int MAXMESSAGELENGTH = 150;
        protected void Page_Load(object sender, EventArgs e)
        {
          //  if (IsPostBack == false)
          //      RadGridErrorList.ClientSettings.Scrolling.ScrollHeight = new Unit(GetScreenHeight() - 500);     
        }

        private int GetScreenHeight()
        {
            DeviceScreenDimensions screenDimensions = Detector.GetScreenDimensions(Request.UserAgent);
            DeviceScreenSize screenSize = Detector.GetScreenSize(Request.UserAgent);

            return screenDimensions.Height != 0 ? screenDimensions.Height : 800;
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            RebindStateGrid(true);
        }

        public void Timer2_Tick(object sender, EventArgs e)
        {
            RebindErrorGrid(true);
        }

        ///////////////////////////////////////////////////////
        /// STATEVIEW
        ///////////////////////////////////////////////////////

        protected void RadGridState_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RebindStateGrid(false);
        }

        private void RebindStateGrid(bool callRebind)
        {
            Timer1.Enabled = false;
            List<Models.Service> services = new List<Models.Service>();
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.GetServiceStates(ref services, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer1.Enabled = true;
                return;
            }
            RadGridState.DataSource = services;
            if (callRebind)
                RadGridErrorList.Rebind();

            Timer1.Enabled = true;
            LabelLastUpdate.Text = string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        }

        protected void RadGridState_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
               /* string tp = (string)e.Item.Cells[7].Text;
                if (tp == null)
                    return;
                ImageButton viewButton = (ImageButton)e.Item.FindControl("ViewButton");
                if (viewButton != null && (tp == "Database" || tp == "FileServer" || tp == "Maintenance" || tp == "Unknown"))
                        viewButton.ImageUrl = "Images/empty.png"; 
*/
            }
        }

        protected void RadGridState_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem item)
            {
                if (item != null)
                {
                    var cell = item["LastMessage"];
                    if (cell.Text.Length > MAXMESSAGELENGTH)
                    {
                        var originaltext = cell.Text;
                        cell.Text = cell.Text.Substring(0, MAXMESSAGELENGTH) + "...";
                        cell.ToolTip = originaltext;
                    }

                    cell = item["InstanceNumber"];
                    if (cell.Text == "0")
                        cell.Text = "";


                     cell = item["Type"];
                    if (cell.Text == "Database" || cell.Text == "FileServer" || cell.Text == "Maintenance" || cell.Text == "Unknown")
                    
                    {
                        var cell2 = (ImageButton)e.Item.FindControl("ViewButton");
                        if (cell2 != null)
                            cell2.ImageUrl = "Images/empty.png";
                       // cell2.Visible = false;
                       // cell2.Text = "";
                    }

                }
            }
        }

        protected void RadGridState_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item == null)
                return;

            if (e.CommandName == "RebindGrid")
            {
                RebindStateGrid(true);
                return;
            }
            if (e.CommandName == "View")
            {
                var textBox = item["Type"];
                if (textBox == null)
                    return;

                if (textBox.Text == "FileImport")
                {
                    Response.Redirect("InputLog.aspx");
                }

                if (textBox.Text == "PlanImport")
                {
                    Response.Redirect("ImportLog.aspx");
                }

                if (textBox.Text == "Processing")
                {
                    Response.Redirect("ProcessLog.aspx");
                }

                if (textBox.Text == "Export")
                {
                    Response.Redirect("ExportLog.aspx");
                }

                if (textBox.Text == "Maintenance")
                {
                    Response.Redirect("MaintLog.aspx");
                }

            }

            if (e.CommandName == "Dismiss")
            {
                var textBox = item["Name"];
                var textBox2 = item["InstanceNumber"];
                if (textBox == null || textBox2 == null)
                    return;
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.ClearServiceErrorState(textBox.Text.Trim(), Utils.StringToInt(textBox2.Text), out string errmsg) == false)
                    Utils.WriteLog(false, "db.ClearServiceErrorState() - " + errmsg);
                RebindStateGrid(true);
            }
        }


        ///////////////////////////////////////////////////////
        /// ERRORVIEW
        ///////////////////////////////////////////////////////

        protected void RadGridErrorList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RebindErrorGrid(false);
        }

        private void RebindErrorGrid(bool callRebind)
        {
            Timer2.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            List<Models.LogItem> logItems = new List<Models.LogItem>();



            if (db.GetLastLogItems(ref logItems, 100, true,  Models.LogItemSource.All,out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer2.Enabled = true;
                return;
            }

            RadGridErrorList.DataSource = logItems;
            if (callRebind)
                RadGridErrorList.Rebind();

            Timer2.Enabled = true;

            LabelLastUpdate.Text = string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        protected void RadGridErrorList_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem item)
            {
                if (item != null)
                {

                    // Change cell background to reflect state
                    TableCell cellState = item["Status"];
                    if (cellState != null)
                    {
                        int n = Utils.StringToInt(cellState.Text.Trim());
                        string s = cellState.Text.Trim();
                        if ((n % 10) == 0)
                        {
                            cellState.BackColor = System.Drawing.Color.LightGreen;
                            cellState.Text = "Success";
                        }
                        else if (Constants.ErrorEvents.Contains(n))
                        {
                            cellState.BackColor = System.Drawing.Color.Red;
                            cellState.ForeColor = System.Drawing.Color.White;
                            cellState.Text = "Error";
                        }
                        else if (Constants.WarningEvents.Contains(n))
                        {
                            cellState.BackColor = System.Drawing.Color.LightYellow;
                            cellState.Text = "Warning";
                        }
                        else // 0
                        {
                            cellState.Text = "";
                        }
                    }
                }
            }
        }

        protected void RadToolBar2_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (e.Item.Value == "RebindGrid")
                RebindErrorGrid(true);
            if (e.Item.Value == "SaveToExcel")
            {
                RadGridErrorList.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
                RadGridErrorList.ExportSettings.IgnorePaging = true;
                RadGridErrorList.ExportSettings.ExportOnlyData = true;
                RadGridErrorList.ExportSettings.OpenInNewWindow = true;
            }
        }

        protected void RadGridErrorList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            
            if (e.CommandName == "RebindGrid")
            {
                RebindErrorGrid(true);
                return;
            }

            if (e.CommandName == "SaveToExcel")
            {
                Timer2.Enabled = false;
                RadGridErrorList.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
                RadGridErrorList.ExportSettings.IgnorePaging = true;
                RadGridErrorList.ExportSettings.ExportOnlyData = true;
                RadGridErrorList.ExportSettings.OpenInNewWindow = true;
                RadGridErrorList.MasterTableView.ExportToExcel();
                Timer2.Enabled = true;
            }

            if (e.CommandName == "RetryErrorFiles")
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.RetryFile(Models.ServiceType.FileImport, "", "", out string errmsg) == false)
                    Utils.WriteLog(false, "db.RetryFile() - " + errmsg);

            }

            if (e.CommandName == "Retry")
            {
                if (!(e.Item is GridDataItem item))
                    return;
                var textBox = item["FileName"];
                if (textBox == null)
                    return;
                string fileName = textBox.Text;
                textBox = item["Message"];
                if (textBox == null)
                    return;

                string inputQueue = textBox.Text;
                textBox = item["Service"];
                if (textBox == null)
                    return;
                string service = textBox.Text.ToLower();

                int m = inputQueue.IndexOf("(");
                int n = inputQueue.IndexOf(")");
                if (m != -1 && n != -1)
                    inputQueue = inputQueue.Substring(m + 1, n - m - 1);

                if (fileName != "" && inputQueue != "" && service.IndexOf("input") != -1)
                {
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    
                    if (db.RetryFile(Models.ServiceType.FileImport, fileName, inputQueue, out string errmsg) == false)
                        Utils.WriteLog(false, "db.RetryFile() - " + errmsg);
                }
            }
        }

       
    }
}