using System;
using System.Collections.Generic;
using System.Text;

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
            readerConnection = isConnected ? "Connected" : "Not connected";
            readerConnectionicon = isConnected ? "rfidconnect" : "rfidnotconnectred";
        }

        public override void ReaderConnectionEvent(bool connection)
        {
            base.ReaderConnectionEvent(connection);
            updateHints();
            aTimer?.Stop();
            aTimer?.Dispose();
        }
    }
}
