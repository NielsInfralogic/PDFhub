using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Spreadsheet;

namespace PDFhub
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // populate pubdate filter
                
                RadDropDownListPublisher.Items.Clear();
                List<string> publishers = Utils.GetPublisherNames();
                foreach (string s in publishers)
                    RadDropDownListPublisher.Items.Add(s);
                if (RadDropDownListPublisher.Items.Count > 0)
                    RadDropDownListPublisher.SelectedIndex = 0;

                PopulateProductFilter();
                PopulateExportFilter();
                PopulatePubDateFilter();
            }

        }

        private void PopulatePubDateFilter()
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            List<DateTime> pubDates = new List<DateTime>();
            db.GetPubDates(Utils.GetPublisherID(RadDropDownListPublisher.SelectedText),
                          RadDropDownListProduct.SelectedText,
                         Utils.GetChannelID(RadDropDownListExport.SelectedText),
                ref pubDates, out string errmsg);
            
            RadDropDownListPubDate.Items.Clear();
            foreach (DateTime dt in pubDates)
                RadDropDownListPubDate.Items.Add(string.Format("{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day));

            if (RadDropDownListPubDate.Items.Count > 0)
                RadDropDownListPubDate.SelectedIndex = 0;
        }

        private void PopulateProductFilter()
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            List<Models.Publication> publications = new List<Models.Publication>();
            db.GetPublications(ref publications, 0, Utils.GetPublisherID(RadDropDownListPublisher.SelectedText), out string errmsg);

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

        private Worksheet FillWorksheet(DataTable data)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Name = "Channel delivery times";
            
            sheet.Columns = new List<Column>();
            var row = new Row() { Index = 0 };
            int columnIndex = 0;
            int[] colWidths = { 90,200,200,80,80,150,150,150,150,150};
            string[] colAlign = { "right", "left", "left", "right", "right", "right", "right", "right", "right" };
            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column() { Width = columnIndex<colWidths.Length ? colWidths[columnIndex] : 80 } );
                string cellValue = dataColumn.ColumnName.Replace("_", " ");
                var cell = new Cell() { Index = columnIndex, Value = cellValue, Bold = true};
                cell.Background = "#CCEE00";
             
                row.AddCell(cell);
                columnIndex++;
            }
            sheet.AddRow(row);
            int rowIndex = 1;
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row() { Index = rowIndex++ };

                columnIndex = 0;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    var cell = new Cell()
                    {
                        TextAlign = columnIndex < colAlign.Length ? colAlign[columnIndex] : "left",
                        Index = columnIndex++  
                    };
                    Type tp = dataRow[dataColumn.ColumnName].GetType();


                    if (tp.Name == "Int32")
                    {
                        cell.Value = (int)dataRow[dataColumn.ColumnName];
                    }
                    else if (tp.Name == "DateTime")
                    {
                        cell.Value = (DateTime)dataRow[dataColumn.ColumnName];
                        cell.Format = "DateTime";
                 
                    }
                    else
                        cell.Value = dataRow[dataColumn.ColumnName].ToString();

                    row.AddCell(cell);

                }

                sheet.AddRow(row);
            }

            return sheet;

        }

        public DataTable GetReportingData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PubDate", typeof(string));
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Export", typeof(string));
            dt.Columns.Add("Pages_In", typeof(Int32));
            dt.Columns.Add("Pages_Sent", typeof(Int32));
            dt.Columns.Add("ReleaseTime", typeof(string));
            dt.Columns.Add("Last_Page_In", typeof(string));
            dt.Columns.Add("Last_Page_Sent", typeof(string));

            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            DateTime pubDate = Utils.DateStringToDateTime(RadDropDownListPubDate.SelectedText);
            List<Models.ReportItem> reportData = new List<Models.ReportItem>();
            if (db.GetReportData(pubDate, Utils.GetPublisherID(RadDropDownListPublisher.SelectedText), 
                RadDropDownListProduct.SelectedText, Utils.GetChannelID(RadDropDownListExport.SelectedText),
                ref reportData, out string errmsg) == false)
            {
                LabelError.Text = "db.GetReportData() - " + errmsg;
                return null;
            }

            foreach (Models.ReportItem reportItem in reportData)
            {
                dt.Rows.Add(Utils.Date2String(reportItem.PubDate), 
                            reportItem.Publication, 
                            reportItem.Channel, 
                            reportItem.Pages,
                            reportItem.PagesSent, 
                            Utils.Time2String(reportItem.ReleaseTime),
                            Utils.Time2String(reportItem.LastPageIn),
                            Utils.Time2String(reportItem.LastPageSent));
            }

            return dt;
        }

        public DataTable GetReportingDataPageHistory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PubDate", typeof(string));
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Export", typeof(string));
            dt.Columns.Add("ReleaseTime", typeof(string));
 
            dt.Columns.Add("Page", typeof(string));
            dt.Columns.Add("Version", typeof(string));
            dt.Columns.Add("Page_In", typeof(string));
            dt.Columns.Add("Page_Sent", typeof(string));
           
           

            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            DateTime pubDate = Utils.DateStringToDateTime(RadDropDownListPubDate.SelectedText);
            List<Models.ReportPageHistoryItem> reportData = new List<Models.ReportPageHistoryItem>();
            if (db.GetReportDataPageHistory(pubDate, Utils.GetPublisherID(RadDropDownListPublisher.SelectedText),
                RadDropDownListProduct.SelectedText, Utils.GetChannelID(RadDropDownListExport.SelectedText),
                ref reportData, out string errmsg) == false)
            {
                LabelError.Text = "db.GetReportData() - " + errmsg;
                return null;
            }

            foreach (Models.ReportPageHistoryItem reportItem in reportData)
            {
                dt.Rows.Add(Utils.Date2String(reportItem.PubDate),
                            reportItem.Publication,
                            reportItem.Channel,
                            Utils.Time2String(reportItem.ReleaseTime),
                            reportItem.PageName,
                            reportItem.Version.ToString(),
                            Utils.Time2String(reportItem.PageIn),
                            Utils.Time2String(reportItem.PageSent));
            }

            return dt;
        }

        protected void RadDropDownListPublisher_ItemSelected(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            PopulatePubDateFilter();
            PopulateProductFilter();
            PopulateExportFilter();
        }

        protected void RadButtonGenerate_Click(object sender, EventArgs e)
        {
            DataTable data;
            if (cbShowPageHistory.Checked == false)
                data = GetReportingData();
            else
                data = GetReportingDataPageHistory();

            if (data == null)
                return;
            var sheet1 = FillWorksheet(data);
            RadSpreadsheet1.Sheets.Add(sheet1);
            RadButtonSave.Enabled = true;
        }


    }
}