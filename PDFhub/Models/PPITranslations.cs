using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class PPITranslations
    {
        public int RuleID { get; set; } = 0;
        public string PPIProduct { get; set; } = "";
        public string PPIEdition { get; set; } = "";
        public string Publication { get; set; } = "";

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;
    }
}