using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Track.Models;

namespace Track.ViewModels
{
    public class TransactionEditViewModel
    {
        public Transaction Transaction { get; set; }
        public IEnumerable<TransactionItem> TransactionItem { get; set; }
    }
}