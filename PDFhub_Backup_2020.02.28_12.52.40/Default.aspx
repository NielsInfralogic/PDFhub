<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PDFhub.Default" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        div.RadToolBar { margin: 0 0 0 0;  padding-top: 0; padding-bottom : 0;}
        div.RadToolBar .rtbUL { width: 100%; }
        div.RadToolBar .rightButton  { float: right;}
         .container-for-grid {
            height: 50%;
        }
    </style>
  
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Timer1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridState" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelLastUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelError" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Timer2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridErrorList" LoadingPanelID="RadAjaxLoadingPanel1" ></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelLastUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="LabelError" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Timer ID="Timer1" runat="server" Interval="15000" OnTick="Timer1_Tick"></asp:Timer>
    <asp:Timer ID="Timer2" runat="server" Interval="15000" OnTick="Timer2_Tick"></asp:Timer>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  Transparency="75" >
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadGrid ID="RadGridState" runat="server" OnNeedDataSource="RadGridState_NeedDataSource"  OnItemDataBound="RadGridState_ItemDataBound" OnItemCommand="RadGridState_ItemCommand" OnItemCreated="RadGridState_ItemCreated"
                    AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small" >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
        <GroupingSettings ShowUnGroupButton="false" />
        <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" CommandItemDisplay="Top" ShowGroupFooter="true">        
             <CommandItemTemplate>
                <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar1" runat="server" AutoPostBack="true"  style="width: 100%;">
                    <Items>                     
                        <telerik:RadToolBarButton runat="server">
                            <ItemTemplate> 
                                <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                    <span>Services</span>
                                </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton  Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton"   >
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </CommandItemTemplate>
            <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>      
            <Columns>
                <telerik:GridImageColumn DataImageUrlFields="StateImageUrl"  DataImageUrlFormatString="images/{0}.png" UniqueName="StateImageUrl" HeaderText="State" DataType="System.String" ImageAlign="Middle" >
                    <HeaderStyle Width="40px" HorizontalAlign="Center" />
                    <ItemStyle Width="40px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridImageColumn>

                <telerik:GridBoundColumn UniqueName="Name"  DataField="Name" HeaderText="Service" DataType="System.String"  >
                    <HeaderStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="InstanceNumber" DataField="InstanceNumber" HeaderText="Instance" DataType="System.Int32">
                    <HeaderStyle Width="65px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="65px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridDateTimeColumn UniqueName="LastEventTime" DataField="LastEventTime" HeaderText="Last Event" >
                    <HeaderStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn UniqueName="LastMessage" DataField="LastMessage" HeaderText="Last Message" DataType="System.String">
                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                 <telerik:GridBoundColumn UniqueName="Type"  DataField="Type" HeaderText="" DataType="System.Int32" Visible="true" >
                    <HeaderStyle Width="1px" />
                    <ItemStyle Width="1px" />
                </telerik:GridBoundColumn>
                 <telerik:GridButtonColumn UniqueName="DismissButton" DataTextFormatString="Clear error" ButtonType="ImageButton" ImageUrl="Images/cancel.png" HeaderText="Clear error" CommandName="Dismiss" DataTextField="Dismiss">
                    <HeaderStyle Width="110px" HorizontalAlign="Center"  />
                    <ItemStyle Width="110px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>
                <telerik:GridButtonColumn UniqueName="ViewButton" DataTextFormatString="View" ButtonType="ImageButton" ImageUrl="Images/view.png" HeaderText="View log" CommandName="View" DataTextField="View">
                    <HeaderStyle Width="80px" HorizontalAlign="Center"  />
                    <ItemStyle Width="80px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>
               

               
            </Columns>       
            
        </MasterTableView>
    </telerik:RadGrid>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
     <telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        $(window).resize(function () {
            GridCreated($find('<%= RadGridErrorList.ClientID %>')); // resize the grid on Window resize
        });
        function GridCreated(sender, args) {
            var parentHeight = $(window).height(); // make grid fit the Window height
            //var parentHeight = sender.get_element().parentElement.offsetHeight; // make grid fit its parent container height
            var scrollArea = sender.GridDataDiv;
            var gridHeaderHeight = (sender.GridHeaderDiv) ? sender.GridHeaderDiv.offsetHeight : 0;
            var gridTopPagerHeight = (sender.TopPagerControl) ? sender.TopPagerControl.offsetHeight : 0;
            var gridDataHeight = sender.get_masterTableView().get_element().offsetHeight;
            var gridFooterHeight = (sender.GridFooterDiv) ? sender.GridFooterDiv.offsetHeight : 0;
            var gridPagerHeight = (sender.PagerControl) ? sender.PagerControl.offsetHeight : 0;
 
            // Do nothing if scrolling is not enabled
            if (!scrollArea) {
                return;
            }
            if (gridDataHeight < 350 || parentHeight > (gridDataHeight + gridHeaderHeight + gridPagerHeight + gridTopPagerHeight + gridFooterHeight)) {
                scrollArea.style.height = gridDataHeight + "px";
            } else {
                scrollArea.style.height = (parentHeight - gridHeaderHeight - gridPagerHeight - gridTopPagerHeight - gridFooterHeight - 2) + "px"
            }
        }
    </script>
