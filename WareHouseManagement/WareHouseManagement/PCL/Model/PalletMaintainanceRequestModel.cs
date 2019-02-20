using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletModel
    {

        public string Tag { get; set; }

        public int TotalProducts { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public PalletStatus PalletStatus { get; set; }

        public IList<PalletBarcodes> PalletBarcodes { get; set; }

        public string CustomerName { get; set; }

        public string ReceivingDate { get; set; }


        public string LotNo { get; set; }

        public int PalletId { get; set; }


    }
    public class PalletBarcodes
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public int WRReceivingProductsId { get; set; }
        public int Quantity { get; set; }
      

}
}
