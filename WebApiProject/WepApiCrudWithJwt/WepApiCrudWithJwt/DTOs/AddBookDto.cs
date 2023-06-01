namespace WepApiCrudWithJwt.DTOs
{
    public class AddBookDto
    {
        public string BookName { get; set; } = null!;
        public int PageNumber { get; set; }

        public string AuthorName { get; set; } = null!;
        public string AuthorSurName { get; set; } = null!;

        public List<string>? BookSorts { get; set; } = new List<string>();


    }
}
