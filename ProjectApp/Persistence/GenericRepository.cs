using Microsoft.EntityFrameworkCore;
using ProjectApp.Core;
using ProjectApp.Core.Interfaces;
namespace ProjectApp.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    public readonly AuctionDbContext _context;
    public DbSet<T> entity => _context.Set<T>();
    public GenericRepository(AuctionDbContext context)
    {
        context = _context;
    }

    public void Add(T entity)
    {
        this.entity.Add(entity);   
        _context.SaveChanges(); 
    }
    
    public List<T> GetAll()
    {
        return this.entity.ToList();   
    }

    public T GetById(int id)
    {
        var entity = _context.Find<T>(id);
        return entity;
    }

    public void Update(T entity)
    {
        _context.Update(entity);
        _context.SaveChanges(); 
    }
}
