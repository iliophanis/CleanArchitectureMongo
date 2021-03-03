namespace Application.Common.Models
{
    public class PageParameters
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool IsAscend { get; set; }
        public string SortBy { get; set; }
    }
}