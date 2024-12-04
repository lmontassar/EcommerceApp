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
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            if (Application.Current.Properties.ContainsKey("AuthToken"))
            {
                if (Application.Current.Properties.ContainsKey("AdminToken"))
                {
                    await Navigation.PushAsync(new AdminDashboardPage());
                }
                else
                {
                    await Navigation.PushAsync(new ProductListPage());
                }
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            try
            {
                var response = await _apiService.PostAsync<LoginResponse>("auth/login", new
                {
                    username = UsernameEntry.Text,
                    password = PasswordEntry.Text
                });

                Application.Current.Properties["AuthToken"] = response.Token;
                Application.Current.Properties["Username"] = UsernameEntry.Text;
                await Application.Current.SavePropertiesAsync();

                Application.Current.MainPage = new NavigationPage(new ProductListPage())
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"],
                    BarTextColor = Color.White
                };
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

        private async void OnAdminLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage());
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}

