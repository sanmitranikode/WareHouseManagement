using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletModel
    {
        public string Tag { get; set; }


     
        public List<PalletBarcodes> PalletBarcodes { get; set; }
    }
    public class PalletBarcodes
    {
      
        public string LotNo { get; set; }
        public int WRReceivingProductsId { get; set; }
        public int Quantity { get; set; }
      
}
}
