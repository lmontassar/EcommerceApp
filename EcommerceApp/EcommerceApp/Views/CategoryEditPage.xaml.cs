using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class CategoryEditPage : ContentPage, INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private Category _category;
        private string _name;

        public CategoryEditPage(Category category = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _category = category ?? new Category();
            BindingContext = this;

            if (_category.id != 0)
            {
                Name = _category.name;
            }
        }

        public string PageTitle => _category.id == 0 ? "Add Category" : "Edit Category";

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

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

            _category.name = Name.Trim();

            try
            {
                if (_category.id == 0)
                {
                    await _apiService.PostAsync<Category>("categories", new { name = Name.Trim() });
                }
                else
                {
                    await _apiService.PutAsync<Category>($"categories/{_category.id}", _category);
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save category: {ex.Message}", "OK");
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
                    await DisplayAlert("Error", $"Failed to delete category: {ex.Message}", "OK");
                }
            }
        }
    }
}

