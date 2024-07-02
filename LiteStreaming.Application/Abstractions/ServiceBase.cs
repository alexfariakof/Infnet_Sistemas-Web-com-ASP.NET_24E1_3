using AutoMapper;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace LiteStreaming.Application.Abstractions;
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
    public abstract List<Dto> FindAll();
    public abstract List<Dto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0);
    public abstract Dto FindById(Guid id);
    public abstract Dto Update(Dto obj);
    public abstract bool Delete(Dto obj);
}