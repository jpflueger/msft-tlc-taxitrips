namespace TLC.Taxi.Data.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}