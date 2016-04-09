using System;

namespace WebApplication.Controls.ShoppingCart
{
    public partial class AddToCartButton : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShoppingCart.CurrentCart.CanRenderAddToCartButton(BasePage.CurrentMediaDetail))
                AddToCartPanel.Visible = false;
            else
                AddToCartPanel.Visible = true;
        }

        protected void AddToCart_OnClick(object sender, EventArgs e)
        {
            if (BasePage.CurrentMediaDetail == null)
                return;

            BasePage.AddToCart(BasePage.CurrentMediaDetail);
        }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}