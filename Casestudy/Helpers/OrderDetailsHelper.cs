using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Helpers
{
    public class OrderDetailsHelper
    {
        public int CustomerID { get; set; }
        public int OrderId { get; set; }
        public string? ProductId { get; set; }
        public int QtyOrdered { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }
        public decimal SellingPrice { get; set; }
        public string? DateCreated { get; set; }
        public string? ProductName { get; set; }
    }
}