namespace TLC.Taxi.Data
{
    public interface IQuery<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>, new()
    {
        (string sql, object pars) ToSql();
    }
}