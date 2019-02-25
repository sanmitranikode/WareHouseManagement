using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class PalletList : ContentPage
    {
        #region Declaration
        public List<PalletlistViewModel> _palletlist = new List<PalletlistViewModel>();
        public List<BinViewModel> _bintaglist = new List<BinViewModel>();
        #endregion

        public PalletList()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            base.OnAppearing();
            loaddata();
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;

        }
        #region Declaration
        private async void loaddata()
        {
            try
            {
                var getpalletdata = await new PalletMaintainanceService().GetPalletLog(GetPalletListUrl.getpalletlist);
                if (getpalletdata.Status == 1)
                {
                    _palletlist = JsonConvert.DeserializeObject<List<PalletlistViewModel>>(getpalletdata.Response.ToString());
                    listofpallets.ItemsSource = null;
                    listofpallets.ItemsSource = _palletlist;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }



        }
        public void clearData()
        {

            PalletTag.Text = "";
            BinTag.Text = "";
            Quantity.Text = "";

        }
        #endregion
        #region Events

        private void fabBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void StockIn_Tapped(object sender, EventArgs e)
        {
            try
            {
                var tappedstock = ((TappedEventArgs)e).Parameter;
                var listitems = (from itm in _palletlist where itm.Tag == tappedstock.ToString() select itm).FirstOrDefault<PalletlistViewModel>();
                popupStockInView.IsVisible = true;
                PalletTag.Text = listitems.Tag;
                Quantity.Text = listitems.TotalProducts.ToString();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

           



        }

        private void Print_Tapped(object sender, EventArgs e)
        {
            try
            {
                var tappedprint = ((TappedEventArgs)e).Parameter;
                var listitems = (from itm in _palletlist where itm.Tag == tappedprint.ToString() select itm).FirstOrDefault<PalletlistViewModel>();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

           

        }

        private void listofpallets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
          
        }

        private async void btn_save_Clicked(object sender, EventArgs e)
        {
            btn_save.IsEnabled = false;
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
                    loaddata();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Inser Fail", "OK");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            btn_save.IsEnabled = true;

        }
    

        private async void btn_close_Clicked(object sender, EventArgs e)
        {
            popupStockInView.IsVisible = false;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PalletMaintainancePage());
        }

        private async void btn_binsearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                btn_binsearch.IsEnabled = false;
                var getbins = await new PalletMaintainanceService().GetPalletLog(GetBintagsUrl.GetBintagList);
                if (getbins.Status == 1)
                {
                    _bintaglist = JsonConvert.DeserializeObject<List<BinViewModel>>(getbins.Response.ToString());
                    sampleList.ItemsSource = _bintaglist;
                }
                popupListView.IsVisible = true;
                btn_binsearch.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        private void sampleList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = (BinViewModel)e.SelectedItem;
                if (item != null)
                {
                    BinTag.Text = item.BinTag;
                    ((ListView)sender).SelectedItem = null;
                }

                popupListView.IsVisible = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            popupListView.IsVisible = false;
        }
        #endregion
    }
}