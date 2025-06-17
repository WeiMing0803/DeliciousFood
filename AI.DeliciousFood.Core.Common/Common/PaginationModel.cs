namespace AI.DeliciousFood.Core.Common.Common;

public class PaginationModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public required string ControllerName { get; set; }
    public required string ActionName { get; set; }
}