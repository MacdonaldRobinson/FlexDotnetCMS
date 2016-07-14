using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Views.PageHandlers.MediaArticle
{
    public partial class Create : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (SelectedMedia == null)
                return;

            MediaType mediaType = MediaTypesMapper.GetByID(MediaDetailsMapper.GetAtleastOneByMedia(SelectedMedia, CurrentLanguage).MediaTypeID);
            MediaTypeSelector.SetMediaTypes(mediaType.MediaTypes);
        }

        protected void CreateMedia_OnClick(object sender, EventArgs e)
        {
            if (!CurrentUser.HasPermission(PermissionsEnum.SaveItems))
            {
                DisplayErrorMessage("Error creating item", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions create items")));
                return;
            }

            if (MediaTypeSelector.MediaTypesDropDownList.SelectedValue == "")
                return;


            if (SelectedMedia != null)
            {
                WebApplication.BasePage.RedirectToMediaDetail(long.Parse(MediaTypeSelector.MediaTypesDropDownList.SelectedValue), 0, SelectedMedia.ID);
            }
            else
            {
                WebApplication.BasePage.RedirectToMediaDetail(long.Parse(MediaTypeSelector.MediaTypesDropDownList.SelectedValue), 0);
            }
        }
    }
}