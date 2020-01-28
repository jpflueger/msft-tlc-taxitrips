namespace TLC.Taxi.Data.Models
{
    public class TaxiTripMetricsQuery : IQuery<TaxiTrip, long>
    {
        public int PickUpLocationId { get; set; }

        public int DropOffLocationId { get; set; }

        public VehicleType? VehicleType { get; set; }

        public (string sql, object pars) ToSql()
        {
            string sql = @"SELECT 
	                avg(DropOffTimestamp - PickUpTimestamp) as AverageDurationSeconds,
	                avg(Cost) as AverageCost
                FROM TaxiTrip
                WHERE PickUpLocationId = @pickUpLocationId and DropOffLocationId = @dropOffLocationId";
            dynamic param = new { pickUpLocationId = PickUpLocationId, dropOffLocationId = DropOffLocationId };

            if (!VehicleType.HasValue)
            {
                sql += " and VehicleType = @vehicleType";
                param.vehicleType = (int)VehicleType.Value;
            }
            
            return (sql, param);
        }
    }
}
