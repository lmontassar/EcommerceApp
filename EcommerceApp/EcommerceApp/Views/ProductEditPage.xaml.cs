using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class ProductEditPage : ContentPage
    {
        private readonly ApiService _apiService;
        private Product _product;

        public ProductEditPage(Product product = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = product ?? new Product();
            BindingContext = this;
            LoadCategories();
        }

        public string PageTitle => _product.id == 0 ? "Add Product" : "Edit Product";
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }
        public bool IsExisting => _product.id != 0;

        public ICommand SaveCommand => new Command(async () => await SaveProduct());
        public ICommand DeleteCommand => new Command(async () => await DeleteProduct());

        private async void LoadCategories()
        {
            Categories = await _apiService.GetAsync<List<Category>>("categories");
            OnPropertyChanged(nameof(Categories));

            if (_product.id != 0)
            {
                Name = _product.name;
                Price = _product.price.ToString();
                Description = _product.description;
                ImageUrl = _product.imageUrl;
                SelectedCategory = Categories.Find(c => c.id == _product.category.id);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(ImageUrl));
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Price) || SelectedCategory == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            _product.name = Name;
            _product.price = decimal.Parse(Price);
            _product.description = Description;
            _product.imageUrl = ImageUrl;
            _product.category = SelectedCategory;

            try
            {
                if (_product.id == 0)
                {
                    await _apiService.PostAsync<Product>("products", _product);
                }
                else
                {
                    await _apiService.PutAsync<Product>($"products/{_product.id}", _product);
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to save product", "OK");
            }
        }

        private async Task DeleteProduct()
        {
            var confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this product?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    await _apiService.DeleteAsync<object>($"products/{_product.id}");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Failed to delete product", "OK");
                }
            }
        }
    }
}

