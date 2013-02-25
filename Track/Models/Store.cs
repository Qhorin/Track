using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Track.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}