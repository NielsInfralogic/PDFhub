<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ProcessLog.aspx.cs" Inherits="PDFhub.ProcessLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
        div.RadToolBar { margin: 0 0 0 0;  padding-top: 0; padding-bottom : 0;}
        div.RadToolBar .rtbUL { width: 100%; }
        div.RadToolBar .rightButton  { float: right;}
      
    </style>
    <script type="text/javascript">  
        function Resize()  
        {  
            setTimeout(function () {  
                var offs = 300;
               
            var scrollArea = document.getElementById("<%= RadGridLog.ClientID %>" + "_GridData");  
            if(scrollArea)  
            {  
                scrollArea.style.height = document.body.offsetHeight - offs + "px";  
            }             
           if(window["<%= RadGridLog.ClientID %>"].ClientSettings.Scrolling.UseStaticHeaders)  
           {  
               var header = document.getElementById("<%= RadGridLog.ClientID %>" + "_GridHeader");  
               scrollArea.style.height = document.body.offsetHeight - header.offsetHeight - offs + "px";  
           }  
           if(window["<%= RadGridLog.ClientID %>"].ClientSettings.Scrolling.UseStaticHeaders && document.getElementById("<%= RadGridLog.ClientID %>" + "_ctl01_Pager"))  
           {  
              var pagerArea = document.getElementById("<%= RadGridLog.ClientID %>" + "_ctl01_Pager");  
              scrollArea.style.height = document.body.offsetHeight - header.offsetHeight - pagerArea.offsetHeight - offs + "px";  
           }  
           }, 200);  
       }  
        windowwindow.onresize = window.onload = Resize;  
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="PDF process log" CssClass="header-text" Width="300" ></telerik:RadLabel>
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
                    <telerik:AjaxUpdatedControl ControlID="RadGridLog" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelLastUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelError" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Timer ID="Timer1" runat="server" Interval="15000" OnTick="Timer1_Tick"></asp:Timer>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  Transparency="75" >
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" PostBackControls="RadToolBar1">
    <telerik:RadGrid ID="RadGridLog" runat="server" OnNeedDataSource="RadGridLog_NeedDataSource"  OnItemDataBound="RadGridLog_ItemDataBound" OnItemCommand="RadGridLog_ItemCommand"
                        AllowSorting="true" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="true"
                        GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small"  >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
        <GroupingSettings ShowUnGroupButton="false" />
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="600"></ClientSettings>
        <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Auto" CommandItemDisplay="Top" ShowGroupFooter="true">
            
            <CommandItemTemplate>
                <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar1" runat="server" AutoPostBack="true"  style="width: 100%;">
                    <Items>
                        <telerik:RadToolBarButton runat="server">
                            <ItemTemplate> 
                            <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                <span>Latest activity</span>
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
            <Columns>
                <telerik:GridDateTimeColumn UniqueName="Time"  DataField="Time" HeaderText="Time" AllowFiltering="false" >
                    <HeaderStyle Width="140px" HorizontalAlign="Left"  />
                    <ItemStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn UniqueName="Status"  DataField="Status" DataType="System.Int32" AllowFiltering="false"  >
                    <HeaderStyle Width="0" />
                    <ItemStyle Width="0" />
                </telerik:GridBoundColumn>

                 <telerik:GridBoundColumn UniqueName="StatusName"  DataField="StatusName" HeaderText="Status" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Service" DataField="Service" HeaderText="Service" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Source" DataField="Source" HeaderText="Source" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="150px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="FileName" DataField="FileName" HeaderText="FileName" DataType="System.String" FilterControlWidth="230">
                    <HeaderStyle Width="280px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="280px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Message" DataField="Message" HeaderText="Message" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridButtonColumn UniqueName="RetryButton" DataTextFormatString="Retry" ButtonType="ImageButton" ImageUrl="Images/loop.png"  HeaderText="Retry" CommandName="Retry" DataTextField="Retry">
                    <HeaderStyle Width="90px" HorizontalAlign="Center"  />
                    <ItemStyle Width="90px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>

            </Columns>            
        </MasterTableView>
    </telerik:RadGrid>
  
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
       <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</asp:Content>
