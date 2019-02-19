using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.PCL.Service
{
    class DamageStockService
    {
        HttpClientHelper _helper;
        public DamageStockService()
        {
            _helper = new HttpClientHelper();
        }
        public async Task<ResultModel> InsertDamageStock(DamageStock model, string Url)
        {
            ResultModel _User = new ResultModel();
            try
            {

                _User = await _helper.Post<DamageStock>(model, Url);

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
