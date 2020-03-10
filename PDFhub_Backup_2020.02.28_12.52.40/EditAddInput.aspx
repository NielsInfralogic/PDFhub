<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAddInput.aspx.cs" Inherits="PDFhub.EditAddInput" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Editing Input</title>
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
                        <td>Setup name:
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtInputName" runat="server" Text='<%# Bind("InputName") %>'  TabIndex="2"></telerik:RadTextBox>                        
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="font-weight: bold;" colspan="2">Naming scheme:</td>
                    </tr>
                     <tr>
                        <td>Naming mask:
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNamingMask" Text='<%# Bind("NamingMask") %>' runat="server"  TabIndex="9" Width="300">
                            </telerik:RadTextBox>
                        </td>
                    </tr>  
                    <tr>
                        <td>Separators:
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSeparators" Text='<%# Bind("Separators") %>' runat="server"  TabIndex="10" Width="70">
                            </telerik:RadTextBox>
                        </td>
                    </tr>  
                     <tr>
                        <td>Search mask:
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSearchMask" Text='<%# Bind( "SearchMask") %>' runat="server" TabIndex="5" Width="70">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                     <tr>
                    <td>Split multipage PDF:
                    </td>
                    <td >
                        <telerik:RadCheckBox ID="cbSplitPDF" runat="server" Text="" Checked="True" TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                    <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Ack-file generation</td>
                </tr>
                <tr>
                    <td>Generate ack-file:
                    </td>
                    <td >
                        <telerik:RadCheckBox ID="RadCheckBoxAckFile" runat="server" Text="" Checked="True" TabIndex="1"></telerik:RadCheckBox>                        
                         &nbsp;&nbsp;Success flag:&nbsp;
                        <telerik:RadNumericTextBox ID="RadNumericAckCode" runat="server" MinValue="0" MaxValue="9999" TabIndex="6"  Width="60" NumberFormat-DecimalDigits="0" >
                                            </telerik:RadNumericTextBox> 
                       
                    </td>         
                </tr>
                <tr>
                    <td>Ack-file folder:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="RadTextBoxAckFileFolder" Text="" runat="server"  TabIndex="9" Width="300">
                        </telerik:RadTextBox>
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Error notification</td>
                </tr>
                <tr>
                    <td>Notification email:
                    </td>
                    <td >
                        <telerik:RadCheckBox ID="cbSendMail" runat="server" Text="" Checked="True" TabIndex="1"></telerik:RadCheckBox>                        
                       
                    </td>         
                </tr>
                <tr>
                    <td>Receivers:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmailReceivers" Text="test@infralogic.dk" runat="server"  TabIndex="9" Width="300">
                        </telerik:RadTextBox>
                    </td>         
                </tr>
               
            </table>
         </td>
            <td>&nbsp;&nbsp;</td>     
        <td style="vertical-align:top">
            <table id="TableRight" border="0" class="module">
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Input source:</td>
                </tr>
                <tr>
                    <td style="width:125px;">Input type:
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="DropDownListType" runat="server"  AutoPostBack="true"  OnItemSelected="DropDownListType_SelectedIndexChanged">
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
                        <asp:Panel ID="PanelSMB" runat="server" Visible="true" GroupingText="SMB" BackColor="Snow" Width="600">
                                <table>
                                    <tr>
                                        <td style="width:120px;">Input folder:
                                        </td>
                                        <td colspan="4">
                                            <telerik:RadTextBox ID="txtInputFolder" runat="server"  TabIndex="4" Width="450">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td>Stable time:
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="RadNumericStableTime" runat="server" MinValue="0" MaxValue="999" TabIndex="6"  Width="50" NumberFormat-DecimalDigits="0" >
                                            </telerik:RadNumericTextBox>                   
                                        </td>
                                        <td style="width:100px;">Poll interval:
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="RadNumericPollTime" runat="server" MinValue="0" MaxValue="999" TabIndex="7"  Width="50" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>                   
                                        </td>
                                        <td style="width:100px;"></td>
                                    </tr>    
                                    <tr>
                                        <td>Use specific user:
                                        </td>
                                        <td colspan="4">
                                            <telerik:RadCheckBox ID="cbSpecificUser" runat="server" Text=""  TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>User name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtUserNameX" Text="" runat="server"  TabIndex="9" Width="100">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPasswordX" Text="" runat="server" TextMode="Password"  TabIndex="10" Width="100">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td></td>

                                    </tr>
                                </table>
