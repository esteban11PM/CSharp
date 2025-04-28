namespace Data.Interfaces.Crud
{
    public interface IQueryById<T>
    {
        Task<T> GetByIdAsync(int id);
    }
}
