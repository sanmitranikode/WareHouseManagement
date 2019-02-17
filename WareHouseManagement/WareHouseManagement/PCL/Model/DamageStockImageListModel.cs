using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class DamageStockImageListModel
    {
        public string BarCode { get; set; }
        public byte[] imageProperty   { get; set; }
        public string ImagePath { get; set; }
    }
}
