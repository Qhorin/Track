using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Track.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal TotalPrice { get; set; }


        public int StoreId { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
    }
}