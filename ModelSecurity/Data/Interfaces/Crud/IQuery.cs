namespace Data.Interfaces.Crud
{
    public interface IQuery<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
