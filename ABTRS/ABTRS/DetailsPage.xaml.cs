using ABTRS.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ABTRS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        private Int64 orderId;
        
        public DetailsPage(Int64 orderId)
        {
            InitializeComponent();

            this.orderId = orderId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set order_id in label
            if (orderId != 99999999999)
            {
                orderIdLabel.Text = "Ordernummer: " + orderId.ToString();
            } 
            else
            {
                orderIdLabel.Text = "Ordernummer: niet gevonden";
            }

            // Check if seats are reserved by an admin account
            if (orderId < 0)
            {
                // Get the username of the admin account
                List<User> users = MainPage.users;
                List<string> accounts = (from user in users where user.user_id == -orderId select user.username).ToList();

                string accountName;

                if (accounts.Count > 0)
                {
                    accountName = accounts[0];
                } else
                {
                    accountName = "Verwijderd account";
                }

                // Setting up icon
                iconStackLayout.BackgroundColor = (Color)App.Current.Resources["errorBackgroundColor"];
                iconLabel.TextColor = (Color)App.Current.Resources["errorTextColor"];
                iconLabel.Text = App.Current.Resources["iconCross"].ToString();

                // Settings labels
                clientNameLabel.Text = accountName;
                errorLabel.Text = "Deze reservering is gemaakt via een adminaccount en kan niet met de app worden gecontroleerd. De reservering is gemaakt met het volgende account: " + accountName;

                // Hiding Listview
                seatsListView.IsVisible = false;
            } else
            {
                // Find seats accociated to order_id
                List<Seat> seats = MainPage.seats;
                seats = (from seat in seats where seat.order_id == orderId select seat).ToList();

                // Check if seats have been found
                if (seats.Count > 0) // Seats have been found
                {
                    // Setting up icon
                    iconStackLayout.BackgroundColor = (Color)App.Current.Resources["succesBackgroundColor"];
                    iconLabel.TextColor = (Color)App.Current.Resources["succesTextColor"];
                    iconLabel.Text = App.Current.Resources["iconCheck"].ToString();

                    // Put client name in label
                    string clientName = seats[0].first_name + " " + seats[0].last_name_prefix + " " + seats[0].last_name;
                    clientName = clientName.Replace("  ", " ");
                    clientNameLabel.Text = clientName;

                    // Show showname and date
                    List<Show> shows = MainPage.shows;
                    List<Show> selectedShow = (from show in shows where show.show_id == seats[0].show_id select show).ToList();
                    DateTime showDate = DateTime.ParseExact(selectedShow[0].date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    if (selectedShow.Count == 1)
                    {
                        showLabel.Text = selectedShow[0].displayname + " - " + showDate.ToString("dd-MM-yyyy HH:mm");
                    }

                    // Check show date
                    DateTime currentDate = DateTime.Now;

                    if (showDate.Date != currentDate.Date)
                    {
                        showDateWarningLabel.IsVisible = true;
                    }

                    // Put seats in ListView
                    List<string> seatNumbers = new List<string>();
                    foreach (Seat seat in seats)
                    {
                        seatNumbers.Add("Rij " + seat.row + ", Stoel " + seat.seat);
                    }

                    seatsListView.ItemsSource = seatNumbers;
                }
                else // No seats have been found
                {
                    // Setting up icon
                    iconStackLayout.BackgroundColor = (Color)App.Current.Resources["errorBackgroundColor"];
                    iconLabel.TextColor = (Color)App.Current.Resources["errorTextColor"];
                    iconLabel.Text = App.Current.Resources["iconCross"].ToString();

                    // Show error message
                    clientNameLabel.Text = "Reservering niet gevonden";

                    // Hide ListView
                    seatsListView.IsVisible = false;

                    // Give more detailed error message
                    errorLabel.Text = "Het ordernummer is niet gevonden. Probeer het opnieuw of controleer de reservering in het beheerportaal.";
                }
            }
        }
    }
}