namespace webApp.DTOs;

public class PagedResult
{
    public int pageNum { get; set; }
    public int pageSize { get; set; }
    public int TotalCount { get; set; }
    public int allpages => (int)Math.Ceiling((double)TotalCount / pageSize);
    public List<GetTrip> trips { get; set; }
}
