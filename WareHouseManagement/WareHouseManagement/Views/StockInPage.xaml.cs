using Com.Zebra.Rfid.Api3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockInPage : ContentPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<PalletlistViewModel> _palleTagtlist = new List<PalletlistViewModel>();

        public List<BinViewModel> _bintaglist = new List<BinViewModel>();



        public StockInPage()
        {
            InitializeComponent();


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            getData();
        }

        private async void getData()
        {
            var getbins = await new PalletMaintainanceService().GetPalletLog(GetBintagsUrl.GetBintagList);
            if (getbins.Status == 1)
            {
                _bintaglist = JsonConvert.DeserializeObject<List<BinViewModel>>(getbins.Response.ToString());
                sampleList.ItemsSource = _bintaglist;
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
          
        }






     
        private async void btnSave_ClickedAsync(object sender, EventArgs e)
        {
            btn_save.IsEnabled = true;
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            try
            {
                StockInPalletModel _model = new StockInPalletModel
                {
                    PalletTag = PalletTag.Text,
                    BinTag = BinTag.Text,
                    Quantity = int.Parse(Quantity.Text)

                };

                var PostDetails = await new StockInPalletService().PostStockInDetail(_model, StockInServiceUrl.PostStockIn);
                if (PostDetails.Status == 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Success", "OK");
                    clearData();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Inser Fail", "OK");
                }
            }
            catch (Exception ex)
            { }
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;

            btn_save.IsEnabled = false;

        }


        public async void GetQuantityByPalletId()
        {
            try
            {

                var GetData = await new PalletMaintainanceService().GetPalletLog(StockInServiceUrl.GetQuantity + PalletTag.Text);
                if (GetData.Status == 1)
                {
                    Quantity.Text = GetData.Response.ToString();
                }
                else { Quantity.Text = "0"; }
            }
            catch (Exception ex)
            {

            }
        }

        public void clearData()
        {

            PalletTag.Text = "";
            BinTag.Text = "";
            Quantity.Text = "";
            btn_save.IsEnabled = false;
        }


        private void PalletTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetQuantityByPalletId();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void btn_binsearch_Clicked(object sender, EventArgs e)
        {
            popupListView.IsVisible = true;

        }

        private void sampleList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (BinViewModel)e.SelectedItem;
            if (item != null)
            {
                BinTag.Text = item.BinTag;
                ((ListView)sender).SelectedItem = null;
            }

            popupListView.IsVisible = false;


        }

        private void BinTag_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btn_palletTagSearch_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            var getPalletTaglist = await new PalletMaintainanceService().GetPalletLog(GetPalletListUrl.getpalletTaglist);
            if (getPalletTaglist.Status == 1)
            {
                _palleTagtlist = JsonConvert.DeserializeObject<List<PalletlistViewModel>>(getPalletTaglist.Response.ToString());
                PalletTaglist.ItemsSource = _palleTagtlist;
            }
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;

            popupPalletTAgListView.IsVisible = true;

        }

        private void PalletTaglist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (PalletlistViewModel)e.SelectedItem;
            if (item != null)
            {
                PalletTag.Text = item.Tag;
                ((ListView)sender).SelectedItem = null;
            }

            popupPalletTAgListView.IsVisible = false;

        }
    }
}