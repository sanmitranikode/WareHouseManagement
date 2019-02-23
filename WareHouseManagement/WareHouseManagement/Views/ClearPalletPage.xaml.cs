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
	public partial class ClearPalletPage : ContentPage
	{
       
     

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        PalletItemResponseModel _pendingItem = new PalletItemResponseModel();
    
        public ClearPalletPage ()
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



      

       
        

    
        public virtual void StatusEvent(Com.Zebra.Rfid.Api3.Events.StatusEventData statusEvent)
        {

        }


        public async void ClearPalletTagAsync()
        {
            try
            {
                StockInPalletResponseModel _model = new StockInPalletResponseModel();
                {
                    _model.Tag = "1234567890";

                };
                var ClearPalletTag = await new ClearPalletTagService().ClearPalletTag(_model, ClearPalletTagUrl.ClearPalletTag);
                if (ClearPalletTag.Status == 1)
                {
                    var PalletData = JsonConvert.DeserializeObject<StockInPalletModel>(ClearPalletTag.Response.ToString());
                }
            }
            catch (Exception ex)
            {


            }

        }

        private async void ReadPalletTag_TextChangedAsync(object sender, TextChangedEventArgs e)
        {

            if (ReadPalletTag.Text != "")
            {
                GetPalletItem();
            }

        }

        private async void GetPalletItem()
        {
            try
            {

                var PalletDetail = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetPalletItemByTagId + "?=" + ReadPalletTag.Text);
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

            }
        }



        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            ClearPalletTagAsync();
        }
    }
}