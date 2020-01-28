using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data.Console
{
    public class Program
    {
        public static readonly string DbPath = "./Assets/tripdata.db3";

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = "taxi",
                Description = "Predict taxi rides in NYC."
            };

            app.Command("boroughs", async (command) => 
            {
                var repo = new SqliteRepository<TaxiZone, short>(DbPath);
                var zones = await repo.GetAllAsync();
                foreach (var zone in zones)
                {
                    System.Console.WriteLine($" [{zone.Id}]: {zone.Zone}, {zone.Borough}");
                }
            });

            app.Command("predict", (command) => 
            {
                var pickUpArgument = command.Argument<short>("[pick-up]", "Where you will be picked up from.");
                var dropOffArgument = command.Argument<short>("[drop-off]", "Where you will be dropped of at.");
                var vehicleOption = command.Option<VehicleType>("-v|--vehicle <VEHICLE>", "The type of vehicle to take", CommandOptionType.SingleValue);

                command.OnExecuteAsync(async (ct) => 
                {
                    var repo = new SqliteRepository<TaxiTrip, long>(DbPath);
                    var query = new TaxiTripMetricsQuery
                    {
                        PickUpLocationId = pickUpArgument.ParsedValue,
                        DropOffLocationId = dropOffArgument.ParsedValue,
                        VehicleType = vehicleOption.ParsedValue
                    };
                    var results = await repo.QueryAsync<TaxiTripMetrics>(query, ct);
                    var metrics = results.SingleOrDefault();
                    System.Console.WriteLine($"Estimated Cost =>     ${metrics.AverageCost:0.00}");
                    System.Console.WriteLine($"Estimated Duration =>  {TimeSpan.FromSeconds(metrics.AverageDurationSeconds)}");
                    
                    return 0;
                });
            });

            return app.Execute(args);
        }
    }
}
