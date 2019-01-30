using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class PalletlistViewModel
    {
        public string Tag { get; set; }

        public int TotalProducts { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }




        public string LotNo { get; set; }
    }
}
