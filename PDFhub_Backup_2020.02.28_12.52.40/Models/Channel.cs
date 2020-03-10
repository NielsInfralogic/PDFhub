using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    public enum ChannelType { SMB = 0, FTP = 1, Email = 2, Google = 3, AmazonS3 = 4 };
    public enum FTPPostCheckMode { None = 0, FileExists = 1, FileSize = 2, Readback = 3 };
    public enum PDFtype { PDFlowres = 0, PDFHighRes = 1, PDFPrint = 2 };

    public enum TriggerType { None=0, ManuelFile=1,ManuelEmail=2, AutoFile=3,AutoEmail=4};

    public class Channel
    {
        public int ChannelID { get; set; } = 0;

        public string Name { get; set; } = "";

        public bool Enabled { get; set; } = true;

        public int OwnerInstance { get; set; } = 1;

        //public int ChannelGroupID { get; set; } = 1;

        //public int PublisherID { get; set; } = 1;

        public bool UseReleaseTime { get; set; } = false;

        public int ReleaseTime { get; set; } = 0;

        public int ReleaseTimeEnd { get; set; } = 0;

        public string TransmitNameFormat { get; set; } = "";

        public string TransmitNameDateFormat { get; set; } = "";

        public int TransmitNameUseAbbr { get; set; } = 0;

        public int TransmitNameOptions { get; set; } = 0;

        public int MiscInt { get; set; } = 0;

        public string MiscString { get; set; } = "";

        public string ConfigFile { get; set; } = "";

        public /*ChannelType*/ int OutputType { get; set; } = (int)ChannelType.SMB;

        public string FTPServer { get; set; } = "";

        public int FTPPort { get; set; } = 21;

        public string FTPUserName { get; set; } = "";

        public string FTPPassword { get; set; } = "";

        public string FTPfolder { get; set; } = "";

        public /*EncryptionType*/ int FTPEncryption { get; set; } = (int)EncryptionType.None;

        public bool FTPPasv { get; set; } = true;

        public bool FTPXCRC { get; set; } = true;

        public int FTPTimeout { get; set; } = 60;

        public int FTPBlockSize { get; set; } = 8192;

        public bool FTPUseTmpFolder { get; set; } = false;

        public /*FTPPostCheck*/ int FTPPostCheck { get; set; } = (int)FTPPostCheckMode.None;

        public string EmailServer { get; set; } = "";

        public int EmailPort { get; set; } = 25;

        public string EmailUserName { get; set; } = "";

        public string EmailPassword { get; set; } = "";

        public string EmailFrom { get; set; } = "";

        public string EmailTo { get; set; } = "";

        public string EmailCC { get; set; } = "";

        public bool EmailUseSSL { get; set; } = false;

        public string EmailSubject { get; set; } = "";

        public string EmailBody { get; set; } = "";

        public bool EmailHTML { get; set; } = false;

        public int EmailTimeout { get; set; } = 60;

        public string OutputFolder { get; set; } = "";

        public bool UseSpecificUser { get; set; } = false;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";

        // From old ChannelGroupw

        public int PDFType { get; set; } = (int)PDFtype.PDFlowres;
        public bool MergedPDF { get; set; } = false;

        public int PDFProcessID { get; set; } = 0;

        public int EditionsToGenerate { get; set; } = 0;
        public bool SendCommonPages { get; set; } = false;

        public string SubFolderNamingConvension { get; set; } = "";

        public string ChannelNameAlias { get; set; } = "";

        public DateTime ConfigChangeTime { get; set; } = DateTime.MinValue;

        public /*TriggerType */ int TriggerMode { get; set; } = (int)TriggerType.None;

        public int  DeleteOldOutputFilesDays { get; set; } = 0;

        public string TriggerEmail { get; set; } = "";

        public bool UsePackageNames { get; set; } = false;

        public bool OnlySentSelectedPages { get; set; } = false;

        public int PageNumberStart { get; set; } = 0;
        public int PageNumberEnd{ get; set; } = 0;


        public List<RegExpression> RegularExpressions;

        // Helpers

//        public string _ChannelGroupName { get; set; } = "";
//        public string _PublisherName { get; set; } = "";
        public string _OutputUrl
        {
            get
            {
                if (OutputType == (int)ChannelType.SMB)
                    return OutputFolder;
                else if (OutputType == (int)ChannelType.FTP)
                    return "ftp://" + FTPUserName + "@" + FTPServer + "/" + FTPfolder;
                else if (OutputType == (int)ChannelType.Email)
                    return "mailto:" + EmailTo;
                else
                    return "";
            }
        }

        public string _StateImageUrl
        {
            get
            {
                return Enabled ? "ok" : "stop";
            }
        }

        public string _ChannelType
        {
            get
            {
                if (OutputType == (int)ChannelType.SMB)
                    return "SMB share";
                else if (OutputType == (int)ChannelType.FTP)
                    return "FTP";
                else if (OutputType == (int)ChannelType.Email)
                    return "Email";
                else if (OutputType == (int)ChannelType.Google)
                    return "Google drive";
                else
                    return "Amazon S3";

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

        public TimeSpan _ReleaseTimeEnd
        {
            get
            {
                int h = ReleaseTimeEnd / 100;
                return new TimeSpan(h, ReleaseTimeEnd - (100 * h), 0);
            }
        }

        /*  public List<string> _ChannelGroupList
          {
              get
              {
                  return Utils.GetChannelGroupNames();
              }
          }

          public List<string> _PublisherNameList
          {
              get
              {
                  return Utils.GetPublisherNames();
              }
          }
          */
        public string _PDFType
        {
            get
            {
                if (PDFType == (int)PDFtype.PDFHighRes)
                    return "PDF Highres RGB";
                else if (PDFType == (int)PDFtype.PDFPrint)
                    return "PDF Print CMYK";
                else
                    return "PDF Lowes RGB";
            }
        }

        public string _TriggerMode
        {
            get
            {
                if (TriggerMode == (int)TriggerType.ManuelFile)
                    return "Manual file";
                else if (TriggerMode == (int)TriggerType.ManuelFile)
                    return "Manual email";
                else if (TriggerMode == (int)TriggerType.AutoFile)
                    return "Auto file";
                else if (TriggerMode == (int)TriggerType.AutoEmail)
                    return "Auto email";
                else
                    return "None";
            }
        }

        public string _EditionsToGenerate
        {
            get
            {
                if (EditionsToGenerate == 1)
                    return "Ed1 only";
                else if (EditionsToGenerate == 2)
                    return "Ed1 + Ed2";
                else if (EditionsToGenerate == 3)
                    return "Ed1 + Ed2 + Ed3";
                else if (EditionsToGenerate == 4)
                    return "Ed1 + Ed2 + Ed3 + Ed4";

                return "All"; // = 0
            }
        }


        public Channel()
        {
            RegularExpressions = new List<RegExpression>();
        }

    }

    public class ChannelShort
    {
        public int ChannelID { get; set; } = 0;
        public string Name { get; set; } = "";
        public bool Enabled { get; set; } = false;
        public bool Selected { get; set; } = true;
    }

   
}