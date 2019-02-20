using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WareHouseManagement.PCL.Model
{
    public class DamagedStockPicture
    {
        public string BarCode { get; set; }
        public byte[] PictureBinary { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public Bitmap bitMapImage { get; set; }
        public ImageSource imageSourceForImageControl { get; set; }
    }
    public class DamageStock
    {
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the  description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// wrreceivingLogProduct identifier
        /// </summary>
        public int WRReceivingLogProductId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of stock creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of stock update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }
       public ICollection<DamagedStockPicture> damagedStockPictures { get; set; }
    }
}
