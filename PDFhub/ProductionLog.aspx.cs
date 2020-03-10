using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Device.Detection;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ProductionLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadDatePickerStartDate.SelectedDate = DateTime.Today;
                RadDatePickerEndDate.SelectedDate = DateTime.Today.AddDays(2);
                PopulateProductFilter();
                PopulateExportFilter();
            }
            Timer1.Enabled = false; // Turn off completely...
        }

        private void PopulateProductFilter()
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            List<Models.Publication> publications = new List<Models.Publication>();
            db.GetPublications(ref publications, 0, 0, out string errmsg);

            RadDropDownListProduct.Items.Clear();
            RadDropDownListProduct.Items.Add("All");
            foreach (Models.Publication publication in publications)
                RadDropDownListProduct.Items.Add(publication.Name);

            if (RadDropDownListProduct.Items.Count > 0)
                RadDropDownListProduct.SelectedIndex = 0;
        }

        private void PopulateExportFilter()
        {
            RadDropDownListExport.Items.Clear();
            RadDropDownListExport.Items.Add("All");
            List<string> exports = Utils.GetChannelNames();
            foreach (string s in exports)
                RadDropDownListExport.Items.Add(s);
            if (RadDropDownListExport.Items.Count > 0)
                RadDropDownListExport.SelectedIndex = 0;
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGridLog.MasterTableView.SortExpressions.Clear();
                RadGridLog.MasterTableView.GroupByExpressions.Clear();
                RadGridLog.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridLog.MasterTableView.SortExpressions.Clear();
                RadGridLog.MasterTableView.GroupByExpressions.Clear();
                RadGridLog.MasterTableView.CurrentPageIndex = RadGridLog.MasterTableView.PageCount - 1;
                RadGridLog.Rebind();
            }
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

            //Timer1.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            DateTime fromDate = RadDatePickerStartDate.SelectedDate.Value;
            DateTime toDate =  RadDatePickerEndDate.SelectedDate.Value;

            if (toDate < fromDate)
                toDate = fromDate;

            List<Models.Production> productions = new List<Models.Production>();

            if (db.GetProductions(ref productions, fromDate, toDate, 0, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
               // Timer1.Enabled = true;
                return;
            }
 
            foreach (Models.Production production in productions)
            {
                List<Models.ChannelProgress> channelProgressList = new List<Models.ChannelProgress>();
                db.GetChannelDetailsForProduction(production.ProductionID, ref channelProgressList, out errmsg);

                production.Channels.Clear();
                foreach (Models.ChannelProgress cp in channelProgressList)
                {
                    int n = cp.PagesWithError > 0 ? -1 : cp.PagesSent;
                    production.Channels.Add($"{n}:{cp.Pages}:{cp.Alias}");
                }
                
            }

            if (RadDropDownListProduct.SelectedText != "All" ) 
            {
                productions = productions.FindAll(p => p.Publication == RadDropDownListProduct.SelectedText).ToList();
            }

            if (RadDropDownListExport.SelectedText != "All")
            {
                productions = productions.FindAll(p => p.ChannelList.Contains(RadDropDownListExport.SelectedText)).ToList();
            }

            RadGridLog.DataSource = productions;
            if (callRebind)
                RadGridLog.Rebind();

            LabelLastUpdate.Text = string.Format("Updated {0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
         //   Timer1.Enabled = true;
        }

        protected void RadGridLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem item)
            {
                if (item != null)
                {
                    if (e.Item.ItemIndexHierarchical.IndexOf(":") == -1)
                    {
                        // master view

                        var cell = item["PageStr"];
                        var cell2 = item["PagesReceivedStr"];


                        if (cell != null && cell2 != null)
                        {
                            if (cell.Text.Trim() == cell2.Text.Trim())
                            {
                                cell2.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }
                        cell = item["ReleaseTimeStr"];
                        cell2 = item["Released"];
                        if (cell != null && cell2 != null)
                        {
                            if (cell2.Text == "True")
                            {
                                cell.BackColor = System.Drawing.Color.LightBlue;
                                cell.Text = "(" + cell.Text + ")";
                            }
                        }
                        
                        cell = item["ChannelList"];
                        if (cell != null)
                        {
                            string sfinal = "";
                            string[] sarray = cell.Text.Split(',');
                            foreach (string s in sarray)
                            {
                                string[] sarray2 = s.Split(':');
                                if (sarray2.Length != 3)
                                    continue;
                                int n1 = Utils.StringToInt(sarray2[0]);
                                int n2 = Utils.StringToInt(sarray2[1]);
                                if (n1 == -1) // indicates error
                                    sfinal += "<span style='background-color:red;color:white;'>&nbsp;" + sarray2[2] + "&nbsp;</span>&nbsp;&nbsp;&nbsp;";
                                else if (n1 >= n2)
                                    sfinal += "<span style='background-color:green;color:white;'>&nbsp;" + sarray2[2] + "&nbsp;</span>&nbsp;&nbsp;&nbsp;";
                                else if (n1 > 0)
                                    sfinal += "<span style='background-color:yellow;'>&nbsp;" + sarray2[2] + "&nbsp;</span>&nbsp;&nbsp;&nbsp;";
                                else
                                    sfinal += "<span style='background-color:lightgray;'>&nbsp;" + sarray2[2] + "&nbsp;</span>&nbsp;&nbsp;&nbsp;";

                            }

                            cell.Text = sfinal;
                        }

                        
                        
                    }
                    else
                    {
                        // detail view

                    

                        TableCell cl = item.Cells[3];

                        var cell = item.Cells[3];// item["Pages"];
                        var cell2 = item.Cells[4];//item["PagesSent"];
                        if (cell != null && cell2 != null)
                        {
                            if (cell.Text.Trim() == cell2.Text.Trim())
                            {
                                cell2.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }

                        cell = item.Cells[7]; // item["PageList"]
                        if (cell != null)
                        {
                            string sfinal = "";
                            string[] sarray = cell.Text.Split(',');
                            foreach (string s in sarray)
                            {
                                if (s.Contains("!"))
                                    sfinal += " <span style='background-color:red;color:white;'>&nbsp;" + s.Replace("!","") + "&nbsp;</span>&nbsp;";
                                else if (s.Contains("*"))
                                    sfinal += " <span style='background-color:lightgray;'>&nbsp;" + s.Replace("*", "") + "&nbsp;</span>&nbsp;";
                                else
                                    sfinal += " <span style='background-color:green;color:white;'>&nbsp;" + s + "&nbsp;</span>&nbsp;";


                            }

                            cell.Text = sfinal;
                        }

                        cell = item.Cells[8]; // item["MergedPDF"]
                        if (cell != null)
                        {
                            if (cell.Text == "1")
                                cell2.ForeColor = System.Drawing.Color.Navy;
                        }


                    }
                }
            }
        }

        protected void RadGridLog_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (e.DetailTableView.DataMember == "Exports")
            { 
                string productionID = dataItem.GetDataKeyValue("ProductionID").ToString();
                if (productionID == null)
                    return;
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.ChannelProgress> channelProgressList = new List<Models.ChannelProgress>();
                db.GetChannelDetailsForProduction(Utils.StringToInt(productionID), ref channelProgressList, out string errmsg);
                e.DetailTableView.DataSource = channelProgressList;
            }
        }

        protected void RadGridLog_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                ImageButton editButton = (ImageButton)e.Item.FindControl("EditButton");
                if (editButton != null)
                    editButton.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductionID"], e.Item.ItemIndex);

                ImageButton resendButton = (ImageButton)e.Item.FindControl("ResendButton");
                if (resendButton != null)
                    resendButton.Attributes["onclick"] = String.Format("return ShowResendForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductionID"], e.Item.ItemIndex);

                ImageButton viewButton = (ImageButton)e.Item.FindControl("ViewButton");
                if (viewButton != null)
                    viewButton.Attributes["onclick"] = String.Format("return ShowViewForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductionID"], e.Item.ItemIndex);

                ImageButton checkButton = (ImageButton)e.Item.FindControl("CheckButton");
                if (checkButton != null)
                    checkButton.Attributes["onclick"] = String.Format("return ShowCheckForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductionID"], e.Item.ItemIndex);

                ImageButton releaseButton = (ImageButton)e.Item.FindControl("ReleaseButton");
                if (releaseButton != null)
                    releaseButton.Attributes["onclick"] = String.Format("return ShowReleaseForm('{0}','{1}');", e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProductionID"], e.Item.ItemIndex);

            }

        }
        protected void RadGridLog_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                RebindLogGrid(true);
                return;
            }

            if (e.CommandName == "SaveToExcel")
            {
               // Timer1.Enabled = false;
                RadGridLog.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
                RadGridLog.ExportSettings.IgnorePaging = true;
                RadGridLog.ExportSettings.ExportOnlyData = true;
                RadGridLog.ExportSettings.OpenInNewWindow = true;
                RadGridLog.MasterTableView.ExportToExcel();
              //  Timer1.Enabled = true;
            }

/*            if (e.CommandName == "Change")
            {
                if (!(e.Item is GridDataItem item))
                    return;
                var textBox = item["ChannelList"];
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
                    db.RetryFile(Models.ServiceType.FileImport, fileName, inputQueue, out string errmsg);

                }
            }*/
        }
    }
}