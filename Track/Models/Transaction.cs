using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Track.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        
        public virtual Store StoreId { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
    }
}