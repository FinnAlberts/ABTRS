using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ABTRS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualInputPage : ContentPage
    {
        public ManualInputPage()
        {
            InitializeComponent();
        }

        private void openDetailPageButton_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(orderIdEntry.Text))
            {
                Int64 orderId;
                bool isOrderId = Int64.TryParse(orderIdEntry.Text, out orderId);

                if (!isOrderId)
                {
                    orderId = 99999999999; // Impossible order_id, as order_id contains of 10 numbers
                }

                Navigation.PushAsync(new DetailsPage(orderId));
            }
            else
            {
                DisplayAlert("Fout", "Het ordernummer moet worden ingevuld", "Oke");
            }
        }
    }
}