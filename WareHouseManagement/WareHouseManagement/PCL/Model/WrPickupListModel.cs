using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class WrPickupListModel
    {

        public string LotNo { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int CustomerId { get; set; }
        public string PONo { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public string ContainerNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
