using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.PCL.Service
{
    class StockOutPalletService
    {
        HttpClientHelper _helper;

        public StockOutPalletService()
        {
            _helper = new HttpClientHelper();
        }

        public async Task<ResultModel> GetPalletByTag(string Url)
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

        public async Task<ResultModel> MobileNumberVerfy(StockInPalletModel model, string Url)
        {
            ResultModel resp = null;
            try
            {

                resp = await _helper.Post<StockInPalletModel>(model, Url);
                return resp;
            }
            catch (Exception ex)
            {
                // Crashes.TrackError(ex);
                return resp;
            }
        }
        
    }
}
