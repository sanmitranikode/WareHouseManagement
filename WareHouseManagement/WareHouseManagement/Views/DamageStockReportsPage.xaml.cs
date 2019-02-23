using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DamageStockReportsPage : ContentPage
	{
        List<DamageProductModel> _damageProductlist = new List<DamageProductModel>();
		public DamageStockReportsPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            LoadDamageList();

            
        }

        private async void LoadDamageList()
        {
            try
            {
                var data = await new PalletMaintainanceService().GetPalletLog(DamageStockUrl.DamageStockList);
                if (data.Status == 1)
                {
                    _damageProductlist = JsonConvert.DeserializeObject<List<DamageProductModel>>(data.Response.ToString());
                    listDamagestock.ItemsSource = null;
                    listDamagestock.ItemsSource = _damageProductlist;
                }

            }
            catch { }
        }

        private async void btn_newAddDamage_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadDamageStockImage());
        }

        private void listDamagestock_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            

            ((ListView)sender).SelectedItem = null;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}