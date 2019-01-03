using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletMaintainanceRequestModel
    {
        public string PalletRFID { get; set; }
        public List<PalletProductsModel> PalletProductsModel { get; set; }
    }
    public class PalletProductsModel
    {
        public int WRReceivedProductId { get; set; }
        public int WRReceivedLogId { get; set; }

    }
}
