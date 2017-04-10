using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FieldEditor
{
    public partial class Default : AdminBasePage
    {
        public IField Field { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            var fieldIdStr = Request["fieldId"];

            if (!string.IsNullOrEmpty(fieldIdStr))
            {
                var fieldId = long.Parse(fieldIdStr);
                var field = FieldsMapper.GetByID(fieldId);

                if (field != null)
                {
                    Field = field;
                    LoadField();
                }
            }
        }

        public void LoadField()
        {
            if (Field == null)
                return;

            var dynamicField = this.ParseControl(Field.AdminControl);

            if (dynamicField.Controls.Count == 0)
                return;

            var control = dynamicField.Controls[0];

            var fieldValue = Field.FieldValue.Replace("{BaseUrl}", URIHelper.BaseUrl);

            if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
            {
                var ctrl = ((WebApplication.Admin.Controls.Fields.IFieldControl)control);
                ctrl.FieldID = Field.ID;
                ctrl.SetValue(fieldValue);
            }
            else
            {
                if (Field.FieldValue != "{" + Field.SetAdminControlValue + "}")
                {
                    if (Field.SetAdminControlValue.Contains("@"))
                    {
                        try
                        {
                            var returnData = ParserHelper.ParseData(Field.SetAdminControlValue, new { Control = control, Field = Field, NewValue = fieldValue });
                        }
                        catch (Exception ex)
                        {
                            DisplayErrorMessage("Error", ErrorHelper.CreateError(ex));
                        }
                    }
                    else
                    {
                        ParserHelper.SetValue(control, Field.SetAdminControlValue, fieldValue);
                    }
                }
            }

            DynamicField.Controls.Add(control);
        }

        public void SaveField()
        {
            if (DynamicField.Controls.Count == 0)
                return;

            var control = DynamicField.Controls[0];
            
            if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
            {
                var ctrl = ((WebApplication.Admin.Controls.Fields.IFieldControl)control);
                ctrl.SetValue(ctrl.GetValue());
                Field.FieldValue = ctrl.GetValue().ToString();
            }
            else
            {
                try
                {
                    Field.FieldValue = ParserHelper.ParseData("{"+Field.GetAdminControlValue+"}", control);
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage("Error", ErrorHelper.CreateError(ex));
                }
            }

            var returnObj = FieldsMapper.Update(Field);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {            
            SaveField();
        }
    }
}