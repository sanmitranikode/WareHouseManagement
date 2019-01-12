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
	public partial class ProductRecords : ContentPage
    {
        WRReceivingLogReportResponseViewModel _model = new WRReceivingLogReportResponseViewModel();
        public ProductRecords ()
		{
			InitializeComponent ();
            GetDetailAsync();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
       public async void GetDetailAsync()
        {
            var ProductReport = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetReportRecord);
            if (ProductReport.Status == 1)
            {
                var Detail = JsonConvert.DeserializeObject<WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());
                _model = Detail;
                PalletList.ItemsSource = Detail.WRReceivingLogModel;
            }
            else
            {
            }
        }
           

        

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}