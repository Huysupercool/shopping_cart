namespace shopping_cart.Data
{
	public class Product
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int? CategoryId { get; set; }
        public decimal Price { set; get; }
        public Category Category { get; set;} 
		public ICollection<OrderProduct>? OrderProducts { get; set;}

		public static implicit operator Product(List<Product> v)
		{
			throw new NotImplementedException();
		}
	}
}