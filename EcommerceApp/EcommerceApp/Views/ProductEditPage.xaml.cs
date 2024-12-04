using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class ProductEditPage : ContentPage, INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private Product _product;

        private string _name;
        private string _price;
        private string _description;
        private string _imageUrl;
        private Category _selectedCategory;
        private List<Category> _categories;

        public ProductEditPage(Product product = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = product ?? new Product();
            BindingContext = this;
            LoadCategories();
        }

        public string PageTitle => _product.id == 0 ? "Add Product" : "Edit Product";

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public bool IsExisting => _product.id != 0;

        public ICommand SaveCommand => new Command(async () => await SaveProduct());
        public ICommand DeleteCommand => new Command(async () => await DeleteProduct());

        private async void LoadCategories()
        {
            try
            {
                Categories = await _apiService.GetAsync<List<Category>>("categories");
                OnPropertyChanged(nameof(Categories));

                if (_product.id != 0)
                {
                    Name = _product.name;
                    Price = _product.price.ToString();
                    Description = _product.description;
                    ImageUrl = _product.imageUrl;
                    SelectedCategory = Categories.Find(c => c.id == _product.category?.id) ?? _product.category;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(ImageUrl));
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Price) || SelectedCategory == null)
            {
                await DisplayAlert("Error", "Please fill in all required fields", "OK");
                return;
            }

            if (!decimal.TryParse(Price, out decimal priceValue))
            {
                await DisplayAlert("Error", "Please enter a valid price", "OK");
                return;
            }

            _product.name = Name;
            _product.price = priceValue;
            _product.description = Description;
            _product.imageUrl = ImageUrl;
            _product.category = SelectedCategory;

            try
            {
                if (_product.id == 0)
                {
                    await _apiService.PostAsync<Product>("products", new
                    {
                        category = new { id = SelectedCategory.id },
                        name = Name,
                        price = priceValue,
                        description = Description,
                        imageUrl = ImageUrl
                    });
                }
                else
                {
                    await _apiService.PutAsync<Product>($"products/{_product.id}", _product);
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save product: {ex.Message}", "OK");
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
                    await DisplayAlert("Error", $"Failed to delete product: {ex.Message}", "OK");
                }
            }
        }
    }
}

