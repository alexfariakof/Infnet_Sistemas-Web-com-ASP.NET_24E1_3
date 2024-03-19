using AutoMapper;
using Repository;

namespace Application;
public abstract class ServiceBase<Dto, Entity> where Dto : class, new() where Entity : class, new()
{
    protected IMapper Mapper { get; set; }
    protected IRepository<Entity> Repository { get; set; }
    protected ServiceBase(IMapper mapper, IRepository<Entity> repository)
    {
        Mapper = mapper;
        Repository = repository;
    }
    public abstract Dto Create(Dto obj);
    public abstract List<Dto> FindAll(Guid userId);
    public abstract Dto FindById(Guid id);
    public abstract Dto Update(Dto obj);
    public abstract bool Delete(Dto obj);
}