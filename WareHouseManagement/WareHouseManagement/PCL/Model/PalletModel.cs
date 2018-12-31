using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class PalletModel
    {
        /// <summary>
        /// Gets or sets the Pallet Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the Pallet Products
        /// </summary>
        public int TotalProducts { get; set; }

        /// <summary>
        /// Gets or sets the date and time of pallets
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time of pallets
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the Pallet Status
        /// </summary>
        public PalletStatus PalletStatus { get; set; }

        /// <summary>
        /// Gets or sets the collection of ProductManufacturer
        /// </summary>
      //  public List<PalletBarcodes> PalletBarcodes { get; set; }


    }
}
