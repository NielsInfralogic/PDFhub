using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Configuration;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders;
using Telerik.Windows.Documents.Fixed.FormatProviders.Text;

namespace PDFhub
{
    public static class Utils
    {
        public static bool ReadConfigBoolean(string setting, bool defaultValue)
        {
            bool ret = defaultValue;
            if (ConfigurationManager.AppSettings[setting] != null)
            {
                try
                {
                    return Convert.ToInt32((string)ConfigurationManager.AppSettings[setting]) == 1;
                }
                catch { }
            }

            return ret;
        }

        public static int ReadConfigInt32(string setting, int defaultValue)
        {
            int ret = defaultValue;
            if (ConfigurationManager.AppSettings[setting] != null)
            {
                try
                {
                    return Convert.ToInt32((string)ConfigurationManager.AppSettings[setting]);
                }
                catch { }
            }

            return ret;
        }

        public static string ReadConfigString(string setting, string defaultValue)
        {
            string ret = defaultValue;
            if (ConfigurationManager.AppSettings[setting] != null)
            {
                try
                {
                    return (string)ConfigurationManager.AppSettings[setting];
                }
                catch { }
            }

            return ret;
        }

        public static int StringToInt(string s)
        {
            int.TryParse(s.Trim(), out int n);
            return n;
        }

