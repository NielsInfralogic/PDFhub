<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResendToChannel.aspx.cs" Inherits="PDFhub.ResendToChannel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Resend pages</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
    <style>
        table.jumbotron {
            margin-bottom: 10px;
            padding: 0.2em 1.0em 0.2em 1.0em;
            border: 1px solid transparent;
            background-color: #e9eaea;
            overflow: hidden;
            margin-left: 5px;
            margin-right: 5px;
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
                    <td colspan="2">
                        <asp:Panel ID="ChannelTablePanel" runat="server" ScrollBars="Vertical" Height="235" BorderWidth="1" BorderStyle="Solid" > 
                            <telerik:RadCheckBoxList ID="RadCheckBoxListChannels" runat="server" AutoPostBack="false" >
                                <Databindings DataTextField="Name" DataValueField="ChannelID" DataSelectedField="Selected" DataEnabledField="Enabled" />
                            </telerik:RadCheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;">
                        <telerik:RadLabel runat="server" Text="Resending" CssClass="header-text" ></telerik:RadLabel>
                        </td>
                        <td>

                    <telerik:RadRadioButtonList ID="RadRadioButtonListResent" runat="server" Direction="Vertical" AutoPostBack="false">
                        <Items>
                            <telerik:ButtonListItem Text="Resent all pages" Enabled="true" Selected="true" Value="0" />
                            <telerik:ButtonListItem Text="Resent only missing pages" Enabled="true" Selected="false" Value="1" />
                        </Items>
                    </telerik:RadRadioButtonList>
                        </td>
                </tr>
                <tr>
                   <td style="vertical-align:top;">
                        <telerik:RadLabel runat="server" Text="Reprocessing" CssClass="header-text" ></telerik:RadLabel>                     
                       </td>
                    <td>
                    <telerik:RadRadioButtonList ID="RadRadioButtonReprocess" runat="server" Direction="Vertical" AutoPostBack="false">
                        <Items>
                            <telerik:ButtonListItem Text="No action" Enabled="true" Selected="true" Value="0" />
                            <telerik:ButtonListItem Text="Reprocess all pages" Enabled="true" Selected="false" Value="2" />
                            <telerik:ButtonListItem Text="Reprocess missing pages" Enabled="true" Selected="false" Value="2" />
                        </Items>
                    </telerik:RadRadioButtonList>
                    </td>
                </tr>
                 <tr>
                   <td >
                        <telerik:RadLabel runat="server" Text="Release" CssClass="header-text" ></telerik:RadLabel>                       
                   </td>
                   <td><telerik:RadCheckBox ID="RadCheckBoxRelease" runat="server" Text="Release now"></telerik:RadCheckBox></td>
                </tr>
                <tr>
                   <td >
                        <telerik:RadLabel runat="server" Text="Triggers" CssClass="header-text" ></telerik:RadLabel>                       
                   </td>
                   <td><telerik:RadCheckBox ID="cbResendTrigger" runat="server" Text="Re-send trigger(s)"></telerik:RadCheckBox></td>
                </tr>
                 <tr>
                    <td colspan="2">&nbsp;<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                </tr>
               
                <tr>    
                    <td colspan="2" align="center">
                        <telerik:RadButton ID="btnUpdate" Text="Apply" runat="server"  OnClick="BtnUpdate_Click"></telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
                    </td>
                </tr>
                 
            </table>             
            <input runat="server" id="HiddenProductionID" type="hidden" value="" />
        </div>
    </form>
</body>
</html>
