<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigMaint.aspx.cs" Inherits="PDFhub.ConfigMaint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Maintenance" CssClass="header-text" Width="300"  ></telerik:RadLabel>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;" >
     <tr>
         <td>
            <asp:Panel ID="PanelPurging" runat="server" GroupingText="Product cleanup" BackColor="Snow">
                <table>
                    <tr>
                        <td style="width:250px;">Max age for products
                        </td>
                        <td colspan="3">
                            <telerik:RadNumericTextBox ID="RadNumericTextBoxMaxAge" runat="server" MinValue="0" MaxValue="99" NumberFormat-DecimalDigits="0" Width="40" Value="2"></telerik:RadNumericTextBox>&nbsp; days after Pubdate          
                        </td>
                    </tr>
                    <tr>
                        <td style="width:250px;">Max age for logdata
                        </td>
                        <td colspan="3">
                            <telerik:RadNumericTextBox ID="RadNumericTextBoxMaxAgeLogData" runat="server" MinValue="0" MaxValue="99" NumberFormat-DecimalDigits="0" Width="40" Value="2"></telerik:RadNumericTextBox>&nbsp; days         
                        </td>
                    </tr>
                </table>
            </asp:Panel>
         </td>
     </tr>

     <tr>
        <td>
            <asp:Panel ID="PanelFeedback" runat="server" GroupingText="Newspilot feedback" BackColor="Snow">
                <table>
                    <tr>
                        <td style="width:250px;">Enable feedback
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbNewspilotFeedback" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  

                    </tr>
                    <tr>
                        <td style="width:250px;">MessageFolder
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtFeedbackMessageFolder" Text="\\newspilotfile1.schibsted.no\NP-Prod\NEWSPILOT\Import\AP\PAGEUPDATES" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                    <tr>
                        <td style="width:200px;">Template file Success
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtFeedbackTemplateSuccess" Text="\\newspinfrafile1\CCDataPDFHUB\CCConfig\MaintService\FeedbackTemplates\Success.xml" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                    <tr>
                        <td style="width:200px;">Template file Error
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtFeedbackTemplateError" Text="\\newspinfrafile1\CCDataPDFHUB\CCConfig\MaintService\FeedbackTemplates\Error.xml" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                    <tr>
                        <td style="width:200px;">Feedback filename
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtFeedbackFilename" Text="%P_%D[YYYYMMDD]_E%E_%S_1_%3N.indd" runat="server" Width="300" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                </table>
            </asp:Panel>


            <asp:Panel ID="PanelTrigger" runat="server" GroupingText="Trigger XML messages" BackColor="Snow">
                <table>
                    <tr>
                        <td style="width:250px;">Enable VisioLink trigger message
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbVisioLinkMessage" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  

                    </tr>
                    
                    <tr>
                        <td >Trigger template file
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtTriggerTemplate" Text="\\newspinfrafile1\CCDataPDFHUB\CCConfig\MaintService\TriggerTemplates\Trigger.xml" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                    <tr>
                        <td>Trigger filename
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtTriggerFileName" Text="%P.xml" runat="server" Width="300" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                </table>
            </asp:Panel>

             <asp:Panel ID="Panel2" runat="server" GroupingText="Trigger emails" BackColor="Snow">
                <table>
                    <tr>
                        <td style="width:250px;">Enable trigger emails
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbTriggerEmails" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>                    
                    <tr>
                        <td >Trigger email template
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtTriggerEmailTemplate" Text="\\newspinfrafile1\CCDataPDFHUB\CCConfig\MaintService\TriggerTemplates\TriggerMail.html" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                   
                </table>
            </asp:Panel>

             <asp:Panel ID="Panel4" runat="server" GroupingText="Pageplan info to receiver" BackColor="Snow">
                <table>
                    <tr>
                        <td style="width:250px;">Send plandata (pagecount) to receiver
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbSendPlanData" runat="server" Text="" Checked='false' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>                    
                    <tr>
                        <td >Plan data template
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtPlanTemplate" Text="\\newspinfrafile1\CCDataPDFHUB\CCConfig\MaintService\PlanTemplates\PlanTemplate.xml" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                   
                </table>
            </asp:Panel>


            <asp:Panel ID="Panel1" runat="server" GroupingText="Email error status feedback" BackColor="Snow">
                <table>  
                    <tr>
                        <td style="width:250px;">Notification on process timeout
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbEmailProcessTimeout" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td >Notification on disk low
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbEmailLowDisk" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td >Notification on database timeout
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbEmailDBdown" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td>Notification on unreachable channel
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbEmailChannelDown" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td>Error email receiver(s)
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtEmailReceiver" Text="" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server" GroupingText="Reporting" BackColor="Snow">
                <table>  
                    <tr>
                        <td style="width:250px;">Daily report (Excel)
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbDailyReport" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td>Report time (daily)
                        </td>
                        <td colspan="3">
                           <telerik:RadTimePicker Runat="server" ID="RadTimePickerReportTime"  TabIndex="8" TimeView-StartTime="5:00"></telerik:RadTimePicker>
                        </td>    
                    </tr>
                    <tr>
                        <td >Include per page details
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbReportDetails" runat="server" Text="" Checked='false' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                      <tr>
                        <td>Email reports
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbEmailReport" runat="server" Text="" Checked='false' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                    <tr>
                        <td >Email receiver(s)
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtReportReceiver" Text="" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                    
                    <tr>
                        <td>Save report to folder
                        </td>
                         <td colspan="3">
                            <telerik:RadCheckBox ID="cbReportToFolder" runat="server" Text="" Checked='false' TabIndex="1"></telerik:RadCheckBox>                        
                        </td>  
                    </tr>
                   
                    <tr>
                        <td >Report folder
                        </td>
                        <td colspan="3">
                        <telerik:RadTextBox ID="txtReportFolder" Text="" runat="server" Width="650" TabIndex="9">
                          </telerik:RadTextBox>
                        </td>    
                    </tr>
                </table>
            </asp:Panel>
        </td>
     </tr>
      <tr>
        <td colspan="3">&nbsp;</td>
    </tr>
    <tr>
       <td align="center" colspan="2">
           <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"></telerik:RadButton>&nbsp;&nbsp;
           <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel"></telerik:RadButton>
       </td>
         <td></td>
     </tr>
     <tr>
        <td colspan="3">&nbsp;</td>
    </tr>

  </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
   <div class="center-div-list">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label>

            </td>
        </tr>
    </table>
  </div>
</asp:Content>
