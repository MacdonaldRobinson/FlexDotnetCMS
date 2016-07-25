using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Controls
{
    public partial class RssListView : System.Web.UI.UserControl
    {
        private IEnumerable<RssItem> items;
        private bool showPostInfo = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Bind();
        }

        private void Bind()
        {
            if (this.items == null)
                return;

            ItemsList.DataSource = this.items.ToList();
            ItemsList.DataBind();

            if (this.items.Count() > Pager.PageSize)
                Pager.Visible = true;
            else
                Pager.Visible = false;
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink Item = (HyperLink)e.Item.FindControl("Item");
                HyperLink ReadMore = (HyperLink)e.Item.FindControl("ReadMore");
                Literal Description = (Literal)e.Item.FindControl("Description");
                Panel PostInfo = (Panel)e.Item.FindControl("PostInfo");

                RssItem DataItem = (RssItem)e.Item.DataItem;

                Item.Text = DataItem.Title;
                Item.NavigateUrl = DataItem.Link;
                Description.Text = DataItem.Description;
                ReadMore.NavigateUrl = Item.NavigateUrl;

                PostInfo.Visible = showPostInfo;

                if (PostInfo.Visible)
                {
                    Literal CreatedOn = (Literal)PostInfo.FindControl("CreatedOn");
                    Literal CreatedBy = (Literal)PostInfo.FindControl("CreatedBy");

                    CreatedOn.Text = DataItem.PubDate;
                    CreatedBy.Text = DataItem.Author;

                    if (DataItem.ObjectReference.GetType().GetInterfaces().Contains(typeof(IMediaDetail)))
                    {
                        IMediaDetail refObject = (IMediaDetail)DataItem.ObjectReference;

                        Literal Tags = (Literal)PostInfo.FindControl("Tags");
                        Literal NumberOfComments = (Literal)PostInfo.FindControl("NumberOfComments");

                        string tags = "";

                        foreach (Tag item in refObject.Media.MediaTags.Select(i => i.Tag))
                            tags += item.Name + ",";

                        Tags.Text = tags;
                        NumberOfComments.Text = refObject.Media.Comments.Count.ToString();
                    }
                }
            }
        }

        public ListView List
        {
            get
            {
                return this.ItemsList;
            }
        }

        public bool ShowPostInfo
        {
            get
            {
                return showPostInfo;
            }
            set
            {
                showPostInfo = value;
            }
        }

        public string SectionTitle
        {
            get
            {
                return ((Literal)ItemsList.FindControl("Title")).Text;
            }
            set
            {
                ((Literal)ItemsList.FindControl("Title")).Text = value;
            }
        }

        public IEnumerable<RssItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                this.Bind();
            }
        }

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}