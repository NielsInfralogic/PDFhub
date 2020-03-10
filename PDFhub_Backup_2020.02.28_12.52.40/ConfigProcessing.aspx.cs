using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class ConfigProcessing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                FillPdfCmykProfileList(true);
            }
        }

        private void FillPdfCmykProfileList(bool forceReload)
        {
            RadListBoxPrintProfiles.Items.Clear();
            if (forceReload)
                Session["PDFProcessList"] = null;
            List<string> pdfProcessNames = Utils.GetPDFProcessNames();
            foreach (string s in pdfProcessNames)
            {
                if (s.IndexOf("(CMYK)") != -1)
                    RadListBoxPrintProfiles.Items.Add(s.Replace(" (CMYK)", ""));
            }
            RadListBoxPrintProfiles.SelectedIndex = 0;
            RadListBoxPrintProfiles_SelectedIndexChanged(null, null);
        }

        protected void RadListBoxPrintProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtHiddenID.Value == "99")
                return;
            Models.PDFProcess pdfProcess = Utils.GetPDFProcessFromName(RadListBoxPrintProfiles.SelectedValue);
            if (pdfProcess != null)
                SetPrintProfileToControls(pdfProcess);
        }

        private void SetPrintProfileToControls(Models.PDFProcess pdfProcess)
        {
            txtCMYKProfileName.Text = pdfProcess.ProcessName.Replace(" (CMYK)", "");
            RadDropDownListPrintProfile.SelectedText = pdfProcess.ConvertProfile.Replace(".kfpx","");
            RadDropDownListPDFVersion.SelectedValue = "0";
    
            RadRadioButtonListCMYKmethod.SelectedIndex = pdfProcess.ExternalProcess ? 1 : 0;
            txtCMYKInputFolder.Text = pdfProcess.ExternalInputFolder;
            txtCMYKOutputFolder.Text = pdfProcess.ExternalOutputFolder;
            txtCMYKErrorFolder.Text = pdfProcess.ExternalErrorFolder;
            RadNumericTextBoxCMYKTimeout.Value = pdfProcess.ProcessTimeOut;
            txtHiddenID.Value = pdfProcess.ProcessID.ToString();


        }

        protected void RadButtonCMYKNew_Click(object sender, EventArgs e)
        {
            txtCMYKProfileName.Text = "";
            RadDropDownListPrintProfile.SelectedIndex = 0;
            RadDropDownListPDFVersion.SelectedValue = "0";
            RadRadioButtonListCMYKmethod.SelectedIndex = 0;
            txtCMYKInputFolder.Text = "";
            txtCMYKErrorFolder.Text = "";
            txtCMYKErrorFolder.Text = "";
            RadNumericTextBoxCMYKTimeout.Value = 300;
            txtHiddenID.Value = "99";
            txtCMYKProfileName.Focus();
        }

        protected void RadButtonCMYKDelete_Click(object sender, EventArgs e)
        {
            Models.PDFProcess pdfProcess = Utils.GetPDFProcessFromName(RadListBoxPrintProfiles.SelectedValue);
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.DeletePDFProcess(pdfProcess.ProcessID, out string errmsg) == false)
                Utils.WriteLog(false, "db.DeletePDFProcess() - " + errmsg);
            
            FillPdfCmykProfileList(true);
        }

        protected void RadButtonCMYKSave_Click(object sender, EventArgs e)
        {
            int id = Utils.StringToInt(txtHiddenID.Value);

            Models.PDFProcess pdfProcess = null;
            if (id > 0 && id< 99)
                pdfProcess = Utils.GetPDFProcessFromID(id);

            if (pdfProcess == null)
                pdfProcess = new Models.PDFProcess();

            pdfProcess.ProcessName = txtCMYKProfileName.Text;
            pdfProcess.ConvertProfile = RadDropDownListPrintProfile.SelectedText;
            if (pdfProcess.ConvertProfile.IndexOf(".kfpx") == -1)
                pdfProcess.ConvertProfile += ".kfpx";
            pdfProcess.ExternalErrorFolder = txtCMYKErrorFolder.Text;
            //RadDropDownListPDFVersion.SelectedValue;

            pdfProcess.ExternalProcess = RadRadioButtonListCMYKmethod.SelectedIndex == 1 ? true : false;
            pdfProcess.ExternalInputFolder = txtCMYKInputFolder.Text;
            pdfProcess.ExternalOutputFolder = txtCMYKOutputFolder.Text;
            pdfProcess.ExternalErrorFolder = txtCMYKErrorFolder.Text;
            pdfProcess.ProcessTimeOut = (int)RadNumericTextBoxCMYKTimeout.Value;
            pdfProcess.ProcessType = Models.PDFProcessType.ToCMYKPDF;

            DataProviders.DBaccess db = new DataProviders.DBaccess();
            if (db.InsertUpdatePDFProcess(pdfProcess, out string errmsg) == false)
                Utils.WriteLog(false, "ERROR: db.InsertUpdatePDFProcess() - " + errmsg) ;

            FillPdfCmykProfileList(true);
        }
    }
}