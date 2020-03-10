<%@ Page Title="Edit/Add user" Language="C#" AutoEventWireup="true" CodeBehind="EditAddUser.aspx.cs" Inherits="PDFhub.EditAddUser" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Editing User</title>
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
                    <td class="title" style="font-weight: bold;" colspan="4">User details:</td>
                </tr>

                <tr>
                    <td style="width:150px;">Account enabled:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbAccountEnabled" runat="server" Text="" Checked='<%# Bind("AccountEnabled") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td >User name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtUserName" runat="server" Text='<%# Bind("UserName") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                <tr>
                    <td>Password:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtPassword" runat="server" Text='<%# Bind("Password") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td>Full name:
                    </td>
                    <td colspan="3">
                       <telerik:RadTextBox ID="txtFullName" Text='<%# Bind("FullName") %>' runat="server"  TabIndex="10" Width="200">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Email:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                    <tr>
                    <td>User group:
                    </td>
                     <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownUserGroup" runat="server" SelectedValue='<%# Bind("_UserGroup") %>' AutoPostBack="false" Width="200" >
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                
                                      
              </table>           
             </td>
             <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
              <td style="vertical-align:top;">
                <table style="padding-left: 20px;border:0px" class="module">

                    <tr align="top">
                         <td class="title" style="font-weight: bold;" >Publication access:</td>
                    </tr>
                    <tr align="top">
                        <td colspan="2">
                            <asp:Panel ID="PublicationTablePanel" runat="server" ScrollBars="Vertical" Height="440" BorderWidth="1">
                                 <telerik:RadCheckBoxList ID="RadCheckBoxListPublications" runat="server" AutoPostBack="false">
                                    <Databindings DataTextField="Name" DataValueField="Id" DataSelectedField="Selected" DataEnabledField="Enabled" />
                                </telerik:RadCheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
              </td>

                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
              <td style="vertical-align:top;">
                <table style="padding-left: 20px;border:0px" class="module">

                    <tr align="top">
                         <td class="title" style="font-weight: bold;" >Publisher access:</td>
                    </tr>
                    <tr align="top">
                        <td colspan="2">
                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="350" BorderWidth="1">
                                 <telerik:RadCheckBoxList ID="RadCheckBoxListPublishers" runat="server" AutoPostBack="false">
                                    <Databindings DataTextField="Name" DataValueField="Id" DataSelectedField="Selected" DataEnabledField="Enabled" />
                                </telerik:RadCheckBoxList>
                            </asp:Panel>
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
        <input runat="server" id="HiddenUserName" type="hidden" value="" />
</div>
</form>
</body>
</html>