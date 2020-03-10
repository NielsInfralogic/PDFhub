using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class ReportItem
    {
        public int ProductionID { get; set; } = 0;
        public DateTime PubDate { get; set; } = DateTime.MinValue;
        public string Publication { get; set; } = "";

        public string Channel { get; set; } = "";
        public int Pages { get; set; } = 0;
        public int PagesSent { get; set; } = 0;
        public DateTime ReleaseTime { get; set; } = DateTime.MinValue;
        public DateTime LastPageIn { get; set; } = DateTime.MinValue;
        public DateTime LastPageSent { get; set; } = DateTime.MinValue;

        public int _ChannelID { get; set; } = 0;

        public int _EditionID { get; set; } = 0;
    }
}