using FrameworkLibrary;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class TagsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
            UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            if (IsPostBack)
                return;

            if (selectedItem.Media != null)
                TagsSelector.SetMedia(selectedItem.Media);
        }

        public void UpdateObjectFromFields()
        {
            IEnumerable<Tag> tags = TagsSelector.GetTags();
            selectedItem.Media.MediaTags.Clear();

            var counter = 0;
            foreach (Tag tag in tags)
            {
                if (!selectedItem.Media.MediaTags.Select(i => i.Tag).Contains(tag))
                {
                    if (tag.ID == 0)
                        TagsMapper.Insert(tag);

                    if (tag.ID != 0)
                    {
                        var newMediaTag = new MediaTag();
                        newMediaTag.MediaID = selectedItem.MediaID;
                        newMediaTag.TagID = tag.ID;
                        newMediaTag.OrderIndex = counter;

                        selectedItem.Media.MediaTags.Add(newMediaTag);
                    }

                    counter++;
                }
            }

            if (selectedItem.Media != null)
                TagsSelector.SetMedia(selectedItem.Media);
        }
    }
}