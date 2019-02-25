
using Microsoft.AppCenter.Crashes;
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
	public partial class ClearBinPage : ContentPage
	{


        #region Declaration

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        PalletItemResponseModel _pendingItem = new PalletItemResponseModel();
        public bool isConnected { get => isConnected; set => OnPropertyChanged(); }
        #endregion

        public ClearBinPage ()
		{
			InitializeComponent ();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
         
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            PalletList.ItemsSource = _model;
        }







        #region Methodes




        public async void ClearPalletTagAsync()
        {
            try
            {
                StockInPalletResponseModel _model = new StockInPalletResponseModel();
                {
                    _model.Tag = "E0000A32";

                };
                var ClearPalletTag = await new ClearPalletTagService().ClearPalletTag(_model, ClearPalletTagUrl.ClearPalletTag);
                if (ClearPalletTag.Status == 1)
                {
                    var PalletData = JsonConvert.DeserializeObject<StockInPalletModel>(ClearPalletTag.Response.ToString());
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


        }
        private async void GetPalletItem()
        {
            try
            {

                var PalletDetail = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetPalletItemByTagId + "?=" + ReadBinTag.Text);
                if (PalletDetail.Status == 1 && PalletDetail != null)
                {
                    var PalletData = JsonConvert.DeserializeObject<PalletItemResponseModel>(PalletDetail.Response.ToString());

                    _pendingItem = PalletData;

                    var pendingdata = _pendingItem.PalletBarcodes.Where(x => x.Status == "Processing").ToList();

                    if (pendingdata.Count == 0)
                    {
                        SaveButton.IsVisible = false;
                        await Application.Current.MainPage.DisplayAlert("Message", "Ready to Clear Pallet", "OK");

                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Your have Pending item", "OK");
                        PalletList.ItemsSource = null;
                        PalletList.ItemsSource = pendingdata;
                    }


                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        #endregion
        #region Events


        private async void ReadPalletTag_TextChangedAsync(object sender, TextChangedEventArgs e)
        {

            if (ReadBinTag.Text != "")
            {
                GetPalletItem();
            }

        }

    


        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            ClearPalletTagAsync();
        }
        #endregion
    }
}