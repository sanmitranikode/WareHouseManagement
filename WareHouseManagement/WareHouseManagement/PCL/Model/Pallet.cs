

using System;
using System.Collections.Generic;

namespace WareHouseManagement.PCL.Model
{
    public partial class Pallet
    {
        // private ICollection<PalletBarcodes> _palletBarcodes;
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
        //public virtual ICollection<PalletBarcodes> PalletBarcodes
        //{
        //    get { return _palletBarcodes ?? (_palletBarcodes = new List<PalletBarcodes>()); }
        //    protected set { _palletBarcodes = value; }
        //}
    }

    public enum PalletStatus
    {
        Clear = 10,
        InUse = 20
    }
}
