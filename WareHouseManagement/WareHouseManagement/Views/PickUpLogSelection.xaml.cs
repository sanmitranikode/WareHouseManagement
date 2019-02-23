using Com.Zebra.Rfid.Api3;
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
        WrPickupListModel _model = new WrPickupListModel();
        WrPickupListResponseViewModel _Responce = new WrPickupListResponseViewModel();

        

        
        public PiclUpLogSelection()
        {
            InitializeComponent();

        }


               protected async override void OnAppearing()
        {
            base.OnAppearing();
          

            GetPickedUpList();

        }

        public async void GetPickedUpList()
        {


            var data = await new PalletMaintainanceService().GetPalletLog(GetPickUpListUrl.GetPickUpList);
            if (data.Status == 1)
            {
             
                var PickUpListData = JsonConvert.DeserializeObject<List<WrPickupListModel>> (data.Response.ToString());
                _Responce.WrPickupListModel = PickUpListData;
                  PalletList.ItemsSource = null;
                PalletList.ItemsSource = _Responce.WrPickupListModel;
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           
        }

      

       
       

     

        public virtual void StatusEvent(Com.Zebra.Rfid.Api3.Events.StatusEventData statusEvent)
        {

        }

        public async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            WrPickupListModel _modelResponse = new WrPickupListModel();
            var tappedHier = ((TappedEventArgs)e).Parameter;
          
            var data = _Responce.WrPickupListModel.Where(a => a.Id == Convert.ToInt32(tappedHier)).FirstOrDefault();

            await Navigation.PushAsync(new PickUpLogList(data.WRPickupListProducts));
        }
    }
}