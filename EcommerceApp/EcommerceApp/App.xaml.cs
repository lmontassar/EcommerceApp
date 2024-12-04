using System;
using EcommerceApp.Views;
using Xamarin.Forms;

namespace EcommerceApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            if (Current.Properties.ContainsKey("AuthToken"))
            {
                // Check if it's an admin token
                if (Current.Properties.ContainsKey("AdminToken"))
                {
                    MainPage = new NavigationPage(new AdminDashboardPage())
                    {
                        BarBackgroundColor = (Color)Current.Resources["PrimaryColor"],
                        BarTextColor = Color.White
                    };
                }
                else
                {
                    MainPage = new NavigationPage(new ProductListPage())
                    {
                        BarBackgroundColor = (Color)Current.Resources["PrimaryColor"],
                        BarTextColor = Color.White
                    };
                }
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["PrimaryColor"],
                    BarTextColor = Color.White
                };
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

