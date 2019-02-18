using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WareHouseManagement.PCL.Model
{
    public class DamageStockImageListModel
    {
        public string BarCode { get; set; }
        public byte[] imageProperty   { get; set; }
        public string ImagePath { get; set; }
        public Bitmap bitMapImage { get; set; }
        public ImageSource imageSourceForImageControl { get; set; }
    }
}
