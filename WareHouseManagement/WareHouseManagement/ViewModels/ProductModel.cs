﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int WRReceivingLogId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cubic { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        //prductDetail
        public string ShortDescription { get; set; }
        public int ProductStatusId { get; set; }
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



    public class WrPickupListResponseViewModel
    {
        public IList<WrPickupListModel> WrPickupListModel { get; set; }
    }




    public class WrPickupListModel
    {
        public DateTime ReceivedDate { get; set; }
        public int CustomerId { get; set; }
        public int ShipperId { get; set; }
        public string CustomerName { get; set; }
        public int ShippingAddressId { get; set; }
        public string PONo { get; set; }
        public string LotNo { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public string ContainerNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
        public IList<WRPickupListProductModel> WRPickupListProducts { get; set; }
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

        public string ProductStatus { get; set; }

        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LotNo { get; set; }
    }
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }

    }
    public class VendorModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
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
