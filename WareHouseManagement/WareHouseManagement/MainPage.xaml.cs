using Android.App;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using WareHouseManagement.Views;
using Xamarin.Forms;

namespace WareHouseManagement
{
    public partial class MainPage : ContentPage
    {

        SharedPreference _objShared = new SharedPreference();
        public static List<ProductModel> ProductListStaticModel { get; set; }
        public MainPage()
        {
            InitializeComponent();


            ProductListStaticModel = new List<ProductModel>();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
          

            GlobalConstant.AccessToken = _objShared.LoadApplicationProperty<string>("AccessToken");
            getProductListOnLoad();
        }
        async void getProductListOnLoad()
        {
            try
            {


                var getproductdata = await new PalletMaintainanceService().GetPalletLog(GetCustomerAndVender.getproductlist);
                if (getproductdata.Status == 1)
                {
                    ProductListStaticModel = JsonConvert.DeserializeObject<List<ProductModel>>(getproductdata.Response.ToString());
                   
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {


            //Application.Current.Properties.Remove("UserEmail");
            //Application.Current.Properties.Remove("Password");
            //Application.Current.Properties.Clear();
            GlobalConstant.UserName =null;
            GlobalConstant.UserPassword = null;
            GlobalConstant.AccessToken = null;
            GlobalConstant.UserId = null;
            _objShared.SaveApplicationProperty("AccessToken", GlobalConstant.AccessToken);
            _objShared.SaveApplicationProperty("UserName", GlobalConstant.UserName);
            _objShared.SaveApplicationProperty("UserPassword", GlobalConstant.UserPassword);
            _objShared.SaveApplicationProperty("UserId", GlobalConstant.UserId);





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
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
            // await Navigation.PushAsync(new PrintStickerPdf()); 
            Navigation.PushAsync(new DamageStockReportsPage());
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        private async void btn_ReaderList_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            await Task.Delay(700);
           
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

       

        private async void print_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new EZPrintListPage());
        }
    }
}
