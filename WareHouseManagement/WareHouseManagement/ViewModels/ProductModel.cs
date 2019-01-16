using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class ProductModel
    {

        public int WRReceivingLogId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cubic { get; set; }
        public string Name { get; set; }
    }
}
