using FrameworkLibrary;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class ShoppingCartSettingsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.CanAddToCart = CanAddToCart.Checked;
            selectedItem.Price = decimal.Parse(Price.Text);
            selectedItem.QuantityInStock = long.Parse(QuantityInStock.Text);
            selectedItem.RecurringTimePeriod = RecurringTimePeriod.SelectedValue;
        }

        public void UpdateFieldsFromObject()
        {
            CanAddToCart.Checked = selectedItem.CanAddToCart;
            Price.Text = selectedItem.Price.ToString();
            QuantityInStock.Text = selectedItem.QuantityInStock.ToString();
            RecurringTimePeriod.SelectedValue = selectedItem.RecurringTimePeriod;
        }
    }
}