<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigExport.aspx.cs" Inherits="PDFhub.ConfigExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= RadGridExport.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("EditAddExport.aspx?ChannelID=" + id, "EditAddExport");
                return false;
            }

            function ShowInsertForm() {
                window.radopen("EditAddExport.aspx", "EditAddExport");
                return false;
            }

            function ShowDocForm(id, rowIndex) {
                var grid = $find("<%= RadGridExport.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);

                window.radopen("ExportDocumentation.aspx?ChannelID=" + id, "ExportDoc");
                return false;
            }
            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
                else {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RebindAndNavigate");
                }
            }
            function RowDblClick(sender, eventArgs) {
                window.radopen("EditAddExport.aspx?ChannelID=" + eventArgs.getDataKeyValue("ChannelID"), "EditAddExport");
            }
        </script>
    </telerik:RadCodeBlock>
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridExport" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridExport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridExport" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
       <telerik:RadGrid ID="RadGridExport" runat="server" 
                   OnNeedDataSource="RadGridExport_NeedDataSource"  OnItemDataBound="RadGridExport_ItemDataBound" 
                    OnItemCreated="RadGridExport_ItemCreated"
                     OnDeleteCommand="RadGridExport_DeleteCommand"  
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="EditForms" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="ChannelID">
         <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new export queue"  AddNewRecordImageUrl="Images/AddRecord.gif"   />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
            <Columns>
                <telerik:GridImageColumn DataImageUrlFields="_StateImageUrl" DataImageUrlFormatString="images/{0}.png" UniqueName="StateImageUrl" HeaderText="Enabled" DataType="System.String" ImageAlign="Middle" >
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridImageColumn>

                <telerik:GridBoundColumn UniqueName="Name"  DataField="Name" HeaderText="Name" DataType="System.String"  >
                    <HeaderStyle Width="180px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="118px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn UniqueName="_ChannelType"  DataField="_ChannelType" HeaderText="Protocol" DataType="System.String">
                    <HeaderStyle Width="120px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="120px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="ChannelNameAlias"  DataField="ChannelNameAlias" HeaderText="Alias" DataType="System.String"  >
                    <HeaderStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_PDFType" DataField="_PDFType" HeaderText="PDF type" DataType="System.String">
                    <HeaderStyle Width="120px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="120px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="MergedPDF" DataField="MergedPDF" HeaderText="Merged PDF" DataType="System.Boolean">
                    <HeaderStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_OutputUrl" DataField="_OutputUrl" HeaderText="Output URL" DataType="System.String">
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                  <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Edit" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" ToolTip="Edit" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                 <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this export setup?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="80px"  HorizontalAlign="Center"  />
                    <ItemStyle Width="80px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

                 <telerik:GridTemplateColumn UniqueName="TemplateDocColumn" HeaderText="Usage" HeaderStyle-Width="90" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="DocButton" runat="server" ToolTip="Doc" ImageUrl="Images/ExportToExcel.gif" ImageAlign="Middle"></asp:ImageButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                  <telerik:GridBoundColumn UniqueName="ChannelID"  DataField="ChannelID" HeaderText="" DataType="System.Int32">
                    <HeaderStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="1px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
            </Columns>                
            <EditFormSettings UserControlName="ConfigExportDetails.ascx" EditFormType="WebUserControl">
                <EditColumn UniqueName="EditCommandColumn1" >
                </EditColumn>
            </EditFormSettings>
             <CommandItemTemplate>
                <a href="#" onclick="return ShowInsertForm();">Add New Export</a>
            </CommandItemTemplate>
        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <ClientEvents OnRowDblClick="RowDblClick" />
        </ClientSettings>
    </telerik:RadGrid>
     <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"  Style="z-index: 100000;">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddExport" runat="server" Title="Editing export" Height="720px"
                Width="1105px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
              <telerik:RadWindow RenderMode="Lightweight" ID="ExportDoc" runat="server" Title="Documentation for export" Height="500px"
                Width="800px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true"   ShowOnTopWhenMaximized ="true">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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
