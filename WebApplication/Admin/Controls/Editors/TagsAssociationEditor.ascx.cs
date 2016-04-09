using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls
{
    public partial class TagsAssociationEditor : System.Web.UI.UserControl
    {
        private Tag selectedItem;

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public void SetItem(Tag item)
        {
            this.selectedItem = item;
            Bind();
        }

        private void Bind()
        {
            if (selectedItem == null)
                return;

            var details = new List<IMediaDetail>();
            var medias = MediasMapper.GetByTag(selectedItem);

            foreach (Media media in medias)
            {
                IMediaDetail detail = MediaDetailsMapper.GetByMedia(media, AdminBasePage.CurrentLanguage);

                if (detail != null)
                    details.Add(detail);
            }

            this.ItemList.DataSource = details;
            this.ItemList.DataBind();
        }

        private void HandleDeleteAssociation(Media selectedMedia)
        {
            selectedItem = BaseMapper.GetObjectFromContext<Tag>(selectedItem);
            selectedMedia = BaseMapper.GetObjectFromContext<Media>(selectedMedia);

            var mediaTag = selectedItem.MediaTags.SingleOrDefault(i => i.MediaID == selectedMedia.ID);

            selectedItem.MediaTags.Remove(mediaTag);

            Return obj = TagsMapper.Update(selectedItem);

            if (obj.IsError)
                BasePage.DisplayErrorMessage("Error deleting association", obj.Error);
            else
            {
                BasePage.DisplaySuccessMessage("Successfully deleted association");
                Bind();
            }
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var commandArgument = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(commandArgument))
            {
                var item = MediaDetailsMapper.GetByID(long.Parse(commandArgument));

                if (item != null)
                {
                    HandleDeleteAssociation(item.Media);
                }
            }
        }
    }
}