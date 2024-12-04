using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class AdminDashboardPage : TabbedPage
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public AdminDashboardPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Products = new ObservableCollection<Product>();
            Categories = new ObservableCollection<Category>();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var products = await _apiService.GetAsync<List<Product>>("products");
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                var categories = await _apiService.GetAsync<List<Category>>("categories");
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                ProductsListView.ItemsSource = Products;
                CategoriesListView.ItemsSource = Categories;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEditPage());
        }

        private async void OnProductTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item is Product product)
                {
                    // Disable selection highlighting
                    ((ListView)sender).SelectedItem = null;

                    Debug.WriteLine($"Navigating to product: {product.name}");

                    if (Navigation != null)
                    {
                        await Navigation.PushAsync(new ProductEditPage(product));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Navigation not available", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex.Message}");
                await DisplayAlert("Error", "Unable to navigate to product details", "OK");
            }
        }

        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CategoryEditPage());
        }

        private async void OnCategoryTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item is Category category)
                {
                    ((ListView)sender).SelectedItem = null;

                    Debug.WriteLine($"Navigating to category: {category.name}");

                    if (Navigation != null)
                    {
                        await Navigation.PushAsync(new CategoryEditPage(category));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Navigation not available", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex.Message}");
                await DisplayAlert("Error", "Unable to navigate to category details", "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirm Logout",
                "Are you sure you want to logout?", "Yes", "No");

            if (confirm)
            {
                // Clear stored credentials
                Application.Current.Properties.Remove("AdminToken");
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }
    }
}

