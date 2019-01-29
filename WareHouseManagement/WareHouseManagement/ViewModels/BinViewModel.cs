using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
     public class BinViewModel
    {
        public string BinCode { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the BinTag
        /// </summary>
        public string BinTag { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        /// 
        public int LineId { get; set; }

        public int PositionId { get; set; }
    }
}
