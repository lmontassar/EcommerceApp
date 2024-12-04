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
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            if (Application.Current.Properties.ContainsKey("AdminToken"))
            {
                Application.Current.MainPage = new NavigationPage(new AdminDashboardPage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"],
                    BarTextColor = Color.White
                };
            }
        }

        private async void OnAdminLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

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
                    Application.Current.Properties["Username"] = UsernameEntry.Text;
                    await Application.Current.SavePropertiesAsync();

                    Application.Current.MainPage = new NavigationPage(new AdminDashboardPage())
                    {
                        BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"],
                        BarTextColor = Color.White
                    };
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

