using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class ConfigMaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int keepDaysProducts = 10;
                int keepDaysLogdata = 10;
                bool useNewsPilotFeedback = true;
                string feedbackMessageFolder = "", feedbackTemplateSuccess = "", feedbackTemplateError = "", feedbackFilename = "";

                DataProviders.DBaccess db = new DataProviders.DBaccess();

                if (db.GetMaintParameters(ref keepDaysProducts, ref keepDaysLogdata,
                    ref useNewsPilotFeedback,
                    ref feedbackMessageFolder,
                    ref feedbackTemplateSuccess,
                    ref feedbackTemplateError,
                    ref feedbackFilename,
                    out string errmsg))
                {
                    RadNumericTextBoxMaxAge.Value = keepDaysProducts;
                    RadNumericTextBoxMaxAgeLogData.Value = keepDaysLogdata;
                    cbNewspilotFeedback.Checked = useNewsPilotFeedback;
                    txtFeedbackMessageFolder.Text = feedbackMessageFolder;
                    txtFeedbackTemplateSuccess.Text = feedbackTemplateSuccess;
                    txtFeedbackTemplateError.Text = feedbackTemplateError;
                    txtFeedbackFilename.Text = feedbackFilename;

                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataProviders.DBaccess db = new DataProviders.DBaccess();
            db.UpdateMaintParameters((int)RadNumericTextBoxMaxAge.Value, (int)RadNumericTextBoxMaxAgeLogData.Value,
                cbNewspilotFeedback.Checked.Value, 
                txtFeedbackMessageFolder.Text, txtFeedbackTemplateSuccess.Text, txtFeedbackTemplateError.Text, txtFeedbackFilename.Text,
                out string errmsg);
        }
    }
}