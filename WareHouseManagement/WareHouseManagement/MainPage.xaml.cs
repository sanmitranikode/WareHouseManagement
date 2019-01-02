using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Views;
using Xamarin.Forms;

namespace WareHouseManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_StockIn_Clicked(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new StockInPage());
        }

        private async void btn_PalletMaintainance_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WarehouseReceivingLogPage());
        }

        private async void btn_ClrPalletTag_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClearPalletTagPage());
        }

        private async void btn_StockOut_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StockOutMenuPage());
        }

        private async void btn_ReaderList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReaderListMainPage());
        }
    }
}
