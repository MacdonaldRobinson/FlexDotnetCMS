using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public partial class MediaTypeFieldsEditor : BaseFieldsEditor
    {
        private MediaType mediaType;

        public void SetItems(MediaType mediaType)
        {
            this.mediaType = mediaType;
            Bind();
        }

        private void Bind()
        {
            if(mediaType != null)
            {
                ItemList.DataSource = mediaType.Fields.ToList();
                ItemList.DataBind();
            }

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
                Update.Text = "Add Field";
            }
            else
            {
                FieldDetailsTitle.Text = "Currently Editing Field ID: " + FieldID.Value;
                Update.Text = "Save Field";
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (mediaType.ID == 0)
            {
                BasePage.DisplayErrorMessage("You can only add fields once you have created the page");
                return;
            }
            var fieldId = long.Parse(FieldID.Value);

            if (fieldId == 0)
            {
                var mediaTypeField = new MediaTypeField();
                UpdatedObjectFromFields(mediaTypeField);
                mediaType.Fields.Add(mediaTypeField);

                foreach (var mediaDetail in mediaType.MediaDetails)
                {
                    var mediaDetailField = new MediaDetailField();
                    mediaDetailField.FieldCode = mediaTypeField.FieldCode;
                    mediaDetailField.FieldLabel = mediaTypeField.FieldLabel;
                    mediaDetailField.FieldValue = mediaTypeField.FieldValue;
                    mediaDetailField.AdminControl = mediaTypeField.AdminControl;
                    mediaDetailField.FrontEndLayout = mediaTypeField.FrontEndLayout;
                    mediaDetailField.GroupName = mediaTypeField.GroupName;
                    mediaDetailField.RenderLabelAfterControl = mediaTypeField.RenderLabelAfterControl;
                    mediaDetailField.UseMediaTypeFieldFrontEndLayout = true;

                    mediaDetailField.GetAdminControlValue = mediaTypeField.GetAdminControlValue;
                    mediaDetailField.SetAdminControlValue = mediaTypeField.SetAdminControlValue;

                    mediaDetailField.MediaTypeField = mediaTypeField;

                    mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                    mediaDetailField.OrderIndex = mediaDetail.Fields.Count;
                    mediaDetail.Fields.Add(mediaDetailField);
                }
            }
            else
            {
                var mediaTypeField = mediaType.Fields.SingleOrDefault(i => i.ID == fieldId);
                var oldFieldCode = mediaTypeField.FieldCode;

                UpdatedObjectFromFields(mediaTypeField);

                foreach (var mediaDetailField in mediaTypeField.MediaDetailFields)
                {
                    mediaDetailField.FieldCode = mediaTypeField.FieldCode;
                    mediaDetailField.FieldLabel = mediaTypeField.FieldLabel;
                    mediaDetailField.AdminControl = mediaTypeField.AdminControl;

                    if(mediaDetailField.UseMediaTypeFieldFrontEndLayout)
                        mediaDetailField.FrontEndLayout = mediaTypeField.FrontEndLayout;

                    mediaDetailField.GroupName = mediaTypeField.GroupName;
                    mediaDetailField.RenderLabelAfterControl = mediaTypeField.RenderLabelAfterControl;

                    if (string.IsNullOrEmpty(mediaDetailField.FieldValue))
                        mediaDetailField.FieldValue = mediaTypeField.FieldValue;

                    mediaDetailField.GetAdminControlValue = mediaTypeField.GetAdminControlValue;
                    mediaDetailField.SetAdminControlValue = mediaTypeField.SetAdminControlValue;
                    mediaDetailField.DateLastModified = DateTime.Now;
                }
            }

            var returnObj = MediaTypesMapper.Update(mediaType);

            if (!returnObj.IsError)
            {
                Bind();

                foreach (var item in mediaType.MediaDetails)
                {
                    item.RemoveFromCache();
                }

            }
            else
            {
                BasePage.DisplayErrorMessage("Error", returnObj.Error);
            }
        }

        private void UpdatedObjectFromFields(MediaTypeField mediaTypeField)
        {
            mediaTypeField.MediaTypeID = mediaType.ID;
            mediaTypeField.FieldCode = FieldCode.Text;
            mediaTypeField.FieldLabel = FieldLabel.Text;
            mediaTypeField.AdminControl = AdminControl.Text;
            mediaTypeField.FrontEndLayout = FrontEndLayout.Text;
            mediaTypeField.GroupName = GroupName.Text;
            mediaTypeField.RenderLabelAfterControl = RenderLabelAfterControl.Checked;
            mediaTypeField.GetAdminControlValue = GetAdminControlValue.Text;
            mediaTypeField.SetAdminControlValue = SetAdminControlValue.Text;
            mediaTypeField.FieldValue = FieldValue.Text;
            mediaTypeField.DateCreated = DateTime.Now;
            mediaTypeField.DateLastModified = DateTime.Now;
            mediaTypeField.Instructions = "";
        }

        private void UpdatedFieldsFromObject(MediaTypeField mediaTypeField)
        {
            FieldID.Value = mediaTypeField.ID.ToString();
            FieldCode.Text = mediaTypeField.FieldCode;
            FieldLabel.Text = mediaTypeField.FieldLabel;
            AdminControl.Text = mediaTypeField.AdminControl;
            FieldValue.Text = mediaTypeField.FieldValue;
            FrontEndLayout.Text = mediaTypeField.FrontEndLayout;
            GroupName.Text = mediaTypeField.GroupName;
            RenderLabelAfterControl.Checked = mediaTypeField.RenderLabelAfterControl;
            GetAdminControlValue.Text = mediaTypeField.GetAdminControlValue;
            SetAdminControlValue.Text = mediaTypeField.SetAdminControlValue;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            UpdatedFieldsFromObject(new MediaTypeField());
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
                var field = mediaType.Fields.SingleOrDefault(i => i.ID == long.Parse(id));

                if (field != null)
                    UpdatedFieldsFromObject(field);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                var field = mediaType.Fields.SingleOrDefault(i => i.ID == long.Parse(id));

                if (field != null)
                {
                    BaseMapper.DeleteObjectFromContext(field);

                    var mediaTypesFields = BaseMapper.GetDataModel().Fields.Where(i => i.ID == field.ID);

                    foreach (var mediaTypesField in mediaTypesFields)
                    {
                        BaseMapper.DeleteObjectFromContext((MediaTypeField)mediaTypesField);
                    }

                    var returnObj = MediaTypesMapper.Update(mediaType);

                    if (!returnObj.IsError)
                    {
                        UpdatedFieldsFromObject(new MediaTypeField());
                        Bind();
                    }
                    else
                    {
                        BasePage.DisplayErrorMessage("Error", returnObj.Error);
                    }
                }
            }
        }
    }
}