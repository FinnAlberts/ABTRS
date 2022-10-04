using ABTRS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ABTRS
{
    public partial class MainPage : TabbedPage
    {
        public static List<Seat> seats;
        public static List<Show> shows;
        public static List<User> users;

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            getData();
        }

        private async void getData()
        {
            string username = LoginPage.username;
            string password = LoginPage.password;
            string apiLocation = LoginPage.apiLocation;

            Uri uri = new Uri(apiLocation + "?data=seats");

            HttpClient myClient = new HttpClient();

            myClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", username + ":" + password);

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                seats = JsonConvert.DeserializeObject<List<Seat>>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                
            }

            uri = new Uri(apiLocation + "?data=shows");

            myClient = new HttpClient();

            myClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", username + ":" + password);

            response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                shows = JsonConvert.DeserializeObject<List<Show>>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }

            uri = new Uri(apiLocation + "?data=users");

            myClient = new HttpClient();

            myClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", username + ":" + password);

            response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                users = JsonConvert.DeserializeObject<List<User>>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
        }

        private void logOutToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}
