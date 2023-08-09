using shopping_cart.Data;
namespace shopping_cart.Models
{
        public class CartItem
        {
            public int Quantity { set; get; }
            public Product Product { set; get; }
        }
}
