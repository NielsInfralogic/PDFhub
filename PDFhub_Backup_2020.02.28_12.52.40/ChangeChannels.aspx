<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeChannels.aspx.cs" Inherits="PDFhub.ChangeChannels" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Change exports</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
    <style>
        table.jumbotron {
            margin-bottom: 10px;
            padding: 0.2em 1.0em 0.2em 1.0em;
            border: 1px solid transparent;
            background-color: #e9eaea;
            overflow: hidden;
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>

</head>

<body>
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
        <div style="padding: 10px 10px 10px 10px;background-color:ghostwhite;">
             <table  class="jumbotron"  style="width:480px;">
                <tr>
                    <td style="width:100px;">
                        <telerik:RadLabel ID="lblProduct" runat="server" Text="" CssClass="header-text" ></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table id="Table2" cellspacing="2" cellpadding="1" width="500" border="0" rules="none" style="border-collapse: collapse;">
                
                <tr>
                    <td>
                        <asp:Panel ID="ChannelTablePanel" runat="server" ScrollBars="Vertical" Height="440" BorderWidth="1" BorderStyle="Solid">
                            <telerik:RadCheckBoxList ID="RadCheckBoxListChannels" runat="server" AutoPostBack="false">
                                <Databindings DataTextField="Name" DataValueField="ChannelID" DataSelectedField="Selected" DataEnabledField="Enabled" />
                            </telerik:RadCheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>    
                    <td align="center">
                        <telerik:RadButton ID="btnUpdate" Text="Apply" runat="server"  OnClick="BtnUpdate_Click"></telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
                    </td>
                </tr>
                  <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
             
            <input runat="server" id="HiddenProductionID" type="hidden" value="" />
        </div>
    </form>
</body>
</html>
