using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.PCL.Service
{
    class StockInPalletService
    {
        HttpClientHelper _helper;

    public StockInPalletService()
    {
        _helper = new HttpClientHelper();
    }


        public async Task<ResultModel> PostStockInDetail(StockInPalletModel model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.PostData<StockInPalletModel>(model, Url);

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
