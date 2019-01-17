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

        //prductDetail
        public string ShortDescription { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public string Sku { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Gtin { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public bool UseMultipleWarehouses { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal ProductCost { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal TotalCubic { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
    public class WrPickupListModel
    {
        public IList<WRPickupListProductModel> WRPickupListProducts { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int CustomerId { get; set; }
        public int ShipperId { get; set; }

        public int ShippingAddressId { get; set; }
        public string PONo { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public string ContainerNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

    }
    public class WRPickupListProductModel
    {

        public int WRPickupListId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int PickedQTY { get; set; }
        public DateTime ExDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cubic { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual WrPickupListModel WRPickupList { get; set; }
        public int PickupListStatusId { get; set; }

        public WRPickUplistStatusModel ProductStatus
        {
            get
            {
                return (WRPickUplistStatusModel)PickupListStatusId;
            }
            set
            {
                PickupListStatusId = (int)value;
            }
        }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LotNo { get; set; }
    }

   
    public enum WRPickUplistStatusModel
    {

        /// <summary>
        /// Processing
        /// </summary>
        Processing = 10,
        /// <summary>
        /// Complete
        /// </summary>
        StockOut = 20,

    }
}
