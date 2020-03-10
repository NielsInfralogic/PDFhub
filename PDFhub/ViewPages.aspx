<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPages.aspx.cs" Inherits="PDFhub.ViewPages" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Page view</title>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1, maximum-scale=1.0, user-scalable=no" />
    <link href="styles/default.css" rel="stylesheet" />

</head>

<body>
    <form id="form1" runat="server">
        <script type="text/javascript">

            function CloseAndRebind(args) {
                GetRadWindow().BrowserWindow.refreshGrid(args);
                GetRadWindow().close();
            }
 
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)
 
                return oWindow;
            }
 
            function CancelEdit() {
                GetRadWindow().close();
            }

            function RefreshParentPage() {
                GetRadWindow().BrowserWindow.document.forms[0].submit();
                GetRadWindow().Close();
            }
        </script>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
          <telerik:RadAjaxManager runat="server">
        <AjaxSettings>
              <telerik:AjaxSetting AjaxControlID="RadImageGallery1">
                <UpdatedControls>          
                    <telerik:AjaxUpdatedControl ControlID="RadImageGallery1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

        <div style="padding: 10px 10px 10px 10px;background-color:darkgray">
           
           
            <telerik:RadImageGallery RenderMode="Lightweight" ID="RadImageGallery1" 
               Width="380px"  runat="server" Height="450px" 
                OnNeedDataSource="RadImageGallery1_NeedDataSource" 
                DataDescriptionField="Description" DataImageField="Picture" DataTitleField="Description" >
            <ClientSettings>
                <AnimationSettings>
                    <NextImagesAnimation Type="Fade" Easing="Ease"  Speed="500" />
                    <PrevImagesAnimation Type="Fade" Easing="Ease" Speed="500" />
                </AnimationSettings>
            </ClientSettings>
            <ThumbnailsAreaSettings Position="Left" Width="100px"  ScrollOrientation="Vertical"  Mode="Thumbnails" ThumbnailsSpacing="5" ThumbnailWidth="100px" ThumbnailHeight="143"  />
                
            <ImageAreaSettings   ShowDescriptionBox="false" NavigationMode="Zone" Height="100%" ResizeMode="Fit"  />
         
        </telerik:RadImageGallery>               
             
            </div>
             <input runat="server" id="HiddenProductionID" type="hidden" value="" />
           
    </form>
</body>
</html>
