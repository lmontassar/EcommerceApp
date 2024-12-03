using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class CartPage : ContentPage
    {
        private readonly ApiService _apiService;
        private Cart _cart;

        public CartPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadCart();
        }

        private async void LoadCart()
        {
            try
            {
                var username = Application.Current.Properties["Username"] as string;
                _cart = await _apiService.GetAsync<Cart>($"cart/user/{username}");
                CartItemsListView.ItemsSource = _cart.items;
                UpdateTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load cart", "OK");
            }
        }

        private void UpdateTotal()
        {
            if (_cart != null && _cart.items != null)
            {
                var totalPrice = _cart.items.Sum(item => item.product.price * item.quantity);
                var totalQuantity = _cart.items.Sum(item => item.quantity);
                TotalLabel.Text = $"Total: {totalPrice:C} ({totalQuantity} items)";
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as CartItem;

            if (item == null)
                return;

            try
            {
                var username = Application.Current.Properties["Username"] as string;
                _cart = await _apiService.DeleteAsync<Cart>($"cart/remove?username={username}&productId={item.product.id}");
                CartItemsListView.ItemsSource = _cart.items;
                UpdateTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to remove item from cart", "OK");
            }
        }

        private async void OnDecrementClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as CartItem;

            if (item == null || item.quantity <= 1)
                return;

            await UpdateCartItemQuantity(item, item.quantity - 1);
        }

        private async void OnIncrementClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as CartItem;

            if (item == null)
                return;

            await UpdateCartItemQuantity(item, item.quantity + 1);
        }

        private async Task UpdateCartItemQuantity(CartItem item, int newQuantity)
        {
            try
            {
                var username = Application.Current.Properties["Username"] as string;
                _cart = await _apiService.PutAsync<Cart>($"cart/update?username={username}&productId={item.product.id}&quantity={newQuantity}");
                CartItemsListView.ItemsSource = _cart.items;
                UpdateTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to update item quantity", "OK");
            }
        }
    }
}

