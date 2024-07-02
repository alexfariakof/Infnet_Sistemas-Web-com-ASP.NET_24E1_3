namespace LiteStreaming.AdministrativeApp.Models;
public class PagerModel: BasePageModel
{
    public PagerModel() { }    
    public int TotalItems { get; private  set; }
    public int CurrentPage {  get; private set; }
    public int PageSize { get; private set; }    
    public int TotalPages { get; private set; }
    public int StartPage {  get; private set; }
    public int EndPage { get; private set; } 
    public int StartRecord { get; private set; }
    public int EndRecord { get; private set; }
    public string? Action { get; private set; }
    public string? SearchText { get; set; }
    public SortModel? SortModel { get; set; }
}