        public static string Date2String(DateTime dt)
        {
            if (dt.Year < 2000)
                return string.Empty;

            return string.Format("{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day);
        }

        public static string Time2String(DateTime dt)
        {
            if (dt.Year < 2000)
                return string.Empty;

            return string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        public static string Time2StringShort(DateTime dt)
        {
            if (dt.Year < 2000)
                return string.Empty;

            return string.Format("{0:00}-{1:00} {2:00}:{3:00}", dt.Day, dt.Month, dt.Hour, dt.Minute);
        }

        public static long StringDateTimeToLong(string DateTimeStr)
        {
            long.TryParse(DateTimeStr.Replace(" ", "").Replace(":", "").Replace("-", ""), out long n);
            return n;
        }

        /// <summary>
        /// Converts string date format YYYY-MM-DD to DateTime
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime DateStringToDateTime(string dateStr)
        {
            dateStr = dateStr.Trim();

            if (dateStr.Length != 10)
                return DateTime.MinValue;

            try
            {
                return new DateTime(StringToInt(dateStr.Substring(0, 4)), StringToInt(dateStr.Substring(5, 2)), StringToInt(dateStr.Substring(8, 2)), 0, 0, 0);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            if (!request.Headers.TryGetValues(key, out IEnumerable<string> keys))
                return null;

            return keys.First();
        }

        public static bool CheckCredentials(string authHeader)
        {
            string userName;
            string password;

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                userName = usernamePassword.Substring(0, seperatorIndex);
                password = usernamePassword.Substring(seperatorIndex + 1);

                if (userName == string.Empty || password == string.Empty)
                    return false;
                if (string.Equals(userName.Trim(), ReadConfigString("Username", "").Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                    password.Trim() == ReadConfigString("Password", "").Trim())
                    return true;
            }

            return false;
        }

        public static void WriteLog(bool toStdOutAlso, string logoutput)
        {
            string logFile = ReadConfigString("LogFile", @"c:\temp\PDFHubWeb.log");
            // Always log to stdout
            if (toStdOutAlso)
                Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + logoutput);

            try
            {
                FileInfo fi = new FileInfo(logFile);
                if (fi.Length > 10 * 1024 * 1024)
                {
                    File.Copy(logFile, logFile + "2", true);
                    File.Delete(logFile);
                }
            }
            catch (Exception)
            {
            }

            if (logFile != "")
            {
                try
                {
                    StreamWriter w = File.AppendText(logFile);
                    w.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + logoutput);
                    w.Flush();
                    w.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        public static List<string> GetUserGroupNames()
        {
            if (HttpContext.Current.Session["UserGroupList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.UserGroup> userGroupList = new List<Models.UserGroup>();
                if (db.GetUserGroups(ref userGroupList, out string errmsg))
                    HttpContext.Current.Session["UserGroupList"] = userGroupList;
                else
                    return new List<string>();
            }
            try
            {
                List<Models.UserGroup> userGroupList = (List<Models.UserGroup>)HttpContext.Current.Session["UserGroupList"];
                return userGroupList.Select(p => p.UserGroupName).ToList();
            }
            catch
            {
            }

            return new List<string>();
        }

        public static string GetUserGroupName(int userGroupID)
        {
            if (HttpContext.Current.Session["UserGroupList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.UserGroup> userGroupList = new List<Models.UserGroup>();
                if (db.GetUserGroups(ref userGroupList, out string errmsg))
                    HttpContext.Current.Session["UserGroupList"] = userGroupList;
                else
                   return "";
            }
            try
            {
                List<Models.UserGroup> userGroupList = (List<Models.UserGroup>)HttpContext.Current.Session["UserGroupList"];
                Models.UserGroup ug = userGroupList.FirstOrDefault(p => p.UserGroupID == userGroupID);
                if (ug != null)
                    return ug.UserGroupName;
            }
            catch
            {
            }

            return "";
        }

        public static int GetUserGroupID(string userGroupName)
        {
            if (HttpContext.Current.Session["UserGroupList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.UserGroup> userGroupList = new List<Models.UserGroup>();
                if (db.GetUserGroups(ref userGroupList, out string errmsg))
                    HttpContext.Current.Session["UserGroupList"] = userGroupList;
                else
                    return 0;
            }
            try
            {
                List<Models.UserGroup> userGroupList = (List<Models.UserGroup>)HttpContext.Current.Session["UserGroupList"];
                Models.UserGroup ug = userGroupList.FirstOrDefault(p => string.Equals(p.UserGroupName, userGroupName, StringComparison.InvariantCultureIgnoreCase));
                if (ug != null)
                    return ug.UserGroupID;
            }
            catch
            {
            }

            return 0;
        }


        public static List<Models.Channel> GetChannels()
        {
            List<Models.Channel> channelList = new List<Models.Channel>();

            if (HttpContext.Current.Session["ChannelList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                if (db.GetChannelList(ref channelList, out string errmsg))
                    HttpContext.Current.Session["ChannelList"] = channelList;
                else
                    Utils.WriteLog(false, "ERROR: db.GetChannelList() - " + errmsg);

            }
            else
            {
                try
                {
                    return (List<Models.Channel>)HttpContext.Current.Session["ChannelList"];
                }
                catch
                {
                }
            }

            return channelList;
        }

        public static List<string> GetChannelNames()
        {
            List<Models.Channel> channelList = GetChannels();
            
            try
            {
                return channelList.Select(p => p.Name ).ToList();
            }
            catch
            {
            }

            return new List<string>();
        }

        public static string GetChannelName(int channelID)
        {
            List<Models.Channel> channelList = GetChannels();

            try
            {                Models.Channel c = channelList.FirstOrDefault(p => p.ChannelID == channelID);
                if (c != null)
                    return c.Name;
            }
            catch
            {
            }

            return "";
        }

        public static int GetChannelID(string channelName)
        {
            List<Models.Channel> channelList = GetChannels();
            try
            {
                Models.Channel c = channelList.FirstOrDefault(p => string.Equals(p.Name,channelName, StringComparison.InvariantCultureIgnoreCase));
                if (c != null)
                    return c.ChannelID;
            }
            catch
            {
            }

            return 0;
        }

        public static string GetPublisherName(int publisherID)
        {
            if (HttpContext.Current.Session["PubliserList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.Publisher> publisterList = new List<Models.Publisher>();
                if (db.GetPublisherList(ref publisterList, out string errmsg))
                    HttpContext.Current.Session["PubliserList"] = publisterList;
                else
                    return "";
            }
            
            try
            {
                List<Models.Publisher> publisherist = (List<Models.Publisher>)HttpContext.Current.Session["PubliserList"];
                Models.Publisher pub = publisherist.FirstOrDefault(p => p.PublisherID == publisherID);
                if (pub != null)
                    return pub.PublisherName;
            }
            catch
            {
            }

            return "";
        }


        public static List<Models.Publisher> GetPublishers()
        {
            List<Models.Publisher> publisterList = new List<Models.Publisher>();
            if (HttpContext.Current.Session["PubliserList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                
                if (db.GetPublisherList(ref publisterList, out string errmsg))
                    HttpContext.Current.Session["PubliserList"] = publisterList;
                else
                    Utils.WriteLog(false, "db.GetPublisherList() - " + errmsg);

            }
            else
            {
                try
                {
                    return (List<Models.Publisher>)HttpContext.Current.Session["PubliserList"];
                }
                catch
                {
                }
            }

            return publisterList;
        }

        public static Models.PDFProcess GetPDFProcessFromName(string processName)
        {
            if (HttpContext.Current.Session["PDFProcessList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.PDFProcess> pdfProcessList = new List<Models.PDFProcess>();
                List<string> pdfProcessNameList = new List<string>();
                if (db.GetPDFProcesses(ref pdfProcessList, out string errmsg))
                {
                    HttpContext.Current.Session["PDFProcessList"] = pdfProcessNameList;
                }
                else
                    return null;
            }
            try
            {
                List<Models.PDFProcess> pdfProcessList = (List<Models.PDFProcess>)HttpContext.Current.Session["PDFProcessList"];
                return pdfProcessList.FirstOrDefault(p => p.ProcessName.Replace(" (CMYK)", "") == processName.Replace(" (CMYK)", ""));
            }
            catch
            {
            }

            return null;
        }

        public static Models.PDFProcess GetPDFProcessFromID(int pdfProcessID)
        {
            if (HttpContext.Current.Session["PDFProcessList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.PDFProcess> pdfProcessList = new List<Models.PDFProcess>();
                List<string> pdfProcessNameList = new List<string>();
                if (db.GetPDFProcesses(ref pdfProcessList, out string errmsg))
                {
                    HttpContext.Current.Session["PDFProcessList"] = pdfProcessNameList;
                }
                else
                    return null;
            }
            try
            {
                List<Models.PDFProcess> pdfProcessList = (List<Models.PDFProcess>)HttpContext.Current.Session["PDFProcessList"];
                return pdfProcessList.FirstOrDefault(p => p.ProcessID == pdfProcessID);
            }
            catch
            {
            }

            return null;
        }

        public static List<string> GetPDFProcessNames()
        {
            if (HttpContext.Current.Session["PDFProcessList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.PDFProcess> pdfProcessList = new List<Models.PDFProcess>();
                List<string> pdfProcessNameList = new List<string>();
                if (db.GetPDFProcesses(ref pdfProcessList, out string errmsg))
                {
                    HttpContext.Current.Session["PDFProcessList"] = pdfProcessList;
                }
                else
                    return new List<string>();
            }
            try
            {
                List<Models.PDFProcess> pdfProcessList = (List<Models.PDFProcess>)HttpContext.Current.Session["PDFProcessList"];
                //return pdfProcessList.Select(p => p.ProcessName + (p.ProcessType== Models.PDFProcessType.ToLowResPDF ? " (lowres) " :(p.ProcessType == Models.PDFProcessType.ToCMYKPDF ? " (CMYK)" : "")) ).ToList();
                return pdfProcessList.Select(p => p.ProcessName).ToList();
            }
            catch
            {
            }

            return new List<string>();
        }

        public static List<string> GetPublisherNames()
        {
            if (HttpContext.Current.Session["PubliserList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.Publisher> publisterList = new List<Models.Publisher>();
                if (db.GetPublisherList(ref publisterList, out string errmsg))
                    HttpContext.Current.Session["PubliserList"] = publisterList;
                else
                   return new List<string>();
            }
            try
            {
                List<Models.Publisher> publisherist = (List<Models.Publisher>)HttpContext.Current.Session["PubliserList"];
                return publisherist.Select(p => p.PublisherName).ToList();
            }
            catch
            {
            }

            return new List<string>();
        }

        public static int GetPublisherID(string publisherName)
        {
            if (HttpContext.Current.Session["PubliserList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.Publisher> publisterList = new List<Models.Publisher>();
                if (db.GetPublisherList(ref publisterList, out string errmsg))
                    HttpContext.Current.Session["PubliserList"] = publisterList;
                else
                    return 0;
            }
            try
            {
                List<Models.Publisher> publisherist = (List<Models.Publisher>)HttpContext.Current.Session["PubliserList"];
                Models.Publisher pub = publisherist.FirstOrDefault(p => string.Equals(p.PublisherName,publisherName, StringComparison.InvariantCultureIgnoreCase));
                if (pub != null)
                    return pub.PublisherID;
            }
            catch
            {
            }

            return 0;
        }


        public static List<string> GetPublicationNames()
        {
            if (HttpContext.Current.Session["PublicationShortList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.PublicationShort> publicationList = new List<Models.PublicationShort>();
                if (db.GetPublicationShortList(ref publicationList, out string errmsg))
                    HttpContext.Current.Session["PublicationShortList"] = publicationList;
                else
                    return new List<string>();
            }
            try
            {
                List<Models.PublicationShort> publicationList = (List<Models.PublicationShort>)HttpContext.Current.Session["PublicationShortList"];
                return publicationList.Select(p => p.Name).ToList();
            }
            catch
            {
            }

            return new List<string>();
        }

        public static string GetPublicationName(int publicationID)
        {
            if (HttpContext.Current.Session["PublicationShortList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();
                List<Models.PublicationShort> publicationList = new List<Models.PublicationShort>();
                if (db.GetPublicationShortList(ref publicationList, out string errmsg))
                    HttpContext.Current.Session["PublicationShortList"] = publicationList;
                else
                    return "";
            }
            try
            {
                List<Models.PublicationShort> publicationList = (List<Models.PublicationShort>)HttpContext.Current.Session["PublicationShortList"];
                Models.PublicationShort pub = publicationList.FirstOrDefault(p => p.PublicationID == publicationID);
                if (pub != null)
                    return pub.Name;

            }
            catch
            {
            }

            return "";
        }

        public static List<Models.PublicationShort> GetPublicationsShort()
        {
            List<Models.PublicationShort> publicationList = new List<Models.PublicationShort>();

            if (HttpContext.Current.Session["PublicationShortList"] == null)
            {
                DataProviders.DBaccess db = new DataProviders.DBaccess();

                if (db.GetPublicationShortList(ref publicationList, out string errmsg))
                    HttpContext.Current.Session["PublicationShortList"] = publicationList;
                else
                    Utils.WriteLog(false, "ERROR: db.GetPublicationShortList() - " + errmsg);


            }

            try
            {
                return (List<Models.PublicationShort>)HttpContext.Current.Session["PublicationShortList"];
            }
            catch
            {
            }


            return publicationList;
        }



        public static bool PdfReadTest(string pdfFile)
        {
            bool readOK = false;
            using (var fileStream = File.OpenRead(pdfFile))
            {
                try
                {
                    RadFixedDocument document = new PdfFormatProvider(fileStream, FormatProviderSettings.ReadAllAtOnce).Import();
                    string textDocument = new TextFormatProvider().Export(document);
                    readOK = textDocument.Length > 0;
                    
                }
                catch
                {
                    readOK = false;
                }
//                System.Diagnostics.Process.Start(path + "DemoPdfTelerik.txt");
            }

            return readOK;
        }
    }
}