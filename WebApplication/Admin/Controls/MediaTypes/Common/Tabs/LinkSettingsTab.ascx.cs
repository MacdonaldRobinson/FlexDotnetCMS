using FrameworkLibrary;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class LinkSettings : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateFieldsFromObject()
        {
            //ShowInMenu.Checked = selectedItem.ShowInMenu;
            ShowInSearchResults.Checked = selectedItem.ShowInSearchResults;
            UseDirectLink.Checked = selectedItem.UseDirectLink;

            selectedItem.DirectLink = MediaDetailsMapper.ConvertUrlsToShortCodes(selectedItem.DirectLink);

            DirectLink.SetValue(selectedItem.DirectLink);
            OpenInNewWindow.Checked = selectedItem.OpenInNewWindow;            
            RenderInFooter.Checked = selectedItem.RenderInFooter;
            ForceSSL.Checked = selectedItem.ForceSSL;
            CssClasses.Text = selectedItem.CssClasses;
            RedirectToFirstChild.Checked = selectedItem.RedirectToFirstChild;
            IsProtected.Checked = selectedItem.IsProtected;
        }

        public void UpdateObjectFromFields()
        {
            //selectedItem.ShowInMenu = ShowInMenu.Checked;
            selectedItem.ShowInSearchResults = ShowInSearchResults.Checked;
            selectedItem.UseDirectLink = UseDirectLink.Checked;
            selectedItem.DirectLink = DirectLink.GetValue().ToString();
            selectedItem.OpenInNewWindow = OpenInNewWindow.Checked;            
            selectedItem.RenderInFooter = RenderInFooter.Checked;
            selectedItem.ForceSSL = ForceSSL.Checked;
            selectedItem.CssClasses = CssClasses.Text;
            selectedItem.RedirectToFirstChild = RedirectToFirstChild.Checked;
            selectedItem.IsProtected = IsProtected.Checked;
        }
    }
}