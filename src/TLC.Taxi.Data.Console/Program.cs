using System;
using System.Collections.Generic;
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
                Description = "Predict taxi rides in NYC.",
            };

            app.Command("boroughs", (command) => 
            {
                var queryOption = command.Option<string>("-q|--query <QUERY>", "A query string to fuzzy search with", CommandOptionType.SingleOrNoValue);

                command.OnExecuteAsync(async (ct) =>
                {
                    try
                    {
                        var repo = new SqliteRepository<TaxiZone, short>(DbPath);
                        var zones = await repo.GetAllAsync();

                        if (!string.IsNullOrEmpty(queryOption.ParsedValue))
                        {
                            zones = zones.Where(z => z.Borough.StartsWith(queryOption.ParsedValue));
                        }

                        foreach (var zone in zones)
                        {
                            System.Console.WriteLine($" [{zone.Id}]: {zone.Zone}, {zone.Borough}");
                        }

                        return 0;
                    }
                    catch (Exception e)
                    {
                        System.Console.Error.WriteLine(e.ToString());
                        return -1;
                    }
                });
            });

            app.Command("predict", (command) => 
            {
                var pickUpArgument = command.Argument<short>("[pick-up]", "Where you will be picked up from.");
                var dropOffArgument = command.Argument<short>("[drop-off]", "Where you will be dropped of at.");
                var vehicleOption = command.Option<VehicleType>("-v|--vehicle <VEHICLE>", "The type of vehicle to take", CommandOptionType.SingleValue);

                command.OnExecuteAsync(async (ct) => 
                {
                    try
                    {
                        var repo = new SqliteRepository<TaxiTrip, long>(DbPath);
                        var query = new TaxiTripMetricsQuery
                        {
                            PickUpLocationId = pickUpArgument.ParsedValue,
                            DropOffLocationId = dropOffArgument.ParsedValue,
                            VehicleType = vehicleOption.ParsedValue
                        };
                        
                        IEnumerable<TaxiTripMetrics> results = await repo.QueryAsync<TaxiTripMetrics>(query, ct);
                        TaxiTripMetrics metrics = results.SingleOrDefault();

                        System.Console.WriteLine($"Estimated Cost =>     ${metrics.AverageCost:0.00}");
                        System.Console.WriteLine($"Estimated Duration =>  {TimeSpan.FromSeconds(metrics.AverageDurationSeconds)}");

                        return 0;
                    }
                    catch (Exception e)
                    {
                        System.Console.Error.WriteLine(e.ToString());
                        return -1;
                    }
                });
            });

            return app.Execute(args);
        }
    }
}
