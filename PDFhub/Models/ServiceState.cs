using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum ServiceType { Unknown = -1, PlanImport = 1, FileImport=2, Processing=3, Export=4, Database=5, FileServer=6, Maintenance = 7 };
    public enum ServiceState { Unknown = -1, Stopped = 0, Running = 1};
    public class Service
    {
        public string ID { get; set; } = "";
        public ServiceType Type { get; set; } = ServiceType.Unknown;
        public ServiceState State { get; set; } = ServiceState.Unknown;
        public string StateImageUrl { get; set; } = "on";
        public string Name { get; set; } = "";
        public int InstanceNumber { get; set; } = 1;
        public string LastEventTime { get; set; } = "";
        public string LastMessage { get; set; } = "";

        public bool ViewButton { get; set; } = true;

    }
}