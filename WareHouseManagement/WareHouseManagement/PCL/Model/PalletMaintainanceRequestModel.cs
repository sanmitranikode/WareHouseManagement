using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletModel
    {
        public string Tag { get; set; }
        public string PalletId { get; set; }
        public string Sku { get; set; }
        public int TotalProducts { get; set; }
        public string LotNo { get; set; }

        public List<PalletBarcodes> PalletBarcodes { get; set; }
    }
    public class PalletBarcodes
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public int WRReceivingProductsId { get; set; }
        public int Quantity { get; set; }
      

}
}
