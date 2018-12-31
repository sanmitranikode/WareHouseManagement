using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class StockOutPalletModel
    {
        /// <summary>
        /// Gets or sets the Pallet Tag
        /// </summary>
        public string Tag { get; set; }


        /// <summary>
        /// Gets or sets the collection of ProductManufacturer
        /// </summary>
        public List<PalletBarcodeModel> PalletBarcodes { get; set; }
    }
}
