using System;
using System.Collections.Generic;
using System.Text;

namespace WareHouseManagement.PCL.Model
{
    public class ResultModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Response { get; set; }
    }
}
