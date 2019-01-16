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

        ProductModel product= new ProductModel();
       WRReceivingLogReportResponseViewModel _model = new WRReceivingLogReportResponseViewModel();
        WRReceivingLogReportResponseViewModel Detail = new WRReceivingLogReportResponseViewModel();
        public ProductRecord (ResultModel model)
		{
            InitializeComponent();
            GetDetailAsync(model);


        }

        private async void GetDetailAsync(ResultModel ProductReport)
        {
             Detail = JsonConvert.DeserializeObject <WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());


            PalletList.ItemsSource = Detail.WRReceivingLogModel.FirstOrDefault().WRReceivingProducts;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }

     
    }
}