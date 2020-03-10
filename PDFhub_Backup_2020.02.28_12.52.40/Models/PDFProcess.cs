using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum PDFProcessType { ToLowResPDF= 0, ToCMYKPDF = 1, None = 2};
    public class PDFProcess
    {
        public int ProcessID { get; set; } = 0;
        public string ProcessName { get; set; } = "";
        public PDFProcessType ProcessType { get; set; } = PDFProcessType.ToLowResPDF;
        public string ConvertProfile { get; set; } = "";
        public bool ExternalProcess { get; set; } = false;
        public string ExternalInputFolder { get; set; } = "";
        public string ExternalOutputFolder { get; set; } = "";
        public string ExternalErrorFolder { get; set; } = "";

        public int ProcessTimeOut { get; set; } = 180;
    }
}