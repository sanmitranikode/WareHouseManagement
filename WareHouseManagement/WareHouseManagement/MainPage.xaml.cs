using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.ViewModels;
using WareHouseManagement.Views;
using Xamarin.Forms;

namespace WareHouseManagement
{
    public partial class MainPage : ContentPage
    {

        SharedPreference _objShared = new SharedPreference();
        private MainPageViewModel viewmodel;
        public MainPage()
        {
            InitializeComponent();
         

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = viewmodel = new MainPageViewModel();

            GlobalConstant.AccessToken = _objShared.LoadApplicationProperty<string>("AccessToken");
            viewmodel.UpdateIn();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewmodel.UpdateOut();
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
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;

            await Task.Delay(700);
            await Navigation.PushAsync(new StockInPage());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void btn_PalletMaintainance_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;

            await Task.Delay(700);

            await Navigation.PushAsync(new PalletList());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;


        }

        private async void btn_ClrPalletTag_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            await Navigation.PushAsync(new ClearTagMaintenance());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void btn_StockOut_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new PrintStickerPdf()); 
             await Navigation.PushAsync(new UploadDamageStockImage());
        }

        private async void btn_ReaderList_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            await Navigation.PushAsync(new ReaderListMainPage());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void Btn_PickUpList_Clicked(object sender, EventArgs e)
        {
            //activityIndicator.IsRunning = true;
            //popupLoadingView.IsVisible = true;
            //await Task.Delay(700);
            //await Navigation.PushAsync(new Reports());
            //popupLoadingView.IsVisible = false;
            //activityIndicator.IsRunning = false;
        }

        private async void Btn_PalletMaintainanceReport_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            await Navigation.PushAsync(new WareHouseReceivingLogReports());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void btn_binTag_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            await Navigation.PushAsync(new ClearPalletAndBinTagPage());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void Btn_ProductBinReport_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            await Navigation.PushAsync(new Reports());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void rfidReader_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReaderListMainPage());
        }

        private async void print_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EZPrintListPage());
        }
    }
}
