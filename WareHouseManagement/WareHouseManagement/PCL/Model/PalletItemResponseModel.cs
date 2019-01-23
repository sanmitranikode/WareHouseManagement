using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
     public class PalletItemResponseModel
    {

        public IList<PalletItemResponse> PalletBarcodes { get; set; }

    }

    public class PalletItemResponse
    {
        public int Id { get; set; }
        public int PalletId { get; set; }

        public string LotNo { get; set; }

        public int WRReceivingProductsId { get; set; }

        public string Barcode { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; }
    }
}
