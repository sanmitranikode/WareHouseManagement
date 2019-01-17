﻿using Com.Zebra.Rfid.Api3;
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
	public partial class ClearPalletPage : ContentPage
	{
        public event PropertyChangedEventHandler PropertyChanged;
        ReaderModel rfidModel = ReaderModel.readerModel;
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

        public ClearPalletPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            UpdateIn();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UpdateOut();
            PalletList.ItemsSource = _model;
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

                                ReadPalletTag.Text = Tags.FirstOrDefault(p => p.RelativeDistance == Tags.Max(p2 => p2.RelativeDistance)).InvID;


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


        public async void ClearPalletTagAsync()
        {
            try
            {
                StockInPalletResponseModel _model = new StockInPalletResponseModel();
                {
                    _model.Tag = "1234567890";

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

            if (ReadPalletTag.Text != "")
            {
                GetPalletItem();
            }

        }

        private async void GetPalletItem()
        {
            try
            {

                var PalletDetail = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetPalletItemByTagId + "?=" + ReadPalletTag.Text);
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