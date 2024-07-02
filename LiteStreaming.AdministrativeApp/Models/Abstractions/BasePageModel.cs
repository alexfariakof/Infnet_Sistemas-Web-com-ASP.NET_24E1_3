namespace LiteStreaming.AdministrativeApp.Models;
public abstract class BasePageModel
{
    private IEnumerable<object>? _items;
    private int _count;
    private int _pageIndex;
    private int _pageSize;
    protected int TotalRecords { get; set; }
    public BasePageModel(int count, int pageIndex, int pageSize)
    {
        _count = count;
        _pageIndex = pageIndex; 
        _pageSize = pageSize;
    }

    public void SetItems<T>(IEnumerable<T>? items) where T : class, new()
    {
        TotalRecords = items.Count();
        _items = items?.Skip((_pageIndex -1) * _pageSize).Take(_pageSize).ToList();
    }

    public IEnumerable<T> GetItems<T>() where T : class, new()
    {
        return _items?.Cast<T>() ?? Enumerable.Empty<T>();
    }
}
