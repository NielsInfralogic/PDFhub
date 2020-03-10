<%@ Page Title="User Management" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="PDFhub.UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="User Management" CssClass="header-text"  ></telerik:RadLabel>
            </td>
        </tr>
    </table>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= RadGridUsers.ClientID %>");

                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);

                window.radopen("EditAddUser.aspx?UserName=" + id, "EditAddUser");
                return false;
            }

            function ShowInsertForm() {
                window.radopen("EditAddUser.aspx", "EditAddUser");
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
                window.radopen("EditAddUser.aspx?UserName=" + eventArgs.getDataKeyValue("UserName"), "EditAddUser");
            }
        </script>
    </telerik:RadCodeBlock>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
     

     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridUsers" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="RadGridUsers">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridUsers" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

  <telerik:RadGrid ID="RadGridUsers" runat="server" 
            OnNeedDataSource="RadGridUsers_NeedDataSource"  OnItemCreated="RadGridUsers_ItemCreated"
            OnDeleteCommand="RadGridUsers_DeleteCommand"
            AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
            AllowFilteringByColumn="false"
            GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="EditForms" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="UserName">
            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new user"  />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
            <Columns>
                  <telerik:GridImageColumn DataImageUrlFields="_EnabledImageUrl"  DataImageUrlFormatString="images/{0}.png" UniqueName="_EnabledImageUrl" HeaderText="Account Enabled" DataType="System.String" ImageAlign="Middle" >
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridImageColumn>

                <telerik:GridBoundColumn UniqueName="UserName"  DataField="UserName" HeaderText="User name" DataType="System.String"  >
                    <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>


                <telerik:GridBoundColumn UniqueName="_UserGroup" DataField="_UserGroup" HeaderText="Usergroup" DataType="System.String">
                    <HeaderStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="FullName" DataField="FullName" HeaderText="FullName" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Email" DataField="Email" HeaderText="Email" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_Publishers" DataField="_Publishers" HeaderText="Publisher list" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_Publications" DataField="_Publications" HeaderText="Publication list" DataType="System.String">
                    <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle   HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
             
                <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Edit" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" Tooltip="Edit" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
  
                <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this user?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="70px" HorizontalAlign="Center" />
                    <ItemStyle Width="70px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

            </Columns>           

              <CommandItemTemplate>
                <a href="#" onclick="return ShowInsertForm();">Add New User</a>
            </CommandItemTemplate>
            
             <EditFormSettings UserControlName="ConfigUserDetails.ascx" EditFormType="WebUserControl">
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
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddUser" runat="server" Title="Editing user" Height="610px"
                Width="820px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
            
        </Windows>
    </telerik:RadWindowManager>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</asp:Content>

