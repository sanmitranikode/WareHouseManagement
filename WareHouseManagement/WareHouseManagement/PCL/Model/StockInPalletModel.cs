using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class StockInPalletModel
    {
        public string PalletTag { get; set; }
        public string BinTag { get; set; }
        public int Quantity { get; set; }
    }
}
