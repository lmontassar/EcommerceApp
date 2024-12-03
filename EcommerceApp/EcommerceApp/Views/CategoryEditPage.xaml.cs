using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class CategoryEditPage : ContentPage
    {
        private readonly ApiService _apiService;
        private Category _category;

        public CategoryEditPage(Category category = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _category = category ?? new Category();
            BindingContext = this;

            if (_category.id != 0)
            {
                Name = _category.name;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string PageTitle => _category.id == 0 ? "Add Category" : "Edit Category";
        public string Name { get; set; }
        public bool IsExisting => _category.id != 0;

        public ICommand SaveCommand => new Command(async () => await SaveCategory());
        public ICommand DeleteCommand => new Command(async () => await DeleteCategory());

        private async Task SaveCategory()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await DisplayAlert("Error", "Please enter a category name", "OK");
                return;
            }

            _category.name = Name;

            try
            {
                if (_category.id == 0)
                {
                    await _apiService.PostAsync<Category>("categories", _category);
                }
                else
                {
                    await _apiService.PutAsync<Category>($"categories/{_category.id}", _category);
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to save category", "OK");
            }
        }

        private async Task DeleteCategory()
        {
            var confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this category?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    await _apiService.DeleteAsync<object>($"categories/{_category.id}");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Failed to delete category", "OK");
                }
            }
        }
    }
}

