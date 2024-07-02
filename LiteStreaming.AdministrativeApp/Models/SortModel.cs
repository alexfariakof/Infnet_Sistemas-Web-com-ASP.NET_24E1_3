using Microsoft.Data.SqlClient;

namespace LiteStreaming.AdministrativeApp.Models;
public class SortModel
{

    public SortOrder SortOrder { get; set; } = SortOrder.Ascending; 
    public string? SortParamName { get; set; } = "default_desc";
    public string SortIcon { get; set; } = SortIcons.SORT_ICON_DESC;
}

public static class SortIcons
{
    public const string SORT_ICON_ASC = "bi bi-sort-alpha-down";
    public const string SORT_ICON_DESC = "bi bi-sort-alpha-up";
}
