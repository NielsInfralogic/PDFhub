using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class ReportPageHistoryItem
    {
        public int ProductionID { get; set; } = 0;
        public DateTime PubDate { get; set; } = DateTime.MinValue;
        public string Publication { get; set; } = "";

        public string Channel { get; set; } = "";

        public DateTime ReleaseTime { get; set; } = DateTime.MinValue;

        public int _ChannelID { get; set; } = 0;

        public int _EditionID { get; set; } = 0;

        public string PageName { get; set; } = "";


        public int Version { get; set; } = 1;
        public DateTime PageIn { get; set; } = DateTime.MinValue;
        public DateTime PageSent { get; set; } = DateTime.MinValue;


    }

    public class ReportPageHistoryItemPage
    {
       

        
    }
}