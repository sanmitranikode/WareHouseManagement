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
	public partial class CreateWrReceivingLogProduct : ContentPage
	{
		public CreateWrReceivingLogProduct ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            popupListViewCustomer.IsVisible = false;
        }

        private void sampleList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void Searchcustomer_Tapped(object sender, EventArgs e)
        {
            popupListViewCustomer.IsVisible = true;
        }

        private void Tapancel_Tapped(object sender, EventArgs e)
        {
            popupListViewVender.IsVisible = false;
        }

        private void listVender_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void TapVenderCancel_Tapped(object sender, EventArgs e)
        {
            popupListViewVender.IsVisible = true;
        }
    }
}