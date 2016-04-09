using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.ShoppingCart
{
    public partial class ShoppingCart : System.Web.UI.UserControl
    {
        private Settings settings = SettingsMapper.GetSettings();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            if (CurrentCart.Items.Count() == 0)
                EmptyCartPanel.Visible = true;
            else
                EmptyCartPanel.Visible = false;

            ShoppingCartList.DataSource = CurrentCart.Items;
            ShoppingCartList.DataBind();
        }

        protected void ReCalculateTotal_OnClick(object sender, EventArgs e)
        {
            if (CurrentCart.Items.Count() == 0)
                return;

            foreach (ListViewDataItem item in ShoppingCartList.Items)
            {
                TextBox Quantity = (TextBox)item.FindControl("Quantity");
                CurrentCart.Items.ElementAt(item.DataItemIndex).Quantity = long.Parse(Quantity.Text);
            }
        }

        protected void PayNow_OnClick(object sender, EventArgs e)
        {
        }

        protected void ShoppingCartList_OnDataBound(object sender, EventArgs e)
        {
            if (CurrentCart.Items.Count() == 0)
                return;

            Literal SubTotal = (Literal)ShoppingCartList.FindControl("SubTotal");
            var ShoppingCartTax = (Literal)ShoppingCartList.FindControl("ShoppingCartTax");
            Literal Total = (Literal)ShoppingCartList.FindControl("Total");

            SubTotal.Text = CurrentCart.GetSubTotal().ToString();
            Total.Text = CurrentCart.GetTotal().ToString();
            ShoppingCartTax.Text = SettingsMapper.GetSettings().ShoppingCartTax.ToString();
        }

        protected void ShoppingCartList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            CartItem item = (CartItem)e.Item.DataItem;
            IMediaDetail detail = item.Item;

            Literal Name = (Literal)e.Item.FindControl("Name");
            Literal Price = (Literal)e.Item.FindControl("Price");
            TextBox Quantity = (TextBox)e.Item.FindControl("Quantity");
            LinkButton RemoveFromCart = (LinkButton)e.Item.FindControl("RemoveFromCart");

            Name.Text = detail.SectionTitle;
            Price.Text = detail.Price.ToString();
            Quantity.Text = item.Quantity.ToString();

            RemoveFromCart.CommandArgument = item.Item.ID.ToString();
        }

        public BasePage BasePage
        {
            get { return (BasePage)this.Page; }
        }

        protected void RemoveFromCart_OnClick(object sender, EventArgs e)
        {
            LinkButton senderItem = (LinkButton)sender;
            CurrentCart.RemoveItem(MediaDetailsMapper.GetByID(long.Parse(senderItem.CommandArgument)));
        }

        public static Cart CurrentCart
        {
            get
            {
                Cart cart = (Cart)ContextHelper.Get("CurrentShoppingCart", ContextType.Session);

                if (cart == null)
                {
                    cart = new Cart(SettingsMapper.GetSettings());
                    ContextHelper.Set("CurrentShoppingCart", cart, ContextType.Session);
                }

                return cart;
            }
        }
    }
}