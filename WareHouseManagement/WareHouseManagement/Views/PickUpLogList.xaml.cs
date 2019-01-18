using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class PickUpLogList : ContentPage
    {
        IList<WRPickupListProductModel> _model;
       

        public PickUpLogList(IList<WRPickupListProductModel> _Data)
        {
            InitializeComponent();
            _model = _Data;
          //  GetPickUpLIstAsync();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            PalletList.ItemsSource = null;
            PalletList.ItemsSource = _model;


        }

        public async void GetPickUpLIstAsync()
        {

            var GetPickUpList = await new PalletMaintainanceService().GetPalletLog(GetPickUpListUrl.GetPickUpList);
            if (GetPickUpList.Status == 1)
            {
               var _model = JsonConvert.DeserializeObject<List<WRPickupListProductModel>>(GetPickUpList.Response.ToString());
           
                PalletList.ItemsSource = null;
                PalletList.ItemsSource = _model;
            }
        }
    }
}