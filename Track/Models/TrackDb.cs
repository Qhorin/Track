using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Track.Models
{
    public class TrackDb : DbContext
    {
        public TrackDb() : base("name=DefaultConnection")
        {
        }   
        public DbSet<Store> Stores { get; set; }
    }
}