using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class CommentsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
            //UpdateFieldsFromObject();
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            var media = selectedItem.Media;

            ApprovedCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Approved.ToString() && c.LanguageID == AdminBasePage.CurrentLanguage.ID).Count().ToString();
            RejectedCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Rejected.ToString() && c.LanguageID == AdminBasePage.CurrentLanguage.ID).Count().ToString();
            PendingCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Pending.ToString() && c.LanguageID == AdminBasePage.CurrentLanguage.ID).Count().ToString();
        }

        public void UpdateObjectFromFields()
        {
        }

        private AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}