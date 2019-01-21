using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManagement.ViewModels
{
    public class BinsDetailByProductModel
    {
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public string Bin { get; set; }
        public DateTime ExDate { get; set; }
    }
}
