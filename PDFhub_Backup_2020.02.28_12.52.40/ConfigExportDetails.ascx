<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigExportDetails.ascx.cs" Inherits="PDFhub.ConfigExportDetails" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;">
    <tr class="EditFormHeader">
        <td>
            <b>File export definition (channel):</b>
        </td>
    </tr>
    <tr>
        <td>
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
                        <telerik:RadTextBox ID="txtChannelName" runat="server" Text='<%# Bind("Name") %>' Width="200"  TabIndex="2"></telerik:RadTextBox>
                        &nbsp;&nbsp;Alias:&nbsp;&nbsp;<telerik:RadTextBox ID="txtChannelNameAlias" Text='<%# Bind("ChannelNameAlias") %>' runat="server"  TabIndex="10" Width="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">PDF Generation:</td>
                </tr>

                <tr>
                    <td>PDF type:
                    </td>
                    <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownListPDFType" runat="server" SelectedValue='<%# Bind("_PDFType") %>' >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="PDF Lowres (RGB)" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="PDF Highres (RGB)" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="PDF Print (CMYK)" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="HTML ePaper bundle" Value="3" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:120px;">Merge PDF:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbMergedPDF" runat="server" Text="" Checked='<%# Bind("MergedPDF") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td>Editions:
                    </td>
                    <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownListEditionsToGenerate" runat="server" SelectedValue='<%# Bind("_EditionsToGenerate") %>' >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="All editions" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="Ed1 only" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2+Ed3" Value="3" />
                            <telerik:DropDownListItem runat="server" Text="Ed1+Ed2+Ed3+Ed4" Value="4" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:120px;">Send common pages:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbSendCommonPages" runat="server" Text="" Checked='<%# Bind("SendCommonPages") %>' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Schedule:</td>
                </tr>
                <tr>
                    <td>Use scheduled window for sending:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbUseReleaseTime" runat="server" Text="" Checked='<%# Bind("UseReleaseTime") %>' TabIndex="8"></telerik:RadCheckBox>
                    </td>
                </tr> 
                <tr>
                    <td>Start time:
                    </td>
                    <td colspan="3">
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTime"  TabIndex="8" DbSelectedDate='<%# Bind("_ReleaseTime") %>'></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>End time:
                    </td>
                    <td colspan="3">
                        <telerik:RadTimePicker Runat="server" ID="RadTimePickerReleaseTimeEnd"  TabIndex="8" TimeView-StartTime='<%# Bind("_ReleaseTimeEnd") %>'></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Naming scheme:</td>
                </tr>
                <tr>
                    <td>Filename definition:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtTransmitNameFormat" Text='<%# Bind("TransmitNameFormat") %>' runat="server"  TabIndex="9" Width="300">
                        </telerik:RadTextBox>&nbsp;&nbsp;Date format:&nbsp;&nbsp;<telerik:RadTextBox ID="txtTransmitNameDateFormat" Text='<%# Bind("TransmitNameDateFormat") %>' runat="server"  TabIndex="10" Width="100">
                        </telerik:RadTextBox>
                    </td>
                </tr>     
                <tr>
                    <td>Subfolder definition:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtSubFolderNamingConvension" Text='<%# Bind("SubFolderNamingConvension") %>' runat="server"  TabIndex="9" Width="300">
                        </telerik:RadTextBox>
                    </td>
                </tr>     
                <tr>
                    <td>Abbreviations:
                    </td>
                    <td colspan="3">
                        <telerik:RadDropDownList ID="RadDropDownListTransmitNameUseAbbr" runat="server" SelectedValue='<%# Bind("TransmitNameUseAbbr") %>' AutoPostBack="true" >
                           <Items>
                            <telerik:DropDownListItem runat="server" Text="Long name" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="Input alias" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Output alias" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Extra alias" Value="3" />
                          </Items>                            
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Export method:</td>
                </tr>
                <tr>
                    <td>Protocol:
                    </td>
                    <td colspan="3">
                       <telerik:RadDropDownList ID="DropDownListType" runat="server" SelectedValue='<%# Bind("OutputType") %>' AutoPostBack="true" >
                          <Items>
                            <telerik:DropDownListItem runat="server" Text="SMB share" Value="0" />
                            <telerik:DropDownListItem runat="server" Text="FTP / SFTP" Value="1" />
                            <telerik:DropDownListItem runat="server" Text="Email attachment" Value="2" />
                            <telerik:DropDownListItem runat="server" Text="Google Drive" Value="3" />
                            <telerik:DropDownListItem runat="server" Text="Amazon S3" Value="4" />
                          </Items>                            
                       </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="PanelSMB" runat="server" Visible="false" GroupingText="SMB" BackColor="Snow">
                                <table>
                                    <tr>
                                        <td style="width:120px;">Output folder:
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadTextBox ID="txtOutputFolder" runat="server" Text='<%# Bind("OutputFolder") %>' TabIndex="4" Width="400">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>                                        
                                    <tr>
                                        <td>Use specific user:
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadCheckBox ID="cbSpecificUser" runat="server" Text="" Checked='<%# Bind("UseSpecificUser") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>User name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtUserName" Text='<%# Bind("UserName") %>' runat="server"  TabIndex="9">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPassword" Text='<%# Bind("Password") %>' runat="server" TextMode="Password"  TabIndex="10">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
</asp:Panel>
<asp:Panel ID="PanelFTP" runat="server" Visible="true" GroupingText="FTP/SFTP" BackColor="Snow">
                        
                                <table style="width:700px;">
                                    <tr>
                                        <td >FTP Server:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpServer" runat="server" Text='<%# Bind("FTPserver") %>' TabIndex="4" Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>FTP folder:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpFolder" runat="server" Text='<%# Bind("FTPfolder") %>' TabIndex="4" Width="300">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>   
                                   
                                    <tr>
                                        <td>FTP User name:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpUsername" Text='<%# Bind("FTPusername") %>' runat="server"  TabIndex="9">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>Password:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtFtpPassword" Text='<%# Bind("FTPPassword") %>' runat="server"   TabIndex="10">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Passive mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpPassive" runat="server" Text="" Checked='<%# Bind("FTPPasv") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                        <td>XCRC mode:
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="cbFtpXcrc" runat="server" Text="" Checked='<%# Bind("FTPXCRC") %>' TabIndex="8"></telerik:RadCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Encryption:
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadDropDownList ID="RadDropDownListEncryption" runat="server" SelectedValue='<%# Bind("FTPEncryption") %>' AutoPostBack="true" Width="250" >
                                               <Items>
                                                <telerik:DropDownListItem runat="server" Text="None (usually port 21)" Value="0" />
                                                <telerik:DropDownListItem runat="server" Text="FTPES Explicit SSL/TLS (usually port 21)" Value="1" />
                                                <telerik:DropDownListItem runat="server" Text="FTPS Implicit SSL/TLS (usually port 990)" Value="2" />
                                                <telerik:DropDownListItem runat="server" Text="SFTP - SSH (usually port 22)" Value="3" />
                                            </Items>
                            
                                            </telerik:RadDropDownList>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td>FTP post check:
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadDropDownList ID="RadDropDownListFTPPostCheck" runat="server" SelectedValue='<%# Bind("FTPPostCheck") %>' Width="250" >
                                               <Items>
                                                <telerik:DropDownListItem runat="server" Text="None" Value="0" />
                                                <telerik:DropDownListItem runat="server" Text="File exists check" Value="1" />
                                                <telerik:DropDownListItem runat="server" Text="File size check" Value="2" />
                                                <telerik:DropDownListItem runat="server" Text="File readback test" Value="3" />
                                            </Items>
                            
                                            </telerik:RadDropDownList>
                                        </td>
                                     </tr>


                                </table>
