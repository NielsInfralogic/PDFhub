<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigPackages.aspx.cs" Inherits="PDFhub.ConfigPackages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Publication packages" CssClass="header-text" Width="300"  ></telerik:RadLabel>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
 
            function onPopUpShowing(sender, args) {
                args.get_popUp().className += " popUpEditForm";
            }
        </script>
    </telerik:RadCodeBlock>
     <telerik:RadGrid ID="RadGridPackage" runat="server" 
                    OnNeedDataSource="RadGridPackage_NeedDataSource" 
                    OnItemCreated="RadGridPackage_ItemCreated" 
                    OnItemCommand="RadGridPackage_ItemCommand"                    
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="InPlace"  AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="PackageID">
            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new package"  />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
            <Columns>

                <telerik:GridBoundColumn UniqueName="Name"  DataField="Name" HeaderText="Package name" DataType="System.String"  >
                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="ProductAlias" DataField="ProductAlias" HeaderText="Product" DataType="System.String">
                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="SectionIndex" DataField="SectionIndex" HeaderText="Section Index" DataType="System.Int32">
                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                   <telerik:GridBoundColumn UniqueName="ConditionStr" DataField="ConditionStr" HeaderText="ConditionStr" DataType="System.String">
                    <HeaderStyle Width="250px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="250px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                  <telerik:GridBoundColumn UniqueName="Comment" DataField="Comment" HeaderText="Comment" DataType="System.String">
                    <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                  <telerik:GridEditCommandColumn EditText="Edit" HeaderText="Edit" >
                    <HeaderStyle Width="80px"  />
                    <ItemStyle Width="80px" HorizontalAlign="Center"/>
                  </telerik:GridEditCommandColumn>

                 <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this input setup?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="80px"  />
                    <ItemStyle Width="80px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

                <telerik:GridBoundColumn UniqueName="PackageID"  DataField="PackageID" HeaderText="" DataType="System.Int32">
                    <HeaderStyle Width="1px" />
                    <ItemStyle Width="1px"  />
                </telerik:GridBoundColumn>

            </Columns>                
           
        </MasterTableView>
        <ClientSettings>
            <ClientEvents OnRowDblClick="RowDblClick" OnPopUpShowing="onPopUpShowing" />
        </ClientSettings>
    </telerik:RadGrid>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div class="center-div-list">
        <table style="width:100%;">
            <tr>
                <td style="width:100px;">
                    <asp:Label ID="Label3" runat="server" Text="Updated"></asp:Label></td>
                <td style="width:300px;">
                    <asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label></td>
                <td>
                    <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
