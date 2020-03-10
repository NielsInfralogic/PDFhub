using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum LogItemType { JobLogItem = 0, ErrorLogItem = 1, WarningLogItem = 2 };

    public enum LogItemSource { All=0, Import=1,Input=2,Processing=3,Export=4,Maintenance=7};



    public class LogItem
    {
        public LogItemType Type { get; set; } = LogItemType.JobLogItem;

        public int Status { get; set; } = 0;
        public DateTime Time {get;set; } = DateTime.MinValue;
        public string Service { get; set; } = "";
        public string Message { get; set; } = "";

        public string FileName { get; set; } = "";

        public string Source { get; set; } = "";
        public string StatusName { get; set; } = "";

    }

    public class FileLog
    {
        public string Queue { get; set; } = "";
        public string FileName { get; set; } = "";

    }
}