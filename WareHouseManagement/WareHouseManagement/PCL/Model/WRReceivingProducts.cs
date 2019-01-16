using System;
using System.Collections.Generic;
using System.Text;
using WareHouseManagement.ViewModels;

namespace WareHouseManagement.PCL.Model
{
    class WRReceivingProducts
    {

        /// <summary>
        /// Gets or sets the Warehouse Receiver identifier
        /// </summary>
        public int WRReceivingLogId { get; set; }

        /// <summary>
        /// Gets or sets the Product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Quantiy 
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the ExDate 
        /// </summary>
        public DateTime ExDate { get; set; }

        /// <summary>
        /// Gets or sets the ExDate 
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the ExDate 
        /// </summary>
        public decimal Cubic { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
       // public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the WRReceivingLog
        /// </summary>
        public virtual WRReceivingLog WRReceivingLog { get; set; }


        /// <summary>
        /// Gets or sets the Trasnaction Product Status
        /// </summary>
        public int ProductStatusId { get; set; }



        /// <summary>
        /// Gets or sets the CreateDate
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the Last update date
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the Trasnaction Product Status
        /// </summary>

    }
}
