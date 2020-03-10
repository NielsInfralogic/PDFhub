<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigImport.aspx.cs" Inherits="PDFhub.ConfigImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td >
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Plan Imports" CssClass="header-text"   ></telerik:RadLabel>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function ShowEditForm(id, rowIndex) {
                        var grid = $find("<%= RadGridImport.ClientID %>");

                        var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                        grid.get_masterTableView().selectItem(rowControl, true);

                        window.radopen("EditAddImport.aspx?ImportID=" + id, "EditAddImport");
                        return false;
                    }

                    function ShowInsertForm() {
                        window.radopen("EditAddImport.aspx", "EditAddImport");
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
                        window.radopen("EditAddImport.aspx?ImportID=" + eventArgs.getDataKeyValue("ImportID"), "EditAddImport");
            }

        </script>
    </telerik:RadCodeBlock>
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridImport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

     <telerik:RadGrid ID="RadGridImport" runat="server" 
                    OnNeedDataSource="RadGridImport_NeedDataSource"  
                    OnItemCreated="RadGridImport_ItemCreated"
                    OnDeleteCommand="RadGridImport_DeleteCommand"
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                    AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView EditMode="EditForms" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="ImportID">
         <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new plan import queue"  />
         <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
            <Columns>
                  <telerik:GridImageColumn DataImageUrlFields="_EnabledImageUrl"  DataImageUrlFormatString="images/{0}.png" UniqueName="_EnabledImageUrl" HeaderText="Enabled" DataType="System.String" ImageAlign="Middle" >
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridImageColumn>

                <telerik:GridBoundColumn UniqueName="Name"  DataField="Name" HeaderText="Import name" DataType="System.String"  >
                    <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="_ImportType" DataField="_ImportType" HeaderText="Type" DataType="System.String">
                    <HeaderStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="InputFolder" DataField="InputFolder" HeaderText="Input folder" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="DoneFolder" DataField="DoneFolder" HeaderText="Done folder" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="ErrorFolder" DataField="ErrorFolder" HeaderText="Error folder" DataType="System.String">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
              
                  <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="Edit" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" ToolTip="Edit" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                 <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this input setup?" ConfirmDialogType="RadWindow"
                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                    <HeaderStyle Width="70px" HorizontalAlign="Center" />
                    <ItemStyle Width="70px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

                  <telerik:GridBoundColumn UniqueName="ImportID"  DataField="ImportID" HeaderText="" DataType="System.Int32">
                    <HeaderStyle Width="1px" />
                    <ItemStyle Width="1px"  />
                </telerik:GridBoundColumn>

            </Columns>                
            <EditFormSettings  UserControlName="ConfigImportDetails.ascx" EditFormType="WebUserControl"  >
                <EditColumn UniqueName="EditCommandColumn1" >
                </EditColumn>                
            </EditFormSettings>
                <CommandItemTemplate>
                <a href="#" onclick="return ShowInsertForm();">Add New Import</a>
            </CommandItemTemplate>

        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <ClientEvents OnRowDblClick="RowDblClick" />
          
        </ClientSettings>
         
         
    </telerik:RadGrid>
      <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"  Style="z-index: 100000;">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddImport" runat="server" Title="Editing import" Height="650px"
                Width="1055px"   ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
         <div class="center-div-list">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
         </div>

</asp:Content>
