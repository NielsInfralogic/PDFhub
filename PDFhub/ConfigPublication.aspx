<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigPublication.aspx.cs" Inherits="PDFhub.ConfigPublication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= RadGridPublication.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("EditAddPublication.aspx?PublicationID=" + id, "EditAddPublication");
                return false;
            }
            function ShowInsertForm() {
                window.radopen("EditAddPublication.aspx", "EditAddPublication");
                return false;
            }

            function ShowDocForm(id, rowIndex) {
                var grid = $find("<%= RadGridPublication.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
                window.radopen("PublicationDocumentation.aspx?PublicationID=" + id, "PublicationDoc");
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
                window.radopen("EditAddPublication.aspx?PublicationID=" + eventArgs.getDataKeyValue("PublicationID"), "EditAddPublication");
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
         <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridPublication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridPublication">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridPublication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
      <telerik:RadGrid ID="RadGridPublication" runat="server" 
                   OnNeedDataSource="RadGridPublication_NeedDataSource"  OnItemCreated="RadGridPublication_ItemCreated"
                   OnDeleteCommand="RadGridPublication_DeleteCommand"
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="EditForms" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="PublicationID">
         <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new publication" AddNewRecordImageUrl="Images/AddRecord.gif"   />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>                   
            <Columns>
                <telerik:GridBoundColumn UniqueName="Name"  DataField="Name" HeaderText="Product" DataType="System.String"  >
                    <HeaderStyle Width="180px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="118px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn UniqueName="InputAlias"  DataField="InputAlias" HeaderText="Alias" DataType="System.String"  >
                    <HeaderStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_PublisherName" DataField="_PublisherName" HeaderText="Publisher" DataType="System.String">
                    <HeaderStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="DefaultApprove" DataField="DefaultApprove" HeaderText="Approve" DataType="System.Boolean">
                    <HeaderStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_ReleaseTimeDays" DataField="_ReleaseTimeDays" HeaderText="Release time" DataType="System.String">
                    <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                 <telerik:GridBoundColumn UniqueName="_DefaultChannels" DataField="_DefaultChannels" HeaderText="Default exports" DataType="System.String">
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Edit" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" Tooltip="Edit" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                 <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this export setup?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="70px"  HorizontalAlign="Center"/>
                    <ItemStyle Width="70px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

                 <telerik:GridTemplateColumn UniqueName="TemplateDocColumn" HeaderText="Documentation" HeaderStyle-Width="90" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="DocButton" runat="server" ToolTip="Doc" ImageUrl="Images/ExportToExcel.gif" ImageAlign="Middle"></asp:ImageButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                   <telerik:GridBoundColumn UniqueName="PublicationID"  DataField="PublicationID" HeaderText="" DataType="System.Int32">
                    <HeaderStyle Width="1px"  />
                    <ItemStyle Width="1px" />
                </telerik:GridBoundColumn>

            </Columns>      
            
            <CommandItemTemplate>
                <a href="#" onclick="return ShowInsertForm();">Add New Publication</a>
            </CommandItemTemplate>
            <EditFormSettings UserControlName="ConfigPublicationDetails.ascx" EditFormType="WebUserControl">
                <EditColumn UniqueName="EditCommandColumn1" >
                </EditColumn>
            </EditFormSettings>
        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <ClientEvents OnRowDblClick="RowDblClick" />
        </ClientSettings>
    </telerik:RadGrid>
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" Style="z-index: 100000">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddPublication" runat="server" Title="Editing publication" Height="690px"
                Width="1060px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
              <telerik:RadWindow RenderMode="Lightweight" ID="PublicationDoc" runat="server" Title="Documantation for publication" Height="500px"
                Width="800px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true"  ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
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
