namespace FrameworkLibrary
{
    public class CartItem
    {
        private IMediaDetail item;
        private long quantity;

        public CartItem(IMediaDetail item, long quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public IMediaDetail Item
        {
            get
            {
                return this.item;
            }
        }

        public long Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                if (value < 1)
                    return;

                if (value > this.item.QuantityInStock)
                    value = this.item.QuantityInStock;

                this.quantity = value;
            }
        }
    }
}