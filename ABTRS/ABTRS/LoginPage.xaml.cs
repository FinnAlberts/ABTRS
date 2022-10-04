using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

                    string apiResponse = String.Empty;
                    // Check connection
                    try
                    {
                        Uri uri = new Uri(apiLocation + "?data=check_connection");

                        HttpClient myClient = new HttpClient();

                        myClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", username + ":" + password);

                        var response = await myClient.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                    catch
                    {
                        await DisplayAlert("Fout", "De API-locatie kon niet worden gevonden. Controleer de URL en probeer het opnieuw.", "Oke");
                    }

                    if (apiResponse == "Connection OK")
                    {
                        // Check camera permission
                        var hasPermission = await Permissions.RequestAsync<Permissions.Camera>();
                        if (hasPermission == PermissionStatus.Granted)
                        {
                            await Navigation.PushAsync(new MainPage());
                        } 
                        else
                        {
                            await DisplayAlert("Fout", "U dient toestemming te geven om de camera te gebruiken om de app te gebruiken.", "Oke");
                        }

                        activityIndicator.IsRunning = false;
                    }
                    else
                    {
                        await DisplayAlert("Fout", "Er kon geen verbinding worden gemaakt. Controleer de ingevoerde gegevens en probeer het opnieuw." + apiResponse, "Oke");
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