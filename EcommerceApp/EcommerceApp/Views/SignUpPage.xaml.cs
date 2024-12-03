using System;
using System.Diagnostics;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class SignUpPage : ContentPage
    {
        private readonly ApiService _apiService;

        public SignUpPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            try
            {
                var response = await _apiService.PostAsync<SignUpResponse>("auth/signup", new
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
                Debug.WriteLine($"Sign up error: {ex}");
                await DisplayAlert("Error", $"Sign up error: {ex}", "OK");
            }
        }
    }

    public class SignUpResponse
    {
        public string Token { get; set; }
    }
}

