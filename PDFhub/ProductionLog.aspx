<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ProductionLog.aspx.cs" Inherits="PDFhub.ProductionLog"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        div.RadToolBar { margin: 0 0 0 0;  padding-top: 0; padding-bottom : 0;}
        div.RadToolBar .rtbUL { width: 100%; }
        div.RadToolBar .rightButton  { float: right;}      
    </style>
    <script type="text/javascript">  
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function ShowEditForm(id, rowIndex) {
                var grid = $find("<%= RadGridLog.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("ChangeChannels.aspx?ProductionID=" + id, "EditAddChannels");
                return false;
            }

            function ShowResendForm(id, rowIndex) {
                var grid = $find("<%= RadGridLog.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("ResendToChannel.aspx?ProductionID=" + id, "ResendToChannel");
                return false;
            }

            function ShowReleaseForm(id, rowIndex) {
                var grid = $find("<%= RadGridLog.ClientID %>");

                 var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                 grid.get_masterTableView().selectItem(rowControl, true);

                 window.radopen("Release.aspx?ProductionID=" + id, "ReleaseChannels");
                 return false;
             }

            function ShowViewForm(id, rowIndex) {
                var grid = $find("<%= RadGridLog.ClientID %>");
 
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
 
                window.radopen("ViewPages.aspx?ProductionID=" + id, "ViewPages");
                return false;
            }

            function ShowCheckForm(id, rowIndex) {
                var grid = $find("<%= RadGridLog.ClientID %>");

                 var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                 grid.get_masterTableView().selectItem(rowControl, true);

                 window.radopen("CheckPages.aspx?ProductionID=" + id, "CheckPages");
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
                window.radopen("ChangeChannels.aspx?ProductionID=" + eventArgs.getDataKeyValue("ProductionID"), "EditAddChannels");
            }
        </script>
    </telerik:RadCodeBlock>

    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Active productions" CssClass="header-text" Width="300" ></telerik:RadLabel>
            </td>
            <td style="float:right;"><asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">



     <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;margin-left:20px;">
        <tr>       
            <td style="width:100px;text-align:right;padding-right:10px;">Start pubdate</td>       
             <td style="width:170px;">
                 <telerik:RadDatePicker ID="RadDatePickerStartDate" runat="server"></telerik:RadDatePicker>
            </td>       
            <td style="width:100px;text-align:right;padding-right:10px;">End pubdate</td>       
             <td style="width:170px;">
                 <telerik:RadDatePicker ID="RadDatePickerEndDate" runat="server"></telerik:RadDatePicker>
            </td>    
             <td style="width:100px; text-align:right;padding-right:10px;">Product</td>       
             <td style="width:170px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListProduct" AutoPostBack="false"  > 
               </telerik:RadDropDownList>
            </td>
             <td style="width:100px;text-align:right;padding-right:10px;">Export</td>       
             <td style="width:170px;">
                <telerik:RadDropDownList runat="server" ID="RadDropDownListExport" AutoPostBack="false"  > 
               </telerik:RadDropDownList>
            </td>
            <td></td>
                      
        </tr>
    </table>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Timer1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelLastUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelError" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridLog">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

             <telerik:AjaxSetting AjaxControlID="RadDropDownListProduct">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

              <telerik:AjaxSetting AjaxControlID="RadDropDownListExport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick"></asp:Timer>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  Transparency="75" >
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" PostBackControls="RadToolBar1">

    <telerik:RadGrid ID="RadGridLog" runat="server" OnNeedDataSource="RadGridLog_NeedDataSource"  OnItemDataBound="RadGridLog_ItemDataBound" OnItemCommand="RadGridLog_ItemCommand" OnItemCreated="RadGridLog_ItemCreated" OnDetailTableDataBind="RadGridLog_DetailTableDataBind" 
                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small"   >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
        <GroupingSettings ShowUnGroupButton="false" />
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="600"></ClientSettings>
        <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Auto" CommandItemDisplay="Top" ShowGroupFooter="true" DataKeyNames="ProductionID" HierarchyLoadMode="ServerBind" >
             <GroupHeaderItemStyle  ForeColor="SteelBlue" Font-Bold="true" BackColor="Wheat" ></GroupHeaderItemStyle>
                 <GroupByExpressions >
                    <telerik:GridGroupByExpression>
                        <SelectFields >
                        <telerik:GridGroupByField FieldName="PubDateStr" HeaderText="Pubdate" HeaderValueSeparator =" "  />
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="PubDateStr" SortOrder="Ascending"  />
                    </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>
            <CommandItemTemplate>
                <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar1" runat="server" AutoPostBack="true"  style="width: 100%;">
                    <Items>
                        <telerik:RadToolBarButton runat="server">
                            <ItemTemplate> 
                            <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                <span>Productions</span>
                            </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton >
                        <telerik:RadToolBarButton Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton"   >
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton Text="Save to Excel" CommandName="SaveToExcel" ImageUrl="Images/Excel.png" OuterCssClass="rightButton"   >
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </CommandItemTemplate>
            <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
          <DetailTables>
            <telerik:GridTableView DataKeyNames="ProductionID" Name="Exports"  DataMember="Exports">
                
                <Columns>
                    <telerik:GridBoundColumn  HeaderText="Export" HeaderButtonType="TextButton" DataField="Name" DataType="System.String">
                        <HeaderStyle Width="125px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        <ItemStyle Width="125px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn  HeaderText="Pages"  HeaderButtonType="TextButton" DataField="Pages" DataType="System.Int32">
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Pages sent" HeaderButtonType="TextButton" DataField="PagesSent" DataType="System.Int32">
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn HeaderText="FirstSent" HeaderButtonType="TextButton" DataField="FirstSent" DataType="System.String">
                        <HeaderStyle Width="115px" HorizontalAlign="Center"/>
                        <ItemStyle Width="115px" HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="LastSent" HeaderButtonType="TextButton" DataField="LastSent" DataType="System.String">
                        <HeaderStyle Width="115px" HorizontalAlign="Center"/>
                        <ItemStyle Width="115px" HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn UniqueName="PageList" HeaderText="PageList"  DataField="PageList" DataType="System.String">
                        <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned"/>
                        <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    </telerik:GridBoundColumn>

                     <telerik:GridBoundColumn UniqueName="MergedPDF" HeaderText=""  DataField="MergedPDF" DataType="System.Int32">
                        <HeaderStyle  Width="1" />
                        <ItemStyle Width="1"  />
                    </telerik:GridBoundColumn>
 
                </Columns>
            </telerik:GridTableView>
              </DetailTables>
            <Columns>
                <telerik:GridBoundColumn UniqueName="Publication" DataField="Publication" HeaderText="Product" DataType="System.String" >
                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="EditionList" DataField="EditionList" HeaderText="Editions" DataType="System.String">
                    <HeaderStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="70px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="PageStr" DataField="PageStr" HeaderText="Pages" DataType="System.String" >                    
                    <HeaderStyle Width="70px" HorizontalAlign="Center"  />
                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="PagesReceivedStr" DataField="PagesReceivedStr" HeaderText="Received" DataType="System.String" >                    
                    <HeaderStyle Width="70px" HorizontalAlign="Center"/>
                    <ItemStyle Width="70px" HorizontalAlign="Center"  />
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn UniqueName="ReleaseTimeStr" DataField="ReleaseTimeStr" HeaderText="Release time" DataType="System.String" >                    
                    <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="ChannelList" DataField="ChannelList" HeaderText="Exports" DataType="System.String" AllowFiltering="false"  >
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned"  Wrap="true"/>
                </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn UniqueName="ProductionID"  DataField="ProductionID" DataType="System.Int32"  >
                    <HeaderStyle Width="1" />
                    <ItemStyle Width="1" />
                </telerik:GridBoundColumn>


                <telerik:GridTemplateColumn UniqueName="ChangeButton" HeaderText="Change exports" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="EditButton" runat="server" ToolTip="Change exports" ImageUrl="Images/Edit.gif" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                
                <telerik:GridTemplateColumn UniqueName="Resend" HeaderText="Resent export(s)" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="ResendButton" runat="server" ToolTip="Resend to destination(s)" ImageUrl="Images/syncproblem.png" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

               

                   <telerik:GridTemplateColumn UniqueName="Release" HeaderText="Force release" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:ImageButton ID="ReleaseButton" runat="server" ToolTip="Release certain exports" ImageUrl="Images/play.png" ImageAlign="Middle" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn UniqueName="View" HeaderText="View" HeaderStyle-Width="65" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="65" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:ImageButton ID="ViewButton" runat="server" ToolTip="View page gallery" ImageUrl="Images/View.png" ImageAlign="Middle" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn UniqueName="Check" HeaderText="Check pages" HeaderStyle-Width="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="1" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:ImageButton ID="CheckButton" runat="server" ToolTip="Read-check all original PDF" ImageUrl="Images/inspect.png" ImageAlign="Middle" ></asp:ImageButton>
                    </ItemTemplate>
                 </telerik:GridTemplateColumn>

                <telerik:GridBoundColumn UniqueName="Released"  DataField="Released" HeaderText="" DataType="System.Boolean">
                    <HeaderStyle Width="1px" />
                    <ItemStyle Width="1px" />
                </telerik:GridBoundColumn>

              </Columns>            
          </MasterTableView>
      </telerik:RadGrid>
  
    </telerik:RadAjaxPanel>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" Style="z-index: 100000">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="EditAddChannels" runat="server" Title="Editing exports" Height="640px"
                Width="540px" ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
            <telerik:RadWindow RenderMode="Lightweight" ID="ResendToChannel" runat="server" Title="Resent to export" Height="640px"
                Width="540px" ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>
             <telerik:RadWindow RenderMode="Lightweight" ID="ViewPages" runat="server" Title="Pages" Height="515"
                Width="430" ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true"  KeepInScreenBounds="true" style="z-index:100001"  >
            </telerik:RadWindow>

            <telerik:RadWindow RenderMode="Lightweight" ID="CheckPages" runat="server" Title="Read-check PDF pages" Height="640px"
                Width="540px" ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
            </telerik:RadWindow>


              <telerik:RadWindow RenderMode="Lightweight" ID="ReleaseChannels" runat="server" Title="Force release" Height="600px"
                Width="540px" ReloadOnShow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" Modal="true" CenterIfModal="true" ShowOnTopWhenMaximized ="true" >
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
