using System;
using System.Collections.Generic;
using System.Linq;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class ProductListPage : ContentPage
    {
        private readonly ApiService _apiService;

        public ProductListPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            try
            {
                var products = await _apiService.GetAsync<List<Product>>("products");
                ProductsListView.ItemsSource = products;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load products", "OK");
            }
        }

        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = e.CurrentSelection.FirstOrDefault() as Product;
            if (selectedProduct == null) return;

            await Navigation.PushAsync(new ProductDetailPage(selectedProduct));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
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

        protected override bool OnBackButtonPressed()
        {
            // Prevent back navigation when logged in
            return true;
        }
    }
}

