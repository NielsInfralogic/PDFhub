<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigImportDetails.ascx.cs" Inherits="PDFhub.ConfigImportDetails" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <script type="text/javascript">
        function OnClientCheckedChangedX(sender, args)
        {   
            var txtControl = $find("<%=txtEmailReceivers.ClientID %>")
            if (txtControl != null)
                txtControl.set_enabled(sender.get_checked());
        }
    </script>

<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;">
    <tr class="EditFormHeader">
        <td>
            <b>Plan import definition:</b>
        </td>
    </tr>
      <tr>
        <td>
            <table id="Table3" border="0" class="module">
                <tr>
                    <td style="width:120px;">Enabled:
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
                    <td style="width:120px;">Input folder:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtInputFolder" runat="server" Text='<%# Bind("InputFolder") %>' TabIndex="4" Width="400">
                        </telerik:RadTextBox>
                    </td>
                </tr> 
                <tr>
                    <td style="width:120px;">Done folder:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtDoneFolder" runat="server" Text='<%# Bind("DoneFolder") %>' TabIndex="4" Width="400">
                        </telerik:RadTextBox>
                    </td>
                </tr> 
                <tr>
                    <td style="width:120px;">Error folder:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtErrorFolder" runat="server" Text='<%# Bind("ErrorFolder") %>' TabIndex="4" Width="400">
                        </telerik:RadTextBox>
                    </td>
                </tr> 
                 <tr>
                    <td style="width:120px;">Log folder:
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
