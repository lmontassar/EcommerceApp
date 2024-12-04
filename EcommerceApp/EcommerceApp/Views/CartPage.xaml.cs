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
                TotalLabel.Text = $"${totalPrice:N2}";
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

                // Update the local item quantity
                item.quantity = newQuantity;

                // Refresh the list view
                CartItemsListView.ItemsSource = null;
                CartItemsListView.ItemsSource = _cart.items;

                UpdateTotal();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to update item quantity", "OK");
            }
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Success", "Thank you for your purchase!", "OK");
            LoadCart();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirm Logout",
                "Are you sure you want to logout?", "Yes", "No");

            if (confirm)
            {
                // Clear stored credentials
                Application.Current.Properties.Remove("AuthToken");
                Application.Current.Properties.Remove("Username");
                await Application.Current.SavePropertiesAsync();

                // Navigate back to login page
                Application.Current.MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"],
                    BarTextColor = Color.White
                };
            }
        }

        private async void OnQuantityTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry && entry.BindingContext is CartItem item)
            {
                if (int.TryParse(e.NewTextValue, out int newQuantity) && newQuantity > 0)
                {
                    await UpdateCartItemQuantity(item, newQuantity);
                }
            }
        }
        private async void OnQuantityUnfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry && entry.BindingContext is CartItem item)
            {
                if (int.TryParse(entry.Text, out int newQuantity) && newQuantity > 0)
                {
                    await UpdateCartItemQuantity(item, newQuantity);
                }
                else
                {
                    // If the entered value is invalid, revert to the original quantity
                    entry.Text = item.quantity.ToString();
                }
            }
        }


        protected override bool OnBackButtonPressed()
        {
            // Allow back navigation in cart page
            return false;
        }
    }
}

