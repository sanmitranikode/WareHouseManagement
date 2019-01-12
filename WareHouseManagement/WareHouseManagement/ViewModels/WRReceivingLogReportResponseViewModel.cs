using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
   public class WRReceivingLogReportResponseViewModel
    {
        private IList<WRReceivingProducts> WRReceivingProducts { get; set; }
        public string LotNo { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int CustomerId { get; set; }
        public string PONo { get; set; }
        public int WRReceivingLogStatusId { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public string ContainerNo { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public virtual Customer Customer { get; set; }
    }

    public class WRReceivingProducts
    {
        public int WRReceivingLogId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cubic { get; set; }
        public virtual Product Product { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int VendorId { get; set; }
        public string Sku { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Gtin { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public bool UseMultipleWarehouses { get; set; }
        public int WarehouseId { get; set; }
    }

    public class Customer
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string EmailToRevalidate { get; set; }
        public string AdminComment { get; set; }
        public bool IsTaxExempt { get; set; }
        public int AffiliateId { get; set; }
        public int VendorId { get; set; }
    }
}
