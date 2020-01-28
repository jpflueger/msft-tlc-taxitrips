using System;
using System.Linq;
using System.Threading.Tasks;
using TLC.Taxi.Data;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            Execute(args).Wait();
        }

        static async Task Execute(string[] args)
        {
            string dbPath = "./Assets/tripdata.db3";
            SqliteRepository<TaxiZone, short> zoneRepo = new SqliteRepository<TaxiZone, short>(dbPath);
            SqliteRepository<TaxiTrip, long> tripRepo = new SqliteRepository<TaxiTrip, long>(dbPath);

            if (args[0] == "zones")
            {
                var zones = await zoneRepo.GetAllAsync();
                foreach (var zone in zones)
                {
                    System.Console.WriteLine($"{zone.Borough} {zone.Zone}");
                }
            }
            else if (args.Length == 3 && args[0] == "trip" && int.TryParse(args[1], out int pickUpLocationId) && int.TryParse(args[2], out int dropOffLocationId))
            {
                var query = new TaxiTripMetricsQuery
                {
                    PickUpLocationId = pickUpLocationId,
                    DropOffLocationId = dropOffLocationId
                };
                var results = await tripRepo.QueryAsync<TaxiTripMetrics>(query);
                var metrics = results.SingleOrDefault();
                System.Console.WriteLine($"Average Duration =  {TimeSpan.FromSeconds(metrics.AverageDurationSeconds)}");
                System.Console.WriteLine($"Average Cost     = ${metrics.AverageCost:0.00}");
            }
        }
    }
}
