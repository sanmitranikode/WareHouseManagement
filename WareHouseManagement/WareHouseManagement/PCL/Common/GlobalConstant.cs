using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Common
{
    public class GlobalConstant
    {
        public static string BaseUrl = "http://97.74.238.15:8029/";
        public static string BaseUrlSignalR = "http://97.74.238.15:8029";
        public static string TokenURL = "http://97.74.238.15:8029/token";

        public static string AWS_ACCESS_KEY = "AWS_ACCESS_KEY";
        public static string AWS_SECRET_KEY = "AWS_SECRET_KEY";
        public static string PREF_USERID = "userId";
        public static string PREF_Password = "password";
        public static string PREF_AccessKEY = "accesskey";
        public static string PREF_Email = "email";
        public static string PREF_LONGITUDE = "longitude";
        public static string PREF_LATITUDE = "latitude";
        public static string AccessToken { get; set; }
        public static string Usertype { get; set; }
        public static string GoogleMapApiKey = "AIzaSyC50XshkjneoleOy4CTQxRmWAv-qYGhd8Q";

    }

    public struct PalletMaintainanceServiceUrl
    {
        public static string GetPalletMaintainanceDetai = "PalletBarcode/PostDetail";
        public static string GetUserLoginDetail = "Users/Authenticate";
        public static string GetPalletreceivinglog = "WarehouseReceiveLog";
        public static string PostPalletreceivinglog = "Pallet";
      

    }


    public struct StockInServiceUrl
    {

        public static string PostStockIn = "StockInPallet";
    }

}
