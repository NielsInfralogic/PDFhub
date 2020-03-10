using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class RegExpression
    {
        public string MatchExpression { get; set; } = "";
        public string FormatExpression { get; set; } = "";

        public string Comment { get; set; } = "";
        
        public int Rank { get; set; } = 1;
    }
}