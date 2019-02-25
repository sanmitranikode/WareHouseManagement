using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarehouseReceivingLogPage : ContentPage
    {
        #region Declaration
        public ObservableCollection<WRReceivingLogResponseViewModel> Items { get; set; }
        #endregion

        public WarehouseReceivingLogPage()
        {
            InitializeComponent();
            
            GetReceiveLogAsync();
            //PalletList.ItemsSource = Items;
        }
#region Methods
        public async void GetReceiveLogAsync()
        {
            try
            {
                var PalletDetail = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetPalletreceivinglog);
                if (PalletDetail.Status == 1)
                {
                    var Detail = JsonConvert.DeserializeObject<List<WRReceivingLogResponseViewModel>>(PalletDetail.Response.ToString());

                    PalletList.ItemsSource = Detail;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

           

        }
        #endregion
        #region Events

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PalletMaintainancePage());
        }
        #endregion

        //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}
    }
}
