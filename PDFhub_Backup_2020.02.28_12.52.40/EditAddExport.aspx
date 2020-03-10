<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAddExport.aspx.cs" Inherits="PDFhub.EditAddExport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Editing Export</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
</head>

<body style="background-color:ghostwhite;">
    <form id="form1" runat="server">
            <script type="text/javascript">

            function CloseAndRebind(args) {
                GetRadWindow().BrowserWindow.refreshGrid(args);
                GetRadWindow().close();
            }
 
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)
 
                return oWindow;
            }
 
            function CancelEdit() {
                GetRadWindow().close();
            }

            function RefreshParentPage() {
                GetRadWindow().BrowserWindow.document.forms[0].submit();
                GetRadWindow().Close();
            }
        </script>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
 <div>
   <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;margin: 10px 10px 10px 10px;">   
     <tr>
        <td style="vertical-align:top">
            <table id="TableLeft" border="0" class="module">
                <tr>
                    <td style="width:140px;">Enabled:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbEnabled" runat="server" Text="" Checked='<%# Bind("Enabled") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td>Channel name:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtChannelName" runat="server" Text='<%# Bind("Name") %>' Width="170"  TabIndex="2"></telerik:RadTextBox>                       
                    </td>
                </tr>
                <tr>
                    <td>Alias:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtChannelNameAlias" Text='<%# Bind("ChannelNameAlias") %>' runat="server"  TabIndex="10" Width="100"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">PDF Generation:</td>
                </tr>
                <tr>
                    <td>PDF type:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="RadDropDownListPDFType" runat="server" SelectedValue='<%# Bind("_PDFType") %>' >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="PDF Lowres (RGB)" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="PDF Highres (RGB)" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="PDF Print (CMYK)" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="HTML ePaper bundle" Value="3" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:120px;">Merge PDF:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbMergedPDF" runat="server" Text="" Checked='<%# Bind("MergedPDF") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td style="width:120px;">Selected pages only:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbSelectedPagesOnly" runat="server" Text="" Checked='<%# Bind("OnlySentSelectedPages") %>' TabIndex="1"></telerik:RadCheckBox>
                        &nbsp;&nbsp;From page&nbsp;
                        <telerik:RadNumericTextBox ID="RadNumericTextBoxFromPage" runat="server"  NumberFormat-DecimalDigits="0" Width="40" ></telerik:RadNumericTextBox>
                        &nbsp;to&nbsp; <telerik:RadNumericTextBox ID="RadNumericTextBoxToPage" runat="server"  NumberFormat-DecimalDigits="0" Width="40"></telerik:RadNumericTextBox>
                    </td>         
                </tr>

                <tr>
                    <td>PDF converter:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="RadDropDownListPDFProcessID" runat="server" SelectedValue='<%# Bind("_PDFProcessID") %>'  Width="250">
                        </telerik:RadDropDownList>
                    </td>
                </tr>

                <tr>
                    <td>Editions:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="RadDropDownListEditionsToGenerate" runat="server" SelectedValue='<%# Bind("_EditionsToGenerate") %>' >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="All editions" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="Ed1 only" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2+Ed3" Value="3" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2+Ed3+Ed4" Value="4" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:160px;">Send common pages:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbSendCommonPages" runat="server" Text="" Checked='<%# Bind("SendCommonPages") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Schedule:</td>
                </tr>
                <tr>
                    <td>Use scheduled sending:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbUseReleaseTime" runat="server" Text="" Checked='<%# Bind("UseReleaseTime") %>' TabIndex="8"></telerik:RadCheckBox>
                    </td>
                </tr> 
                <tr>
                    <td>Start time:
                    </td>
                    <td>
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTime"  TabIndex="8" DbSelectedDate='<%# Bind("_ReleaseTime") %>'></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>End time:
                    </td>
                    <td>
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTimeEnd"  TabIndex="8" TimeView-StartTime='<%# Bind("_ReleaseTimeEnd") %>'></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Naming scheme:</td>
                </tr>
                <tr>
                    <td>Filename definition:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTransmitNameFormat" Text='<%# Bind("TransmitNameFormat") %>' runat="server"  TabIndex="9" Width="300"></telerik:RadTextBox>
                    </td>
                </tr>     
                <tr>
                    <td>Date format:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTransmitNameDateFormat" Text='<%# Bind("TransmitNameDateFormat") %>' runat="server"  TabIndex="10" Width="120"></telerik:RadTextBox>
                   </td>
                </tr>   
                 <tr>
                    <td>Use package definition:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbUsePackageNames" runat="server" Text="" Checked='<%# Bind("UsePackageNames") %>' TabIndex="8"></telerik:RadCheckBox>&nbsp;&nbsp;(for output product combinations)
                    </td>
                </tr>
                <tr>
                    <td>Subfolder definition:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubFolderNamingConvension" Text='<%# Bind("SubFolderNamingConvension") %>' runat="server"  TabIndex="9" Width="300"></telerik:RadTextBox>
                    </td>
                </tr>     
                <tr>
                    <td>Abbreviations:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="RadDropDownListTransmitNameUseAbbr" runat="server" SelectedValue='<%# Bind("TransmitNameUseAbbr") %>' AutoPostBack="true" >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="Long name" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="Input alias" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Output alias" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Extra alias" Value="3" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
         </td>
            <td>&nbsp;</td> 
        <td style="vertical-align:top">
            <table id="TableRight" border="0" class="module">
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Destination:</td>
                </tr>
                <tr>
                    <td>Protocol:
                    </td>
                    <td>
                       <telerik:RadDropDownList ID="DropDownListType" runat="server" SelectedValue='<%# Bind("OutputType") %>' AutoPostBack="true" OnSelectedIndexChanged="DropDownListType_SelectedIndexChanged" >
                          <Items>
                            <telerik:DropDownListItem runat="server" Text="SMB share" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="FTP / SFTP" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Email attachment" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Google Drive" Value="3" />
                            <telerik:DropDownListItem runat="server" Text="Amazon S3" Value="4" />
                          </Items>                            
                       </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="PanelSMB" runat="server" Visible="false" GroupingText="SMB" BackColor="Snow">
                            <table>
                                <tr>
                                    <td>Output folder:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtOutputFolder" runat="server" Text='<%# Bind("OutputFolder") %>' TabIndex="4" Width="380">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>                                        
                                <tr>
                                    <td>Use specific user:
                                    </td>
                                    <td>
                                        <telerik:RadCheckBox ID="cbSpecificUser" runat="server" Text="" Checked='<%# Bind("UseSpecificUser") %>' TabIndex="8"></telerik:RadCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>User name:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtUserNameX" Text="" runat="server"  TabIndex="9">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPasswordX" Text="" runat="server" TextMode="Password"  TabIndex="10">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
