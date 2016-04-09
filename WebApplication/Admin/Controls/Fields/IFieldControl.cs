using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public interface IFieldControl
    {
        void RenderControlInAdmin();

        void RenderControlInFrontEnd();

        long FieldID { get; set; }

        void SetValue(object value);

        object GetValue();
    }
}