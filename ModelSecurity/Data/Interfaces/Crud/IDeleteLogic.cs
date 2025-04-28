namespace Data.Interfaces.Crud
{
    public interface IDeleteLogic
    {
        Task<Object> DeleteLogicalAsync(int id); 
    }
}
