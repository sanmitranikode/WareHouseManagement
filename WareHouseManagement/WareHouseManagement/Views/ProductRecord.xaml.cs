using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductRecord : ContentPage
	{
        #region Declaration
        List<ViewModels.WRReceivingProducts> product= new List<ViewModels.WRReceivingProducts>();
        #endregion

        public ProductRecord(List<ViewModels.WRReceivingProducts> _product)
		{
            product = _product;
            InitializeComponent();
            GetDetailAsync();


        }
        #region Methods

        private async void GetDetailAsync()
        {
         
            PalletList.ItemsSource = product;
        }
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }

     
    }
}