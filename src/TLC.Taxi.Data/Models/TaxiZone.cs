namespace TLC.Taxi.Data.Models
{
    public class TaxiZone : IEntity<short>
    {
        public short Id { get; set; }

        public string Borough { get; set; }

        public string Zone { get; set; }
    }
}