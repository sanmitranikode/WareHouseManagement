using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductBinDetailReport : ContentPage
	{
        List<ProductModel> _Model = new List<ProductModel>();
        public ProductBinDetailReport ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetProductList();
        }
        public async void GetProductList()
        {
            try
            {
                var data = await new PalletMaintainanceService().GetPalletLog(ProductUrl.GetProduct);
                if (data.Status == 1)
                {
                    var ProductListData = JsonConvert.DeserializeObject<List<ProductModel>>(data.Response.ToString());
                    _Model = ProductListData;
                    ProductBinReportList.ItemsSource = null;
                    ProductBinReportList.ItemsSource = _Model;
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        public async void GetProductListByBarcode()
        {
            try
            {
                var data = await new PalletMaintainanceService().GetPalletLog(ProductUrl.GetProduct+"?="+ txt_BarcodeSearch.Text);
                if (data.Status == 1)
                {
                    var ProductListData = JsonConvert.DeserializeObject<List<ProductModel>>(data.Response.ToString());
                    _Model = ProductListData;
                    ProductBinReportList.ItemsSource = null;
                    ProductBinReportList.ItemsSource = _Model;
                }
            }
            catch (Exception ex) { Crashes.TrackError(ex); }

        }

        private async void Txt_BarcodeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetProductListByBarcode();
          
        }

      
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                activityIndicator.IsRunning = true;
                popupLoadingView.IsVisible = true;
                var tappedHier = ((TappedEventArgs)e).Parameter;
                var data = await new PalletMaintainanceService().GetPalletLog(ProductUrl.GetProductDetails + "?=" + tappedHier);
                if (data.Status == 1)
                {
                    var ProductListData = JsonConvert.DeserializeObject<List<BinsDetailByProductModel>>(data.Response.ToString());
                    await Navigation.PushAsync(new ProductBinDetailReportList(ProductListData));

                }
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
            }
            catch(Exception ex)
            {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
               
                await Application.Current.MainPage.DisplayAlert("Message", "Product Is Not Available in Bin", "OK");

            }
        }

        private void listofpallets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void ProductBinReportList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var tappedHier = (ProductModel) e.SelectedItem;
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            if (tappedHier != null)
            {
                var data = await new PalletMaintainanceService().GetPalletLog(ProductUrl.GetProductDetails + "?=" + tappedHier.Id);
                if (data.Status == 1)
                {
                    var ProductListData = JsonConvert.DeserializeObject<List<BinsDetailByProductModel>>(data.Response.ToString());
                    await Navigation.PushAsync(new ProductBinDetailReportList(ProductListData));

                }
            }
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
            ((ListView)sender).SelectedItem = null;

        }
    }
}