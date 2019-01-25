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
      
        IList<PalletItemResponse> palletItem;
        ProductBarcodeResponseModel listitem;
        PalletModel PalletMaintainanceRequest = new PalletModel();
        List<LotNumberList> LotNumberList;
        public event PropertyChangedEventHandler PropertyChanged;
        ReaderModel rfidModel = ReaderModel.readerModel;
        public List<TagItem> Tags = new List<TagItem>();
        private Object tagreadlock = new object();
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();
        object item;

        bool EditOption = false;

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        List<PalletItemResponseModel> data = new List<PalletItemResponseModel>();

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
            bool EditOption = false;
            BtnEditpencil.Icon = "edit_icon.png";

            LoadLotNo();
            // GetUserLoginAsync();
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
        public void HHTriggerEvent(bool pressed)
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

        private async void txt_Barcode_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (txt_Barcode.Text != "")
            {
               
                ProductBarcodeRequestModel _User = new ProductBarcodeRequestModel
                {
                    Barcode = txt_Barcode.Text,
                    LotNo = txt_lotNo.Text,
                    CheckQuantity=(txt_SetQuantity.Text!="" && txt_SetQuantity.Text!="0")?Convert.ToInt32(txt_SetQuantity.Text):1


                };
                var PalletDetail = await new PalletMaintainanceService().GetPalletMaintainanceDetail(_User, PalletMaintainanceServiceUrl.GetPalletMaintainanceDetai);
                if (PalletDetail.Status == 2)
                {

                    await Application.Current.MainPage.DisplayAlert("Message", PalletDetail.Message, "OK");
                    return;
                }
                if (PalletDetail.Status == 1)
                {
                    if (EditOption == true)
                    {
                        AddProductInGreedForUpdate(PalletDetail.Response);
                    }
                    else
                    {
                        var PalletData = JsonConvert.DeserializeObject<ProductBarcodeResponseModel>(PalletDetail.Response.ToString());
                        if (PalletData.CheckQty == true)
                        {
                            try
                            {
                                if (_model.Count != 0)
                                {
                                    var selected = _model.Where(x => x.Barcode == PalletData.Barcode && x.LotNo == PalletData.LotNo).FirstOrDefault();

                                    if (selected != null)
                                    {
                                        _model.Remove(selected);
                                        _model.Add(new ProductBarcodeResponseModel
                                        {
                                            Id = PalletData.Id,
                                            Barcode = PalletData.Barcode,
                                            LotNo = PalletData.LotNo,
                                            WrReceivingProductId = PalletData.WrReceivingProductId,
                                            ProductName = PalletData.ProductName,
                                            Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? txt_SetQuantity.Text : ((Convert.ToInt32(selected.Quantity) + 1) > Convert.ToInt32(PalletData.Quantity)) ? PalletData.Quantity.ToString() : (Convert.ToInt32(selected.Quantity) + 1).ToString()
                                        });
                                        items = new PalletMaintanancedataBindingModel(_model);
                                        int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                                        lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                                        PalletList.ItemsSource = null;
                                        PalletList.ItemsSource = items.Items;
                                        txt_Barcode.Text = "";
                                        txt_SetQuantity.Text = "";
                                    }
                                    else
                                    {
                                        _model.Add(new ProductBarcodeResponseModel
                                        {
                                            Id = PalletData.Id,
                                            Barcode = PalletData.Barcode,
                                            LotNo = PalletData.LotNo,
                                            WrReceivingProductId = PalletData.WrReceivingProductId,
                                            ProductName = PalletData.ProductName,
                                            Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? txt_SetQuantity.Text : "1"
                                        }

                                        );
                                        items = new PalletMaintanancedataBindingModel(_model);
                                        int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                                        //  int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                                        lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                                        PalletList.ItemsSource = null;
                                        PalletList.ItemsSource = items.Items;
                                        txt_Barcode.Text = "";
                                        txt_SetQuantity.Text = "";
                                    }
                                }
                                else
                                {
                                    _model.Add(new ProductBarcodeResponseModel
                                    {
                                        Id = PalletData.Id,
                                        Barcode = PalletData.Barcode,
                                        LotNo = PalletData.LotNo,
                                        WrReceivingProductId = PalletData.WrReceivingProductId,
                                        ProductName = PalletData.ProductName,
                                        Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? txt_SetQuantity.Text : "1"
                                    }

                                    );
                                    items = new PalletMaintanancedataBindingModel(_model);
                                    int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                                    //  int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                                    lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                                    PalletList.ItemsSource = null;
                                    PalletList.ItemsSource = items.Items;
                                    txt_Barcode.Text = "";
                                    txt_SetQuantity.Text = "";
                                }

                            }

                            catch (Exception ex)
                            {

                            }

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Message", "Product Quantity Is Smaller Than Entered Quantity ", "OK");
                        }
                    }

                }

            }
        }
        public async void AddProductInGreedForUpdate(object Response)
        {
            var PalletData = JsonConvert.DeserializeObject<ProductBarcodeResponseModel>(Response.ToString());
            if (PalletData.CheckQty == true)
            {
                try
                {
                    if (palletItem!=null)
                    {
                        var selected = palletItem.Where(x => x.Barcode == PalletData.Barcode && x.LotNo == PalletData.LotNo).FirstOrDefault();

                        if (selected != null)
                        {
                            palletItem.Remove(selected);
                            palletItem.Add(new PalletItemResponse
                            {
                                Id =0,
                                Barcode = PalletData.Barcode,
                                LotNo = PalletData.LotNo,
                                WRReceivingProductsId =Convert.ToInt32( PalletData.WrReceivingProductId),
                                ProductName = PalletData.ProductName,
                                Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ?Convert.ToInt32( txt_SetQuantity.Text) : ((Convert.ToInt32(selected.Quantity) + 1) > Convert.ToInt32(PalletData.Quantity)) ?Convert.ToInt32( PalletData.Quantity) : (Convert.ToInt32(selected.Quantity) + 1)
                            });
                          
                            int TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                            lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                            PalletList.ItemsSource = null;
                            PalletList.ItemsSource = palletItem;
                            txt_Barcode.Text = "";
                            txt_SetQuantity.Text = "";
                        }
                        else
                        {
                            palletItem.Add(new PalletItemResponse
                            {
                                Id =0,
                                Barcode = PalletData.Barcode,
                                LotNo = PalletData.LotNo,
                                WRReceivingProductsId =Convert.ToInt32( PalletData.WrReceivingProductId),
                                ProductName = PalletData.ProductName,
                                Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ?Convert.ToInt32( txt_SetQuantity.Text) : 1
                            }

                            );
                           
                            int TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                            lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                            PalletList.ItemsSource = null;
                            PalletList.ItemsSource = palletItem;
                            txt_Barcode.Text = "";
                            txt_SetQuantity.Text = "";
                        }
                    }
                    else
                    {
                        palletItem.Add(new PalletItemResponse
                        {
                            Id = 0,
                            Barcode = PalletData.Barcode,
                            LotNo = PalletData.LotNo,
                            WRReceivingProductsId =Convert.ToInt32( PalletData.WrReceivingProductId),
                            ProductName = PalletData.ProductName,
                            Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ?Convert.ToInt32( txt_SetQuantity.Text) : 1
                        }

                        );
                      
                        int TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                        //  int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                        lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                        PalletList.ItemsSource = null;
                        PalletList.ItemsSource = palletItem;
                        txt_Barcode.Text = "";
                        txt_SetQuantity.Text = "";
                    }

                }

                catch (Exception ex)
                {

                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Product Quantity Is Smaller Than Entered Quantity ", "OK");
            }
        }


        public async void PostPalletMaintainanceDetailAsync()
        {

            PalletModel PalletMaintainanceRequest = new PalletModel();
            PalletMaintainanceRequest.Tag = txt_PalletTagNo.Text;
            if (EditOption == true)
            {
                PalletMaintainanceRequest.TotalProducts = palletItem.Sum(a => a.Quantity);
            }
            else
            {
                PalletMaintainanceRequest.TotalProducts = _model.Sum(a => Convert.ToInt32(a.Quantity));
            }

            List<PalletBarcodes> PalletBarcodes = new List<PalletBarcodes>();

            try
            {
                if (EditOption == true)
                {
                    foreach (var item in palletItem)
                    {
                        PalletBarcodes productmodel = new PalletBarcodes();
                        productmodel.Id = item.Id;
                        productmodel.LotNo = (item.LotNo);
                        productmodel.WRReceivingProductsId = Convert.ToInt32(item.WRReceivingProductsId);
                        productmodel.Quantity = Convert.ToInt32(item.Quantity);
                        PalletBarcodes.Add(productmodel);

                    }
                }
                else
                {
                    foreach (var item in _model)
                    {
                        PalletBarcodes productmodel = new PalletBarcodes();
                        
                        productmodel.LotNo = (item.LotNo);
                        productmodel.WRReceivingProductsId = Convert.ToInt32(item.WrReceivingProductId);
                        productmodel.Quantity = Convert.ToInt32(item.Quantity);
                        PalletBarcodes.Add(productmodel);

                    }
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
                    if (EditOption == true) { await Application.Current.MainPage.DisplayAlert("Message", "Updation Fail", "OK"); } else { await Application.Current.MainPage.DisplayAlert("Message", "Insert Fail", "OK"); }
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
            PalletList.ItemsSource = null;
            PalletList.ItemsSource = items.Items;
            txt_lotNo.Text = "";
            lbl_totalQuantity.Text = "Total Quantity = " +"0";
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (EditOption == true)
            {
                if (palletItem != null && txt_PalletTagNo.Text != "" && txt_PalletTagNo.Text != null)
                {
                    PostPalletMaintainanceDetailAsync();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                }
            }
            else
            {
                if (_model != null && txt_PalletTagNo.Text != "" && txt_PalletTagNo.Text != null)
                {
                    PostPalletMaintainanceDetailAsync();
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                }
            }
                
           
           

        }

        private async void Img_deleteRow_Clicked(object sender, EventArgs e)
        {
            // Img_deleteRow.ClassId = "imageId";



            item = ((TappedEventArgs)e).Parameter;

            try
            {
                int TotalQty = 0;
                if (EditOption == true)
                {
                    
                    var listitems = (from itm in palletItem where itm.Id ==Convert.ToInt32( item) select itm).FirstOrDefault<PalletItemResponse>();
                    var PalletDetailDelete = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.DeletePalletItem + "?ItemTag="+txt_PalletTagNo.Text+"&Id="+listitems.Id);
                    if (PalletDetailDelete.Status == 1)
                    {
                        palletItem.Remove(listitems);
                        PalletList.ItemsSource = null;
                        PalletList.ItemsSource = palletItem;
                      
                        
                    }
                    else
                    {
                        palletItem.Remove(listitems);
                        PalletList.ItemsSource = null;
                        PalletList.ItemsSource = palletItem;
                    }
                    TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                   
                }
                else
                {
                    listitem = (from itm in items.Items where itm.Id == Convert.ToInt32(item) select itm).FirstOrDefault<ProductBarcodeResponseModel>();
                    items.Items.Remove(listitem);
                    _model.Remove(listitem);
                     TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                   
                }
                lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
            }
            catch
            {
                Console.WriteLine("Select Item ");
            }

        }
        private void srchbox_carret_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var data = (ViewModels.LotNumberList)e.ChosenSuggestion;
            txt_lotNo.Text = data.LotNo;

        }
        private async void srchbox_carret_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (txt_lotNo.Text != "" && txt_lotNo.Text != null)
            {
                txt_lotNo.ItemsSource = string.IsNullOrWhiteSpace(txt_lotNo.Text) ? null : LotNumberList.Where(x => x.LotNo.StartsWith(txt_lotNo.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
        }

        public async void LoadLotNo()
        {
            var data = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetlotNoreceive);
            if (data.Status == 1)
            {
                LotNumberList = JsonConvert.DeserializeObject<List<LotNumberList>>(data.Response.ToString());
            }
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {

            LoadLotNo();
        }



        private async void EditItem_Clicked(object sender, EventArgs e)
        {
            if(EditOption == true)
            {
                 EditOption = false;
                BtnEditpencil.Icon = "edit_icon.png";
                SaveUpdateButton.Text = "Save";
            }
            else
            {
                EditOption = true;
                txt_PalletTagNo.Focus();
                BtnEditpencil.Icon = "Save_icon.png";
                SaveUpdateButton.Text = "Update";
            }

            PalletList.ItemsSource = null;
            txt_PalletTagNo.Text = "";
        }



        private async void GetPalletItem()
        {
            try
            {

                var PalletDetail = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetPalletItemByTagId + "?=" + txt_PalletTagNo.Text);
                if (PalletDetail.Status == 1 && PalletDetail != null)
                {
                    var PalletData = JsonConvert.DeserializeObject<PalletItemResponseModel>(PalletDetail.Response.ToString());

                    palletItem = PalletData.PalletBarcodes;

                    // data.Add(PalletData);
                    //  items = PalletData;
                    PalletList.ItemsSource = null;
                    PalletList.ItemsSource = palletItem;
                    int TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                    //  int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                    lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                }
                else
                {
                    palletItem = null;
                    PalletList.ItemsSource = null;
                    PalletList.ItemsSource = palletItem;
                    int TotalQty = palletItem.Sum(a => Convert.ToInt32(a.Quantity));
                    //  int TotalQty = items.Items.Sum(a => Convert.ToInt32(a.Quantity));
                    lbl_totalQuantity.Text = "Total Quantity = " + TotalQty.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Txt_PalletTagNo_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (EditOption == true)
            {
                try
                {
                    GetPalletItem();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}