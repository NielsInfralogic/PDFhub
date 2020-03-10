<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ViewQueues.aspx.cs" Inherits="PDFhub.ViewQueues" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
        div.RadToolBar { margin: 0 0 0 0;  padding-top: 0; padding-bottom : 0;}
        div.RadToolBar .rtbUL { width: 100%; }
        div.RadToolBar .rightButton  { float: right;}
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Present queues" CssClass="header-text" Width="300" ></telerik:RadLabel>
            </td>
            <td style="float:right;"><asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

             <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Timer1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridQueue1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGridQueue2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGridQueue3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelLastUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelError" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  Transparency="75" >
    </telerik:RadAjaxLoadingPanel>

     <table style="width:100%;">
          <tr>
            <td style="width:33%;">
               <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" PostBackControls="RadToolBar1">

                <telerik:RadGrid ID="RadGridQueue1" runat="server" OnNeedDataSource="RadGridQueue1_NeedDataSource"  OnItemCommand="RadGridQueue1_ItemCommand"
                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        GridLines="Horizontal" ShowStatusBar="false"  ShowHeader="true"  ShowFooter="false" Font-Size="Small"  >
                    <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
                    <GroupingSettings ShowUnGroupButton="false" />
                    <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="600"></ClientSettings>
                    <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Auto" CommandItemDisplay="Top" ShowGroupFooter="false">
                    <CommandItemTemplate>
                        <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar1" runat="server" AutoPostBack="true"  style="width: 100%;">
                            <Items>
                                <telerik:RadToolBarButton runat="server">
                                    <ItemTemplate> 
                                    <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                        <span>Input queue</span>
                                    </div>
                                    </ItemTemplate>
                                </telerik:RadToolBarButton >
                                <telerik:RadToolBarButton Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton">
                                </telerik:RadToolBarButton>

                            </Items>
                        </telerik:RadToolBar>
                    </CommandItemTemplate>
          
                    <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
                    <Columns>
                        <telerik:GridDateTimeColumn UniqueName="Queue"  DataField="Queue" HeaderText="Queue" DataType="System.String"  AllowFiltering="false" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn UniqueName="FileName"  DataField="FileName" HeaderText="FileName" DataType="System.String" AllowFiltering="false"  >
                            <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                     </Columns>            
                 </MasterTableView>
             </telerik:RadGrid>
              </telerik:RadAjaxPanel>
            </td>
              <td style="width:33%;">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" PostBackControls="RadToolBar2">
                <telerik:RadGrid ID="RadGridQueue2" runat="server" OnNeedDataSource="RadGridQueue2_NeedDataSource"  OnItemCommand="RadGridQueue2_ItemCommand"
                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small"  >
                    <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
                    <GroupingSettings ShowUnGroupButton="false" />
                    <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="600"></ClientSettings>
                    <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Auto" CommandItemDisplay="Top" ShowGroupFooter="false">
                    <CommandItemTemplate>
                        <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar2" runat="server" AutoPostBack="true"  style="width: 100%;">
                            <Items>
                                <telerik:RadToolBarButton runat="server">
                                    <ItemTemplate> 
                                    <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                        <span>Process queue</span>
                                    </div>
                                    </ItemTemplate>
                                </telerik:RadToolBarButton >
                                <telerik:RadToolBarButton Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton">
                                </telerik:RadToolBarButton>

                            </Items>
                        </telerik:RadToolBar>
                    </CommandItemTemplate>
          
                    <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
                    <Columns>
                        <telerik:GridDateTimeColumn UniqueName="Queue"  DataField="Queue" HeaderText="ProcessType" DataType="System.String"  AllowFiltering="false" >
                            <HeaderStyle Width="140px" HorizontalAlign="Left"  />
                            <ItemStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn UniqueName="FileName"  DataField="FileName" HeaderText="FileName" DataType="System.String" AllowFiltering="false"  >
                            <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                     </Columns>            
                 </MasterTableView>
             </telerik:RadGrid>
            </telerik:RadAjaxPanel>
              </td>
              <td style="width:33%;">
                <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" PostBackControls="RadToolBar3">
                <telerik:RadGrid ID="RadGridQueue3" runat="server" OnNeedDataSource="RadGridQueue3_NeedDataSource"  OnItemCommand="RadGridQueue3_ItemCommand"
                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small"  >
                    <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
                    <GroupingSettings ShowUnGroupButton="false" />
                    <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="600"></ClientSettings>
                    <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Auto" CommandItemDisplay="Top" ShowGroupFooter="false">
                    <CommandItemTemplate>
                        <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar3" runat="server" AutoPostBack="true"  style="width: 100%;">
                            <Items>
                                <telerik:RadToolBarButton runat="server">
                                    <ItemTemplate> 
                                    <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                        <span>Transmit queue</span>
                                    </div>
                                    </ItemTemplate>
                                </telerik:RadToolBarButton >
                                <telerik:RadToolBarButton Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton">
                                </telerik:RadToolBarButton>

                            </Items>
                        </telerik:RadToolBar>
                    </CommandItemTemplate>
          
                    <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
                    <Columns>
                        <telerik:GridDateTimeColumn UniqueName="Queue"  DataField="Queue" HeaderText="Export" DataType="System.String"  AllowFiltering="false" >
                            <HeaderStyle HorizontalAlign="Left"  />
                            <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn UniqueName="FileName"  DataField="FileName" HeaderText="FileName" DataType="System.String" AllowFiltering="false"  >
                            <HeaderStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                            <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                        </telerik:GridBoundColumn>

                     </Columns>            
                 </MasterTableView>
              </telerik:RadGrid>
             </telerik:RadAjaxPanel>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</asp:Content>
