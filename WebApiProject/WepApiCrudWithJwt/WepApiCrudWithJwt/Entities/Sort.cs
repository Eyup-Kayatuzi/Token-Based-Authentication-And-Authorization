namespace WepApiCrudWithJwt.Models
{
    public class Sort
    {
        public int Id { get; set; }
        public string SortName { get; set; } = null!;
        public virtual ICollection<BookSort> BookSorts { get; set; } = new List<BookSort>();
    }
}
