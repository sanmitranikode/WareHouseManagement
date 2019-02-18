
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
        List<DamageStockImageListModel> damagelist=new List<DamageStockImageListModel>();
        public List<ProductBarcodeResponseModel> _barcodelist = new List<ProductBarcodeResponseModel>();
        bool EditOption = false;
        int forgrid=0;
        public UploadDamageStockImage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
           // LoadLotNo();

            bool EditOption = false;
        }

            private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
           if(txt_Barcode.Text=="" || txt_Barcode.Text == null)
            {
                DisplayAlert("Alert!", "Fill up Barcode", "OK");
                txt_Barcode.Focus();

            }
            else
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                    return;
             
                byte[] value = ImageToBinary(file.Path);


                System.IO.FileInfo info = new System.IO.FileInfo(file.Path);
                string fileName = info.Name;
                Bitmap val = BitmapFactory.DecodeByteArray(value, 0, value.Length);
                var source = ImageSource.FromStream(() =>
                  {
                      var stream = file.GetStream();
                      file.Dispose();
                      return stream;
                  });

                damagelist.Add(new DamageStockImageListModel() { BarCode = txt_Barcode.Text, imageProperty = value, ImagePath = file.Path, bitMapImage = val, imageSourceForImageControl = source });

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


            // DisplayAlert("File Location", file.Path, "OK");




        }
      


        public byte[] ImageToBinary(string imagePath)
        {

            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        private void btn_Save_DamageStock_Clicked(object sender, EventArgs e)
        {

             DisplayAlert("Massage", "Saved SuccessFully" , "OK");

        }

        private void btn_ClearDamage_Clicked(object sender, EventArgs e)
        {
            var children = GridList.Children.ToList();
            foreach (var child in children)
            {
                GridList.Children.Remove(child);
            }
            damagelist = null;
            txt_SetQuantity.Text = null;
            txt_Barcode.Text = null;
            //foreach (var child in children)
            //{
            //    Grid.SetRow(child, Grid.GetRow(child) - 1);
            //}

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

        }
        private async void SearchTapped_Tapped(object sender, EventArgs e)
        {
            if (EditOption == false)
            {
                if (txt_lotNo.Text != null && txt_lotNo.Text != "")
                {
                    var getbarcode = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetBarcode + txt_lotNo.Text);
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
    }
}