using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Views.PageHandlers.Tags
{
    public partial class Detail : AdminBasePage
    {
        private Tag selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = TagsMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
            TagsAssociationEditor.SetItem(selectedItem);
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New Tag";
            else
                return "Editing Tag: " + selectedItem.Name;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Name = Name.Text;
            selectedItem.Description = Description.Text;
            selectedItem.SefTitle = StringHelper.CreateSlug(selectedItem.Name);
            selectedItem.ThumbnailPath = ThumbnailPath.GetValue().ToString();
        }

        private void UpdateFieldsFromObject()
        {
            Name.Text = selectedItem.Name;
            Description.Text = selectedItem.Description;
            ThumbnailPath.SetValue(selectedItem.ThumbnailPath);
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = TagsMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<Tag>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (selectedItem.ID == 0)
                returnObj = TagsMapper.Insert(selectedItem);
            else
                returnObj = TagsMapper.Update(selectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
                DisplaySuccessMessage("Successfully Saved Item");
        }
    }
}