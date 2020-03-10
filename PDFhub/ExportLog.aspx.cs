using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Device.Detection;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ExportLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

               /* Telerik.Web.UI.RadToolBarItem item = RadToolBar1.FindItemByValue("Item1");
                if (item == null)
                    return;
                DropDownList DropDownList1 = (DropDownList)item.FindControl("ChannelSelector");
                if (DropDownList1 == null)
                    return;
                List<string> channels = Utils.GetChannelNames();
                DropDownList1.Items.Add("All");
                foreach (string s in channels)
                    DropDownList1.Items.Add(s);*/
            }
            //  RadGridLog.ClientSettings.Scrolling.ScrollHeight = new Unit(GetScreenHeight() - 300);
        }

        private int GetScreenHeight()
        {
            DeviceScreenDimensions screenDimensions = Detector.GetScreenDimensions(Request.UserAgent);
            DeviceScreenSize screenSize = Detector.GetScreenSize(Request.UserAgent);

            return screenDimensions.Height != 0 ? screenDimensions.Height : 800;
        }

        protected void RadGridLog_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RebindLogGrid(false);
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            RebindLogGrid(true);
        }

        private void RebindLogGrid(bool callRebind)
        {

            Timer1.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            List<Models.LogItem> logItems = new List<Models.LogItem>();

            if (db.GetLastLogItems(ref logItems, Utils.ReadConfigInt32("MaxLogItemsToShow", 500), false, Models.LogItemSource.Export, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer1.Enabled = true;
                return;
            }

            RadGridLog.DataSource = logItems;
            if (callRebind)
                RadGridLog.Rebind();

            LabelLastUpdate.Text = string.Format("Updated {0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Timer1.Enabled = true;
        }

        protected void RadGridLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem item)
            {
                if (item != null)
                {
                    var cell = item["Message"];
                    if (cell.Text.Length > 60)
                    {
                        var originaltext = cell.Text;
                        cell.Text = cell.Text.Remove(60) + "...";
                        cell.ToolTip = originaltext;
                    }

                    cell = item["Status"];
                    var cell2 = item["StatusName"];
                    int n = Utils.StringToInt(cell.Text);

                    if ((n % 10) == 0)
                    {
                        cell2.BackColor = System.Drawing.Color.LightGreen;
                        cell2.Text = "Success";
                    }
                    else if ((n % 6) == 0)
                    {
                        cell2.BackColor = System.Drawing.Color.Red;
                        cell2.ForeColor = System.Drawing.Color.White;
                        cell2.Text = "Error";
                    }
                    else if ((n % 7) == 0)
                    {
                        cell2.BackColor = System.Drawing.Color.LightYellow;
                        cell2.Text = "Warning";
                    }

                }
            }
        }

        protected void RadGridLog_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.FilterCommandName)
            {

            }

            if (e.CommandName == "RebindGrid")
            {
                RebindLogGrid(true);
                return;
            }

            if (e.CommandName == "SaveToExcel")
            {
                Timer1.Enabled = false;
                RadGridLog.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
                RadGridLog.ExportSettings.IgnorePaging = true;
                RadGridLog.ExportSettings.ExportOnlyData = true;
                RadGridLog.ExportSettings.OpenInNewWindow = true;
                RadGridLog.MasterTableView.ExportToExcel();
                Timer1.Enabled = true;
            }

            if (e.CommandName == "GetLog")
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                string logFile1 = "";
                string logFile2 = "";
                string logFile3 = "";

                db.GetLogFiles(Models.ServiceType.Export, 1, ref logFile1, ref logFile2, ref logFile3, out string errmsg);
                if (logFile1 != "")
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFile1);
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(fileInfo.FullName);
                    Response.End();
                }


            }

            if (e.CommandName == "Retry")
            {
                if (!(e.Item is GridDataItem item))
                    return;
                var textBox = item["FileName"];
                if (textBox == null)
                    return;
                string fileName = textBox.Text;
                textBox = item["Source"];
                if (textBox == null)
                    return;
                string inputQueue = textBox.Text;

                if (fileName != "" && inputQueue != "")
                {
                    DataProviders.DBaccess db = new DataProviders.DBaccess();
                    db.RetryFile(Models.ServiceType.Processing, fileName, inputQueue, out string errmsg);

                }
            }
        }
    

        

        protected void RadGridLog_PreRender(object sender, EventArgs e)
        {
            if (RadGridLog.MasterTableView.FilterExpression != string.Empty)
            {
                //
            }
        }

    }
}