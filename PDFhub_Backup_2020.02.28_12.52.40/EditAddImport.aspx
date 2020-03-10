<%@ Page Title="Edit/add planimport" Language="C#" AutoEventWireup="true" CodeBehind="EditAddImport.aspx.cs" Inherits="PDFhub.EditAddImport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Editing Import</title>
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
                if (window.radWindow)
                    oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow; //IE (and Moz as well)
 
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
        <div style="padding: 10px 10px 10px 10px;background-color:ghostwhite;">
            <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;">   
                <tr>
                    <td style="vertical-align:top">
                        <table id="Table3" border="0" class="module">
                            <tr>
                                <td style="width:140px;">Enabled:
                                </td>
                                <td colspan="3">
                                    <telerik:RadCheckBox ID="cbEnabled" runat="server" Text="" Checked='<%# Bind("Enabled") %>' TabIndex="1"></telerik:RadCheckBox>                        
                                </td>         
                               </tr>
                            <tr>
                                <td>Channel name:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtImportName" runat="server" Text='<%# Bind("Name") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>
                            </td>
                        </tr>
                         <tr>
                            <td>Plan format:
                            </td>
                            <td colspan="3">
                                <telerik:RadDropDownList ID="DropDownListType" runat="server" SelectedValue='<%# Bind("ImportType") %>' AutoPostBack="true" Width="200" >
                                  <Items>
                                    <telerik:DropDownListItem runat="server" Text="Newspilot" Value="1" />
                                    <telerik:DropDownListItem runat="server" Text="PPI XML" Value="2" />
                                    <telerik:DropDownListItem runat="server" Text="InfraLogic XML" Value="3" />
                                </Items>                            
                              </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="title" style="font-weight: bold;" colspan="4">Folder setup</td>
                        </tr>
                         <tr>
                            <td >Input folder:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtInputFolder" runat="server" Text='<%# Bind("InputFolder") %>' TabIndex="4" Width="400">
                                </telerik:RadTextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td >Done folder:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtDoneFolder" runat="server" Text='<%# Bind("DoneFolder") %>' TabIndex="4" Width="400">
                                </telerik:RadTextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td >Error folder:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtErrorFolder" runat="server" Text='<%# Bind("ErrorFolder") %>' TabIndex="4" Width="400">
                                </telerik:RadTextBox>
                            </td>
                        </tr> 
                         <tr>
                            <td >Copy folder:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtCopyFolder" runat="server" Text='<%# Bind("CopyFolder") %>' TabIndex="4" Width="400">
                                </telerik:RadTextBox>
                            </td>
                        </tr> 
                         <tr>
                            <td >Log folder:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtLogFolder" runat="server" Text='<%# Bind("LogFolder") %>' TabIndex="4" Width="400">
                                </telerik:RadTextBox>
                            </td>
                        </tr> 

                         <tr>
                            <td class="title" style="font-weight: bold;" colspan="4">Error notification</td>
                        </tr>
                        <tr>
                            <td>Notification email:
                            </td>
                            <td colspan="3">
                                <telerik:RadCheckBox ID="cbSendMail" runat="server" Text="" Checked='false' TabIndex="1" AutoPostBack="false" OnClientCheckedChanged="OnClientCheckedChangedX"></telerik:RadCheckBox>                        
                            </td>         
                        </tr>
                        <tr>
                            <td>Receivers:
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtEmailReceivers" Text="" runat="server"  TabIndex="9" Width="400">
                                  </telerik:RadTextBox>
                            </td>         
                        </tr>
                     </table>
    

                    </td>
                    <td style="vertical-align:top">
                        <table id="Table4" border="0" class="module">
                            <tr>
                                <td >PPI name translation
                                </td>
                            </tr>
                            <tr>
                              <td>
                                <telerik:RadGrid ID="RadGridPPI" runat="server" 
                                        OnNeedDataSource="RadGridPPI_NeedDataSource" 
                                        OnItemCreated="RadGridPPI_ItemCreated" 
                                        OnItemCommand="RadGridPPI_ItemCommand"                    
                                        AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                                        AllowFilteringByColumn="false"
                                        GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small" Width="450" >
                                    <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
                                    <GroupingSettings ShowUnGroupButton="false" />
                                    <MasterTableView EditMode="InPlace"  AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
                                        CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="RuleID">
                                        <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new translation" ShowRefreshButton="false"  />
                                     <HeaderStyle  BackColor="LightSteelBlue" ForeColor="Black" ></HeaderStyle>
                                        <FooterStyle BackColor="Beige"></FooterStyle>
                   
                                <Columns>

                                    <telerik:GridBoundColumn UniqueName="PPIProduct"  DataField="PPIProduct" HeaderText="PPI product" DataType="System.String"  >
                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn UniqueName="PPIEdition" DataField="PPIEdition" HeaderText="PPI edition" DataType="System.String">
                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                        <ItemStyle  Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    </telerik:GridBoundColumn>

             
                                    <telerik:GridBoundColumn UniqueName="Publication" DataField="Publication" HeaderText="Publication" DataType="System.String">
                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                        <ItemStyle  Width="100px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    </telerik:GridBoundColumn>

                                      <telerik:GridEditCommandColumn EditText="Edit" HeaderText="Edit" >
                                        <HeaderStyle Width="70px"  />
                                        <ItemStyle Width="70px" HorizontalAlign="Center"/>
                                      </telerik:GridEditCommandColumn>

                                     <telerik:GridButtonColumn HeaderText="Delete" ConfirmText="Delete this input setup?" ConfirmDialogType="RadWindow"
                                                     ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >              
                                        <HeaderStyle Width="70px"  />
                                        <ItemStyle Width="70px" HorizontalAlign="Center"/>
                                    </telerik:GridButtonColumn>

                                      <telerik:GridBoundColumn UniqueName="RuleID" DataField="RuleID" HeaderText="" DataType="System.Int32">
                                        <HeaderStyle Width="1px"/>
                                        <ItemStyle  Width="1px"  />
                                    </telerik:GridBoundColumn>

                                </Columns>                
           
                                    </MasterTableView>
                                    <ClientSettings>
                                        
                                          <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                          </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                       &nbsp;
                   </td>
                </tr>
   
                <tr>
                    <td align="center" colspan="3">
                        <telerik:RadButton ID="btnUpdate" Text="Insert" runat="server"  OnClick="btnUpdate_Click"></telerik:RadButton>
                       &nbsp;
                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" OnClientClicked="CancelEdit" ></telerik:RadButton>
                   </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                       &nbsp;
                   </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
