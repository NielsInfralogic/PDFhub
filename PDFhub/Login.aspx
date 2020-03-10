<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PDFhub.Login" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>PDF Hub login</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />
</head>

<body style="background-color:ghostwhite;">
    <form id="form1" runat="server">        
        <table id="Table2" cellspacing="2" cellpadding="1" width="100%"  border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;margin: 10px 10px 10px 10px; height:100%;width:100%;">   
            <tr><td>
            <table style="text-align:center;">
                <tr>
                    <td colspan="3" style="text-align:center;">
                        <asp:Label ID="Label1" runat="server" Text="PDF Hub"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>

                <tr>
                    <td colspan="3" style="text-align:center;">
                        <asp:Label ID="Label2" runat="server" Text="Login"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Username"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                        <
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:center;">
                        <telerik:RadButton ID="btnLogin" runat="server" Text="Login"></telerik:RadButton>                      
                    </td>
                </tr>

            </table>
        </td></tr></table>                
    </form>
</body>
</html>
