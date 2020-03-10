using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace PDFhub
{
    public partial class ViewQueues : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            RebindQueue1Grid(true);
            RebindQueue2Grid(true);
            RebindQueue3Grid(true);
        }

        protected void RadGridQueue1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RebindQueue1Grid(false);
        }

        protected void RadGridQueue1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                RebindQueue1Grid(true);
                return;
            }
        }

        private void RebindQueue1Grid(bool callRebind)
        {

            Timer1.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            List<Models.FileLog> logItems = new List<Models.FileLog>();

            if (db.GetQueueFilesLog(0, ref logItems, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer1.Enabled = true;
                return;
            }

            RadGridQueue1.DataSource = logItems;
            if (callRebind)
                RadGridQueue1.Rebind();

            LabelLastUpdate.Text = string.Format("Updated {0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Timer1.Enabled = true;
        }

        protected void RadGridQueue2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RebindQueue2Grid(false);
        }

        protected void RadGridQueue2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                RebindQueue2Grid(true);
                return;
            }
        }

        private void RebindQueue2Grid(bool callRebind)
        {

            Timer1.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            List<Models.FileLog> logItems = new List<Models.FileLog>();

            if (db.GetConvertQueueFilesLog(ref logItems, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer1.Enabled = true;
                return;
            }

            RadGridQueue2.DataSource = logItems;
            if (callRebind)
                RadGridQueue2.Rebind();
        }

        protected void RadGridQueue3_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RebindQueue3Grid(false);
        }

        protected void RadGridQueue3_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                RebindQueue3Grid(true);
                return;
            }
        }

        private void RebindQueue3Grid(bool callRebind)
        {

            Timer1.Enabled = false;
            LabelError.Text = "";
            DataProviders.DBaccess db = new DataProviders.DBaccess();

            List<Models.FileLog> logItems = new List<Models.FileLog>();

            if (db.GetTransmitQueueFilesLog(ref logItems, out string errmsg) == false)
            {
                LabelError.Text = errmsg;
                Timer1.Enabled = true;
                return;
            }

            RadGridQueue3.DataSource = logItems;
            if (callRebind)
                RadGridQueue3.Rebind();
        }

    }
}