namespace TLC.Taxi.Data.Models
{
    public class TaxiTripMetricsQuery : IQuery<TaxiTrip, long>
    {
        public int PickUpLocationId { get; set; }

        public int DropOffLocationId { get; set; }

        public (string sql, object pars) ToSql()
        {
            return (
                @"SELECT 
	                avg(DropOffTimestamp - PickUpTimestamp) as AverageDurationSeconds,
	                avg(Cost) as AverageCost
                FROM TaxiTrip
                WHERE PickUpLocationId = @pickUpLocationId and DropOffLocationId = @dropOffLocationId", 
                new { pickUpLocationId = PickUpLocationId, dropOffLocationId = DropOffLocationId });
        }
    }
}
