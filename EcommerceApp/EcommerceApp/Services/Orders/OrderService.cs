using System.Threading.Tasks;
using EcommerceApp.Models.Orders;
using EcommerceApp.Services;

namespace EcommerceApp.Services.Orders
{
    public class OrderService
    {
        private readonly ApiService _apiService;

        public OrderService()
        {
            _apiService = new ApiService();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            return await _apiService.PostAsync<Order>("orders", order);
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await _apiService.GetAsync<Order>($"orders/{orderId}");
        }
    }
}

