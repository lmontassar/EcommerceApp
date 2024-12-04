using System;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class ProductDetailPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Product _product;
        private int _quantity = 1;

        public ProductDetailPage(Product product)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = product;
            ProductImage.Source = product.FullImageUrl;
            NameLabel.Text = product.name;
            DescriptionLabel.Text = product.description;
            PriceLabel.Text = product.price.ToString("C");
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            try
            {
                var username = Application.Current.Properties["Username"] as string;
                await _apiService.PostAsync<Cart>("cart/add2", new
                {
                    username = username,
                    productId = _product.id,
                    quantity = _quantity
                });
                await DisplayAlert("Success", "Product added to cart", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to add product to cart", "OK");
            }
        }

        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                QuantityEntry.Text = _quantity.ToString();
            }
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            _quantity++;
            QuantityEntry.Text = _quantity.ToString();
        }

        private void OnQuantityUnfocused(object sender, EventArgs e)
        {
            if (int.TryParse(QuantityEntry.Text, out int newQuantity) && newQuantity > 0)
            {
                _quantity = newQuantity;
            }
            else
            {
                // If invalid value, revert to current quantity
                QuantityEntry.Text = _quantity.ToString();
            }
        }
    }
}

