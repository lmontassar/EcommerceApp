using System;
using System.Xml;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class ProductDetailPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Product _product;

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
        private int _quantity = 1;

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            try
            {
                int q = _quantity;
                var usernam = Application.Current.Properties["Username"] as string;
                await _apiService.PostAsync<Cart>("cart/add2", new
                {
                    username = usernam,
                    productId = _product.id,
                    quantity = q
                });
                await DisplayAlert("Success", "Product added to cart", "OK");
            }
            catch (Exception ex)
            {
                var usernam = Application.Current.Properties["Username"] as string;
                await DisplayAlert("Error", $"Failed to add product to cart {usernam}", "OK");
            }
        }
        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                QuantityLabel.Text = _quantity.ToString();
            }
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            _quantity++;
            QuantityLabel.Text = _quantity.ToString();
        }

    }
}

