using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClearTagMaintenance : ContentPage
	{
		public ClearTagMaintenance ()
		{
			InitializeComponent ();
		}

        private async void btn_ClrPalletTag_Clicked(object sender, EventArgs e)
        {
          ;
            await Navigation.PushAsync(new ClearPalletAndBinTagPage());
     
        }
        private async void btn_binTag_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClearPalletAndBinTagPage());
        }
    }
}