using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls
{
    public partial class AddIMediaDetailToUser : System.Web.UI.UserControl
    {
        public IMediaDetail MediaDetailToAdd
        {
            get { return (IMediaDetail)ViewState["IMediaDetail"]; }
            private set { ViewState["IMediaDetail"] = value; }
        }

        public void SetIMediaDetailToAdd(IMediaDetail mediaDetail)
        {
            MediaDetailToAdd = mediaDetail;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (MediaDetailToAdd == null)
                return;

            var found = MediaDetailToAdd.Media.UsersMedias.Where(i => i.MediaID == MediaDetailToAdd.MediaID).ToList();

            if (found.Count == 0)
            {
                AddToUser.Visible = true;
                RemoveFromUser.Visible = false;
            }
            else
            {
                AddToUser.Visible = false;
                RemoveFromUser.Visible = true;
            }
        }

        protected void AddToUser_OnClick(object sender, EventArgs e)
        {
            var usersMediaDetails = new UserMedia();
            usersMediaDetails.MediaID = MediaDetailToAdd.MediaID;
            usersMediaDetails.UserID = BasePage.CurrentUser.ID;
            usersMediaDetails.DateCreated = usersMediaDetails.DateLastModified = DateTime.Now;
            usersMediaDetails.PermissionID = PermissionsMapper.GetPermissionsFromEnum(PermissionsEnum.AccessProtectedSections).ID;

            var currentMediaDetail = BaseMapper.GetObjectFromContext((MediaDetail)MediaDetailToAdd);
            currentMediaDetail.Media.UsersMedias.Add(usersMediaDetails);

            var returnObj = MediaDetailsMapper.Update(currentMediaDetail);

            if (!returnObj.IsError)
            {
                BasePage.DisplaySuccessMessage("Successfully added to My Plan");
                UpdateVisibility();
            }
            else
            {
                BasePage.DisplayErrorMessage("Error adding to My Plan", returnObj.Error);
            }
        }

        public FrontEndBasePage BasePage
        {
            get { return (FrontEndBasePage)this.Page; }
        }

        protected void RemoveFromUser_OnClick(object sender, EventArgs e)
        {
            var currentMediaDetail = BaseMapper.GetObjectFromContext((MediaDetail)MediaDetailToAdd);
            var found = currentMediaDetail.Media.UsersMedias.SingleOrDefault(i => i.MediaID == currentMediaDetail.MediaID);

            if (found == null)
                return;

            var returnObj = UsersMediasMapper.DeletePermanently(found);

            if (!returnObj.IsError)
            {
                currentMediaDetail.Media.UsersMedias.Remove(found);
                returnObj = MediaDetailsMapper.Update(currentMediaDetail);
                UpdateVisibility();
            }

            if (!returnObj.IsError)
            {
                BasePage.DisplaySuccessMessage("Successfully removed from My Plan");
            }
            else
            {
                BasePage.DisplayErrorMessage("Error removing from My Plan", returnObj.Error);
            }
        }
    }
}