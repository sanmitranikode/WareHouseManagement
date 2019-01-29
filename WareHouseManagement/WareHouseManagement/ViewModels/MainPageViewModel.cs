using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace WareHouseManagement.ViewModels
{
    class MainPageViewModel: BaseViewModel
    {
        private string _connectionStatus;
        private string _connectionStatusicon;
        private System.Timers.Timer aTimer;
        public string readerConnection { get => _connectionStatus; set { _connectionStatus = value; OnPropertyChanged(); } }
        public string readerConnectionicon { get => _connectionStatusicon; set { _connectionStatusicon = value; OnPropertyChanged(); } }
        public MainPageViewModel()
        {
            updateHints();
           
        }

        private void updateHints()
        {
            //readerConnection = isConnected ? ShowAlert("Connected"): ShowAlert("Disconnect");

            //if (isConnected)
            //{
            //    ShowAlert("Reader Connected");
            //}
            //else
            //{
            //    ShowAlert("Reader Disconnected");

            //}
            readerConnectionicon = isConnected ? "rfidconnect" : "rfidnotconnectred";
        }

        public override void ReaderConnectionEvent(bool connection)
        {
            base.ReaderConnectionEvent(connection);
            updateHints();
            aTimer?.Stop();
            aTimer?.Dispose();
        }
        public override void ReaderAppearanceEvent(bool appeared)
        {
            base.ReaderAppearanceEvent(appeared);
            ThreadPool.QueueUserWorkItem(o =>
            {
                updateHints();
                // if single reader and not connected then connect same
                //if (AllItems.Count == 1 && !rfidModel.isConnected)
                //    ItemSelected(AllItems[0]);
            });
        }
        private void ShowAlert(string v)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Android.App.Application.Context, v, ToastLength.Short).Show();
            });
            Console.WriteLine(v);
        }


    }
}
