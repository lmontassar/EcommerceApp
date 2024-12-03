namespace EcommerceApp.Models
{
    public class Product
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }

        public string FullImageUrl => $"http://192.168.43.66:8089/api/products{imageUrl}";
    }
}