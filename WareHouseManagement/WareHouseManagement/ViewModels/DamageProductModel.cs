using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class DamageProductModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string Sku { get; set; }
        /// <summary>
        /// Gets or sets the damaged stock quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the  description
        /// </summary>
        public string ShortDescription { get; set; }
    }
}
