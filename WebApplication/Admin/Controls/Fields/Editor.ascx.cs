using FrameworkLibrary;
using System;
using System.Web.UI.WebControls;
using WebApplication.Admin;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class Editor : BaseFieldControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            if (Height.Value != 0.0)
            {
                Instance.Height = Height;
            }

            base.OnPreRender(e);
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
            return Instance.Text;
        }

        public override void SetValue(object value)
        {
            Instance.Text = value.ToString();
        }

        public Unit Height { get; set; }
    }
}