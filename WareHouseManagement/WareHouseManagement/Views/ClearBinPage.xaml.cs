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
	public partial class ClearBinPage : ContentPage
	{
        public event PropertyChangedEventHandler PropertyChanged;
       
        public List<TagItem> Tags = new List<TagItem>();
        private Object tagreadlock = new object();
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        PalletItemResponseModel _pendingItem = new PalletItemResponseModel();
        public bool isConnected { get => isConnected; set => OnPropertyChanged(); }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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


            }

        }

        private async void ReadPalletTag_TextChangedAsync(object sender, TextChangedEventArgs e)
        {

            if (ReadBinTag.Text != "")
            {
                GetPalletItem();
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

            }
        }



        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            ClearPalletTagAsync();
        }
    }
}