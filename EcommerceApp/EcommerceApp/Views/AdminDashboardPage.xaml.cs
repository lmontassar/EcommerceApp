using System;
using System.Collections.Generic;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class AdminDashboardPage : TabbedPage
    {
        private readonly ApiService _apiService;

        public AdminDashboardPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadProducts();
            LoadCategories();
        }

        private async void LoadProducts()
        {
            var products = await _apiService.GetAsync<List<Product>>("products");
            ProductsListView.ItemsSource = products;
        }

        private async void LoadCategories()
        {
            var categories = await _apiService.GetAsync<List<Category>>("categories");
            CategoriesListView.ItemsSource = categories;
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEditPage());
        }

        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoryEditPage());
        }

        private async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Product product)
            {
                await Navigation.PushAsync(new ProductEditPage(product));
            }
        }

        private async void OnCategorySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Category category)
            {
                await Navigation.PushAsync(new CategoryEditPage(category));
            }
        }
    }
}

