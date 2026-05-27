namespace AI.DeliciousFood.Web.Manage.Models;

public class PagedResult<T>
{
    public int Total { get; set; }
    public List<T> Rows { get; set; } = [];
}
