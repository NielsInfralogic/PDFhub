<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigInput.aspx.cs" Inherits="PDFhub.ConfigInput" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= RadGridInput.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("EditAddInput.aspx?InputID=" + id, "EditAddInput");
                return false;
            }

            function ShowInsertForm() {
                window.radopen("EditAddInput.aspx", "EditAddInput");
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
                window.radopen("EditAddInput.aspx?InputID=" + eventArgs.getDataKeyValue("InputID"), "EditAddInput");
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridInput" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="RadGridInput">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridInput" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

      <telerik:RadGrid ID="RadGridInput" runat="server" 
                    OnPreRender="RadGridInput_PreRender" OnNeedDataSource="RadGridInput_NeedDataSource"  OnItemCreated="RadGridInput_ItemCreated"
                    OnUpdateCommand="RadGridInput_UpdateCommand" OnDeleteCommand="RadGridInput_DeleteCommand"
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="EditForms" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="InputID">
            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new input queue"  />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
              <Columns>
                <telerik:GridImageColumn DataImageUrlFields="_StateImageUrl"  DataImageUrlFormatString="images/{0}.png" UniqueName="StateImageUrl" HeaderText="Enabled" DataType="System.String" ImageAlign="Middle" >
                    <HeaderStyle Width="40px" HorizontalAlign="Center" />
                    <ItemStyle Width="40px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridImageColumn>
             
                <telerik:GridBoundColumn UniqueName="_InputTypeStr"  DataField="_InputTypeStr" HeaderText="Type" DataType="System.String">
                    <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="InputName"  DataField="InputName" HeaderText="Name" DataType="System.String"  >
                    <HeaderStyle Width="180px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="118px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="InputPath" DataField="InputPath" HeaderText="Folder" DataType="System.String">
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                  <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Edit" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" ToolTip="Edit" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                 <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this input setup?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    <ItemStyle Width="80px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>
                
                <telerik:GridBoundColumn UniqueName="InputID"  DataField="InputID" HeaderText="" DataType="System.Int32">
                    <HeaderStyle Width="1px" />
                    <ItemStyle Width="1px"  />
                </telerik:GridBoundColumn>

            </Columns>                
            <EditFormSettings UserControlName="ConfigExportDetails.ascx" EditFormType="WebUserControl">
                <EditColumn UniqueName="EditCommandColumn1" >
                </EditColumn>
            </EditFormSettings>
             <CommandItemTemplate>
                <a href="#" onclick="return ShowInsertForm();">Add New Input</a>
            </CommandItemTemplate>

        </MasterTableView>
       
           <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <ClientEvents OnRowDblClick="RowDblClick" />
        </ClientSettings>
    </telerik:RadGrid>

         <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"  Style="z-index: 100000;">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddInput" runat="server" Title="Editing input" Height="700px"
                Width="1078px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
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
