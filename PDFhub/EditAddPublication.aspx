<%@ Page Title="Edit publication" Language="C#" AutoEventWireup="true" CodeBehind="EditAddPublication.aspx.cs"  Inherits="PDFhub.EditAddPublication" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Editing Publication</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
   <div style="padding: 10px 10px 10px 10px;background-color:ghostwhite;">
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

   <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;">
    <tr>
        <td>
          <table>
            <tr style="vertical-align:top;">
              <td style="vertical-align:top;">
               <table id="Table3" border="0" class="module">
                 <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Naming:</td>
                </tr>
                <tr>
                    <td style="width:265px;">Publication name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtPublicationName" runat="server" Text='<%# Bind("Name") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                <tr>
                    <td>Short name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtInputAlias" runat="server" Text='<%# Bind("InputAlias") %>' Width="100"  TabIndex="2"></telerik:RadTextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td>Output name:
                    </td>
                    <td colspan="3">
                       <telerik:RadTextBox ID="txtOutputAlias" Text='<%# Bind("OutputAlias") %>' runat="server"  TabIndex="10" Width="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Alternative name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtExtendedAlias" runat="server" Text='<%# Bind("ExtendedAlias") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                    <tr>
                    <td>VisioLink name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtExtendedAlias2" runat="server" Text='<%# Bind("ExtendedAlias2") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                 <tr>
                    <td>Annum text:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtAnnumText" runat="server" Text='<%# Bind("AnnumText") %>' Width="100"  TabIndex="2"></telerik:RadTextBox>                        
                    </td>
                </tr>

                    <tr>
                    <td>Publisher:
                    </td>
                    <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownListPublisher" runat="server" SelectedValue='<%# Bind("_PublisherName") %>' AutoPostBack="true" >
                        </telerik:RadDropDownList>
                    </td>

                    
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Release time:</td>
                </tr>
                <tr>
                    <td >No release time used for:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbNoReleaseTimeUsed" runat="server" Text="Low"  TabIndex="1" AutoPostBack="false" ></telerik:RadCheckBox> 
                        &nbsp;
                        <telerik:RadCheckBox ID="cbNoReleaseTimeUsedHighres" runat="server" Text="High" AutoPostBack="false" TabIndex="1"></telerik:RadCheckBox> 
                        &nbsp;
                        <telerik:RadCheckBox ID="cbNoReleaseTimeUsedPrint" runat="server" Text="Print" AutoPostBack="false" TabIndex="1"></telerik:RadCheckBox> 
                    </td>         
                </tr>


                <tr>
                    <td>Time:
                    </td>
                    <td colspan="3">
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTime"  TabIndex="8" DbSelectedDate='<%# Bind("_ReleaseTime") %>'></telerik:RadTimePicker>
                    </td>
                </tr>
                    <tr>
                    <td>Days before pubdate (+/-):
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericReleaseDays" runat="server" MinValue="-999" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "ReleaseDays") %>' Width="40" NumberFormat-DecimalDigits="0" >
                                            </telerik:RadNumericTextBox> 
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Other options:</td>
                </tr>
                <tr>
                    <td>Priority:
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericTextBoxPriority" runat="server" MinValue="0" MaxValue="100" TabIndex="6" DbValue='<%# Bind( "DefaultPriority") %>' Width="50" NumberFormat-DecimalDigits="0"  >
                                            </telerik:RadNumericTextBox> &nbsp;&nbsp;(100: highest)
                    </td>
                </tr>
                <tr>
                    <td >Allow pages without plan:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbAllowUnplanned" runat="server" Text="" Checked='<%# Bind("AllowUnplanned") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>


                <tr>
                    <td >Requires page approval:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbDefaultApprove" runat="server" Text="" Checked='<%# Bind("DefaultApprove") %>' AutoPostBack="false" TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td >Change output pubdate:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbPubdateMove" runat="server" Text="" Checked='<%# Bind("PubdateMove") %>' AutoPostBack="false" TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                 <tr>
                    <td >Pubdate move (days):
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericTextBoxPubdateMoveDays" runat="server" MinValue="-999" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "PubdateMoveDays") %>' Width="50" NumberFormat-DecimalDigits="0"  >
                                            </telerik:RadNumericTextBox>  
                    </td>         
                </tr>
                 <tr>
                    <td >Delete plan after (days):
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericTextBoxAutoPurgeKeepDays" runat="server" MinValue="0" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "AutoPurgeKeepDays") %>' Width="50" NumberFormat-DecimalDigits="0"  >
                                            </telerik:RadNumericTextBox>&nbsp; (0: global setting)
                    </td>         
                </tr>

                   </tr>
                 <tr>
                    <td >Delete sent files after:
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericTextBoxAutoPurgeKeepDaysArchive" runat="server" MinValue="0" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "AutoPurgeKeepDaysArchive") %>' Width="50" NumberFormat-DecimalDigits="0"  >
                                            </telerik:RadNumericTextBox>&nbsp;(for archive exports)
                    </td>         
                </tr>

                   
                                                                       
              </table>           
             </td>
             <td>&nbsp;</td>
              <td style="vertical-align:top;">
              <table style="padding-left: 20px;border:0px" class="module">
                <tr align="top">
                     <td class="title" style="font-weight: bold;" >Exports:</td>
                </tr>
                <tr align="top">
                    <td colspan="2">
                        <asp:Panel ID="ChannelTablePanel" runat="server" ScrollBars="Vertical" Height="440" BorderWidth="1">
                            <asp:GridView ID="GridViewPublicationChannels" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" Font-Size="Smaller" HeaderStyle-BackColor="Snow" >
                            <Columns>
                                 <asp:TemplateField HeaderText = "Use" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                       <asp:CheckBox ID="cbUse" runat="server"  Checked='<%# Eval("Use") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChannelName" HeaderText="ChannelName" ItemStyle-Width="180" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText = "Trigger" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrigger" runat="server" Text='<%# Eval("_Trigger") %>' Visible = "false" />
                                        
                                        <asp:DropDownList ID="ddlTrigger" runat="server">
                                            <asp:ListItem Text="None" Value="None" ></asp:ListItem>
                                            <asp:ListItem Text="Manuel" Value="Manuel" ></asp:ListItem>
                                            <asp:ListItem Text="Email" Value="Email" ></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PubDate move (days)" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                   <ItemTemplate>
                                    <asp:TextBox ID="txtPubDateMoveDays" runat="server" Text='<%# Eval("PubDateMoveDays") %>' Width="20" ></asp:TextBox>
                                   </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Release delay (hours)" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                   <ItemTemplate>
                                    <asp:TextBox ID="txtReleaseDelay" runat="server" Text='<%# Eval("ReleaseDelay") %>' Width="20"></asp:TextBox>
                                   </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText = "Send plan" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                       <asp:CheckBox ID="cbSendPlan" runat="server"  Checked='<%# Eval("SendPlan") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:BoundField DataField="ChannelID" HeaderText="" ItemStyle-Width="1" Visible="true" />
                            </Columns>
                            </asp:GridView>
                        </asp:Panel>
                     </td>
                    </tr>
                  <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Reporting:</td>
                </tr>
                <tr>
                    <td >Send email-report:
                    </td>
                    <td colspan="1">
                        <telerik:RadCheckBox ID="cbSendReport" runat="server" Text="" Checked='<%# Bind("SendReport") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td >Email receiver(s):
                    </td>
                    <td colspan="1">
                        <telerik:RadTextBox ID="txtEmailRecipient" runat="server" Text='<%# Bind("EmailRecipient") %>' Width="250"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>         
                </tr>   
                  </table>
              </td>
              </tr>
                
            </table>
         </td>
     </tr>
       <tr><td>&nbsp;<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></td></tr>
     <tr>    
       <td align="center">
            <telerik:RadButton ID="btnUpdate" Text="Insert" runat="server"  OnClick="btnUpdate_Click"></telerik:RadButton>
           &nbsp;
            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
       </td>
     </tr>
     <tr>
       <td>&nbsp;</td>         
    </tr>
</table>
        <asp:Label id="InjectScript" runat="server"></asp:Label>
</div>
</form>
</body>
</html>