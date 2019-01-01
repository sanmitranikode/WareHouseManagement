using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.ViewModels
{

    public class ProductBarcodeRequestModel
    {
        public string Barcode { get; set; }
        public string LotNo { get; set; }
    }
    public class ProductBarcodeResponseModel
    {
        public string LotNo { get; set; }
        public string Barcode { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
    }
}
