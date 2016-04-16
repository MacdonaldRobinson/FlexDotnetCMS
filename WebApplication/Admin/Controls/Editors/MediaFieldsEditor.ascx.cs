using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public partial class MediaFieldsEditor : BaseFieldsEditor
    {
        private IMediaDetail mediaDetail;

        public void SetItems(IMediaDetail mediaDetail)
        {
            this.mediaDetail = mediaDetail;
            Bind();
        }

        private void Bind()
        {
            ItemList.DataSource = mediaDetail.Fields.OrderBy(i => i.OrderIndex).ToList();
            ItemList.DataBind();

            BindFieldTypeDropDown(FieldTypeDropDown);
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (FieldID.Value == "0")
            {
                FieldDetailsTitle.Text = "Create a new field";
                Update.Text = "Add";
            }
            else
            {
                FieldDetailsTitle.Text = "Currently Editing Field ID: " + FieldID.Value;
                Update.Text = "Save";
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (mediaDetail.ID == 0)
            {
                BasePage.DisplayErrorMessage("You can only add fields once you have created the page");
                return;
            }
            var fieldId = long.Parse(FieldID.Value);

            if (fieldId == 0)
            {
                var mediaField = new MediaDetailField();
                UpdatedObjectFromFields(mediaField);

                mediaField.OrderIndex = mediaDetail.Fields.Count;

                mediaDetail.Fields.Add(mediaField);
            }
            else
            {
                var mediaField = mediaDetail.Fields.SingleOrDefault(i => i.ID == fieldId);
                UpdatedObjectFromFields(mediaField);
            }

            var returnObj = MediaDetailsMapper.Update(mediaDetail);

            if (!returnObj.IsError)
            {
                Bind();                
            }
            else
            {
                BasePage.DisplayErrorMessage("Error", returnObj.Error);
            }
        }

        private void UpdatedObjectFromFields(MediaDetailField mediaField)
        {
            mediaField.MediaDetailID = mediaDetail.ID;
            mediaField.FieldCode = FieldCode.Text;
            mediaField.FieldLabel = FieldLabel.Text;
            mediaField.AdminControl = AdminControl.Text;
            mediaField.FieldValue = ParserHelper.ParseData(FieldValue.Text, BasePage.TemplateVars, true);
            mediaField.GroupName = GroupName.Text;
            mediaField.RenderLabelAfterControl = RenderLabelAfterControl.Checked;
            mediaField.GetAdminControlValue = GetAdminControlValue.Text;
            mediaField.SetAdminControlValue = SetAdminControlValue.Text;
            mediaField.FrontEndLayout = FrontEndLayout.Text;
            mediaField.UseMediaTypeFieldFrontEndLayout = UseMediaTypeFieldFrontEndLayout.Checked;

            mediaField.DateCreated = DateTime.Now;
            mediaField.DateLastModified = DateTime.Now;
        }

        private void UpdatedFieldsFromObject(MediaDetailField mediaField)
        {
            FieldID.Value = mediaField.ID.ToString();
            FieldCode.Text = mediaField.FieldCode;
            FieldLabel.Text = mediaField.FieldLabel;
            AdminControl.Text = mediaField.AdminControl;
            FieldValue.Text = ParserHelper.ParseData(mediaField.FieldValue, BasePage.TemplateVars);
            GroupName.Text = mediaField.GroupName;
            RenderLabelAfterControl.Checked = mediaField.RenderLabelAfterControl;
            FrontEndLayout.Text = mediaField.FrontEndLayout;
            GetAdminControlValue.Text = mediaField.GetAdminControlValue;
            SetAdminControlValue.Text = mediaField.SetAdminControlValue;
            UseMediaTypeFieldFrontEndLayout.Checked = mediaField.UseMediaTypeFieldFrontEndLayout;
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                var field = mediaDetail.Fields.SingleOrDefault(i => i.ID == long.Parse(id));

                if (field != null)
                    UpdatedFieldsFromObject(field);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                var field = mediaDetail.Fields.SingleOrDefault(i => i.ID == long.Parse(id));

                if (field != null)
                {
                    var mediaDetailFields = BaseMapper.GetDataModel().Fields.Where(i => i.ID == field.ID);

                    foreach (MediaDetailField mediaDetailField in mediaDetailFields)
                    {
                        var fieldFiles = mediaDetailField.FieldFiles.ToList();
                        var fieldAssociations = mediaDetailField.FieldAssociations.ToList();

                        foreach (var fieldFile in fieldFiles)
                        {
                            BaseMapper.DeleteObjectFromContext(fieldFile);
                        }

                        foreach (var fieldAssociation in fieldAssociations)
                        {
                            BaseMapper.GetDataModel().FieldAssociations.Remove(fieldAssociation);
                        }

                        BaseMapper.DeleteObjectFromContext(mediaDetailField);
                    }

                    var returnObj = MediaDetailsMapper.Update(mediaDetail);

                    if (!returnObj.IsError)
                    {
                        UpdatedFieldsFromObject(new MediaDetailField());
                        Bind();
                    }
                    else
                    {
                        BasePage.DisplayErrorMessage("Error", returnObj.Error);
                    }
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            UpdatedFieldsFromObject(new MediaDetailField());
        }
    }
}