using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Editors
{
    public partial class MediaTypeFieldsEditor : BaseFieldsEditor
    {
        private MediaType mediaType;

        public void SetItems(MediaType mediaType)
        {
            this.mediaType = mediaType;
            BindFieldTypeDropDown(FieldTypeDropDown);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            if(mediaType != null)
            {
                ItemList.DataSource = mediaType.Fields.OrderBy(i=>i.OrderIndex).ToList();
                ItemList.DataBind();
            }
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

            if (FieldCode.Text == "")
            {
                BasePage.DisplayErrorMessage("'FieldCode' cannot be blank");
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
                    mediaDetailField.CopyFrom(mediaTypeField);
                    
                    mediaDetailField.UseMediaTypeFieldFrontEndLayout = true;
                    mediaDetailField.UseMediaTypeFieldDescription = true;

                    if(mediaDetailField.FrontEndSubmissions == null)
                        mediaDetailField.FrontEndSubmissions = "";

                    if (mediaDetailField.FieldSettings == null)
                        mediaDetailField.FieldSettings = "";

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
                    var mediaDetailFieldValue = mediaDetailField.FieldValue;
                    mediaDetailField.CopyFrom(mediaTypeField);

                    if (mediaDetailField.UseMediaTypeFieldDescription)
                        mediaDetailField.FieldDescription = mediaTypeField.FieldDescription;

                    if (mediaDetailField.UseMediaTypeFieldFrontEndLayout)
                        mediaDetailField.FrontEndLayout = mediaTypeField.FrontEndLayout;

                    if (string.IsNullOrEmpty(mediaDetailFieldValue))
                        mediaDetailField.FieldValue = mediaTypeField.FieldValue;
                    else
                        mediaDetailField.FieldValue = mediaDetailFieldValue;


                    mediaDetailField.DateLastModified = DateTime.Now;
                }
            }

            var returnObj = MediaTypesMapper.Update(mediaType);

            if (!returnObj.IsError)
            {
                Bind();

                /*var liveMediaDetails = mediaType.MediaDetails.Where(i => i.HistoryVersionNumber == 0);

                foreach (var item in liveMediaDetails)
                {
                    item.RemoveFromCache();
                }*/

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
            mediaTypeField.FieldDescription = FieldDescription.GetValue().ToString();
            mediaTypeField.AdminControl = AdminControl.Text;
            mediaTypeField.FrontEndLayout = FrontEndLayout.Text;
            mediaTypeField.GroupName = GroupName.Text;
            mediaTypeField.RenderLabelAfterControl = RenderLabelAfterControl.Checked;
            mediaTypeField.GetAdminControlValue = GetAdminControlValue.Text;
            mediaTypeField.SetAdminControlValue = SetAdminControlValue.Text;
            mediaTypeField.ShowFrontEndFieldEditor = ShowFrontEndFieldEditor.Checked;
            mediaTypeField.FieldValue = FieldValue.Text;

            if(mediaTypeField.FrontEndSubmissions == null)
                mediaTypeField.FrontEndSubmissions = "";

            if (mediaTypeField.FieldSettings == null)
                mediaTypeField.FieldSettings = "";

            mediaTypeField.DateCreated = DateTime.Now;
            mediaTypeField.DateLastModified = DateTime.Now;
            mediaTypeField.Instructions = "";
            mediaTypeField.IsGlobalField = IsGlobalField.Checked;
        }

        private void UpdatedFieldsFromObject(MediaTypeField mediaTypeField)
        {
            FieldID.Value = mediaTypeField.ID.ToString();
            FieldCode.Text = mediaTypeField.FieldCode;
            FieldLabel.Text = mediaTypeField.FieldLabel;
            FieldDescription.SetValue(mediaTypeField.FieldDescription);
            AdminControl.Text = mediaTypeField.AdminControl;
            FieldValue.Text = mediaTypeField.FieldValue;
            FrontEndLayout.Text = mediaTypeField.FrontEndLayout;
            GroupName.Text = mediaTypeField.GroupName;
            RenderLabelAfterControl.Checked = mediaTypeField.RenderLabelAfterControl;
            GetAdminControlValue.Text = mediaTypeField.GetAdminControlValue;
            SetAdminControlValue.Text = mediaTypeField.SetAdminControlValue;
            ShowFrontEndFieldEditor.Checked = mediaTypeField.ShowFrontEndFieldEditor;
            IsGlobalField.Checked = mediaTypeField.IsGlobalField;
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

        public MediaTypeField GetDataItemFromSender(Control sender)
        {
            //var dataItemIndex = ((ItemList.PageSize * ItemList.PageIndex) +
            var dataItemIndex = ((GridViewRow)(sender).Parent.Parent).DataItemIndex;
            var dataItem = ((List<MediaTypeField>)ItemList.DataSource).ElementAt(dataItemIndex);

            return dataItem;
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            var field = GetDataItemFromSender((Control)sender);

            if (field != null)
            {
                UpdatedFieldsFromObject(field);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var field = GetDataItemFromSender((Control)sender);

            if (field!= null && field.ID != 0)
            {
                //TODO: Only un-comment for testing
                var mediaDetailFields = field.MediaDetailFields.ToList();

                foreach (var mediaDetailField in mediaDetailFields)
                {
                    var fieldAssociations = mediaDetailField.FieldAssociations.ToList();
                    foreach (var item in fieldAssociations)
                    {
                        if(!item.MediaDetail.MediaType.ShowInSiteTree)
                        {
                            var media = item.MediaDetail.Media;

                            MediaDetailsMapper.ClearObjectRelations(item.MediaDetail);                                                       
                            BaseMapper.DeleteObjectFromContext(item.MediaDetail);                            
                        }
                    }

                    BaseMapper.DeleteObjectFromContext(mediaDetailField);
                }
                // End of TODO

                BaseMapper.DeleteObjectFromContext(field);

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

        protected void CopyFields_Click(object sender, EventArgs e)
        {
            var otherMediaType = MediaTypeSelector.GetSelectedMediaType();

            foreach (var otherMediaTypeField in otherMediaType.Fields)
            {
                if(!mediaType.Fields.Any(i=>i.FieldCode == otherMediaTypeField.FieldCode))
                {
                    var mediaTypeField = new MediaTypeField();
                    mediaTypeField.CopyFrom(otherMediaTypeField);
                    mediaTypeField.DateCreated = mediaTypeField.DateLastModified = DateTime.Now;

                    mediaType.Fields.Add(mediaTypeField);

                    foreach (var mediaDetail in mediaType.MediaDetails)
                    {
                        var mediaDetailField = new MediaDetailField();
                        mediaDetailField.CopyFrom(mediaTypeField);

                        mediaDetailField.UseMediaTypeFieldFrontEndLayout = true;
                        mediaDetailField.UseMediaTypeFieldDescription = true;

                        if (mediaDetailField.FrontEndSubmissions == null)
                            mediaDetailField.FrontEndSubmissions = "";

                        if (mediaDetailField.FieldSettings == null)
                            mediaDetailField.FieldSettings = "";

                        mediaDetailField.MediaTypeField = mediaTypeField;

                        mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                        mediaDetailField.OrderIndex = mediaDetail.Fields.Count;
                        mediaDetail.Fields.Add(mediaDetailField);
                    }
                }
            }

            var returnObj = MediaTypesMapper.Update(mediaType);

            if (!returnObj.IsError)
            {
                Bind();
            }
            else
            {
                BasePage.DisplayErrorMessage("Error", returnObj.Error);
            }
        }
    }
}