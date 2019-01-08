using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.PCL.Service
{
    class ClearPalletTagService
    {

        HttpClientHelper _helper;

        public ClearPalletTagService()
        {
            _helper = new HttpClientHelper();
        }


        public async Task<ResultModel> ClearPalletTag(StockInPalletResponseModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.Post<StockInPalletResponseModel>(model, Url);

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
