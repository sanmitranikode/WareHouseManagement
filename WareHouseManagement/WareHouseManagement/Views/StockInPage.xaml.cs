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
	public partial class StockInPage : ContentPage
	{
		public StockInPage ()
		{
			InitializeComponent ();


		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetUserLoginAsync();
        }
        public async Task GetUserLoginAsync()
        {
            UserModel _User = new UserModel
            {
                Username = "nealandmassy@gmail.com",
                Password = "lalit4mm"
            };
            var UserDetail = await new PalletMaintainanceService().GetLoginDetail(_User, PalletMaintainanceServiceUrl.GetUserLoginDetail);
            if (UserDetail.Status == 1)
            {
                var UserData = JsonConvert.DeserializeObject<UserLoginViewModel>(UserDetail.Response.ToString());
                GlobalConstant.AccessToken = UserData.Token;
            }
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