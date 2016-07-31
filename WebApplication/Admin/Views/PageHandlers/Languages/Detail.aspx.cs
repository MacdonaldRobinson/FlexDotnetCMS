using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Views.Languages
{
    public partial class Detail : AdvanceOptionsBasePage
    {
        private Language selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = LanguagesMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New Language";
            else
                return "Editing Language: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.DisplayName = Name.Text;
            selectedItem.CultureCode = CultureCode.Text;
            selectedItem.UriSegment = UriSegment.Text;
            selectedItem.Description = Description.Text;
            selectedItem.IsActive = IsActive.Checked;
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            CultureCode.Text = selectedItem.CultureCode;
            UriSegment.Text = selectedItem.UriSegment;
            Description.Text = selectedItem.Description;
            IsActive.Checked = selectedItem.IsActive;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = LanguagesMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<Language>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj = selectedItem.Validate();

            if (!returnObj.IsError)
            {
                if (selectedItem.ID == 0)
                    returnObj = LanguagesMapper.Insert(selectedItem);
                else
                    returnObj = LanguagesMapper.Update(selectedItem);
            }

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                DisplaySuccessMessage("Successfully Saved Item");

                /*var rootDetails = BaseMapper.GetObjectFromContext((MediaDetail)FrameworkBaseMedia.RootMediaDetail);
                rootDetails.VirtualPath = URIHelper.GetBaseUrlWithLanguage(selectedItem);*/

                /*returnObj = MediaDetailsMapper.Update(rootDetails);

                if (!returnObj.IsError)
                    MediaDetailsMapper.StartUpdateVirtualPathForAllChildren(rootDetails);
                else
                    DisplaySuccessMessage("Error updating root media item");*/
            }
        }
    }
}