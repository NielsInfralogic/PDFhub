using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub
{
    public static class Constants
    {
        public static readonly string[] InputType = { "SMB share", "FTP", "S-FTP", "Google Drive", "Amazon S3", "Email attachment" };

        public static readonly int[] ErrorEvents = { 6,16,36,46,56,996,116,186};
        public static readonly int[] WarningEvents = { 997, 117, 187 };
    }

}