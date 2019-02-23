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
	public partial class StockInPage : ContentPage
	{
        public event PropertyChangedEventHandler PropertyChanged;
        ReaderModel rfidModel = ReaderModel.readerModel;
        public List<TagItem> Tags = new List<TagItem>();
        public List<PalletlistViewModel> _palleTagtlist = new List<PalletlistViewModel>();
        private Object tagreadlock = new object();
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();
        public List<BinViewModel> _bintaglist = new List<BinViewModel>();


        public bool isConnected { get => isConnected; set => OnPropertyChanged(); }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public StockInPage ()
		{
			InitializeComponent ();


		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();


            UpdateIn();
            getData();


          
          

        }

        private async void getData()
        {
            var getbins = await new PalletMaintainanceService().GetPalletLog(GetBintagsUrl.GetBintagList);
            if (getbins.Status == 1)
            {
                _bintaglist = JsonConvert.DeserializeObject<List<BinViewModel>>(getbins.Response.ToString());
                sampleList.ItemsSource = _bintaglist;
            }
           
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UpdateOut();
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
                                if (PalletTag.IsFocused == true)
                                {

                                    PalletTag.Text = Tags.FirstOrDefault(p => p.RelativeDistance == Tags.Max(p2 => p2.RelativeDistance)).InvID;
                                }

                                if (BinTag.IsFocused == true)
                                {

                                    BinTag.Text = Tags.FirstOrDefault(p => p.RelativeDistance == Tags.Max(p2 => p2.RelativeDistance)).InvID;
                                }
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
        private async void btnSave_ClickedAsync(object sender, EventArgs e)
        {
            btn_save.IsEnabled = true;
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
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
            catch(Exception ex)
            { }
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;

            btn_save.IsEnabled = false;

        }


        public async void GetQuantityByPalletId()
        {
            try
            {

                var GetData = await new PalletMaintainanceService().GetPalletLog(StockInServiceUrl.GetQuantity + PalletTag.Text);
                if (GetData.Status == 1)
                {
                    Quantity.Text = GetData.Response.ToString();
                }
                else { Quantity.Text = "0"; }
            }catch(Exception ex)
            {
               
            }
        }

         public void clearData()
        {

            PalletTag.Text = "";
            BinTag.Text = "";
            Quantity.Text = "";
            btn_save.IsEnabled = false;
        }
       

        private void PalletTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetQuantityByPalletId();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           
        }

        private void btn_binsearch_Clicked(object sender, EventArgs e)
        {
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

        private void BinTag_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private async void btn_palletTagSearch_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            popupLoadingView.IsVisible = true;
            var getPalletTaglist = await new PalletMaintainanceService().GetPalletLog(GetPalletListUrl.getpalletTaglist);
            if (getPalletTaglist.Status == 1)
            {
                _palleTagtlist = JsonConvert.DeserializeObject<List<PalletlistViewModel>>(getPalletTaglist.Response.ToString());
                PalletTaglist.ItemsSource = _palleTagtlist;
            }
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
         
            popupPalletTAgListView.IsVisible = true;

        }

        private void PalletTaglist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (PalletlistViewModel)e.SelectedItem;
            if (item != null)
            {
                PalletTag.Text = item.Tag;
                ((ListView)sender).SelectedItem = null;
            }

            popupPalletTAgListView.IsVisible = false;

        }
    }
}