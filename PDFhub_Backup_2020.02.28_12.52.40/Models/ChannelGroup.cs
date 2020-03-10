using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFhub.Models
{
    //public enum PDFtype { PDFlowres = 0, PDFHighRes = 1, PDFPrint = 2};
    /* public class ChannelGroup
     {
         public int ChannelGroupID { get; set; } = 0;
         public string Name { get; set; } = "";

         public string TransmitNameFormat { get; set; } = "";
         public string TransmitNameDateFormat { get; set; } = "";
         public int TransmitNameUseAbbr { get; set; } = 0;
         public int TransmitNameOptions { get; set; } = 0;
         public  int PDFType { get; set; } = (int)PDFtype.PDFlowres;
         public bool MergedPDF { get; set; } = false;

         public int EditionsToGenerate { get; set; } = 0;
         public bool SendCommonPages { get; set; } = false;

         public string SubFolderNamingConvension { get; set; } = "";

         public string ChannelGroupNameAlias { get; set; } = "";

         public int MiscInt { get; set; } = 0;
         public string MiscString { get; set; } = "";


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
     }*/
}