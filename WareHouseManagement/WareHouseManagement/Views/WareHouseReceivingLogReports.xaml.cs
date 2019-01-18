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
	public partial class WareHouseReceivingLogReports : ContentPage
    {
        ResultModel ProductReport;
        WRReceivingLogReportResponseViewModel _model = new WRReceivingLogReportResponseViewModel();
        WRReceivingLogReportResponseViewModel Detail = new WRReceivingLogReportResponseViewModel();
        
        public WareHouseReceivingLogReports()
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
             ProductReport = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetReportRecord);
            if (ProductReport.Status == 1)
            {
                 Detail = JsonConvert.DeserializeObject<WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());
                _model = Detail;
                PalletList.ItemsSource = Detail.WRReceivingLogModel;
            }
            else
            {
            }

          
        }
        
        private async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            var tappedHier = ((TappedEventArgs)e).Parameter;
            ProductReport.Status = 1;
              var data = Detail.WRReceivingLogModel.Where(a => a.Id ==Convert.ToInt32(tappedHier)).FirstOrDefault().WRPickupListProducts;
           
            await Navigation.PushAsync(new ProductRecord(data));
        }

        private void PalletList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var tappedHier = e;
        }
    }
}