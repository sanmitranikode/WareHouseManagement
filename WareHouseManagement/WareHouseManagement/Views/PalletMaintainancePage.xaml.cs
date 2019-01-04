using Android.Text;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private Command onDownloadClicked;
        List< ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel> ();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        string LotNo = "";
        public PalletMaintainancePage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        BindingContext = new ProductBarcodeResponseModel();
            GetUserLoginAsync();
            PalletList.ItemsSource = _model;
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
                    _model.Add(PalletData);
                    items = new PalletMaintanancedataBindingModel(_model);
                    PalletList.ItemsSource = null;
                    PalletList.ItemsSource = items.Items;
                }
              
               
            }
        }
        public ICommand OnDownloadClicked
        {
            get
            {
                if (onDownloadClicked == null)
                {
                    onDownloadClicked = new Command(() => DownloadRun(Id));
                }

                return onDownloadClicked;
            }
        }

        public void DownloadRun(object parameter)
        {

            //Debug.WriteLine(parameter)  ;
        }
        public async void PostPalletMaintainanceDetailAsync()
        {
            
            PalletMaintainanceRequestModel PalletMaintainanceRequest = new PalletMaintainanceRequestModel();
            PalletMaintainanceRequest.PalletRFID = "12345";
            PalletProductsModel productmodel=new PalletProductsModel();
            List<PalletProductsModel> listproductmodel = new List<PalletProductsModel>();

            try
            {

                foreach (var item in _model)
                {
                    productmodel.WRReceivedLogId = Convert.ToInt32(item.LotNo);
                    productmodel.WRReceivedProductId = Convert.ToInt32(item.ProductId);
                    listproductmodel.Add(productmodel);
                }
                PalletMaintainanceRequest.PalletProductsModel = listproductmodel;


                // var RFID= int.Parse(txt_Barcode.Text);
                var PostDetails = await new PalletMaintainanceService().PostPalletMaintainanceDetail(PalletMaintainanceRequest, PalletMaintainanceServiceUrl.PostPalletreceivinglog);
                if (PostDetails.Status == 1)
                {


                }
            }
            catch(Exception ex)
            {

            }
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
           
        }

        private void Img_deleteRow_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            ProductBarcodeResponseModel listitem = (from itm in items.Items where itm.Barcode == item.CommandParameter.ToString() select itm).FirstOrDefault<ProductBarcodeResponseModel>();
            items.Items.Remove(listitem);
        }




        //private void PalletList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    //var Val = (ProductBarcodeResponseModel)e.SelectedItem;
        //    //LotNo = Val.LotNo;
        //    //var data = _model.Single(s => s.LotNo == LotNo);
        //    //_model.Remove(data);
        //}


    }
}