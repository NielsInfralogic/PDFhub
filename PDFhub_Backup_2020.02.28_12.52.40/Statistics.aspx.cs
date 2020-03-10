using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class Statistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LineChart.DataSource = GetStatData(Utils.StringToInt(Interval.SelectedValue));
            LineChart.DataBind();

            GetPageStats();
        }

        private void GetPageStats()
        {
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            DateTime lastPageIn = DateTime.MinValue;
            DateTime lastPageOut = DateTime.MinValue;
            int pageCount = 0;
            db.GetLastPageStat(ref lastPageIn, ref lastPageOut, ref pageCount, out string errmsg);
            if (lastPageIn != DateTime.MinValue)
                lblLastPageInTime.Text = Utils.Time2String(lastPageIn);
            if (lastPageOut != DateTime.MinValue)
                lblLastPageOutTime.Text = Utils.Time2String(lastPageOut);
            if (pageCount > 0)
                lblTotalPages.Text = pageCount.ToString();
        }

        private DataSet GetStatData(int hours)
        {
            LabelError.Text = "";
            DataSet ds = new DataSet("Stat");
            DataTable dt = new DataTable("Input");
            ds.Tables.Add(dt);

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            List<DateTime> samplesInput = new List<DateTime>();
            List<DateTime> samplesProcess = new List<DateTime>();
            List<DateTime> samplesExport = new List<DateTime>();
            if (db.GetLogEvents(Models.SeriesType.Input, hours, ref samplesInput, out string errmsg) == false)
            {
                LabelError.Text = "GetLogEvents(1) - " + errmsg;
                return ds;
            }
            if (db.GetLogEvents(Models.SeriesType.Processing, hours, ref samplesProcess, out errmsg) == false)
            {
                LabelError.Text = "GetLogEvents(2) - " + errmsg;
                return ds;
            }
            if (db.GetLogEvents(Models.SeriesType.Export, hours, ref samplesExport, out errmsg) == false)
            {
                LabelError.Text = "GetLogEvents(3) - " + errmsg;
                return ds;
            }

            dt.Columns.Add("Time", Type.GetType("System.Double"));
            dt.Columns.Add("ValueInput", Type.GetType("System.Int32"));
            dt.Columns.Add("ValueProcess", Type.GetType("System.Int32"));
            dt.Columns.Add("ValueExport", Type.GetType("System.Int32"));

            // Divide timeline in 24 chunks
            DateTime endTime = DateTime.Now;
            DateTime startTime = endTime.AddMinutes(-1.0 * hours * 60.0);
            double minutesPerInterval = hours * 60.0 / 24.0;
            int maxY = 0;
            for (int i = 0; i < 24; i++)
            {
                int i1 = samplesInput.Count(p => p >= startTime && p <= startTime.AddMinutes(minutesPerInterval));
                int i2 = samplesProcess.Count(p => p >= startTime && p <= startTime.AddMinutes(minutesPerInterval));
                int i3 = samplesExport.Count(p => p >= startTime && p <= startTime.AddMinutes(minutesPerInterval));
                if (i1 > maxY)
                    maxY = i1;
                if (i2 > maxY)
                    maxY = i2;
                if (i3 > maxY)
                    maxY = i3;
                dt.Rows.Add(-1.0 * hours * (24.0 - (double)i) / 24.0, i1, i2, i3);
                startTime = startTime.AddMinutes(minutesPerInterval);
            }
            LineChart.PlotArea.YAxis.MaxValue = maxY;
            LineChart.PlotArea.YAxis.Step = maxY / 10;
            return ds;
        }
    }
}