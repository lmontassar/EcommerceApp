namespace EcommerceApp.Models
{
    public class Product
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }

        public Category category { get; set; }

        public string FullImageUrl => $"http://172.20.10.4:8089/api/products{imageUrl}";
    }
}