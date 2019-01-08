using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    class StockInPalletResponseModel
    {
        public string Tag { get; set; }
        public List<PalletBarcodeModel> PalletBarcodes { get; set; }
    }
}
