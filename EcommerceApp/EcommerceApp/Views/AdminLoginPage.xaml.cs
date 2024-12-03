using System;
using EcommerceApp.Services;
using Xamarin.Forms;

namespace EcommerceApp.Views
{
    public partial class AdminLoginPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AdminLoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnAdminLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var response = await _apiService.PostAsync<AdminLoginResponse>("auth/admin-login", new
                {
                    username = UsernameEntry.Text,
                    password = PasswordEntry.Text
                });

                if (response.Role == "ADMIN")
                {
                    Application.Current.Properties["AdminToken"] = response.Token;
                    await Application.Current.SavePropertiesAsync();
                    await Navigation.PushAsync(new AdminDashboardPage());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid admin credentials", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Login failed", "OK");
            }
        }
    }

    public class AdminLoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}

