using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum PackageCondition { Always = 0, Weekdays = 1, Weekends = 2 };
    public class Package
    {
        public int PackageID { get; set; } = 0;
        public string Name { get; set; } = "";
        public int PublicationID { get; set; } = 0;
        public int SectionIndex { get; set; } = 1;

        public int Condition { get; set; } = (int)PackageCondition.Always;

        public string Comment { get; set; } = "";

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;

        // Helper only..
        public string ProductAlias { get; set; } = "";

        public string ConditionStr
        {
            get
            {
                if (Condition == (int)PackageCondition.Weekdays)
                    return "Weekdays";
                else if (Condition == (int)PackageCondition.Weekends)
                    return "Weekends";
                else
                    return "Always";
            }
        }

       
    }
}