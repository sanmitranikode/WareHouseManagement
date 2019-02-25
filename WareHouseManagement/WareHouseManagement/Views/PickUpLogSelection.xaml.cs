
using Microsoft.AppCenter.Crashes;
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
	public partial class PiclUpLogSelection : ContentPage
	{
        #region Declaration
        WrPickupListModel _model = new WrPickupListModel();
        WrPickupListResponseViewModel _Responce = new WrPickupListResponseViewModel();

        #endregion


        public PiclUpLogSelection()
        {
            InitializeComponent();

        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
          

            GetPickedUpList();

        }
        #region Methods

        public async void GetPickedUpList()
        {
            try
            {

                var data = await new PalletMaintainanceService().GetPalletLog(GetPickUpListUrl.GetPickUpList);
                if (data.Status == 1)
                {

                    var PickUpListData = JsonConvert.DeserializeObject<List<WrPickupListModel>>(data.Response.ToString());
                    _Responce.WrPickupListModel = PickUpListData;
                    PalletList.ItemsSource = null;
                    PalletList.ItemsSource = _Responce.WrPickupListModel;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }




        }
        #endregion

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           
        }

        #region Events
        public async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {

                WrPickupListModel _modelResponse = new WrPickupListModel();
                var tappedHier = ((TappedEventArgs)e).Parameter;

                var data = _Responce.WrPickupListModel.Where(a => a.Id == Convert.ToInt32(tappedHier)).FirstOrDefault();

                await Navigation.PushAsync(new PickUpLogList(data.WRPickupListProducts));

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        #endregion
    }
}