using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum InputType { SMB = 0, FTP = 1, Email = 2,Google = 3,Amazon = 4};
    public enum EncryptionType {  None = 0, FTPES = 1, FTPS = 2, SFTP = 3};

    public class InputConfiguration
    {
        public int InputID { get; set; } = 0;
        public InputType InputType { get; set; } = InputType.SMB;
        public bool Enabled { get; set; } = true;
        public string InputName { get; set; } = "";
        public string InputPath { get; set; } = "";
        public string SearchMask { get; set; } = "*.*";
        public int StableTime { get; set; } = 2;
        public int PollTime { get; set; } = 1;
        public bool UseRegExp { get; set; } = true;
        public string NamingMask { get; set; } = "%P-%D[yyyymmdd]-%E-%S-%N";
        public string Separators { get; set; } = ".-";
        public string FTPserver { get; set; } = "";
        public string FTPusername { get; set; } = "";
        public string FTPpassword { get; set; } = "";
        public string FTPfolder { get; set; } = "";
        public int FTPport { get; set; } = 21;
        public bool FTPpasw { get; set; } = true;
        public bool FTPxcrc { get; set; } = true;
        public EncryptionType FTPtls { get; set; } = EncryptionType.None;
        public bool UseSpecificUser { get; set; } = false;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool CallScript { get; set; } = true;
        public string ScriptName { get; set; } = "";
        public bool UseRegex { get; set; } = false;

        public bool MakeCopy { get; set; } = false;
        public string CopyFolder { get; set; } = "";

        public string AckFileFolder { get; set; } = "";
        public bool SendAckFile { get; set; } = false;
        public int AckFlagValue { get; set; } = 1035;

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;


        public List<RegExpression> RegularExpressions;

        // helpers

        public string _StateImageUrl
        {
            get {
                return Enabled ? "ok" : "stop";
            }
        }
        public string _InputTypeStr
        {
            get { 
                return Constants.InputType [(int)InputType];
            }
        }

       


        public InputConfiguration()
        {
            RegularExpressions = new List<RegExpression>();
        }

    }
}