using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Track.Models
{
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }
    }
}