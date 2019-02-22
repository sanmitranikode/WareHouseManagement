using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Common
{
    public class GlobalConstant
    {
        public static string BaseUrl = "http://3.16.25.240:8025/";
        public static string BaseUrlSignalR = "http://3.16.25.240:8025";
        public static string TokenURL = "http://3.16.25.240:8025/token";

        public static string AWS_ACCESS_KEY = "AWS_ACCESS_KEY";
        public static string AWS_SECRET_KEY = "AWS_SECRET_KEY";
        public static string PREF_USERID = "userId";
        public static string PREF_Password = "password";
        public static string PREF_AccessKEY = "accesskey";
        public static string PREF_Email = "email";
        public static string PREF_LONGITUDE = "longitude";
        public static string PREF_LATITUDE = "latitude";
        public static string AccessToken { get; set; }
        public static string UserName { get; set; }
        public static string UserPassword { get; set; }
        public static string Usertype { get; set; }
        public static string GoogleMapApiKey = "AIzaSyC50XshkjneoleOy4CTQxRmWAv-qYGhd8Q";

    }

    public struct PalletMaintainanceServiceUrl
    {
        public static string GetPalletMaintainanceDetai = "PalletBarcode/PostDetail";
        public static string GetUserLoginDetail = "Users/Authenticate";
        public static string GetPalletreceivinglog = "WarehouseReceiveLog";
        public static string PostPalletreceivinglog = "Pallet";
        public static string GetlotNoreceive = "PalletBarcode/GetLotNoList?ValueType=";
        public static string DeletePalletItem = "Pallet/DeletePalletItem";

        public static string GetPalletItemByTagId = "Pallet/GetPalletItemByTag";
        public static string GetReportRecord = "PalletBarcode/GetPalletMaintainanceReportDetail?Count=";
        public static string GetBarcode = "PalletBarcode/GetBarcodeList?LotNo=";
    }


    public struct StockInServiceUrl
    {

        public static string PostStockIn = "StockInPallet";
        public static string GetQuantity = "Pallet/";

    }

    public struct ClearPalletTagUrl
    {
        public static string ClearPalletTag = "PalletClear";

    }
    public struct ProductUrl
    {
        public static string GetProduct = "BinDetailByProduct/GetProductList";
        public static string GetProductDetails = "BinDetailByProduct/GetBinsDetail";
        public static string postproducts = "WrReceivingLogProduct/AddProductWrRecLog";
        public static string postEditproducts = "WrReceivingLogProduct/EditProductWrRecLog";
        public static string postdeleteproducts = "WrReceivingLogProduct/DeleteWrReceivingLogProduct";
    }

    public struct ClearBinTagUrl
    {
        public static string ClearBinTag = "BinClear";

    }


    public struct GetPickUpListUrl
    {
        public static string GetPickUpList = "PickUpList";

    }
    public struct GetBintagsUrl
    {
        public static string GetBintagList = "BinsList";
    }
  
    public struct GetPalletListUrl
    {
        public static string getpalletlist = "Pallet/GetPalletList";
        public static string getpalletTaglist = "Pallet/GetPalletTagList";
    }
    public struct GetCustomerAndVender
    {
        public static string getcustomerlist = "WrReceivingLogProduct/UserCustomerList";
        public static string getvenderlist = "WrReceivingLogProduct/GetVendorList";
        public static string getproductlist = "WrReceivingLogProduct/GetProductNameList?keyword=";
        public static string getmaxlotno = "WrReceivingLogProduct/GetMaxLotNo";
    }

    public struct DamageStockUrl
    {
        public static string InsertDamageStock = "DamageStock";
    }







}
