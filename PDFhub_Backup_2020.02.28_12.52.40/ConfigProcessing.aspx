<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageSingleMenu.Master" AutoEventWireup="true" CodeBehind="ConfigProcessing.aspx.cs" Inherits="PDFhub.ConfigProcessing"   %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table  class="jumbotron"  style="width:100%;">
        <tr>
            <td style="width:100px;">
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="PDF processing" CssClass="header-text" Width="300"  ></telerik:RadLabel>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server"  >
<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;background-color:ghostwhite;" >
   
     <tr>
        <td style="width:600px;vertical-align:top;">
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="PanelLowRes" runat="server" GroupingText="Lowres PDF generation" BackColor="Snow">
                            <table>
                                <tr>
                                    <td style="width:200px;">Convert profile:
                                    </td>
                                       <td colspan="3">
                                        <telerik:RadDropDownList ID="RadDropDownListLowresProfile" runat="server" SelectedValue='0' AutoPostBack="true" Width="400" >
                                           <Items>
                                            <telerik:DropDownListItem runat="server" Text="Online publishing 96dpi RGB" Value="0" />
                                            <telerik:DropDownListItem runat="server" Text="Convert to sRGB" Value="1" />
                                        </Items>                            
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                <asp:Panel ID="PanelPDFPrint" runat="server" GroupingText="Print PDF generation" BackColor="Snow">
                <table>
                    <tr>
                         <td style="width:200px; vertical-align:top;">Available CMYK conv:
                        </td>
                        <td >
                            <telerik:RadListBox ID="RadListBoxPrintProfiles" runat="server" OnSelectedIndexChanged="RadListBoxPrintProfiles_SelectedIndexChanged" Height="120" AutoPostBack="true" Width="350"></telerik:RadListBox>
                        </td>
                          <td style="vertical-align:top; text-align:left;">  
                              <table>
                                  <tr><td>
                                    <telerik:RadButton ID="RadButtonCMYKNew" runat="server" Text="New" OnClick="RadButtonCMYKNew_Click" ></telerik:RadButton>
                                  </td></tr>
                                  <tr><td>
                                    <telerik:RadButton ID="RadButtonCMYKDelete" runat="server" Text="Delete" OnClick="RadButtonCMYKDelete_Click"></telerik:RadButton>
                                  </td></tr>
                                    <tr><td>
                                    <telerik:RadButton ID="RadButtonCMYKSave" runat="server" Text="Save" OnClick="RadButtonCMYKSave_Click"></telerik:RadButton>
                                  </td></tr>
                                  </table>
                          </td>
                        <td></td>
                    </tr>
                        <tr>
                        <td>Profile name</td>
                        <td colspan="3">
                           <telerik:RadTextBox ID="txtCMYKProfileName" runat="server" Text="" TabIndex="4" Width="270"></telerik:RadTextBox>
                            <asp:HiddenField runat="server" ID="txtHiddenID"  />
                         </td>
                    </tr>



                    <tr>
                        <td style="width:200px;">Convert preset:
                        </td>
                        <td colspan="3">
                            <telerik:RadDropDownList ID="RadDropDownListPrintProfile" runat="server" AutoPostBack="true" Width="400" >
                                <Items>
                                    <telerik:DropDownListItem runat="server" Text="Convert to CMYK only (ISOnewspaper 26 (IFRA))" Value="0" />
                                    <telerik:DropDownListItem runat="server" Text="Convert to CMYK only (PSO Coated 300prct NP (ECI))" Value="1" />
                                    <telerik:DropDownListItem runat="server" Text="Convert to CMYK only (PSO LWC Improved (ECI))" Value="2" />
                                    </Items>                            
                            </telerik:RadDropDownList>
                        </td>                   
                    </tr>
                    <tr>
                        <td>PDF version:</td>
                        <td colspan="3">
                            <telerik:RadDropDownList ID="RadDropDownListPDFVersion" runat="server" SelectedValue='0' AutoPostBack="true" >
                               <Items>
                                    <telerik:DropDownListItem runat="server" Text="Keep version" Value="0" />
                                    <telerik:DropDownListItem runat="server" Text="PDF v1.3" Value="1" />
                                    <telerik:DropDownListItem runat="server" Text="PDF v1.4" Value="2" />
                                    <telerik:DropDownListItem runat="server" Text="PDF v1.5" Value="3" />
                                    <telerik:DropDownListItem runat="server" Text="PDF v1.6" Value="4" />
                                    <telerik:DropDownListItem runat="server" Text="PDF v1.7" Value="5" />
                                </Items>                          
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr style="vertical-align:top;">
                        <td >CMYK process method:</td>
                        <td colspan="3">
                            <telerik:RadRadioButtonList ID="RadRadioButtonListCMYKmethod" runat="server">
                                <Items>
                                    <telerik:ButtonListItem Text="callas PDF Library" Selected="true" />
                                    <telerik:ButtonListItem Text="External (ColorFactory/Claro)" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Input folder:</td>
                        <td colspan="3">
                           <telerik:RadTextBox ID="txtCMYKInputFolder" runat="server" Text="" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                         </td>
                    </tr>
                    <tr>
                        <td>Output folder:
                        </td>
                        <td colspan="3">
                          <telerik:RadTextBox ID="txtCMYKOutputFolder" runat="server" Text="" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                        </td>
                    </tr>
                    <tr>
                        <td>Error folder:
                        </td>
                        <td colspan="3">
                          <telerik:RadTextBox ID="txtCMYKErrorFolder" runat="server" Text="" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                        </td>
                    </tr>
                    <tr>
                        <td>Process timeout:
                        </td>
                        <td colspan="3">
                            <telerik:RadNumericTextBox ID="RadNumericTextBoxCMYKTimeout" runat="server" MinValue="20" MaxValue="9999" TabIndex="6" Width="70" NumberFormat-DecimalDigits="0" Value="300">
                            </telerik:RadNumericTextBox>&nbsp;&nbsp;sec.                      
                         </td>
                    </tr>
                </table>
            </asp:Panel>
 
                    </td>
                </tr>
            </table>
        </td>

        <td style="width:600px;vertical-align:top;">
             <table>
                <tr>
                    <td>
            <asp:Panel ID="PanelProofing" runat="server" GroupingText="Proof generation" BackColor="Snow" >
                <table>
                    <tr>
                        <td style="width:200px;">Proof resolution:</td>
                        <td colspan="3"><telerik:RadNumericTextBox ID="RadNumericProofResolution" runat="server" MinValue="20" MaxValue="999" TabIndex="6" Width="70" NumberFormat-DecimalDigits="0"  Value="72" Skin="Silk">
                             </telerik:RadNumericTextBox>&nbsp;&nbsp;dpi</td>
                    </tr>
                    <tr>
                        <td>Proof color emulation:</td>
                        <td colspan="3">
                            <telerik:RadDropDownList ID="DropDownListProofICC" runat="server" SelectedValue='0' AutoPostBack="true" >
                               <Items>
                                <telerik:DropDownListItem runat="server" Text="ISONewspaper26v4" Value="0" />
                                <telerik:DropDownListItem runat="server" Text="ISONewspaper26v5" Value="1" />
                                <telerik:DropDownListItem runat="server" Text="ISOcoated_v2_eci" Value="2" />
                            </Items>
                            
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">Proof method:</td>
                        <td colspan="3">
                            <telerik:RadRadioButtonList  ID="RadRadioButtonListRIP" runat="server">
                                <Items>
                                    <telerik:ButtonListItem Text="callas PDF Library"  />
                                    <telerik:ButtonListItem Text="GhostScript (external)" Selected="true" />
                                    <telerik:ButtonListItem Text="Harlequin RIP (external)" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Input folder:</td>
                        <td colspan="3">
                          <telerik:RadTextBox ID="txtProofInputFolder" runat="server" Text="\\SCH-NO-PDFHUB01\PreviewLQ\In" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                         </td>
                    </tr>
                    <tr>
                        <td>Output folder:</td>
                        <td colspan="3">
                          <telerik:RadTextBox ID="txtProofOutputFolder" runat="server" Text="\\SCH-NO-PDFHUB01\PreviewLQ\Out" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                         </td>
                    </tr>
                    <tr>
                        <td>Error folder:</td>
                        <td colspan="3">
                          <telerik:RadTextBox ID="txtProofErrorFolder" runat="server" Text="\\SCH-NO-PDFHUB01\PreviewLQ\Error" TabIndex="4" Width="400"></telerik:RadTextBox>                                    
                         </td>
                    </tr>
                    <tr>
                        <td>Process timeout:
                        </td>
                        <td colspan="3">
                            <telerik:RadNumericTextBox ID="RadNumericTextBoxProofTimeout" runat="server" MinValue="20" MaxValue="9999" TabIndex="6"  Width="70" NumberFormat-DecimalDigits="0" Value="300">
                            </telerik:RadNumericTextBox>&nbsp;&nbsp;sec.                      
                         </td>
                    </tr>
         
                </table>
            </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
            <asp:Panel ID="Panel1" runat="server" GroupingText="Error notification" BackColor="Snow">
                <table>
                    <tr>
                        <td>Send notification email:
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="cbSendMail" runat="server" Text="" Checked='true' TabIndex="1"></telerik:RadCheckBox>                        
                    </td>  
                    </tr>
                    <tr>
                    <td>Receivers:
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmailReceivers" Text="test@infralogic.dk" runat="server" Width="400"  TabIndex="9">
                          </telerik:RadTextBox>
                    </td>         
                </tr>
                </table>
            </asp:Panel>
                    </td>
                </tr>
            </table> 
       </td>
         <td></td>
    
    </tr>
     <tr>
        <td colspan="3">&nbsp;</td>
    </tr>
    <tr>
       <td align="center" colspan="2">
           <telerik:RadButton ID="btnSave" runat="server" Text="Save"></telerik:RadButton>&nbsp;&nbsp;
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
