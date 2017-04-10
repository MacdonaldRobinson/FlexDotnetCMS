using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public class ControlsHolder
    {
        public Panel AdminPanel { get; set; }
        public Panel FrontEndPanel { get; set; }
        public PlaceHolder DynamicContent { get; set; }
    }

    public abstract class BaseFieldControl : System.Web.UI.UserControl, IFieldControl
    {
        public virtual void Page_PreRender(object sender, EventArgs e)
        {
            if (BasePage.IsInAdminSection)
            {
                RenderControlInAdmin();
            }
            else
            {
                RenderControlInFrontEnd();
            }
        }

        public long FieldID { get; set; }
        private MediaDetailField _field = null;

        public new bool IsPostBack
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod == "POST";
            }
        }

        public MediaDetailField GetField()
        {
            if (_field != null)
                return _field;

            _field = (MediaDetailField)BaseMapper.GetDataModel().Fields.Find(FieldID);

            if (_field == null)
                _field = new MediaDetailField();

            return _field;
        }

        public abstract void SetValue(object value);

        public abstract object GetValue();

        public ControlsHolder GetControlsHolder()
        {
            var adminPanel = (Panel)this.FindControl("AdminPanel");
            var frontEndPanel = (Panel)this.FindControl("FrontEndPanel");
            var dynamicContent = (PlaceHolder)this.FindControl("DynamicContent");

            if (adminPanel == null || frontEndPanel == null)
                throw new Exception("You must have 3 controls, 2 panels with the IDs 'AdminPanel' and 'FrontEndPanel' and 1 PlaceHolder control inside the frontend control called 'DynamicContent'");

            return new ControlsHolder { AdminPanel = adminPanel, FrontEndPanel = frontEndPanel, DynamicContent = dynamicContent };
        }

        public virtual void RenderControlInAdmin()
        {
            var controlsHolder = GetControlsHolder();

            controlsHolder.AdminPanel.Visible = true;
            controlsHolder.FrontEndPanel.Visible = false;
        }

        public virtual void RenderControlInFrontEnd()
        {
            var controlsHolder = GetControlsHolder();

            controlsHolder.AdminPanel.Visible = false;
            controlsHolder.FrontEndPanel.Visible = true;

            var found = GetField();

            var code = found.FrontEndLayout;

            if (found.UseMediaTypeFieldFrontEndLayout && found.MediaTypeField != null)
                code = found.MediaTypeField.FrontEndLayout;

            var data = ParserHelper.ParseData(code, found);
            data = ParserHelper.ParseData(data, found);

            controlsHolder.DynamicContent.Controls.Add(this.ParseControl(data));
        }
    }
}