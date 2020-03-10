<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigHome.aspx.cs" Inherits="PDFhub.ConfigHome" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align:center;" >
           <table style="width:80%;">
                   <tr style="height: 120px">
               
                        <td >
                            <telerik:RadButton ID="RadButtonFileInput" ButtonType="LinkButton" runat="server" Text="File Input Queues" Font-Size="Large"  Icon-PrimaryIconUrl="images/Folder-add-50.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50"  Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20"  Height="100" Width="300" NavigateUrl="ConfigInput.aspx"  ></telerik:RadButton>
                        </td>
                          <td>
                             <telerik:RadButton ID="RadButtonImport" ButtonType="LinkButton" runat="server" Text="Plan Import" Font-Size="Large"  Icon-PrimaryIconUrl="images/Planning-50.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigImport.aspx"  ></telerik:RadButton>
                         </td>
                          <td>
                            <telerik:RadButton ID="RadButtonUsers" ButtonType="LinkButton" runat="server" Text="Users" Font-Size="Large"  Icon-PrimaryIconUrl="images/User-50.png" Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="UserManagement.aspx" ></telerik:RadButton > 
                        </td>
                </tr>

                   <tr style="height: 120px">
                    <td>
                            <telerik:RadButton ID="RadButtonProcessing" ButtonType="LinkButton" runat="server" Text="Processing rules" Font-Size="Large"  Icon-PrimaryIconUrl="images/Pdf-48.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigProcessing.aspx" ></telerik:RadButton>
                       </td>
                          <td>
                             <telerik:RadButton ID="RadButtonProducts" ButtonType="LinkButton" runat="server" Text="Products" Font-Size="Large"  Icon-PrimaryIconUrl="images/Document-48.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigPublication.aspx"></telerik:RadButton>
                    </td>
                          <td>
                             <telerik:RadButton ID="RadButtonProductPacks" ButtonType="LinkButton" runat="server" Text="Product packages" Font-Size="Large"  Icon-PrimaryIconUrl="images/Package-48.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigPackages.aspx"></telerik:RadButton>
                    </td>
                </tr>
                   <tr style="height: 120px">

                        <td>
                            <telerik:RadButton ID="RadButtonExports" ButtonType="LinkButton" runat="server" Text="Exports" Font-Size="Large"  Icon-PrimaryIconUrl="images/Cloud-48.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigExport.aspx"></telerik:RadButton>
                        </td>
                        <td>
                             <telerik:RadButton ID="RadButtonMaint"  ButtonType="LinkButton" runat="server" Text="Maintenance" Font-Size="Large"  Icon-PrimaryIconUrl="images/Process-50.png"  Icon-PrimaryIconHeight="50" Icon-PrimaryIconWidth="50" Icon-PrimaryIconLeft="30"  Icon-PrimaryIconTop="20" Height="100" Width="300" NavigateUrl="ConfigMaint.aspx"></telerik:RadButton>
                        </td>
                        <td></td>
                </tr>
          
        </table>
        </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server"/>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server"/>