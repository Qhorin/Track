namespace Track.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Track.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Track.Models.TrackDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Track.Models.TrackDb context)
        {
            context.Stores.AddOrUpdate(r => r.Name,
                new Store
                {
                    Name = "Harris Teeter",
                    Number = 1234,
                    Address = "College Road",
                    Phone = "910-555-1234",
                    City = "Wilmington",
                    State = "NC",
                    Country = "USA"
                },
                new Store
                {
                    Name = "Food Lion",
                    Number = 0987,
                    Address = "17th Street",
                    Phone = "910-555-9087",
                    City = "Wilmington",
                    State = "NC",
                    Country = "USA"
                },
                new Store
                {
                    Name = "Whole Foods",
                    Number = 2345,
                    Address = "Oleander Drive",
                    Phone = "910-555-2345",
                    City = "Wilmington",
                    State = "NC",
                    Country = "USA"
                }
              );

        }
    }
}
