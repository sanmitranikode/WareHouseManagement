
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Linq;
using System.Collections.Generic;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using WareHouseManagement.PCL.Model;
using Android.Graphics;
using System.IO;
using WareHouseManagement.PCL.Common;
using System.Collections.ObjectModel;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadDamageStockImage : ContentPage
    {
        List<LotNumberList> LotNumberList;

        byte[] damagepic = null;
        List<DamagedStockPicture> damagelist = new List<DamagedStockPicture>();
        public List<ProductBarcodeResponseModel> _barcodelist = new List<ProductBarcodeResponseModel>();
        bool EditOption = false;
        int forgrid = 0;
        public UploadDamageStockImage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            LoadLotNo();

            bool EditOption = false;
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
           

            if (checkValidation()==true)
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Damage Stock Images",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.Custom,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                    return;

                byte[] ByteImage = ImageToBinary(file.Path);
                System.IO.FileInfo info = new System.IO.FileInfo(file.Path);
                string fileName = info.Name;
                Bitmap BitmapImage = BitmapFactory.DecodeByteArray(ByteImage, 0, ByteImage.Length);

                var source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
                damagelist.Add(new DamagedStockPicture() { BarCode = txt_Barcode.Text, PictureBinary = ByteImage, PicturePath = file.Path, PictureName = fileName, bitMapImage = BitmapImage, imageSourceForImageControl = source });

                //  DamagestockListView.ItemsSource = damagelist;
                for (int rowIndex = 0; rowIndex < 1; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < 1; columnIndex++)
                    {

                        //var product = productArrayList[productIndex];
                        var lblbarcode = new Label
                        {
                            Text = txt_Barcode.Text,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center


                        };

                        var image = new Image
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Center
                        };
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
                        GridList.RowDefinitions.Add(new RowDefinition { Height = new GridLength(250) });
                        GridList.Children.Add(lblbarcode, 0, forgrid);
                        GridList.Children.Add(image, 1, forgrid);

                    }
                }
                forgrid = forgrid + 1;
            }
            else
            {


            }


            // DisplayAlert("File Location", file.Path, "OK");




        }

        private bool checkValidation()
        {
            if (txt_lotNo.Text == "" || txt_lotNo.Text == null)
            {
                DisplayAlert("Alert!", "Fill up Lot No", "OK");
                txt_lotNo.Focus();
                return false;
            }
            else
                if (txt_Barcode.Text == "" || txt_Barcode.Text == null)
            {
                DisplayAlert("Alert!", "Fill up Barcode", "OK");
                txt_Barcode.Focus();
                return false;

            }
            else
            {
                return true;
            }


           
        }

        public byte[] ImageToBinary(string imagePath)
        {

            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        private async void btn_Save_DamageStock_Clicked(object sender, EventArgs e)
        {
            if(checkValidation()==true )
            {
                if (damagelist != null && WrReceivingLogProductId.Text!="" && WrReceivingLogProductId.Text!=null)
                {
                    List<DamagedStockPicture> _PictureList = new List<DamagedStockPicture>();

                    var damageStock = new DamageStock
                    {
                        Quantity = Convert.ToInt32(txt_SetQuantity.Text),
                        ShortDescription = txt_Description.Text,
                        WRReceivingLogProductId = Convert.ToInt32(WrReceivingLogProductId.Text),
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow,
                        damagedStockPictures = damagelist.Select(x =>
                        {
                            return new DamagedStockPicture
                            {
                                PictureBinary = x.PictureBinary,
                                PictureName = x.PictureName,
                                PicturePath = x.PicturePath
                            };
                        }).ToList()

                    };
                    var getbins = await new DamageStockService().InsertDamageStock(damageStock, DamageStockUrl.InsertDamageStock);
                    if (getbins.Status == 1)
                    {
                        ClearData();
                        DisplayAlert("Massage", "Saved SuccessFully", "OK");
                    }
                    else
                    {
                        DisplayAlert("Massage", "Insertion Fail", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Massage", "Select Image First ", "OK");
                }

            }
           
        }
        public void ClearData()
        {
            getBarcode();

            var children = GridList.Children.ToList();
            foreach (var child in children)
            {
                GridList.Children.Remove(child);
            }
            damagelist = null;
            txt_Description.Text = null;
            txt_SetQuantity.Text = null;
            txt_Barcode.Text = null;
        }

        private void btn_ClearDamage_Clicked(object sender, EventArgs e)
        {
            ClearData();
            //foreach (var child in children)
            //{
            //    Grid.SetRow(child, Grid.GetRow(child) - 1);
            //}

        }
        public async void LoadLotNo()
        {
            var data = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetlotNoreceive + "DamageStock");
            if (data.Status == 1)
            {
                LotNumberList = JsonConvert.DeserializeObject<List<LotNumberList>>(data.Response.ToString());
            }
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {

            LoadLotNo();
        }
        private async void srchbox_carret_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (txt_lotNo.Text != "" && txt_lotNo.Text != null)
            {
                txt_lotNo.ItemsSource = string.IsNullOrWhiteSpace(txt_lotNo.Text) ? null : LotNumberList.Where(x => x.LotNo.StartsWith(txt_lotNo.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
        }
        private void srchbox_carret_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var data = (ViewModels.LotNumberList)e.ChosenSuggestion;
            txt_lotNo.Text = data.LotNo;
            getBarcode();

        }
        public async void getBarcode()
        {
            var sendlot = LotNumberList.Where(a => a.LotNo.Contains(txt_lotNo.Text.Trim())).FirstOrDefault().WrReceivingLogId;

            var getbarcode = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetBarcode + sendlot);
            if (getbarcode.Status == 1)
            {
                _barcodelist = JsonConvert.DeserializeObject<List<ProductBarcodeResponseModel>>(getbarcode.Response.ToString());
                barcodeList.ItemsSource = _barcodelist;
            }
        }
        private async void SearchTapped_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (EditOption == false)
                {
                    if (txt_lotNo.Text != null && txt_lotNo.Text != "")
                    {
                        bool has = LotNumberList.Any(cus => cus.LotNo == txt_lotNo.Text);
                        if (has == true)
                        {
                            if (_barcodelist != null)
                            {
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
            catch (Exception ex) { }

        }
        private void SearchBarCode_Tapped(object sender, EventArgs e)
        {
            //txt_Barcode.Text = null;
            //txt_SetQuantity.Text = null;
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
                WrReceivingLogProductId.Text = PalletDetail.Id.ToString();

            }
            popupListViewBarcode.IsVisible = false;

        }

        private void txt_Barcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_barcodelist != null)
            {
                var Barcodes = _barcodelist.Where(a => a.Barcode == txt_Barcode.Text).FirstOrDefault();
                if (Barcodes != null)
                {
                    txt_SetQuantity.Text = Barcodes.Quantity;
                    WrReceivingLogProductId.Text = Barcodes.Id.ToString();
                }
                else
                {
                    WrReceivingLogProductId.Text = "";
                }
            }
        }
    }
}