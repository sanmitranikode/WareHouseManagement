using System;
using System.Collections.Generic;
using System.Text;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.ViewModels
{
    public class PalletlistViewModel
    {
        public string Tag { get; set; }

        public int TotalProducts { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public PalletStatus PalletStatus { get; set; }

      

        public string CustomerName { get; set; }

        public string ReceivingDate { get; set; }


        public string LotNo { get; set; }

        public int PalletId { get; set; }

    }
}
