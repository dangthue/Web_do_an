namespace Web_Đồ_An.Models
{
    public class ShopCart
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal PriceSale { get; set; }
		public decimal Total { get; set; }

    }
}
