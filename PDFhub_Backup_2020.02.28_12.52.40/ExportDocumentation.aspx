<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportDocumentation.aspx.cs" Inherits="PDFhub.ExportDocumentation" %>

<<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Export documentation</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
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
        <div>
             <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0"  style="border-collapse: collapse;background-color:ghostwhite;margin: 10px 10px 10px 10px;">   
                <tr>
                    <td style="vertical-align:top;width:220px;">Used in publications:
                    </td>
                        <td style="vertical-align:top">
                            <telerik:RadListBox ID="RadListBoxPublications" runat="server" Height ="300" Width="300"></telerik:RadListBox>
                     </td>
                 </tr>
             </table>
       </div>
    </form>
</body>
</html>