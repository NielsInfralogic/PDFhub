<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="PDFhub.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Reporting" CssClass="header-text" Width="300" ></telerik:RadLabel>
            </td>
            <td style="float:right;"><asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
   <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;margin-left:20px;">
        <tr>
             <td style="width:120px;">Publisher</td>       
             <td style="width:200px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListPublisher" AutoPostBack="true" OnItemSelected="RadDropDownListPublisher_ItemSelected" > 
               </telerik:RadDropDownList>
            </td>

             <td style="width:120px;">Product</td>       
             <td style="width:200px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListProduct" AutoPostBack="false"  > 
               </telerik:RadDropDownList>
            </td>

             <td style="width:140px;">Show page history</td>   
            <td style="width:50px;">
                <telerik:RadCheckBox ID="cbShowPageHistory" runat="server" Text=""></telerik:RadCheckBox>
            </td>

           
             <td> <telerik:RadButton ID="RadButtonGenerate" runat="server" Text="Generate report" Width="140" OnClick="RadButtonGenerate_Click"></telerik:RadButton></td>             
            <td></td>
        </tr>
         <tr>
             <td style="width:120px;">Publication date</td>       
             <td style="width:200px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListPubDate" AutoPostBack="false" >
               </telerik:RadDropDownList>
            </td>
             <td style="width:120px;">Export</td>       
             <td style="width:200px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListExport" AutoPostBack="false"  > 
               </telerik:RadDropDownList>
            </td>
            <td style="width:140px;"></td>   
            <td style="width:50px;"></td>
            
              <td> <telerik:RadButton ID="RadButtonSave" runat="server" Text="Save to Excel" Width="140"  OnClientClicked="exportFile" Enabled="false" ></telerik:RadButton></td>
            <td></td>
                  
         </tr>
        <tr>
             <td colspan="7">&nbsp;</td>
        </tr>
         <tr>
             <td colspan="7">
                 <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" ColumnsCount="20" Width="1200"></telerik:RadSpreadsheet>
             </td>
        </tr>
    </table>
    <script type="text/javascript">
        function exportFile() {
            var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
            spreadsheet.saveAsExcel();
        }
 
        function importFile(sender, args) {
            var file = null;
            if (args.get_file) {
                file = args.get_file();
            }
            else if (!(Telerik.Web.Browser.ie && Telerik.Web.Browser.version == "9")) {
                file = args.get_fileInputField().files[0];
            }
            if (file) {
                var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                spreadsheet.fromFile(file);
            }
 
            $telerik.$(args.get_row()).remove();
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</asp:Content>
