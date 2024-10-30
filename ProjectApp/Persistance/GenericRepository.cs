using ProjectApp.Core;
using ProjectApp.Core.Interfaces;
namespace ProjectApp.Persistance;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public List<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T GetById(int id)
    {
        throw new NotImplementedException();
    }
}