using Android.Text;
using Com.Zebra.Rfid.Api3;
using Microsoft.AppCenter.Crashes;
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
using XLabs.Forms.Controls;

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
        public List<BinViewModel> _bintaglist = new List<BinViewModel>();
        public List<ProductBarcodeResponseModel> _barcodelist = new List<ProductBarcodeResponseModel>();
        private Object tagreadlock = new object();
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();
        object item;

        bool EditOption = false;
        bool checkQty = false;

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
            try
            {
                bool EditOption = false;
                BtnEditpencil.Icon = "edit_icon.png";

                LoadLotNo();
                // GetUserLoginAsync();
                PalletList.ItemsSource = _model;
                stk_pallettag.IsVisible = false;
                UpdateIn();
            }
            catch (Exception ex){ }

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
                Crashes.TrackError(ex);
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
            try
            {

                if (txt_Barcode.Text != "")
                {
                    ProductBarcodeRequestModel _User = new ProductBarcodeRequestModel
                    {
                        Barcode = txt_Barcode.Text,
                        LotNo = txt_lotNo.Text,
                        CheckQuantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? Convert.ToInt32(txt_SetQuantity.Text) : 1


                    };
                    addproductingrid(_User);

                }
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
            }
        }
       public async void addproductingrid(ProductBarcodeRequestModel _User)
        {
            
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
                                    if (Convert.ToInt32(selected.Quantity) >= Convert.ToInt32(PalletData.Quantity)) { await Application.Current.MainPage.DisplayAlert("Message", "No More Product Found", "OK"); }
                                    else
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
                            Crashes.TrackError(ex);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Product Quantity Is Smaller Than Entered Quantity ", "OK");
                    }
                }

            }
        }
        public async void AddProductInGreedForUpdate(object Response)
        {
            try
            {
                var PalletData = JsonConvert.DeserializeObject<ProductBarcodeResponseModel>(Response.ToString());
                if (PalletData.CheckQty == true)
                {
                    try
                    {
                        if (palletItem != null)
                        {
                            var selected = palletItem.Where(x => x.Barcode == PalletData.Barcode && x.LotNo == PalletData.LotNo).FirstOrDefault();

                            if (selected != null)
                            {
                                if (Convert.ToInt32(selected.Quantity) >= Convert.ToInt32(PalletData.Quantity)) { await Application.Current.MainPage.DisplayAlert("Message", "No More Product Found", "OK"); }
                                else
                                {
                                    palletItem.Remove(selected);
                                    palletItem.Add(new PalletItemResponse
                                    {
                                        Id = 0,
                                        Barcode = PalletData.Barcode,
                                        LotNo = PalletData.LotNo,
                                        WRReceivingProductsId = Convert.ToInt32(PalletData.WrReceivingProductId),
                                        ProductName = PalletData.ProductName,
                                        Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? Convert.ToInt32(txt_SetQuantity.Text) : ((Convert.ToInt32(selected.Quantity) + 1) > Convert.ToInt32(PalletData.Quantity)) ? Convert.ToInt32(PalletData.Quantity) : (Convert.ToInt32(selected.Quantity) + 1)
                                    });

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
                                    WRReceivingProductsId = Convert.ToInt32(PalletData.WrReceivingProductId),
                                    ProductName = PalletData.ProductName,
                                    Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? Convert.ToInt32(txt_SetQuantity.Text) : 1
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
                                WRReceivingProductsId = Convert.ToInt32(PalletData.WrReceivingProductId),
                                ProductName = PalletData.ProductName,
                                Quantity = (txt_SetQuantity.Text != "" && txt_SetQuantity.Text != "0" && txt_SetQuantity.Text != null) ? Convert.ToInt32(txt_SetQuantity.Text) : 1
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
                        await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Product Quantity Is Smaller Than Entered Quantity ", "OK");
                }
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
            }
        }


        public async void PostPalletMaintainanceDetailAsync()
        {
            try
            {
                PalletModel PalletMaintainanceRequest = new PalletModel();
                PalletMaintainanceRequest.Tag = txt_PalletTagNo.Text;
                PalletMaintainanceRequest.LotNo = txt_lotNo.Text;
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
                        if (EditOption == false)
                        {
                            popupStockInView.IsVisible = true;
                            PalletTag.Text = txt_PalletTagNo.Text;
                            Quantity.Text = PalletBarcodes.Sum(a=>a.Quantity).ToString();
                        }

                           // await Application.Current.MainPage.DisplayAlert("Message", "Success", "OK");
                        ClearData();
                    }
                    else
                    {
                        if (EditOption == true) { await Application.Current.MainPage.DisplayAlert("Message", "Updation Fail", "OK"); } else { await Application.Current.MainPage.DisplayAlert("Message", "Insert Fail", "OK"); }
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
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
            stk_pallettag.IsVisible = false;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
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
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
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
                SaveUpdateButton.Text = "Save/Print";
                btn_Save_StockIn.IsEnabled = true;
                stk_pallettag.IsVisible = false;
            }
            else
            {
                EditOption = true;
                txt_PalletTagNo.Focus();
                BtnEditpencil.Icon = "Save_icon.png";
                SaveUpdateButton.Text = "Update";
                btn_Save_StockIn.IsEnabled = false;
                stk_pallettag.IsVisible = true;
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
                await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
            }
        }

        private async void Txt_PalletTagNo_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (txt_PalletTagNo.Text != "")
            {
                if (EditOption == true)
                {
                    try
                    {
                        GetPalletItem();
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
                    }
                }
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                
                if (EditOption == false)
                {
                    if (_model != null)
                    {
                        PostPalletMaintainanceDetailAsync();
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                    }
                }


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
            }
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
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Inser Fail", "OK");
                }
            }
            catch (Exception ex)
            { Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message", "Inser Fail" , "OK");
            }

            btn_save.IsEnabled = true;
            popupStockInView.IsVisible = false;

        }
        public void clearData()
        {

            PalletTag.Text = "";
            BinTag.Text = "";
            Quantity.Text = "";

        }
        private void btn_close_Clicked(object sender, EventArgs e)
        {
            popupStockInView.IsVisible = false;
        }

        private void btn_clear_Clicked(object sender, EventArgs e)
        {

        }

        private async void btn_binsearch_Clicked(object sender, EventArgs e)
        {
            var getbins = await new PalletMaintainanceService().GetPalletLog(GetBintagsUrl.GetBintagList);
            if (getbins.Status == 1)
            {
                _bintaglist = JsonConvert.DeserializeObject<List<BinViewModel>>(getbins.Response.ToString());
                sampleList.ItemsSource = _bintaglist;
            }
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            popupListView.IsVisible = false;
        }

        private async void SearchTapped_Tapped(object sender, EventArgs e)
        {
            if (EditOption == false)
            {
                if (txt_lotNo.Text != null && txt_lotNo.Text != "")
                {
                    var getbarcode = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetBarcode+LotNumberList.Where(a=>a.LotNo==txt_lotNo.Text.Trim()).FirstOrDefault().WrReceivingLogId);
                    if (getbarcode.Status == 1)
                    {
                        _barcodelist = JsonConvert.DeserializeObject<List<ProductBarcodeResponseModel>>(getbarcode.Response.ToString());
                        barcodeList.ItemsSource = _barcodelist;
                    }
                    popupListViewBarcode.IsVisible = true;
                }
                else
                {
                    DisplayAlert("Message", "Fill Lot No", "Ok");
                    txt_lotNo.Focus();
                }
            }
            else
            {
                DisplayAlert("Message", "Select Save Option", "Ok");
            }
           
        }

        private void SearchBarCode_Tapped(object sender, EventArgs e)
        {
            txt_Barcode.Text = null;
            txt_SetQuantity.Text = null;
            popupListViewBarcode.IsVisible = false;
        }

       

        private void checkbox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
          
            
                CheckBox isCheckedOrNot = (CheckBox)sender;
            if (isCheckedOrNot.Checked == true)
            {
                string barcode = isCheckedOrNot.Text;
                var PalletDetail = isCheckedOrNot.BindingContext as ProductBarcodeResponseModel;
                txt_SetQuantity.Text = PalletDetail.Quantity;
                txt_Barcode.Text = barcode;
               
            }
            
        }
        private void txt_LotNo_Unfocused(object sender, FocusEventArgs e)
        {


        }
        private void txt_Barcode_Unfocused(object sender, FocusEventArgs e)
        {

        }
    
    }
}