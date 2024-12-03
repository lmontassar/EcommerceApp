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

            // Navigate to the detail page
            await Navigation.PushAsync(new ProductDetailPage(selectedProduct));

            // Clear selection
            ((CollectionView)sender).SelectedItem = null;
        }


        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
    }
}

