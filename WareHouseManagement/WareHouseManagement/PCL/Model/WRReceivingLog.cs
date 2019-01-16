using System;
using System.Collections.Generic;
using System.Text;
using WareHouseManagement.ViewModels;

namespace WareHouseManagement.PCL.Model
{
    class WRReceivingLog
    {
        private ICollection<WRReceivingProducts> _wrReceivingProducts;

        /// <summary>
        /// Gets or sets the warehouseReceivingId
        /// </summary>
        public string LotNo { get; set; }
        /// <summary>
        /// Gets or sets the Received Date
        /// </summary>
        public DateTime ReceivedDate { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the PONo
        /// </summary>
        public string PONo { get; set; }

        /// <summary>
        /// Gets or sets the PONo
        /// </summary>
        public int WRReceivingLogStatusId { get; set; }


        /// <summary>
        /// Gets or sets the Deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the Deleted
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets the ContainerNo
        /// </summary>
        public string ContainerNo { get; set; }
        /// <summary>
        /// Gets or sets the Create Date
        /// </summary>
        public DateTime CreateDate { get; set; }


        /// <summary>
        /// Gets or sets the Last Update Date
        /// </summary>
        public DateTime LastUpdateDate { get; set; }



        #region Navigation properties

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
       // public virtual Customer Customer { get; set; }



        #endregion
        /// <summary>
        /// Gets or sets the collection of WrReceivingProducts
        /// </summary>
        public virtual ICollection<WRReceivingProducts> WrReceivingProducts
        {
            get { return _wrReceivingProducts ?? (_wrReceivingProducts = new List<WRReceivingProducts>()); }
            protected set { _wrReceivingProducts = value; }
        }

        #region Custom properties

        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        public WRReceivingLogStatus WRReceivingLogStatus
        {
            get
            {
                return (WRReceivingLogStatus)WRReceivingLogStatusId;
            }
            set
            {
                WRReceivingLogStatusId = (int)value;
            }
        }
        #endregion


    }
    public enum WRReceivingLogStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 10,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40
    }
}