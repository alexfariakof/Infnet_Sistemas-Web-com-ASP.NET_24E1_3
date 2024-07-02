using Microsoft.Data.SqlClient;

namespace LiteStreaming.Application.Abstractions;
public interface IService<T> where T : class, new()
{
    T Create(T obj);
    List<T> FindAll();
    List<T> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0);
    T FindById(Guid id);
    T Update(T obj);
    bool Delete(T obj);
}