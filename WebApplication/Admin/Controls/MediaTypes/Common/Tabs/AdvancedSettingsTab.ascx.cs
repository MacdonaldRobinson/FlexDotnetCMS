using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class AdvancedSettingsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;

            if (selectedItem.ID != 0)
            {
                var allMediaTypes = MediaTypesMapper.GetDataModel().MediaTypes.Where(i => i.IsActive);
                var allowedMediaTypes = new List<MediaType>();

                foreach (var mediaType in allMediaTypes)
                {
                    MediaTypeEnum mediaTypeEnum = MediaTypeEnum.Page;
                    Enum.TryParse(mediaType.Name, out mediaTypeEnum);

                    var tmpMediaDetail = MediaDetailsMapper.CreateByMediaTypeEnum(mediaTypeEnum);
                    if (tmpMediaDetail.GetType() == selectedItem.GetType())
                    {
                        allowedMediaTypes.Add(mediaType);
                    }
                }

                MediaTypes.DataSource = allowedMediaTypes;
                MediaTypes.DataTextField = "Name";
                MediaTypes.DataValueField = "ID";
                MediaTypes.DataBind();
            }

            //UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            MediaDetailID.Text = selectedItem.ID.ToString();
            MediaID.Text = selectedItem.MediaID.ToString();

            Handler.SetValue(selectedItem.Handler);
            MediaType.Text = MediaTypesMapper.GetByID(selectedItem.MediaTypeID).Name.ToString();
            EnableCaching.Checked = selectedItem.EnableCaching;

            if (selectedItem.LastUpdatedByUserID != 0)
                LastModifiedByUser.Text = UsersMapper.GetByID(selectedItem.LastUpdatedByUserID).UserName;

            if (selectedItem.CreatedByUserID != 0)
                CreatedByUser.Text = UsersMapper.GetByID(selectedItem.CreatedByUserID).UserName;

            if (selectedItem.ID != 0)
                OrderIndex.Text = MediasMapper.GetByMediaDetail(selectedItem).OrderIndex.ToString();

            MasterPageSelector.SelectedValue = selectedItem.MasterPageID.ToString();
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.Handler = Handler.GetValue().ToString();
            selectedItem.EnableCaching = EnableCaching.Checked;

            if (MasterPageSelector.SelectedValue != "")
                selectedItem.MasterPageID = long.Parse(MasterPageSelector.SelectedValue);
            else
                selectedItem.MasterPageID = null;
        }

        protected void MediaTypeChange_Click(object sender, System.EventArgs e)
        {
            var mewMediaTypeId = long.Parse(MediaTypes.SelectedValue);

            var newMediaType = MediaTypesMapper.GetDataModel().MediaTypes.FirstOrDefault(i => i.ID == mewMediaTypeId);

            if ((newMediaType != null) && (newMediaType.ID != selectedItem.MediaType.ID))
            {
                selectedItem.MediaTypeID = newMediaType.ID;
                selectedItem.ShowInMenu = newMediaType.ShowInMenu;
                selectedItem.ShowInSearchResults = newMediaType.ShowInSearchResults;

                foreach (var mediaTypeField in newMediaType.Fields)
                {
                    var foundField = selectedItem.Fields.SingleOrDefault(i => i.FieldCode == mediaTypeField.FieldCode);

                    if (foundField != null)
                        continue;

                    var newField = new MediaDetailField();
                    newField.FieldCode = mediaTypeField.FieldCode;
                    newField.FieldLabel = mediaTypeField.FieldLabel;
                    newField.AdminControl = mediaTypeField.AdminControl;
                    newField.GetAdminControlValue = mediaTypeField.GetAdminControlValue;
                    newField.SetAdminControlValue = mediaTypeField.SetAdminControlValue;
                    newField.FieldValue = mediaTypeField.FieldValue;

                    newField.DateCreated = DateTime.Now;
                    newField.DateLastModified = DateTime.Now;

                    selectedItem.Fields.Add(newField);
                }

                if (selectedItem.UseMediaTypeLayouts)
                {
                    selectedItem.MainLayout = newMediaType.MainLayout;
                    selectedItem.FeaturedLayout = newMediaType.FeaturedLayout;
                    selectedItem.SummaryLayout = newMediaType.SummaryLayout;
                }

                var returnObj = MediaDetailsMapper.Update(selectedItem);

                if (returnObj.IsError)
                {
                    BasePage.DisplayErrorMessage("Error", returnObj.Error);
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
        }
    }
}