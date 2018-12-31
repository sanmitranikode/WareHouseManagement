using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public static class ValidationMessages
    {
        public static string Success = "Success";
        public static string Failure = "Failure";

        public static string PalletCreateSuccess = "Pallet is Created Successfully";
        public static string PalletCreateFailed = "Error while creating pallet";
        public static string PalletStockInSuccess = "Pallet is Stocked";
        public static string PalletStockInAlreadyExist = "Pallet is already Stocked in Bin";
        public static string PalletStockInFailed = "Error while pallet stock in process";
        public static string PalletClearSuccess = "Pallet is clear";
        public static string PalletClearFailed = "Error while clearing pallet";
    }
}
