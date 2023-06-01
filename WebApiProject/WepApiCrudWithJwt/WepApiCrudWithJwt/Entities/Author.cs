namespace WepApiCrudWithJwt.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = null!;
        public string AuthorSurName { get; set; } = null!;
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();    
    }
}
