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

            if (dataTable != null)
            {
                dataTable.DefaultView.Sort = "DateSubmitted DESC";

                FormSubmissions.DataSource = dataTable;
                FormSubmissions.DataBind();
            }
        }

        protected void ExportCSV_Click(object sender, EventArgs e)
        {
            var newDataTable = new DataTable();
            var dataTable = (FormSubmissions.DataSource as DataTable);

            newDataTable = dataTable.DefaultView.ToTable();

            var csv = newDataTable.DataTableToCSV(',');

            Services.BaseService.WriteRawCSV(csv, "Submissions");
        }

        protected void ClearAllSubmissions_Click(object sender, EventArgs e)
        {
            var field = GetField();
            field.FieldFrontEndSubmissions = "";

            var returnObj = FieldsMapper.Update(field);

            if (!returnObj.IsError)
            {
                FormSubmissions.DataSource = new DataTable();
                FormSubmissions.DataBind();
            }
        }
        protected void FormSubmissions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FormSubmissions.PageIndex = e.NewPageIndex;

            var dataTable = (FormSubmissions.DataSource as DataTable);

            FormSubmissions.DataSource = dataTable;
            FormSubmissions.DataBind();
        }
    }
}