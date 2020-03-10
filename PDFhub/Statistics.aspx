<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="PDFhub.Statistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Statistics" CssClass="header-text" Width="300" ></telerik:RadLabel>
            </td>
            <td style="float:right;"><asp:Label ID="LabelLastUpdate" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="true">
        <AjaxSettings>
            
            <telerik:AjaxSetting AjaxControlID="Interval">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LineChart" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  Transparency="75" >
    </telerik:RadAjaxLoadingPanel>
     <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;margin-left:20px;">
         <tr>
             <td style="width:100px;">Interval</td>       
             <td>
                <telerik:RadDropDownList runat="server" ID="Interval" AutoPostBack="true" Label="Period">
                    <Items>
                        <telerik:DropDownListItem Text="Last 6 hour" Value="6" />
                        <telerik:DropDownListItem Text="Last 12 hours" Value="12" />
                        <telerik:DropDownListItem Text="Last 24 hours" Value="24" />
                    </Items>
               </telerik:RadDropDownList>
            </td>
        </tr>
         <tr>
             <td colspan="2">

         <telerik:RadHtmlChart runat="server" ID="LineChart" Width="800" Height="500" Transitions="true" Skin="Silk">
            <Appearance>
                <FillStyle BackgroundColor="Transparent"></FillStyle>
            </Appearance>
             <ChartTitle Text="Processing performance">
                <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
            </ChartTitle>
             <Legend>
                <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
            </Legend>
             <PlotArea>
                 <Appearance>
                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                </Appearance>
                 <XAxis DataLabelsField="Time" AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">
                    
                    <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                    <TitleAppearance Position="Center" RotationAngle="0" Text="Time (hours)"></TitleAppearance>
                </XAxis>
                 <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                        MaxValue="100" MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false" Step="25">
                    <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                    <TitleAppearance Position="Center" RotationAngle="0" Text="Files/hour processed"></TitleAppearance>
                </YAxis>
                <Series>
                    <telerik:LineSeries Name="Input" DataFieldY="ValueInput" >
                        <Appearance>
                            <FillStyle BackgroundColor="#5cb8e3"></FillStyle>
                        </Appearance>
                        <LabelsAppearance DataFormatString="{0}" Position="Above"></LabelsAppearance>
                        <LineAppearance Width="1" />
                        <MarkersAppearance MarkersType="Circle" BackgroundColor="#5cb8e3" Size="4" BorderColor="#5cb8e3"
                                BorderWidth="2"></MarkersAppearance>
                        <TooltipsAppearance BackgroundColor="#5cb8e3" Color="White" DataFormatString="{0}"></TooltipsAppearance>                        
                   </telerik:LineSeries>

                   <telerik:LineSeries Name="Processing" DataFieldY="ValueProcess">
                        <Appearance>
                            <FillStyle BackgroundColor="#90b720"></FillStyle>
                        </Appearance>
                       <LabelsAppearance DataFormatString="{0}" Position="Above"></LabelsAppearance>
                       <LineAppearance Width="1" />
                        <MarkersAppearance MarkersType="Square" BackgroundColor="#90b720" Size="4" BorderColor="#90b720"
                            BorderWidth="2"></MarkersAppearance>
                        <TooltipsAppearance BackgroundColor="#90b720" Color="White" DataFormatString="{0}"></TooltipsAppearance>
                    </telerik:LineSeries>

                    <telerik:LineSeries Name="Export" DataFieldY="ValueExport">
                        <Appearance>
                            <FillStyle BackgroundColor="#2d6b99"></FillStyle>
                        </Appearance>
                       <LabelsAppearance DataFormatString="{0}" Position="Above"></LabelsAppearance>
                       <LineAppearance Width="1" />
                        <MarkersAppearance MarkersType="Square" BackgroundColor="#eb6d17" Size="4" BorderColor="#eb6d17"
                            BorderWidth="2"></MarkersAppearance>
                        <TooltipsAppearance BackgroundColor="#eb6d17" Color="White" DataFormatString="{0}"></TooltipsAppearance>
                    </telerik:LineSeries>
              </Series>
           </PlotArea>
        </telerik:RadHtmlChart>

    </td>
    </tr>
         <tr>
             <td style="width:100px;">Last page in:</td>    
             <td><asp:Label ID="lblLastPageInTime" runat="server" Text=""></asp:Label></td>   
         </tr>
         <tr>
             <td style="width:100px;">Last page out:</td>    
             <td><asp:Label ID="lblLastPageOutTime" runat="server" Text=""></asp:Label></td>   
         </tr>
         <tr>
             <td style="width:100px;">Total pages:</td>    
             <td><asp:Label ID="lblTotalPages" runat="server" Text=""></asp:Label></td>   
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