</asp:Panel>
<asp:Panel ID="PanelFTP" runat="server" Visible="true" GroupingText="FTP/SFTP" BackColor="Snow">
                        
                                <table style="width:500px;">
                                    <tr>
                                        <td >FTP Server:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpServer" runat="server" Text='<%# Bind("FTPserver") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>FTP folder:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpFolder" runat="server" Text='<%# Bind("FTPfolder") %>' TabIndex="4" Width="380">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>   
                                   
                                    <tr>
                                        <td>FTP User name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpUsername" Text='<%# Bind("FTPusername") %>' runat="server"  TabIndex="9" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpPassword" Text='<%# Bind("FTPPassword") %>' runat="server"   TabIndex="10" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Passive mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpPassive" runat="server" Text="" Checked='<%# Bind("FTPPasv") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>XCRC mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpXcrc" runat="server" Text="" Checked='<%# Bind("FTPXCRC") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Encryption:
                                        </td>
                                        <td>
                                            <telerik:RadDropDownList ID="RadDropDownListEncryption" runat="server" SelectedValue='<%# Bind("FTPEncryption") %>' AutoPostBack="true" Width="300" >
                                               <Items>
                                                <telerik:DropDownListItem runat="server" Text="None (usually port 21)" Value="0" />
                                                <telerik:DropDownListItem runat="server" Text="FTPES Explicit SSL/TLS (usually port 21)" Value="1" />
                                                <telerik:DropDownListItem runat="server" Text="FTPS Implicit SSL/TLS (usually port 990)" Value="2" />
                                                <telerik:DropDownListItem runat="server" Text="SFTP - SSH (usually port 22)" Value="3" />
                                            </Items>
                            
                                            </telerik:RadDropDownList>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td >FTP post check:
                                        </td>
                                        <td>
                                            <telerik:RadDropDownList ID="RadDropDownListFTPPostCheck" runat="server" SelectedValue='<%# Bind("FTPPostCheck") %>' Width="300" >
                                               <Items>
                                                <telerik:DropDownListItem runat="server" Text="None" Value="0" />
                                                <telerik:DropDownListItem runat="server" Text="File exists check" Value="1" />
                                                <telerik:DropDownListItem runat="server" Text="File size check" Value="2" />
                                                <telerik:DropDownListItem runat="server" Text="File readback test" Value="3" />
                                            </Items>
                            
                                            </telerik:RadDropDownList>
                                        </td>
                                     </tr>

                                </table>
