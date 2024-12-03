using System;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var response = await _apiService.PostAsync<LoginResponse>("auth/login", new
                {
                    username = UsernameEntry.Text,
                    password = PasswordEntry.Text
                });

                // Store the token securely (you might want to use a secure storage solution)
                Application.Current.Properties["AuthToken"] = response.Token;
                Application.Current.Properties["Username"] = UsernameEntry.Text;
                await Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new ProductListPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}

