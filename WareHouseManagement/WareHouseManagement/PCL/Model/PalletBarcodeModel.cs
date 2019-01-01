using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletBarcodeModel
    {
        
        public int PalletId { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Pallet Id
        /// </summary>
        public bool Selected { get; set; }
    }
}
