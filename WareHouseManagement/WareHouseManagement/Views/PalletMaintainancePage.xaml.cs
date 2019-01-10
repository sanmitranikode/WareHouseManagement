using Android.Text;
using Com.Zebra.Rfid.Api3;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
       

        public event PropertyChangedEventHandler PropertyChanged;
          ReaderModel rfidModel = ReaderModel.readerModel;
        public List<TagItem> Tags=new List<TagItem>();
        private Object tagreadlock = new object();
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();


     
        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel> ();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        public bool isConnected { get => isConnected; set => OnPropertyChanged(); }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PalletMaintainancePage()
        {
            InitializeComponent();
        }
        internal void UpdateIn()
        {
            rfidModel.TagRead += TagReadEvent;
            rfidModel.TriggerEvent += HHTriggerEvent;
            rfidModel.StatusEvent += StatusEvent;
            rfidModel.ReaderConnectionEvent += ReaderConnectionEvent;
            rfidModel.ReaderAppearanceEvent += ReaderAppearanceEvent;
        }

        internal void UpdateOut()
        {
            rfidModel.TagRead -= TagReadEvent;
            rfidModel.TriggerEvent -= HHTriggerEvent;
            rfidModel.StatusEvent -= StatusEvent;
            rfidModel.ReaderConnectionEvent -= ReaderConnectionEvent;
            rfidModel.ReaderAppearanceEvent -= ReaderAppearanceEvent;
        }
     

        protected async override void OnAppearing()
        {
            base.OnAppearing();
           
    
            GetUserLoginAsync();
            PalletList.ItemsSource = _model;

            UpdateIn();
           




        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UpdateOut();
        }

        public virtual void ReaderConnectionEvent(bool connection)
        {
            isConnected = connection;
        }

        public virtual void ReaderAppearanceEvent(bool appeared)
        {

        }
        public  void HHTriggerEvent(bool pressed)
        {
            try
            {

                if (pressed)
                {
                    Tags.Clear();
                    tagListDict.Clear();

                    bool bret = rfidModel.PerformInventory();
                    rfidModel.TagRead += TagReadEvent;
                    if (bret)
                    {
                        Thread.Sleep(200);
                        rfidModel.StopInventory();
                        rfidModel.TagRead -= TagReadEvent;
                        if (Tags.Count > 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                txt_PalletTagNo.Text = Tags.FirstOrDefault(p => p.RelativeDistance == Tags.Max(p2 => p2.RelativeDistance)).InvID;
                                });

                       
                        
                        }
                    }
                }
                else
                {
                    rfidModel.StopInventory();
                    rfidModel.TagRead -= TagReadEvent;

                }
             
                }
            catch (Exception ex)
            {
                rfidModel.StopInventory();
                rfidModel.TagRead -= TagReadEvent;
            }

        }

        public virtual void TagReadEvent(TagData[] aryTags)
        {
            lock (tagreadlock)
            {
                for (int index = 0; index < aryTags.Length; index++)
                {
                    //Console.WriteLine("Tag ID " + aryTags[index].TagID);

                    String tagID = aryTags[index].TagID;
                    if (tagListDict.ContainsKey(tagID))
                    {
                        tagListDict[tagID] = tagListDict[tagID] + aryTags[index].TagSeenCount;
                    }
                    else
                    {
                        tagListDict.Add(tagID, aryTags[index].TagSeenCount);
                        Tags.Add(new TagItem { InvID = tagID, RSSI = 0, RelativeDistance = 0, TagCount = aryTags[index].TagSeenCount });
                    }
                    UpdateDistance(tagID, tagListDict[tagID], aryTags[index].PeakRSSI);
                }


            }
        }
        private void UpdateDistance(string tagID, int count, short rssi)
        {
            var found = Tags.FirstOrDefault(x => x.InvID == tagID);
            if (found != null)
            {
                found.TagCount = count;
                // normalize
                int nr = rssi;
                if (rssi < -72)
                    nr = -72;
                if (rssi > -22)
                    nr = -22;
                found.RelativeDistance = (nr + 72) * 2;
                //Console.WriteLine("Tag ID " + found.InvID + " " + found.TagCount + " " + rssi + " " + found.RelativeDistance);
            }
        }
        public virtual void StatusEvent(Com.Zebra.Rfid.Api3.Events.StatusEventData statusEvent)
        {

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
                    LotNo = txt_lotNo.Text
                };
                var PalletDetail = await new PalletMaintainanceService().GetPalletMaintainanceDetail(_User, PalletMaintainanceServiceUrl.GetPalletMaintainanceDetai);
                if (PalletDetail.Status == 1)
                {
                    var PalletData = JsonConvert.DeserializeObject<ProductBarcodeResponseModel>(PalletDetail.Response.ToString());
                    try
                    {
                        var selected = _model.Where(x => x.WrReceivingProductId == PalletData.WrReceivingProductId).First();
                        if (selected != null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Message", "Your have Already added This Item", "OK");
                        }
                        else
                        {
                            _model.Add(PalletData);
                            items = new PalletMaintanancedataBindingModel(_model);
                            int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                            lbl_totalQuantity.Text= "Total Quantity = " + TotalQty.ToString();
                            PalletList.ItemsSource = null;
                            PalletList.ItemsSource = items.Items;
                            txt_Barcode.Text = "";
                        }
                    }
                    catch(Exception ex)
                    {
                        _model.Add(PalletData);
                        items = new PalletMaintanancedataBindingModel(_model);
                        int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                        lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                        PalletList.ItemsSource = null;
                        PalletList.ItemsSource = items.Items;
                    }
                    
                }
              
               
            }
        }
       

       
        public async void PostPalletMaintainanceDetailAsync()
        {
            
            PalletModel PalletMaintainanceRequest = new PalletModel();
            PalletMaintainanceRequest.Tag = txt_PalletTagNo.Text;
         
           
            List<PalletBarcodes> PalletBarcodes = new List<PalletBarcodes>();

            try
            {
                foreach (var item in _model)
                {
                    PalletBarcodes productmodel = new PalletBarcodes();
                    productmodel.LotNo =(item.LotNo);
                    productmodel.WRReceivingProductsId = Convert.ToInt32(item.WrReceivingProductId);
                    productmodel.Quantity =Convert.ToInt32( item.Quantity);
                    productmodel.WRReceivingProductsId = Convert.ToInt32(item.WrReceivingProductId);
                    PalletBarcodes.Add(productmodel);
                 
                }

                PalletMaintainanceRequest.PalletBarcodes = PalletBarcodes;

                // var RFID= int.Parse(txt_Barcode.Text);
                var PostDetails = await new PalletMaintainanceService().PostPalletMaintainanceDetail(PalletMaintainanceRequest, PalletMaintainanceServiceUrl.PostPalletreceivinglog);
                if (PostDetails.Status == 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Success", "OK");
                    ClearData();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Insert Fail", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }



        public void ClearData()
        {

            txt_PalletTagNo.Text = "";
            txt_Barcode.Text = "";
            _model = null;
            items.Items = null;
            PalletList.ItemsSource = items.Items;
            txt_lotNo.Text = "";
            lbl_totalQuantity.Text = "Total Quantity = 0";
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (_model != null && txt_PalletTagNo.Text!="" && txt_PalletTagNo.Text !=null)
            {
                PostPalletMaintainanceDetailAsync();
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
            }

        }

        private async void Img_deleteRow_Clicked(object sender, EventArgs e)
        {
            // Img_deleteRow.ClassId = "imageId";

           

            var item = ((TappedEventArgs)e).Parameter;
            
            try
            {

                ProductBarcodeResponseModel listitem = (from itm in items.Items where itm.Barcode == item.ToString() select itm).FirstOrDefault<ProductBarcodeResponseModel>();
                items.Items.Remove(listitem);
                _model.Remove(listitem);
                int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();

            }
            catch
            {
                Console.WriteLine("Select Item " );
            }
    
        }
        private void srchbox_carret_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var data = (PCL.Model.WRReceivingLogResponseViewModel)e.ChosenSuggestion;
            txt_lotNo.Text = data.LotNo;

        }
        private async void srchbox_carret_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (txt_lotNo.Text != "" && txt_lotNo.Text != null)
            {
                var data = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetlotNoreceive);
                if (data.Status == 1)
                {
                    var getlot = JsonConvert.DeserializeObject<List<PCL.Model.WRReceivingLogResponseViewModel>>(data.Response.ToString());
                    txt_lotNo.ItemsSource = string.IsNullOrWhiteSpace(txt_lotNo.Text) ? null : getlot.Where(x => x.LotNo.StartsWith(txt_lotNo.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
            }

        }


    }
}