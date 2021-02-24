using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ABTRS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static string apiLocation;
        public static string username;
        public static string password;

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            apiLocationEntry.Text = Preferences.Get("api", "");
            usernameEntry.Text = Preferences.Get("username", "");
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            apiLocation = apiLocationEntry.Text;
            username = usernameEntry.Text;
            password = passwordEntry.Text;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (!String.IsNullOrEmpty(apiLocation) && !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
                {
                    Preferences.Set("api", apiLocation);
                    Preferences.Set("username", username);

                    // Check connection
                    Uri uri = new Uri(apiLocation + "?data=check_connection&username=" + username + "&password=" + password);

                    HttpClient myClient = new HttpClient();

                    var response = await myClient.GetAsync(uri);

                    string apiResponse = "";
                    if (response.IsSuccessStatusCode)
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }

                    if (apiResponse == "Connection OK")
                    {
                        await Navigation.PushAsync(new MainPage());
                        activityIndicator.IsRunning = false;
                    }
                    else
                    {
                        await DisplayAlert("Fout", "Er kon geen verbinding worden gemaakt. Controleer de ingevoerde gegevens en probeer het opnieuw.", "Oke");
                        activityIndicator.IsRunning = false;
                    }
                }
                else
                {
                    await DisplayAlert("Fout", "Niet alle velden zijn ingevuld.", "Oke");
                    activityIndicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Fout", "Er is op dit moment geen internetverbinding beschikbaar. Verbind met internet via WiFi of mobiele data en probeer het opnieuw.", "Oke");
                activityIndicator.IsRunning = false;
            }
        }  
    }
}