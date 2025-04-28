namespace Data.Interfaces.Crud
{
    public interface IUpdate<T>
    {
        Task<bool> UpdateAsync(T Entity);
    }
}
