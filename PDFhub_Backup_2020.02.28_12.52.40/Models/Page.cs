using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class Page
    {
        public int MasterCopySeparationSet { get; set; } = 0;
        public int  Status { get; set; } = 0;
        public int ProofStatus { get; set; } = 0;
        public string PageName { get; set; } = "1";
        public string Section { get; set; } = "1";

        public int Version { get; set; } = 0;

        public string VirtualImagePath { get; set; } = "";

        public int _EditionID { get; set; } = 0;


        public string FileName { get; set; } = "";
    }
    
}