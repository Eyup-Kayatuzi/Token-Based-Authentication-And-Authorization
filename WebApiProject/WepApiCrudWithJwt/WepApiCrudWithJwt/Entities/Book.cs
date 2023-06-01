namespace WepApiCrudWithJwt.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; } = null!;
        public int PageNumber { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; } = null!;
        public virtual ICollection<BookSort> BookSorts { get;} = new List<BookSort>();

    }
}
