using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Views.PageHandlers.MediaTypes
{
    public partial class Detail : AdvanceOptionsBasePage
    {
        private MediaType selectedItem = null;

        protected void Page_init(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = MediaTypesMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            MediaTypeFieldsEditor.SetItems(selectedItem);

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New MediaType";
            else
                return "Editing MediaType: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.MediaTypeHandler = MediaTypeHandler.GetValue().ToString();

            if (MasterPageSelector.SelectedValue != "")
                selectedItem.MasterPageID = long.Parse(MasterPageSelector.SelectedValue);
            else
                selectedItem.MasterPage = null;

            selectedItem.Label = Label.Text;
            selectedItem.IsActive = IsActive.Checked;
            selectedItem.ShowInMenu = ShowInMenu.Checked;
            selectedItem.ShowInSiteTree = ShowInSiteTree.Checked;
            selectedItem.ShowInSearchResults = ShowInSearchResults.Checked;
            selectedItem.EnableCaching = EnableCaching.Checked;
            selectedItem.AddRoles(MultiRoleSelector.GetSelectedRoles());
            selectedItem.AddChildMediaTypes(MultiMediaTypeSelector.GetSelectedMediaTypes());
            selectedItem.MainLayout = MediaDetailsMapper.ConvertATagsToShortCodes(MainLayout.Text);
            selectedItem.SummaryLayout = MediaDetailsMapper.ConvertATagsToShortCodes(SummaryLayout.Text);
            selectedItem.FeaturedLayout = MediaDetailsMapper.ConvertATagsToShortCodes(FeaturedLayout.Text);
            selectedItem.UseMediaTypeLayouts = UseMediaTypeLayouts.Checked;
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            MediaTypeHandler.SetValue(selectedItem.MediaTypeHandler);

            if (selectedItem.MasterPage != null)
                MasterPageSelector.SelectedValue = selectedItem.MasterPage.ID.ToString();

            Label.Text = selectedItem.Label;
            IsActive.Checked = selectedItem.IsActive;
            ShowInMenu.Checked = selectedItem.ShowInMenu;
            ShowInSiteTree.Checked = selectedItem.ShowInSiteTree;

            ShowInSearchResults.Checked = selectedItem.ShowInSearchResults;
            EnableCaching.Checked = selectedItem.EnableCaching;
            MainLayout.Text = selectedItem.MainLayout;
            SummaryLayout.Text = selectedItem.SummaryLayout;
            FeaturedLayout.Text = selectedItem.FeaturedLayout;
            UseMediaTypeLayouts.Checked = selectedItem.UseMediaTypeLayouts;

            MultiRoleSelector.SetSelectedRoles(selectedItem.GetRoles());
            MultiMediaTypeSelector.SetMediaTypes(selectedItem.MediaTypes.ToList());
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = MediaTypesMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<MediaType>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj = selectedItem.Validate();

            if (!returnObj.IsError)
            {
                if (selectedItem.ID == 0)
                    returnObj = MediaTypesMapper.Insert(selectedItem);
                else
                    returnObj = MediaTypesMapper.Update(selectedItem);
            }

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                var mediaDetails = selectedItem.MediaDetails.Where(i => i.HistoryForMediaDetail == null);

                foreach (var mediaDetail in mediaDetails)
                {
                    mediaDetail.RemoveFromCache();
                }

                DisplaySuccessMessage("Successfully Saved Item");
            }
        }
    }
}