</asp:Panel>
<asp:Panel ID="PanelEmail" runat="server" Visible="false" GroupingText="Email attachments" BackColor="Snow">
                                <table style="width:500px;">
                                    <tr>
                                        <td >SMTP Server:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailServer" runat="server" Text='<%# Bind("EmailServer") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >SMTP Port:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailPort" runat="server" Text='<%# Bind("EmailPort") %>' TabIndex="4" Width="100">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >SMTP Username:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailUserName" runat="server" Text='<%# Bind("EmailUserName") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >SMTP Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailPassword" runat="server" Text='<%# Bind("EmailPassword") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >TO address:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailTo" runat="server" Text='<%# Bind("EmailTo") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >FROM address:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailFrom" runat="server" Text='<%# Bind("EmailFrom") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td >CC address:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailCC" runat="server" Text='<%# Bind("EmailCC") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Use SSL:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbEmailUseSSL" runat="server" Text="" Checked='<%# Bind("EmailUseSSL") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >Email subject:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailSubject" runat="server" Text='<%# Bind("EmailSubject") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >Email template:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RadEmailBody" runat="server" Text='<%# Bind("RadEmailBody") %>' TabIndex="4"  Width="380" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Use HTML body:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbEmailHTML" runat="server" Text="" Checked='<%# Bind("EmailHTML") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td >Email timeout:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtEmailTimeout" runat="server" Text='<%# Bind("EmailTimeout") %>' TabIndex="4" Width="100" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                 

                                </table>
</asp:Panel>
<asp:Panel ID="PanelGoogle" runat="server" Visible="false" GroupingText="Google drive" BackColor="Snow">
     <table style="width:500px;">
                                    <tr>
                                        <td >Google Client ID:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtGoogleClientID" runat="server" Text="" TabIndex="4" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >Client Secret:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtGoogleClientSecret" runat="server" Text="" TabIndex="4" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td >Scope URL:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtGoogleScopeURL" runat="server" Text="https://www.googleapis.com/auth/drive" TabIndex="4" Width="380">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td >POST (upload) URL:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RadTextBox1" runat="server" Text="https://www.googleapis.com/drive/v3/files/" TabIndex="4" Width="380">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>

     </table>
</asp:Panel>
<asp:Panel ID="PanelS3" runat="server" Visible="false" GroupingText="Amazon S3" BackColor="Snow">
      <table style="width:500px;">
          <tr>
                                        <td >AWS Endpoint:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RadTextBox4" runat="server" Text="https://s3-eu-west-1.amazonaws.com" TabIndex="4" Width="380">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >AWS Access key:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtAWSAccessKey" runat="server" Text="" TabIndex="4" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >Bucket name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RadTextBox2" runat="server" Text="test" TabIndex="4" >
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >Upload subpath:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RadTextBox3" runat="server" Text="qa_data/pdf/" TabIndex="4" Width="380">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
          </table>
