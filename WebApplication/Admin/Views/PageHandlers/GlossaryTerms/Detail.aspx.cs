using FrameworkLibrary;
using System;

namespace WebApplication.Admin.Views.PageHandlers.GlossaryTerms
{
    public partial class Detail : AdminBasePage
    {
        private GlossaryTerm selectedItem = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            long id;

            if (long.TryParse(Request["id"], out id))
            {
                selectedItem = GlossaryTermsMapper.GetByID(id);

                if (!IsPostBack)
                    UpdateFieldsFromObject();
            }

            this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
        }

        private string GetSectionTitle()
        {
            if (selectedItem == null)
                return "New GlossaryTerm";
            else
                return "Editing GlossaryTerm: " + selectedItem.Term;
        }

        private void UpdateObjectFromFields()
        {
            selectedItem.Term = Term.Text;
            selectedItem.Definition = Definition.Text;
        }

        private void UpdateFieldsFromObject()
        {
            Term.Text = selectedItem.Term;
            Definition.Text = selectedItem.Definition;
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (selectedItem == null)
                selectedItem = GlossaryTermsMapper.CreateObject();
            else
                selectedItem = BaseMapper.GetObjectFromContext<GlossaryTerm>(selectedItem);

            UpdateObjectFromFields();

            Return returnObj;

            if (selectedItem.ID == 0)
                returnObj = GlossaryTermsMapper.Insert(selectedItem);
            else
                returnObj = GlossaryTermsMapper.Update(selectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                UpdateFieldsFromObject();
                DisplaySuccessMessage("Successfully Saved Item");
            }
        }
    }
}