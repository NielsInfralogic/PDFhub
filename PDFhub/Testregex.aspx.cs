using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDFhub
{
    public partial class Testregex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Models.RegExpression> regExpressions = (List<Models.RegExpression>)Session["RegexListToTest"];
            
        }
    }
}