</asp:Panel>
                        <asp:Panel ID="PanelFTP" runat="server" Visible="false" GroupingText="FTP/SFTP" BackColor="Snow" Width="600">
                        
                                <table>
                                    <tr>
                                        <td style="width:130px;">FTP Server:
                                        </td>
                                        <td> 
                                            <telerik:RadTextBox ID="txtFtpServer" runat="server" Text='<%# Bind("FTPserver") %>' TabIndex="4" Width="170">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td style="width:130px;">FTP folder:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpFolder" runat="server" Text='<%# Bind("FTPfolder") %>' TabIndex="4" Width="170">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>   
                                   
                                    <tr>
                                        <td>FTP User name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpUsername" Text='<%# Bind("FTPusername") %>' runat="server"  TabIndex="9" Width="170">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpPassword" Text='<%# Bind("FTPpassword") %>' runat="server" TextMode="Password"  TabIndex="10" Width="170">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Passive mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpPassive" runat="server" Text="" Checked='<%# Bind("FTPpasw") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                        <td>XCRC mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpXcrc" runat="server" Text="" Checked='<%# Bind("FTPxcrc") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                     <tr>
                                    <td>Encryption:
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadDropDownList ID="RadDropDownListEncryption" runat="server" SelectedValue='<%# Bind("FTPtls") %>' AutoPostBack="true" Width="300" >
                                           <Items>
                                            <telerik:DropDownListItem runat="server" Text="None (usually port 21)" Value="0" />
                                            <telerik:DropDownListItem runat="server" Text="FTPES Explicit SSL/TLS (usually port 21)" Value="1" />
                                            <telerik:DropDownListItem runat="server" Text="FTPS Implicit SSL/TLS (usually port 990)" Value="2" />
                                            <telerik:DropDownListItem runat="server" Text="SFTP - SSH (usually port 22)" Value="3" />
                                        </Items>
                            
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>
                        <asp:Panel ID="PanelEmail" runat="server" Visible="false" GroupingText="Email attachments" BackColor="Snow">
                        </asp:Panel>
                        <asp:Panel ID="PanelGoogle" runat="server" Visible="false" GroupingText="Google drive" BackColor="Snow">
                        </asp:Panel>
                        <asp:Panel ID="PanelS3" runat="server" Visible="false" GroupingText="Amazon S3" BackColor="Snow">
                        </asp:Panel>
                    </td>
                </tr>  
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Archiving/Input copy</td>
                </tr>
                <tr>
                    <td>Copy input files:
                    </td>
                    <td >
                        <telerik:RadCheckBox ID="cbArchive" runat="server" Text="" Checked="False" TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td>Copy folder:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCopyFolder" Text="" runat="server"  TabIndex="9" Width="440">
                        </telerik:RadTextBox>
                    </td>         
                </tr>
             </table>
            </td>
     </tr>
    <tr>
        <td class="title" style="font-weight: bold;" colspan="3">Regular expression parsing:</td>
    </tr>
    <tr>
        <td colspan="3">Use regular expressions:&nbsp;&nbsp;        
            <telerik:RadCheckBox ID="cbRegex" runat="server" Text="" Checked='<%# Bind("UseRegex") %>' TabIndex="1"></telerik:RadCheckBox>                        
        </td>         
    </tr>
    <tr>
        <td colspan="3">
            <telerik:RadGrid ID="RadGridRegex" runat="server" 
                    OnNeedDataSource="RadGridRegex_NeedDataSource"  OnItemCreated="RadGridRegex_ItemCreated" OnItemCommand="RadGridRegex_ItemCommand"
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small" Width="1050" >
                        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
                        <GroupingSettings ShowUnGroupButton="false" />
                        <MasterTableView EditMode="InPlace" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
                            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="Rank">
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new regular expression" ShowRefreshButton="false"  />                   
                            <Columns>                
                                <telerik:GridBoundColumn UniqueName="Rank"  DataField="Rank" HeaderText="Rank" DataType="System.Int32">
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="MatchExpression"  DataField="MatchExpression" HeaderText="Match expression" DataType="System.String">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="FormatExpression"  DataField="FormatExpression" HeaderText="Format expression" DataType="System.String"  >
                                    <HeaderStyle Width="250px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle Width="250px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Comment" DataField="Comment" HeaderText="Comment" DataType="System.String">
                                    <HeaderStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                  <telerik:GridEditCommandColumn EditText="Edit" HeaderText="Edit">
                                      <HeaderStyle Width="60px" />
                                  </telerik:GridEditCommandColumn>

                                 <telerik:GridButtonColumn  ConfirmText="Delete this regex?" ConfirmDialogType="RadWindow"
                                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete"  HeaderText="Delete">   
                                    <HeaderStyle Width="60px" />
                                </telerik:GridButtonColumn>
                                 <telerik:GridButtonColumn HeaderText="Up"  CommandName="Up" ButtonType="ImageButton" ImageUrl="Images/MoveUp.gif">   
                                    <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                      <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                                 <telerik:GridButtonColumn HeaderText="Down"  CommandName="Down" ButtonType="ImageButton" ImageUrl="Images/MoveDown.gif">   
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                     <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>

                            </Columns>                
                        </MasterTableView>
                    </telerik:RadGrid>
        </td> 
   </tr>

     <tr>
       
        <td colspan="3">Test input:&nbsp;&nbsp;<telerik:RadTextBox ID="RadTextBoxTestInput"  runat="server"  TabIndex="10" Width="280">
            </telerik:RadTextBox>
        
       &nbsp;&nbsp;Result:&nbsp;&nbsp;<telerik:RadTextBox ID="RadTextBoxTestResult"  runat="server"  TabIndex="10" Width="280" ReadOnly="true">
                            </telerik:RadTextBox>
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadButton ID="btnTestRegex" Text="Test rename expressions" runat="server" OnClick="btnTestRegex_Click"></telerik:RadButton>
           
       </td>
     </tr>
     <tr>
       <td align="center" colspan="3">&nbsp;</td>
     </tr>
     <tr>
      <td align="center" colspan="3">
            <telerik:RadButton ID="btnUpdate" Text="Insert" runat="server" OnClick="btnUpdate_Click"></telerik:RadButton>
           &nbsp;
            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
       </td>
    </tr>     
            </table>
        </div>
              
    </form>
</body>
</html>
