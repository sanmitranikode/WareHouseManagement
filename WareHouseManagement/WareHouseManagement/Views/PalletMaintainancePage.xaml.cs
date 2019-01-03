using Android.Text;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class PalletMaintainancePage : ContentPage
    {
        ProductBarcodeResponseModel _model = new ProductBarcodeResponseModel();
        PalletMaintainanceRequestModel PalletMaintainanceRequest = new PalletMaintainanceRequestModel();
        public ObservableCollection<ProductBarcodeResponseModel> _myObservableCollection;
       
        public PalletMaintainancePage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ProductBarcodeResponseModel();
            GetUserLoginAsync();
          //  PostPalletMaintainanceDetailAsync();

        }
        public async Task GetUserLoginAsync()
        {
            UserModel _User = new UserModel
            {
                Username = "nealandmassy@gmail.com",
                Password = "lalit4mm"
            };
            var UserDetail = await new PalletMaintainanceService().GetLoginDetail(_User, PalletMaintainanceServiceUrl.GetUserLoginDetail);
            if (UserDetail.Status == 1)
            {
                var UserData = JsonConvert.DeserializeObject<UserLoginViewModel>(UserDetail.Response.ToString());
                GlobalConstant.AccessToken = UserData.Token;
            }
        }

        private async void txt_Barcode_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (txt_Barcode.Text != "")
            {
                ProductBarcodeRequestModel _User = new ProductBarcodeRequestModel
                {
                    Barcode = txt_Barcode.Text,
                    LotNo = "001285"
                };
                var PalletDetail = await new PalletMaintainanceService().GetPalletMaintainanceDetail(_User, PalletMaintainanceServiceUrl.GetPalletMaintainanceDetai);
                if (PalletDetail.Status == 1)
                {
                    var PalletData = JsonConvert.DeserializeObject<ProductBarcodeResponseModel>(PalletDetail.Response.ToString());
                    PalletList.ItemsSource = new List<ProductBarcodeResponseModel>
                    {
                       new ProductBarcodeResponseModel{Barcode=PalletData.Barcode,ProductName=PalletData.ProductName,Quantity=PalletData.Quantity, LotNo=PalletData.LotNo }
                    };
                   
                }
            }
        }

        public async void PostPalletMaintainanceDetailAsync()
        {
            PalletMaintainanceRequestModel PalletMaintainanceRequest = new PalletMaintainanceRequestModel
            {
                PalletRFID = "12345",

            };
           var RFID= int.Parse(txt_Barcode.Text);
            var PostDetails = await new PalletMaintainanceService().PostPalletMaintainanceDetail(PalletMaintainanceRequest, PalletMaintainanceServiceUrl.PostPalletreceivinglog);
            if (PostDetails.Status == 1)
            {


            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var test = e;
        }
    }
}