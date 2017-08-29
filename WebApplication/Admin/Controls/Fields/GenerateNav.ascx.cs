using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class GenerateNav :  BaseFieldControl
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
            return FieldValue.Value;
        }

        public override void SetValue(object value)
        {
            FieldValue.Value = value.ToString();

            var field = GetField();           
        }        

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}