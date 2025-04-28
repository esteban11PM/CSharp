namespace Data.Interfaces.Crud
{
    public interface IDelete
    {
        Task<Object> DeletePersistentAsync(int id);
    }
}


