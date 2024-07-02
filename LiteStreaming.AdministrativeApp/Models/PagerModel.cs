namespace LiteStreaming.AdministrativeApp.Models;
public class PagerModel: BasePageModel
{
    private PagerModel() { }    
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

    public PagerModel(int totalItems, int currentPage, int pageSizes = 5) 
    {
        this.TotalItems = totalItems;
        this.CurrentPage = currentPage;
        this.PageSize = pageSizes;
        int totalPages = (int)Math.Ceiling((decimal)totalItems/(decimal)pageSizes);
        TotalPages = totalPages;

        int startPage = currentPage - 5;
        int endPage = currentPage + 4;

        if(startPage <= 0)
        {
            endPage = endPage - (startPage - 1);
            startPage = 1;
        }

        if(endPage> totalPages)
        {
            endPage = totalPages;
            if (endPage > 10)
                startPage = endPage - 9;

        }

        StartRecord = (currentPage - 1) * pageSizes + 1;
        EndRecord = StartRecord - 1 + pageSizes;

        if (EndRecord > TotalItems)
            EndRecord = TotalItems;
        if(TotalItems == 0)
        {
            StartPage = 0;
            StartRecord = 0;
            CurrentPage = 0;
            EndRecord = 0;
        }
        else
        {
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