</asp:Panel>
                    </td>
                </tr>            
                <tr>
                    <td style="vertical-align:top;">Generate hash checksum:
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonListCheckSum" runat="server">
                             <asp:ListItem Text="No checksum file" Selected="True" />
                             <asp:ListItem Text="MD5 hash (in filename.md5)" Selected="False" />
                             <asp:ListItem Text="SHA256 hash  (in filename.sha256)" Selected="False" />
                  
                        </asp:RadioButtonList>
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Triggers</td>
                </tr>
                <tr>
                    <td>Finalization trigger:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="RadDropDownListTriggerMode" runat="server" SelectedValue='<%# Bind("TriggerMode") %>' Width="300" >
                        <Items>
                        <telerik:DropDownListItem runat="server" Text="None" Value="0" />
                        <telerik:DropDownListItem runat="server" Text="Manuel (WebCenter) - xml file" Value="1" />
                        <telerik:DropDownListItem runat="server" Text="Manuel (WebCenter) - e-mail" Value="1" />
                        <telerik:DropDownListItem runat="server" Text="Auto - e-mail when all pages sent" Value="2" />
                        <telerik:DropDownListItem runat="server" Text="Auto - XML to output folder" Value="3" />
                    </Items>
                        </telerik:RadDropDownList>
                                              
                    </td>         
                </tr>
                <tr>
                    <td>Email receiver(s):
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmailReceivers" Text="test@infralogic.dk" runat="server" Width="300"  TabIndex="9">
                          </telerik:RadTextBox>
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Cleanup</td>
                </tr>
                <tr>
                    <td>Achive export:
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbDeleteOldOutputFiles" runat="server" Text="" Checked='<%# Bind("DeleteOldOutputFiles") %>' TabIndex="8"></telerik:RadCheckBox>
                        &nbsp;(max age of files in output folder based on publication setting) 
                    </td>         
                </tr>

                

            </table>
         </td>
     </tr>
        <tr>
        <td colspan="3">&nbsp;</td>
    </tr>
     <tr>
       <td align="center" colspan="3">
            <telerik:RadButton ID="btnUpdate" Text="Insert" runat="server"  OnClick="btnUpdate_Click"></telerik:RadButton>
           &nbsp;
            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
       </td>
     </tr>
     <tr>
        <td class="title" style="font-weight: bold;" colspan="3">Regular expression output file renaming:</td>
     </tr>
     <tr>
        <td colspan="3">
            <telerik:RadGrid ID="RadGridRegex" runat="server" 
                                OnNeedDataSource="RadGridRegex_NeedDataSource"  OnItemCreated="RadGridRegex_ItemCreated" OnItemCommand="RadGridRegex_ItemCommand"
                                AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                                AllowFilteringByColumn="false"
                                GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small" Width="1010">
                <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
                <GroupingSettings ShowUnGroupButton="false" />
                <MasterTableView EditMode="InPlace" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
                    CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="Rank">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new regular expression" ShowRefreshButton="false"  />                   
                    <Columns>                
                        <telerik:GridBoundColumn UniqueName="Rank"  DataField="Rank" HeaderText="Rank" DataType="System.Int32">
                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn UniqueName="MatchExpression"  DataField="MatchExpression" HeaderText="Match expression" DataType="System.String">
                            <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn UniqueName="FormatExpression"  DataField="FormatExpression" HeaderText="Format expression" DataType="System.String"  >
                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                            <telerik:GridEditCommandColumn EditText="Edit">
                                <HeaderStyle Width="80px" />
                            </telerik:GridEditCommandColumn>

                            <telerik:GridButtonColumn  ConfirmText="Delete this regex?" ConfirmDialogType="RadWindow"
                                            ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >   
                                <HeaderStyle Width="80px" />
                        </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn HeaderText="Up"  CommandName="Up" ButtonType="ImageButton" ImageUrl="Images/MoveUp.gif">   
                            <HeaderStyle Width="80px" />
                        </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn HeaderText="Down"  CommandName="Down" ButtonType="ImageButton" ImageUrl="Images/MoveDown.gif">   
                            <HeaderStyle Width="80px" />
                        </telerik:GridButtonColumn>
                        <telerik:GridBoundColumn UniqueName="Comment" DataField="Comment" HeaderText="Comment" DataType="System.String">
                            <HeaderStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                    </Columns>                
                </MasterTableView>
            </telerik:RadGrid>        

        </td> 
          
     </tr>
     
     <tr>
       <td align="center">&nbsp;</td>         
    </tr>
</table>
        </div>
    </form>
</body>
</html>
