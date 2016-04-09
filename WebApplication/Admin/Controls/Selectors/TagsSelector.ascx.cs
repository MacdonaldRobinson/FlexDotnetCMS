using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls
{
    public partial class TagsSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.AdminBasePage.TemplateVars.ContainsKey("TagsJSON"))
                this.AdminBasePage.TemplateVars.Add("TagsJSON", TagsMapper.GetAllJSON());

            if (!this.AdminBasePage.TemplateVars.ContainsKey("CurrentItemTagsJSON"))
                this.AdminBasePage.TemplateVars.Add("CurrentItemTagsJSON", "[{}]");
        }

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

        private void Bind()
        {
            this.AdminBasePage.TemplateVars["CurrentItemTagsJSON"] = TagsMapper.GetJSON(SelectedTags);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Bind();
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
            IEnumerable<string> values = TagValues.Value.Trim().ToLower().Split(',').Distinct();

            foreach (string value in values)
            {
                var tagName = value.Trim().ToLower();

                if ((tagName == "") || (items.Any(i => i.Name == tagName)))
                    continue;

                Tag item = TagsMapper.GetByName(tagName);

                if (item != null)
                    item = BaseMapper.GetObjectFromContext<Tag>(item);

                if ((item == null) || (item.ID == 0))
                {
                    item = TagsMapper.CreateObject();
                    item.Name = tagName;
                    item.Description = tagName;
                    item.SefTitle = URIHelper.PrepairUri(tagName);
                    item.DateCreated = item.DateLastModified = DateTime.Now;
                }

                if (item != null)
                    items.Add(item);
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