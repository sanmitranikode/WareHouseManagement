using Microsoft.AppCenter.Crashes;
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
        #region Declaration
        ResultModel ProductReport;
        WRReceivingLogReportResponseViewModel _model = new WRReceivingLogReportResponseViewModel();
        WRReceivingLogReportResponseViewModel Detail = new WRReceivingLogReportResponseViewModel();
        int Count = 0;
        #endregion
        public WareHouseReceivingLogReports()
		{
			InitializeComponent ();
           

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Count = (_model.WRReceivingLogModel == null) ? 0 : _model.WRReceivingLogModel.Count();
            GetDetailAsync();
        }
        #region Methods
        public async void GetDetailAsync()
        {
            try
            {
                activityIndicator.IsRunning = true;
                popupLoadingView.IsVisible = true;
                int value = (_model.WRReceivingLogModel == null) ? 0 : _model.WRReceivingLogModel.Count();
                ProductReport = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetReportRecord + Count);
                if (ProductReport.Status == 1)
                {
                    Detail = JsonConvert.DeserializeObject<WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());
                    _model = Detail;
                    PalletList.ItemsSource = Detail.WRReceivingLogModel;
                }
                else
                {
                }

                activityIndicator.IsRunning = false;
                popupLoadingView.IsVisible = false;

            }catch(Exception ex)
            {
                Crashes.TrackError(ex);

            }

        }

        #endregion
        #region Events
        private async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            try
            {
                var tappedHier = ((TappedEventArgs)e).Parameter;
                ProductReport.Status = 1;
                var data = Detail.WRReceivingLogModel.Where(a => a.LotNo == tappedHier).FirstOrDefault().wrReceivingProducts;

                await Navigation.PushAsync(new ProductRecord(data));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                
            }


        }

        private void PalletList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var tappedHier = e;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWrReceivingLogProduct(null));
        }

        private async void nextlistData_Clicked(object sender, EventArgs e)
        {
            try
            {
                activityIndicator.IsRunning = true;
                popupLoadingView.IsVisible = true;
                Count = Count + 10;
                ProductReport = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetReportRecord + Count);
                if (ProductReport.Status == 1)
                {
                    Detail = JsonConvert.DeserializeObject<WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());
                    _model = Detail;
                    PalletList.ItemsSource = Detail.WRReceivingLogModel;
                    if (_model.WRReceivingLogModel.Count < 10)
                    {
                        nextlistData.IsEnabled = false;
                        previouslistdata.IsEnabled = true;
                    }
                }
                else
                {
                }
                activityIndicator.IsRunning = false;
                popupLoadingView.IsVisible = false;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


        }

        private async void previouslistdata_Clicked(object sender, EventArgs e)
        {
            try
            {
                activityIndicator.IsRunning = true;
                popupLoadingView.IsVisible = true;

                Count = Count - 10;
                if (Count < 0) { Count = 0; }
                ProductReport = await new PalletMaintainanceService().GetPalletLog(PalletMaintainanceServiceUrl.GetReportRecord + Count);
                if (ProductReport.Status == 1)
                {
                    Detail = JsonConvert.DeserializeObject<WRReceivingLogReportResponseViewModel>(ProductReport.Response.ToString());
                    _model = Detail;
                    PalletList.ItemsSource = Detail.WRReceivingLogModel;
                    if (Count <= 0)
                    {
                        nextlistData.IsEnabled = true;
                        previouslistdata.IsEnabled = false;
                    }

                }
                else
                {
                }
                activityIndicator.IsRunning = false;
                popupLoadingView.IsVisible = false;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            

        }

        private async void TapEditeDetails_Tapped(object sender, EventArgs e)
        {
            try
            {
                var tappedEdit = ((TappedEventArgs)e).Parameter;
                var data = Detail.WRReceivingLogModel.Where(a => a.Id == Convert.ToInt32(tappedEdit)).FirstOrDefault();
                await Navigation.PushAsync(new CreateWrReceivingLogProduct(data));


            }catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        private async void TapViewDetails_Tapped(object sender, EventArgs e)
        {
            try
            {
                var tappedHier = ((TappedEventArgs)e).Parameter;
                ProductReport.Status = 1;
                var data = Detail.WRReceivingLogModel.Where(a => a.LotNo == tappedHier).FirstOrDefault().wrReceivingProducts;

                await Navigation.PushAsync(new ProductRecord(data));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }



        }
        #endregion
    }
}