using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class WRReceivingLogReportResponseViewModel
    {
        public IList<WRReceivingLogModel> WRReceivingLogModel { get; set; }

    }

    public class WRReceivingLogModel
    {
      

        public string LotNo { get; set; }

        public DateTime ReceivedDate { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFullName { get; set; }

        public int WRReceivingLogStatusId { get; set; }

        public int UserId { get; set; }

        public string PONo { get; set; }

        public decimal TotalWeight { get; set; }

        public decimal TotalCubic { get; set; }

        public int TotalQuantity { get; set; }

        public int Id { get; set; }

        public string Status { get; set; }
        public bool Deleted { get; set; }


        public bool Active { get; set; }

        public string ContainerNo { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
        public List<WRReceivingProducts> wrReceivingProducts { get; set; }
    }

    public class WRReceivingProducts
    {
        public int Id { get; set; }
        public int WRReceivingLogId { get; set; }
        public int WRReceivingProductId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cubic { get; set; }
        public string ProductName { get; set; }
        public int ProductStatusId { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string Status { get; set; }
    }

    public enum ProductStatus
    {
        Pending = 10,
        Processing = 20,
        StockIn = 30,
        StockOut = 40,
        PickUp = 50
    }


}
