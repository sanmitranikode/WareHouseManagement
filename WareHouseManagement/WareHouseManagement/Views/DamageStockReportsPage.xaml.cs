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
	public partial class DamageStockReportsPage : ContentPage
	{
		public DamageStockReportsPage ()
		{
			InitializeComponent ();
		}

        private async void btn_newAddDamage_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadDamageStockImage());
        }
    }
}