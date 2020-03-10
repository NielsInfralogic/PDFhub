using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    


    public class Publication
    {
        public int PublicationID { get; set; } = 0;
        public string Name { get; set; } = "";

        public int PageFormatID { get; set; } = 0;

        public bool TrimToFormat { get; set; } = false;

        public double LatestHour { get; set; } = 0.0;

        public int DefaultProofID { get; set; } = 0;

        public bool DefaultApprove { get; set; } = false;

        public int DefaultPriority { get; set; } = 0;

        public int AutoPurgeKeepDays { get; set; } = 0;

        public int AutoPurgeKeepDaysArchive { get; set; } = 0;

        public string EmailRecipient { get; set; } = "";

        public string EmailCC { get; set; } = "";

        public string EmailSubject { get; set; } = "";

        public string EmailBody { get; set; } = "";

        public string UploadFolder { get; set; } = "";

        public DateTime Deadline { get; set; } = DateTime.MinValue;
        public int CustomerID { get; set; } = 0;
        public int PublisherID { get; set; } = 0;

        public bool AllowUnplanned { get; set; } = false;

        public string AnnumText {get;set;} = "";

        public List<PublicationChannel> PublicationChannels { get; set; }

        public int NoReleaseTime { get; set; } = 0;
        public int ReleaseDays { get; set; } = 0;
        public int ReleaseTime { get; set; } = 0;
        public bool PubdateMove { get; set; } = false;
        public int PubdateMoveDays { get; set; } = 0;
        public string InputAlias { get; set; } = "";
        public string OutputAlias { get; set; } = "";
        public string ExtendedAlias { get; set; } = "";
        public string ExtendedAlias2 { get; set; } = "";
        public bool SendReport { get; set; } = false;

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;

        public Publication()
        {
            PublicationChannels = new List<PublicationChannel>();
        }

        public string _PublisherName
        {
            get
            {
                return Utils.GetPublisherName(PublisherID);
            }
        }

        public string _DefaultChannels
        {
            get
            {
                string s = "";
                foreach(PublicationChannel pubChannel in PublicationChannels)
                {
                    if (s != "")
                        s += ",";
                    s += Utils.GetChannelName(pubChannel.ChannelID);
                }

                return s;
            }
        }

        public string _ReleaseTimeDays
        {
            get
            {

                int h = ReleaseTime / 100;
                return string.Format("{0:00}:{1:00} - {2}d", h, ReleaseTime - h * 100, ReleaseDays);
            }
        }

        public TimeSpan _ReleaseTime
        {
            get
            {
                int h = ReleaseTime / 100;
                return new TimeSpan(h, ReleaseTime - (100 * h), 0);
            }
        }

       
    }

}