﻿using Android.Text;

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
using WareHouseManagement.PCL.Helper;
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
        #region Declaration

        IList<PalletItemResponse> palletItem;
        ProductBarcodeResponseModel listitem;
        PalletModel PalletMaintainanceRequest = new PalletModel();
        List<LotNumberList> LotNumberList;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<TagItem> Tags = new List<TagItem>();
        public List<BinViewModel> _bintaglist = new List<BinViewModel>();
        public List<ProductBarcodeResponseModel> _barcodelist = new List<ProductBarcodeResponseModel>();
        object item;
        CloudPrintHelper _printHelper = new CloudPrintHelper();

        bool EditOption = false;
        bool checkQty = false;

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        List<PalletItemResponseModel> data = new List<PalletItemResponseModel>();

        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        #endregion
        public PalletMaintainancePage()
        {
            InitializeComponent();
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
              
            }
            catch (Exception ex){ }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           
        }
   


        #region Methods
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
                    AddProductInGridForUpdate(PalletDetail.Response);
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

                txt_Barcode.Focus();


            }
        }
        public async void AddProductInGridForUpdate(object Response)
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
                        Crashes.TrackError(ex);
                        await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Product Quantity Is Smaller Than Entered Quantity ", "OK");
                }
            }catch(Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
            }
        }

        public async void PostPalletMaintainanceDetailAsync(string type)
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
                            if (type == "SaveAndStockIn")
                            {
                                var printPalletdata = JsonConvert.DeserializeObject<PrintPalletModel>(PostDetails.Response.ToString());
                                var value = await DisplayAlert("Print", "Do you want to print", "Yes", "No");
                                if (value == true)
                                {
                                    var source = new HtmlWebViewSource();
                                    var browser = new WebView();
                                    var htmlSource = new HtmlWebViewSource();

                                    var htmlHelper = _printHelper.PrintData(printPalletdata);
                                    source.Html = htmlHelper;

                                    browser.Source = source;
                                    var printService = DependencyService.Get<IPrintService>();
                                    printService.Print(browser);
                                }
                                popupStockInView.IsVisible = true;
                                if(txt_PalletTagNo.Text==""|| txt_PalletTagNo.Text == null)
                                {
                                    PalletTag.Text = printPalletdata.Tag;
                                }
                                else
                                {
                                    PalletTag.Text = txt_PalletTagNo.Text;
                                }
                              
                                Quantity.Text = PalletBarcodes.Sum(a => a.Quantity).ToString();

                            }
                            else
                            {
                                popupStockInView.IsVisible = false;
                              
                                var printPalletdata = JsonConvert.DeserializeObject<PrintPalletModel>(PostDetails.Response.ToString());
                                 var value = await DisplayAlert("Print", "Do you want to print", "Yes", "No");
                                if (value == true)
                                {
                                    var source = new HtmlWebViewSource();
                                    var browser = new WebView();

                                    var htmlSource = new HtmlWebViewSource();
                                    var htmlHelper = _printHelper.PrintData(printPalletdata);
                                    source.Html = htmlHelper;

                                    browser.Source = source;
                                    var printService = DependencyService.Get<IPrintService>();
                                    printService.Print(browser);
                                    // await Navigation.PushAsync(new EZPrintListPage(palletPrint));
                                }
                            }
                            ClearData();

                        }
                    }
                    else
                    {
                        if (EditOption == true) { await Application.Current.MainPage.DisplayAlert("Message", "Updation Fail", "OK"); } else { await Application.Current.MainPage.DisplayAlert("Message", "Insert Fail", "OK"); }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await Application.Current.MainPage.DisplayAlert("Message",ex.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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
        public async void LoadLotNo()
        {
            try
            {
                var data = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetlotNoreceive + "Pallet");
                if (data.Status == 1)
                {
                    LotNumberList = JsonConvert.DeserializeObject<List<LotNumberList>>(data.Response.ToString());
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
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
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
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
            }
        }
        private async void btn_SavePrint(object sender, EventArgs e)
        {
            try
            {
                if (EditOption == true)
                {
                    if (palletItem != null && txt_PalletTagNo.Text != "" && txt_PalletTagNo.Text != null)
                    {
                        PostPalletMaintainanceDetailAsync("SaveAndPrint");
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                    }
                }
                else
                {
                    if (_model != null)
                    {
                        PostPalletMaintainanceDetailAsync("SaveAndPrint");
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                    }
                }


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
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

                    var listitems = (from itm in palletItem where itm.Id == Convert.ToInt32(item) select itm).FirstOrDefault<PalletItemResponse>();
                    var PalletDetailDelete = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.DeletePalletItem + "?ItemTag=" + txt_PalletTagNo.Text + "&Id=" + listitems.Id);
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Message", ex.ToString(), "OK");
            }

        }
        private void srchbox_carret_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                var data = (ViewModels.LotNumberList)e.ChosenSuggestion;
                txt_lotNo.Text = data.LotNo;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


        }
        private async void srchbox_carret_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (txt_lotNo.Text != "" && txt_lotNo.Text != null)
            {
                txt_lotNo.ItemsSource = string.IsNullOrWhiteSpace(txt_lotNo.Text) ? null : LotNumberList.Where(x => x.LotNo.StartsWith(txt_lotNo.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
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

        private async void btn_SaveStockIn(object sender, EventArgs e)
        {
            try
            {
                
                if (EditOption == false)
                {
                    if (_model != null)
                    {
                        PostPalletMaintainanceDetailAsync("SaveAndStockIn");
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Fill All Detail", "OK");
                    }
                }


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

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
      
        private void btn_close_Clicked(object sender, EventArgs e)
        {
            popupStockInView.IsVisible = false;
        }

        private void btn_clear_Clicked(object sender, EventArgs e)
        {

        }

        private async void btn_binsearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                var getbins = await new PalletMaintainanceService().GetPalletLog(GetBintagsUrl.GetBintagList);
                if (getbins.Status == 1)
                {
                    _bintaglist = JsonConvert.DeserializeObject<List<BinViewModel>>(getbins.Response.ToString());
                    sampleList.ItemsSource = _bintaglist;
                }
                popupListView.IsVisible = true;

            }
            catch (Exception ex) { Crashes.TrackError(ex); }
           
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
            try
            {
                if (EditOption == false)
                {
                    if (txt_lotNo.Text != null && txt_lotNo.Text != "")
                    {

                        //var sendlot = LotNumberList.Where(a => a.LotNo.Contains(txt_lotNo.Text.Trim())).FirstOrDefault().WrReceivingLogId;
                        bool has = LotNumberList.Any(cus => cus.LotNo == txt_lotNo.Text);
                        if (has == true)
                        {
                            var sendlot = LotNumberList.Where(a => a.LotNo.Contains(txt_lotNo.Text.Trim())).FirstOrDefault().WrReceivingLogId;

                            var getbarcode = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetBarcode + sendlot);
                            if (getbarcode.Status == 1)
                            {
                                _barcodelist = JsonConvert.DeserializeObject<List<ProductBarcodeResponseModel>>(getbarcode.Response.ToString());
                                barcodeList.ItemsSource = _barcodelist;
                            }
                            popupListViewBarcode.IsVisible = true;
                        }
                        else
                        {
                            DisplayAlert("Alert!", "Check Lot No", "Ok");
                            txt_lotNo.Focus();
                        }




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
            catch (Exception ex){ Crashes.TrackError(ex); }
           
           
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

        private void barcodeList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        #endregion
    }
}