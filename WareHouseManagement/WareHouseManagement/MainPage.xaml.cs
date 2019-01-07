using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.Views;
using Xamarin.Forms;

namespace WareHouseManagement
{
    public partial class MainPage : ContentPage
    {

        SharedPreference _objShared = new SharedPreference();
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GlobalConstant.AccessToken = _objShared.LoadApplicationProperty<string>("AccessToken");
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {


            //Application.Current.Properties.Remove("UserEmail");
            //Application.Current.Properties.Remove("Password");
            //Application.Current.Properties.Clear();
            GlobalConstant.AccessToken = null;
            _objShared.SaveApplicationProperty("AccessToken", GlobalConstant.AccessToken);



            Navigation.InsertPageBefore(new LogInPage(), this);
                await Navigation.PopAsync();
            
            
            
        }
       

        private async void btn_StockIn_Clicked(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new StockInPage());
        }

        private async void btn_PalletMaintainance_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PalletMaintainancePage());
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
