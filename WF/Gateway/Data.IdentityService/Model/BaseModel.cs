namespace Data.IdentityService
{
    public class BaseModel
    {
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string OrderBy { get; set; } = "ASC";
        public int PageIndex { get; set; }
    }
}
