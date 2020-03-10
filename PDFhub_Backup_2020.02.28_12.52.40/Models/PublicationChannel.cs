using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum PushTrigger { None = 0, Manuel = 1, Email = 2};
    public class PublicationChannel
    {
        public int ChannelID { get; set; } = 0;
        public int Trigger { get; set; } = (int)PushTrigger.None;
        public int PubDateMoveDays { get; set; } = 0;
        public int ReleaseDelay { get; set; } = 0;

        public bool SendPlan { get; set; } = false;

        public string _Trigger
        {
            get
            {
                if (Trigger == 1)
                    return "Manuel";
                else if (Trigger == 2)
                    return "Email";
                return "None";
            }
        }
    }

    // For GUI..
    public class PublicationChannelAll
    {
        public bool Use { get; set; } = false;
        public string ChannelName { get; set; } = "";
        public int Trigger { get; set; } = (int)PushTrigger.None;
        public int PubDateMoveDays { get; set; } = 0;
        public int ReleaseDelay { get; set; } = 0;

        public int ChannelID { get; set; } = 0;
        public bool SendPlan { get; set; } = false;


        public string _Trigger
        {
            get
            {
                if (Trigger == 1)
                    return "Email";
                else if (Trigger == 2)
                    return "Manuel";
                return "None";
            }
        }

        public string _ImageOnOff
        {
            get
            {
                return Use ? "ok" : "stop";
            }
        }
    }

}