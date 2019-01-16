using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    class PalletResponceModel
    {

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
        /// Gets or sets an pallet Status  identifier
        /// </summary>
        public int PalletStatusId { get; set; }
    }
}
