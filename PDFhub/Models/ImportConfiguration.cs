using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum ImportFormat { NewsPilot=1, PPI=2, InfraLogicXML = 3 };
    public class ImportConfiguration
    {
        public ImportConfiguration()
        {
            PPITranslations = new List<PPITranslations>();
        }

        public int ImportID { get; set; } = 0;
        public string Name { get; set; } = "";
        public bool Enabled { get; set; } = true;

        public int OwnerInstance { get; set; } = 1;
        public string InputFolder { get; set; } = "";
        public string DoneFolder { get; set; } = "";
        public string CopyFolder { get; set; } = "";
        public string ErrorFolder { get; set; } = "";
        public string LogFolder { get; set; } = "";
        public /*ImportType*/ int ImportType { get; set; } = (int)ImportFormat.NewsPilot;
        public string ConfigFile { get; set; } = "";
        public string ConfigFile2 { get; set; } = "";

        public bool SendErrorEmail { get; set; } = false;
        public string EmailReceiver { get; set; } = "";

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;

        public List<PPITranslations> PPITranslations { get; set; }
        public string _ImportType
        {
            get
            {
                if (ImportType == (int)ImportFormat.PPI)
                    return "PPI";
                if (ImportType == (int)ImportFormat.InfraLogicXML)
                    return "InfraLogic XML";
                return "Newspilot";
            }
        }

        public string _EnabledImageUrl
        {
            get
            {
                return Enabled ? "Ok" : "Stop";
            }
        }
    } 
}