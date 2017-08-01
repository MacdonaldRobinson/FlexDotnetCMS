using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Data;
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
            if(FrontEndPanel.Visible == true && IsPostBack)
            {

            }
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

            var submissions = GetField().FieldFrontEndSubmissions;
            var dataTable = StringHelper.JsonToObject<DataTable>(submissions);

            FormSubmissions.DataSource = dataTable;
            FormSubmissions.DataBind();
        }

        protected void ExportCSV_Click(object sender, EventArgs e)
        {
            var submissions = GetField().FieldFrontEndSubmissions;
            var dataTable = StringHelper.JsonToObject<DataTable>(submissions);

            var csv = dataTable.DataTableToCSV(',');

            Services.BaseService.WriteRawCSV(csv, "Submissions");
        }
    }
}