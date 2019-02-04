using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.ViewModels;

namespace WareHouseManagement.PCL.Service
{
    class WRRecievingLogService
    {
        HttpClientHelper _helper;

        public WRRecievingLogService()
        {
            _helper = new HttpClientHelper();
        }

        public async Task<ResultModel> DeleteWRRecievingLogProduct(WRReceivingProducts model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.PostData<WRReceivingProducts>(model, Url);

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
