using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class NameId
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = 0;
        public bool Selected { get; set; } = false;

        public bool Enabled { get; set; } = true;
    }
}