namespace LoginIdentity.Models
{
    public class PageList
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public int TotalItems { get; set; }
        public string SortColumn { get; set; }
        public OrderWay OrderWay { get; set; } = OrderWay.Asc;

        public IEnumerable<ListItemModel> Items { get; set; } = new List<ListItemModel>();
        public int TotalPages
        {
            get
            {
                return TotalItems / PageSize + (TotalItems % PageSize > 0 ? 1 : 0);
            }
        }
    }

	public class ListItemModel
	{
		public int Id { get; set; }
		public string StoreName { get; set; }
	}

	public enum OrderWay
    {
        Asc,
        Desc
    }
}