</telerik:RadCodeBlock>
   
   <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" PostBackControls="RadToolBar2">
    <div class="container-for-grid">
       <telerik:RadGrid ID="RadGridErrorList" runat="server" OnNeedDataSource="RadGridErrorList_NeedDataSource"  OnItemDataBound="RadGridErrorList_ItemDataBound" OnItemCommand="RadGridErrorList_ItemCommand"
                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                        GridLines="Horizontal" ShowStatusBar="true"  ShowHeader="true"  ShowFooter="false" Font-Size="Small" >
        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>
        <GroupingSettings ShowUnGroupButton="false" />
         <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"></ClientSettings>
        <MasterTableView AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" CommandItemDisplay="Top" ShowGroupFooter="true">
              <CommandItemTemplate>
                <telerik:RadToolBar RenderMode="Lightweight" ID="RadToolBar2" runat="server" AutoPostBack="true"  style="width: 100%;">
                    <Items>
                        <telerik:RadToolBarButton runat="server">
                            <ItemTemplate> 
                            <div style="padding-left: 10px;padding-top:5px; font-size: larger;">
                                <span>Latest errors</span>
                            </div>
                            </ItemTemplate>
                        </telerik:RadToolBarButton >

                          <telerik:RadToolBarButton Text="Retry error files" CommandName="RetryErrorFiles" ImageUrl="Images/syncproblem.png"   >
                        </telerik:RadToolBarButton>

                        <telerik:RadToolBarButton Text="Refresh" CommandName="RebindGrid" ImageUrl="Images/Refresh.png" OuterCssClass="rightButton"   >
                        </telerik:RadToolBarButton>
                      
                    </Items>
                </telerik:RadToolBar>
            </CommandItemTemplate>
            <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
            <FooterStyle BackColor="Beige"></FooterStyle>
                   
            <Columns>

                <telerik:GridDateTimeColumn UniqueName="Time" DataField="Time" HeaderText="Last Event" AllowFiltering="false"  >
                    <HeaderStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="140px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn UniqueName="Status"  DataField="Status" HeaderText="Status" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="80px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Service" DataField="Service" HeaderText="Service" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="110px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn UniqueName="FileName" DataField="FileName" HeaderText="FileName" DataType="System.String" FilterControlWidth="200">
                    <HeaderStyle Width="280px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                    <ItemStyle Width="280px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="Message" DataField="Message" HeaderText="Message" DataType="System.String" AllowFiltering="false" >
                    <HeaderStyle  HorizontalAlign="Center" CssClass="rgRowRightAligned" />
                    <ItemStyle  HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                </telerik:GridBoundColumn>

                <telerik:GridButtonColumn UniqueName="RetryButton" DataTextFormatString="Retry" ButtonType="ImageButton" ImageUrl="Images/loop.png"  HeaderText="Retry" CommandName="Retry" DataTextField="Retry" >
                    <HeaderStyle Width="90px" HorizontalAlign="Center" />
                    <ItemStyle Width="90px" HorizontalAlign="Center"/>
                </telerik:GridButtonColumn>
            </Columns>            
        </MasterTableView>
    </telerik:RadGrid>
    </div>
   </telerik:RadAjaxPanel>
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <table style="width:100%;">
     
        <tr>
            <td style="width:100px;">
                <asp:Label ID="Label3" runat="server" Text="Opdateret"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table> 
</asp:Content>
