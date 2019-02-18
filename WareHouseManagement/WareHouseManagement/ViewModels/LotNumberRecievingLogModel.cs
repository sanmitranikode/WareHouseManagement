using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class LotNumberRecievingLogModel
    {
        public IList<LotNumberList> LotNoNumber { get; set; }

    }

    public class LotNumberList
    {
        public int WrReceivingLogId { get; set; }
        public string LotNo { get; set; }

    }



}
