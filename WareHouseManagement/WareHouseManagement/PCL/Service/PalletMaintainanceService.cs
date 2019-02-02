using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.ViewModels;

namespace WareHouseManagement.PCL.Service
{
    class PalletMaintainanceService
    {
        HttpClientHelper _helper;

        public PalletMaintainanceService()
        {
            _helper = new HttpClientHelper();
        }
        public async Task<ResultModel> GetLoginDetail(UserModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.Post<UserModel>(model, Url);
                
                return _User;
            }
            catch (Exception ex)
            {
                // Crashes.TrackError(ex);
                return _User;
            }
        }
        public async Task<ResultModel> GetPalletMaintainanceDetail(ProductBarcodeRequestModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.Post<ProductBarcodeRequestModel>(model, Url);

                return _User;
            }
            catch (Exception ex)
            {
                // Crashes.TrackError(ex);
                return _User;
            }
        }

        public async Task<ResultModel> PostPalletMaintainanceDetail(PalletModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.PostData<PalletModel>(model, Url);

                return _User;
            }
            catch (Exception ex)
            {
                // Crashes.TrackError(ex);
                return _User;
            }
        }
       


        public async Task<ResultModel> GetPalletLog(string Url)
        {
            ResultModel resp = null;
            try
            {
                resp = await _helper.Get<ResultModel>(Url);
                return resp;
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return resp;
            }
        }
        public async Task<ResultModel> PostWRReceivingProduct(WRReceivingLogModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.PostData<WRReceivingLogModel>(model, Url);

                return _User;
            }
            catch (Exception ex)
            {
                // Crashes.TrackError(ex);
                return _User;
            }
        }

    }
}
