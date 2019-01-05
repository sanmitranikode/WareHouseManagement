using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StockInPage : ContentPage
	{
		public StockInPage ()
		{
			InitializeComponent ();


		}

        private async void btnSave_ClickedAsync(object sender, EventArgs e)
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

        }


        public void clearData()
        {

            PalletTag.Text = "";
            BinTag.Text = "";
            Quantity.Text = "";
        }

      
    }
}