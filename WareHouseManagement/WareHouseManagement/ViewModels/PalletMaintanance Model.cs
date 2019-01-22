using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{

    public class ProductBarcodeRequestModel
    {
        public string Barcode { get; set; }
        public string LotNo { get; set; }
        public int CheckQuantity { get; set; }
    }
    public class ProductBarcodeResponseModel
    {
        public string LotNo { get; set; }
        public string Barcode { get; set; }
        public string WrReceivingProductId { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public bool CheckQty { get; set; }
    }
    public class WRReceivingLogResponseViewModel
    {
        public int Id { get; set; }
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
        public int WRReceivingLogStatus { get; set; }
        public string Customer { get; set; }
    }

}
