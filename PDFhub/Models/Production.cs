using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public class ProductionSection
    {
        public string Section { get; set; } = "";
        public int Pages { get; set; } = 0;
        public int PagesReceived { get; set; } = 0;
    }

    public class ProductionEdition
    {
        public string Edition { get; set; } = "";
        public int Pages { get; set; } = 0;
        public int PagesReceived { get; set; } = 0;
    }
    public class Production
    {
        public int ProductionID { get; set; } = 0;
        public DateTime PubDate { get; set; } = DateTime.MinValue;
        public string Publication { get; set; } = "";
        public string Alias { get; set; } = "";
        public DateTime ReleaseTime { get; set; } = DateTime.MinValue;

        public bool Released { get; set; } = false;
        public List<ProductionEdition> Editions { get; set; }
        public List<ProductionSection> Sections { get; set; }

        public List<string> Channels { get; set; }

     

        public Production()
        {
            Sections = new List<ProductionSection>();
            Editions = new List<ProductionEdition>();
            Channels = new List<string>();
           
        }

        public string PubDateStr
        {
            get
            {
                return Utils.Date2String(PubDate);
            }
        }

        public string PageStr
        {
            get
            {
                string s = "";
                int n = 0;
                foreach (ProductionEdition ed in Editions)
                {
                    n += ed.Pages;
                    if (s != "")
                       s += ", ";
                   s += ed.Pages.ToString();
                }
                return s;
            }
        }

        public string PagesReceivedStr
        {
            get
            {
                string s = "";
               // int n = 0;
                foreach (ProductionEdition ed in Editions)
                {
                  //  n += sec.PagesReceived;
                  if (s != "")
                       s += ", ";
                    s += ed.PagesReceived.ToString();
                }
                return s;
            }
        }

        public string PageSectionStr
        {
            get
            {
                string s = "";
                foreach (ProductionSection sec in Sections)
                {
                    if (s != "")
                        s += ", ";
                    s += sec.Section + ":" + sec.Pages.ToString();
                }
                return s;
            }
        }

        public string ChannelList
        {
            get
            {
                string s = "";
                foreach (string ch in Channels)
                {
                    if (s != "")
                        s += ", ";
                    s += ch;
                }
                return s;
            }
        }

        public string EditionList
        {
            get
            {
                string s = "";
                foreach (ProductionEdition ed in Editions)
                {
                    if (s != "")
                        s += ", ";
                    s += ed.Edition;
                }
                return s;
            }
        }

        public string ReleaseTimeStr
        {
            get
            {
                return Utils.Time2StringShort(ReleaseTime);
            }
        }

    }

    public class ChannelProgress
    {
        public int ProductionID { get; set; } = 0;
        public int ChannelID { get; set; } = 0;
        public string Name { get; set; } = "";
        public int Pages { get; set; } = 0;
        public int PagesSent { get; set; } = 0;

        public int PagesWithError { get; set; } = 0;
        public string PageList { get; set; } = "";
        public string Alias { get; set; } = "";

        public string FirstSent { get; set; } = "";
        public string LastSent { get; set; } = "";

        public int MergedPDF { get; set; } = 0;
    }
}