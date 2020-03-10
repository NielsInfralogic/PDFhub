<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Testregex.aspx.cs" Inherits="PDFhub.Testregex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Test regular expression match/rename</title>
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
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                    else
                if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow; //IE (and Moz as well)
 
                return oWindow;
            }
 
            function CancelEdit() {
                GetRadWindow().close();
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
                        <telerik:RadLabel runat="server" Text="Regex test" CssClass="header-text" ></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table id="Table2" cellspacing="2" cellpadding="1" width="500" border="0" rules="none" style="border-collapse: collapse;">
                
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Input name" CssClass="header-text" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="RadTextBoxInput" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <telerik:RadButton ID="RadButton1" Text="Test" runat="server"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Result" CssClass="header-text" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="RadTextBoxResult" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>    
                    <td>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnCancel" Text="Close" runat="server" OnClientClicked="CancelEdit"></telerik:RadButton>
                    </td>
                </tr>
                  <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
            </table>
             
        </div>
    </form>
</body>
</html>
