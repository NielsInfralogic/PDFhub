<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ConfigPublicationDetails.ascx.cs" Inherits="PDFhub.ConfigPublicationDetails" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script >
    function checkBoxClick(sender, args) {
        var grid = $find("<%= RadGridPublicationChannels.ClientID %>");
        var masterTableView = grid.get_masterTableView();
        var batchEditingManager = grid.get_batchEditingManager();
        var parentCell = $telerik.$(sender).closest("td")[0];
        var initialValue = sender.checked;
        sender.checked = !sender.checked;

        batchEditingManager.changeCellValue(parentCell, initialValue);
    }

</script>
<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;">
    <tr class="EditFormHeader">
        <td>
            <b>Product name definition:</b>
        </td>
    </tr>
    <tr>
        <td>
          <table>
            <tr align="top">
              <td>
               <table id="Table3" border="0" class="module">
                 <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Naming:</td>
                </tr>
                <tr>
                    <td>Publication name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtPublicationName" runat="server" Text='<%# Bind("Name") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                <tr>
                    <td>Short name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtInputAlias" runat="server" Text='<%# Bind("InputAlias") %>' Width="100"  TabIndex="2"></telerik:RadTextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td>Output name:
                    </td>
                    <td colspan="3">
                       <telerik:RadTextBox ID="txtOutputAlias" Text='<%# Bind("OutputAlias") %>' runat="server"  TabIndex="10" Width="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Alternative name:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtExtendedAlias" runat="server" Text='<%# Bind("ExtendedAlias") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>
                </tr>
                 <tr>
                    <td>Annum text:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtAnnumText" runat="server" Text='<%# Bind("AnnumText") %>' Width="100"  TabIndex="2"></telerik:RadTextBox>                        
                    </td>
                </tr>

                    <tr>
                    <td>Publisher:
                    </td>
                    <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownListPublisher" runat="server" SelectedValue='<%# Bind("_PublisherName") %>' AutoPostBack="true" >
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Release time:</td>
                </tr>
                <tr>
                    <td>Time:
                    </td>
                    <td colspan="3">
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTime"  TabIndex="8" DbSelectedDate='<%# Bind("_ReleaseTime") %>'></telerik:RadTimePicker>
                       
                       
                    </td>
                </tr>
                    <tr>
                    <td>Days before pubdate (+/-):
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericReleaseDays" runat="server" MinValue="-999" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "ReleaseDays") %>' Width="40" NumberFormat-DecimalDigits="0" N >
                                            </telerik:RadNumericTextBox> 
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Other options:</td>
                </tr>
                <tr>
                    <td >Requires page approval:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbDefaultApprove" runat="server" Text="" Checked='<%# Bind("DefaultApprove") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td >Change pubdate in output:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbPubdateMove" runat="server" Text="" Checked='<%# Bind("PubdateMove") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                 <tr>
                    <td >Pubdate move days:
                    </td>
                    <td colspan="3">
                         <telerik:RadNumericTextBox ID="RadNumericTextBoxPubdateMoveDays" runat="server" MinValue="-999" MaxValue="999" TabIndex="6" DbValue='<%# Bind( "PubdateMoveDays") %>' Width="50" NumberFormat-DecimalDigits="0"  >
                                            </telerik:RadNumericTextBox>  
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Notifications:</td>
                </tr>
                <tr>
                    <td >Send email-report:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbSendReport" runat="server" Text="" Checked='<%# Bind("SendReport") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td >Email receiver(s):
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmailRecipient" runat="server" Text='<%# Bind("EmailRecipient") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>                      
                    </td>         
                </tr>                                                            
              </table>           
             </td>
              <td align="top" >
              <table style="padding-left: 20px;border:0px" class="module">
                <tr align="top">
                     <td class="title" style="font-weight: bold;" >Exports:</td>
                </tr>
                <tr align="top">
                    <td>
                       <telerik:RadGrid ID="RadGridPublicationChannels" runat="server" OnPreRender="RadGridPublicationChannels_PreRender"
                                     OnNeedDataSource="RadGridPublicationChannels_NeedDataSource" 
                                     OnBatchEditCommand="RadGridPublicationChannels_BatchEditCommand" OnItemUpdated="RadGridPublicationChannels_ItemUpdated" 
                                     AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                                     AllowFilteringByColumn="false" AllowAutomaticUpdates="True"
                                     GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small" Width="600" >
                        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
                        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-ScrollHeight="500" ></ClientSettings>
                        <GroupingSettings ShowUnGroupButton="false" />
                        <MasterTableView EditMode="Batch" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
                            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="ChannelName">
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" SaveChangesText="Save changes" CancelChangesText="Cancel changes" />                    
                            <BatchEditingSettings EditType="Cell" OpenEditingEvent="Click" />
                            <Columns>                

                                <telerik:GridTemplateColumn DataField="Use" HeaderText="Use" UniqueName="Use">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="CheckBoxUse" Enabled="true" Checked='<%# Eval("Use") %>' onclick="checkBoxClick(this, event);" Width="40" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="CheckBoxUseEdit" />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn UniqueName="ChannelName"  DataField="ChannelName" HeaderText="Export name" DataType="System.String">
                                    <HeaderStyle Width="230px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle Width="230px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn HeaderText="Trigger" DefaultInsertValue="None"  UniqueName="_Trigger" DataField="_Trigger">
                                    <ItemTemplate>                                                               
                                        <%# Eval("_Trigger") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadDropDownList DataValueField="_Trigger" DataTextField="_Trigger" RenderMode="Lightweight" runat="server" ID="TriggerIDDropDown" SelectedValue='<%# DataBinder.Eval(Container, "_Trigger") %>' Width="90">
                                            <Items>
                                                <telerik:DropDownListItem Text="None" />
                                                <telerik:DropDownListItem Text="Email" />
                                                <telerik:DropDownListItem Text="Manuel" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="110px" HorizontalAlign="Center" />
                                    <ItemStyle Width="110px" HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridNumericColumn UniqueName="PubDateMoveDays" DataField="PubDateMoveDays" HeaderText="PubDate move days" DataType="System.String">
                                    <HeaderStyle Width="110px" HorizontalAlign="Left" />
                                    <ItemStyle Width="110px" HorizontalAlign="Center" />
                                </telerik:GridNumericColumn>

                                <telerik:GridNumericColumn UniqueName="ReleaseDelay" DataField="ReleaseDelay" HeaderText="ReleaseDelay" DataType="System.String">
                                    <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                </telerik:GridNumericColumn>
                            </Columns>                
                        </MasterTableView>
                    </telerik:RadGrid>
                     </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
         </td>
     </tr>
     <tr>
        <td>&nbsp;</td>
    </tr>
     <tr>
       <td align="center">
            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'></asp:Button>&nbsp;
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
       </td>
     </tr>
     <tr>
       <td align="center">&nbsp;</td>         
    </tr>
</table>
