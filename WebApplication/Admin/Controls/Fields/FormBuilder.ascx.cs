using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class FormBuilder : BaseFieldControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {            
        }       

        public override void RenderControlInAdmin()
        {
            base.RenderControlInAdmin();
        }

        public override void RenderControlInFrontEnd()
        {
            base.RenderControlInFrontEnd();
        }

        public override object GetValue()
        {
            return FormBuilderData.Value;
        }

        public override void SetValue(object value)
        {
            FormBuilderData.Value = value.ToString();
        }
    }
}