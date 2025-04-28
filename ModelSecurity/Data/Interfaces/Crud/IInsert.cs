namespace Data.Interfaces.Crud
{
    public interface IInsert<T>
    {
        Task<T> CreateAsync(T Entity);
    }
}