</asp:Panel>
<asp:Panel ID="PanelEmail" runat="server" Visible="false" GroupingText="Email attachments" BackColor="Snow">
</asp:Panel>
<asp:Panel ID="PanelGoogle" runat="server" Visible="false" GroupingText="Google drive" BackColor="Snow">
</asp:Panel>
<asp:Panel ID="PanelS3" runat="server" Visible="false" GroupingText="Amazon S3" BackColor="Snow">
</asp:Panel>
                    </td>
                </tr>  
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Error notification</td>
                </tr>
                <tr>
                    <td>Notification email:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbSendMail" runat="server" Text="" Checked="True" TabIndex="1"></telerik:RadCheckBox>                        
                    </td>         
                </tr>
                <tr>
                    <td>Receivers:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmailReceivers" Text="test@infralogic.dk" runat="server"  TabIndex="9">
                          </telerik:RadTextBox>
                    </td>         
                </tr>
                   <tr>
                    <td class="title" style="font-weight: bold;" colspan="4">Regular expression parsing:</td>
                </tr>
                <tr>
                    <td colspan="4">
                    <telerik:RadGrid ID="RadGridRegex" runat="server" 
                                     OnNeedDataSource="RadGridRegex_NeedDataSource"  OnItemCreated="RadGridRegex_ItemCreated"
                                     OnUpdateCommand="RadGridRegex_UpdateCommand" OnDeleteCommand="RadGridRegex_DeleteCommand" OnItemCommand="RadGridRegex_ItemCommand"
                                     AllowSorting="false" ShowGroupPanel="false" RenderMode="Auto" AutoGenerateColumns="False" 
                                     AllowFilteringByColumn="false"
                                     GridLines="Horizontal" ShowStatusBar="true" Font-Size="Small"  >
                        <StatusBarSettings LoadingText ="Getting data.." ReadyText="Updated"/>        
                        <GroupingSettings ShowUnGroupButton="false" />
                        <MasterTableView EditMode="InPlace" AllowCustomSorting="false" AllowSorting="false" TableLayout="Fixed" 
                            CommandItemDisplay="Top" EditFormSettings-PopUpSettings-KeepInScreenBounds="true" DataKeyNames="Rank">
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="true"  AddNewRecordText="Add new regular expression"  />                   
                            <Columns>                
                                <telerik:GridBoundColumn UniqueName="Rank"  DataField="Rank" HeaderText="Rank" DataType="System.Int32">
                                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="MatchExpression"  DataField="MatchExpression" HeaderText="Match expression" DataType="System.String">
                                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="FormatExpression"  DataField="FormatExpression" HeaderText="Format expression" DataType="System.String"  >
                                    <HeaderStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Comment" DataField="Comment" HeaderText="Comment" DataType="System.String">
                                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                    <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="rgRowRightAligned" />
                                </telerik:GridBoundColumn>

                                  <telerik:GridEditCommandColumn EditText="Edit">
                                      <HeaderStyle Width="80px" />
                                  </telerik:GridEditCommandColumn>

                                 <telerik:GridButtonColumn  ConfirmText="Delete this regex?" ConfirmDialogType="RadWindow"
                                                 ConfirmTitle="Delete" ButtonType="FontIconButton" Text="Delete" CommandName="Delete" >   
                                                                           <HeaderStyle Width="80px" />
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn HeaderText="Up"  CommandName="Up" ButtonType="ImageButton" ImageUrl="Images/MoveUp.gif">   
                                    <HeaderStyle Width="80px" />
                                </telerik:GridButtonColumn>
                                 <telerik:GridButtonColumn HeaderText="Down"  CommandName="Down" ButtonType="ImageButton" ImageUrl="Images/MoveDown.gif">   
                                    <HeaderStyle Width="80px" />
                                </telerik:GridButtonColumn>

                            </Columns>                
                        </MasterTableView>
                    </telerik:RadGrid>
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
