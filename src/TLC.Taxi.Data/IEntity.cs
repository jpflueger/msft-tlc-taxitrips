namespace TLC.Taxi.Data
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}