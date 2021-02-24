using ABTRS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ABTRS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private bool focussed;
        
        public ScanPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            focussed = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            focussed = false;
        }

        private async void scanView_OnScanResult(ZXing.Result result)
        {
            if (focussed)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Int64 orderId;
                    bool isOrderId = Int64.TryParse(result.Text, out orderId);

                    if (!isOrderId)
                    {
                        orderId = 99999999999; // Impossible order_id, as order_id contains of 10 numbers
                    }

                    await Navigation.PushAsync(new DetailsPage(orderId));
                });
            }
        }
    }
}