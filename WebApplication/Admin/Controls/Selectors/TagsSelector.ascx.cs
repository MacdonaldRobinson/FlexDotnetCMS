using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls
{
    public partial class TagsSelector : System.Web.UI.UserControl
    {
        public IEnumerable<Tag> SelectedTags
        {
            get
            {
                IEnumerable<Tag> tags = (IEnumerable<Tag>)ContextHelper.Get("TagSelectorSelectedTags", ContextType.Session);
                return tags;
            }
            set
            {
                foreach (Tag item in value)
                {
                    if (item == null)
                        return;

                    var hasTag = TagValues.Value.ToLower().Split(',').ToList().Contains(item.Name.ToLower());

                    if (!hasTag)
                        TagValues.Value += item.Name + ",";
                }

                ContextHelper.Set("TagSelectorSelectedTags", value, ContextType.Session);
            }
        }

        public void SetMedia(Media item)
        {
            SelectedTags = item.MediaTags.OrderBy(i => i.OrderIndex).Select(i => i.Tag);
        }

        public void SetDefaultTag(string tagName)
        {
            if (TagValues.Value == "")
                TagValues.Value = tagName;
        }

        public IEnumerable<Tag> GetTags()
        {
            var items = new List<Tag>();
            IEnumerable<string> values = TagValues.Value.Trim().Split(',').Distinct();

            var count = 0;
            foreach (string value in values)
            {
                var tagName = value.Trim();

                var lowerCaseTagName = tagName.ToLower();

                if ((tagName == "") || (items.Any(i => i.Name.ToLower() == lowerCaseTagName)))
                    continue;

                long id;
                long.TryParse(tagName, out id);
                Tag item = null;

                if (id != 0)
                    item = TagsMapper.GetByID(long.Parse(tagName));

                if (item == null)
                    item = TagsMapper.GetByName(tagName);

                if (item != null)
                    item = BaseMapper.GetObjectFromContext<Tag>(item);

                if ((item == null) || (item.ID == 0))
                {
                    item = TagsMapper.CreateObject();
                    item.Name = tagName;
                    item.Description = tagName;
                    item.OrderIndex = count;
                    item.SefTitle = URIHelper.PrepairUri(tagName);
                    item.DateCreated = item.DateLastModified = DateTime.Now;
                }

                if (item != null)
                    items.Add(item);

                count++;
            }

            return items;
        }

        private AdminBasePage AdminBasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}