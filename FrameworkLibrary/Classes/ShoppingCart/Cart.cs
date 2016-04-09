using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public Cart(Settings currentSettings)
        {
            CurrentSettings = currentSettings;
        }

        public Settings CurrentSettings { get; private set; }

        public bool CanRenderAddToCartButton(IMediaDetail detail)
        {
            CartItem foundItem = GetCartItemByDetail(detail);

            if (detail == null)
                return false;

            if (!detail.CanAddToCart)
                return false;

            if (detail.QuantityInStock <= 0)
                return false;

            if ((foundItem != null) && (detail.QuantityInStock <= foundItem.Quantity))
                return false;

            return true;
        }

        public void AddItem(CartItem item)
        {
            AddItem(item.Item);
        }

        public void RemoveItem(CartItem item)
        {
            RemoveItem(item.Item);
        }

        public CartItem GetCartItemByDetail(IMediaDetail detail)
        {
            return items.Where(i => i.Item.ID == detail.ID).FirstOrDefault();
        }

        public void AddItem(IMediaDetail item)
        {
            CartItem foundItem = items.Where(i => i.Item.ID == item.ID).FirstOrDefault();

            if (foundItem != null)
            {
                if (foundItem.Quantity >= item.QuantityInStock)
                    return;

                foundItem.Quantity += 1;
            }
            else
            {
                if (item.QuantityInStock < 1)
                    return;

                CartItem newItem = new CartItem(item, 1);
                items.Add(newItem);
            }
        }

        public void RemoveItem(IMediaDetail item)
        {
            CartItem foundItem = items.Where(i => i.Item.ID == item.ID).FirstOrDefault();

            if (foundItem != null)
                items.Remove(foundItem);
        }

        public IEnumerable<CartItem> Items
        {
            get
            {
                return items;
            }
        }

        public decimal GetTotal()
        {
            return GetSubTotal() + GetTax();
        }

        public decimal GetTax()
        {
            decimal subTotal = GetSubTotal();

            return (subTotal * CurrentSettings.ShoppingCartTax) / 100;
        }

        public decimal GetSubTotal()
        {
            decimal subTotal = 0;

            foreach (CartItem item in items)
            {
                subTotal += (item.Item.Price * item.Quantity);
            }

            return subTotal;
        }
    }
}