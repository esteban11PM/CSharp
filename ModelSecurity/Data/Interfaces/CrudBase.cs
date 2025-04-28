using Data.Interfaces.Crud;

namespace Data.Interfaces
{
    public interface CrudBase<T> : IQuery<T>, IInsert<T>, IUpdate<T>, IDelete, IDeleteLogic, IQueryById<T> where T : class
    {
    }
}
