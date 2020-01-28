namespace TLC.Taxi.Data.Models
{
    public class TaxiTrip : IEntity<long>
    {
        public long Id { get; set; }

        public long PickUpTimestamp { get; set; }

        public long DropOffTimestamp { get; set; }

        public short PickUpLocationId { get; set; }

        public short DropOffLocationId { get; set; }

        public VehicleType VehicleType { get; set; }

        public float? Cost { get; set; }
    }
}